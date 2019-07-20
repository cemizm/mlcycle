using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DataLayer;
using Backend.DataLayer.Models;
using Backend.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulerController : Controller
    {
        private IJobService jobService;
        private LongPollingService longPollingService;
        private SyncService syncService;

        public SchedulerController(IJobService jobService, LongPollingService longPollingService, SyncService syncService) 
        {
            this.jobService = jobService;
            this.longPollingService = longPollingService;
            this.syncService = syncService;
        }


        [HttpGet()]
        public async Task<ActionResult> GetPending()
        {
            List<Job> jobs = await jobService.GetScheduledJobs();

            if(jobs.Count() == 0) 
            {
                if(!(await longPollingService.Wait(LongPollingService.StepsCreated)))
                    return StatusCode(408);

                jobs = await jobService.GetScheduledJobs();
            }

            var steps = (from j in jobs
                         from s in j.Steps
                         where s.State == ProcessingState.Scheduled
                         select new {
                             ProjectId = j.ProjectId,
                             JobId = j.Id,
                             Created = j.Created,
                             Initiator = j.Initiator,
                             Step = new {
                                 Number = s.Number,
                                 Name = s.Name,
                                 Docker = s.Docker
                             }
                         });

            return Ok(steps);
        }

        [HttpPost("{id}/step/{number}/claim")]
        public async Task<ActionResult> Claim(Guid id, int number)
        {
            if(id == Guid.Empty)
                return BadRequest();

            using(await syncService.GetLockObject(SyncService.Job))
            {
                var job = await jobService.GetById(id);
                if(job == null)
                    return NotFound("Job not found");
                
                var step = job.Steps.Find(s => s.Number == number);
                if(step == null)
                    return NotFound("Step not found");

                if(step.State != ProcessingState.Scheduled)
                    return StatusCode(406, "Step is not scheduled");

                job.State = ProcessingState.InProgress;
                
                step.Start = DateTime.UtcNow;
                step.State = ProcessingState.InProgress;

                await jobService.Update(id, job);
            }

            return Ok();
        }

        [HttpPost("{id}/step/{number}/complete")]
        public async Task<ActionResult> Complete(Guid id, int number)
        {
            return await Finish(id, number);
        }

        [HttpPost("{id}/step/{number}/error")]
        public async Task<ActionResult> Error(Guid id, int number)
        {
            return await Finish(id, number, true);
        }

        private async Task<ActionResult> Finish(Guid id, int number, bool error=false)
        {
            if(id == Guid.Empty)
                return BadRequest();
            
            Step next = null;

            using(await syncService.GetLockObject(SyncService.Job))
            {
                var job = await jobService.GetById(id);
                if(job == null)
                    return NotFound("Job not found");

                Step current = job.Steps.Find(s => s.Number == number);
                if(current == null)
                    return NotFound("Step not found");

                if(current.State != ProcessingState.InProgress)
                    return StatusCode(406, "Step is not in progress");
                
                current.State = error ? ProcessingState.Error : ProcessingState.Done;
                current.End = DateTime.UtcNow;

                if(!error)
                    next = job.Steps.Find(s => s.Number == current.Number + 1);

                if(next == null)
                {
                    job.Finished = DateTime.UtcNow;
                    job.State = current.State;
                }
                else
                    next.State = ProcessingState.Scheduled;

                await jobService.Update(id, job);
            }

            if(next != null)
                longPollingService.Notify(LongPollingService.StepsCreated);

            return Ok();
        }
    }
}