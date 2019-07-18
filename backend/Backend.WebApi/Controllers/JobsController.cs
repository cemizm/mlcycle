using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DataLayer;
using Backend.DataLayer.Models;
using Backend.WebApi.Models;
using Backend.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.WebApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : Controller
    {
        private readonly IProjectService projectService;
        private readonly IJobService jobService;
        private readonly IFragmentService fragmentService;
        private readonly SyncService syncService;
        private readonly LongPollingService longPollingService;

        public JobsController(IProjectService projectService, IJobService jobService, IFragmentService fragmentService, SyncService syncService, LongPollingService longPollingService) 
        {
            this.projectService = projectService;
            this.jobService = jobService;
            this.fragmentService = fragmentService;
            this.syncService = syncService;
            this.longPollingService = longPollingService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var jobs = await jobService.GetAll();
            var projects = (await projectService.GetAll()).ToList();

            var result = jobs.Select(job => JobToViewModel(job, projects.Find(p => p.Id == job.ProjectId)));
            
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if(id == Guid.Empty)
                return BadRequest();

            var job = await jobService.GetById(id);
            if(job == null)
                return NotFound();

            var project = await projectService.GetById(job.ProjectId);
            var fragments = await fragmentService.GetByJob(job.Id);
            
            var tmp = JobToViewModel(job, project, fragments);

            return Ok(tmp);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Append(Guid id, [FromBody]List<StepModel> models)
        {
            if(id == Guid.Empty)
                return BadRequest();
            
            if(models == null || models.Count() == 0)
                return BadRequest();

            Job job = null;
            using(await syncService.GetLockObject(SyncService.Job))
            {
                job = await jobService.GetById(id);
                if(job == null)
                    return NotFound();

                if(job.State != ProcessingState.InProgress)
                    return StatusCode(406, "Job is not in progress");

                var max = job.Steps.Select(s => s.Number).Max() + 1;

                var steps = models.Select(m => new Step { Number = max + models.IndexOf(m), 
                                                          Name = m.Name, 
                                                          Docker = m.Docker,
                                                          State = ProcessingState.Created
                                                        });

                job.Steps.AddRange(steps);

                await jobService.Update(id, job);
            }

            return Ok(job);
        }

        [HttpPost("project/{projectId}/trigger")]
        public async Task<ActionResult> TriggerBuild(Guid projectId, JobInitiator initiator)
        {
            if(projectId == Guid.Empty)
                return BadRequest();

            Project project = await projectService.GetById(projectId);
            if(project == null)
                return NotFound();

            IEnumerable<Job> jobs = await jobService.GetBy(t=>t.ProjectId == projectId && t.State != ProcessingState.Done);
            if(jobs.Count() > 0)
                return new StatusCodeResult(503);
            
            Job job = new Job();
            job.Created = DateTime.Now;
            job.Initiator = initiator;
            job.ProjectId = project.Id;
            job.State = ProcessingState.Created;

            job.Steps = new List<Step>(new Step[]{
                new Step {
                    Number = 0,
                    Name = "Bootstrap",
                    State = ProcessingState.Scheduled,
                }
            });

            await jobService.Add(job);

            longPollingService.Notify(LongPollingService.StepsCreated);

            return Ok(job);
        }

        #region Helpers

        private object JobToViewModel(Job job, Project project, List<Fragment> fragments = null)
        {
            return new {
                Id = job.Id,
                Created = job.Created,
                Finished = job.Finished,
                State = job.State,
                Initiator = job.Initiator,
                Project = project,
                Steps = job.Steps.Select(s => StepToViewModel(s, fragments))
            };
        }

        private object StepToViewModel(Step step, List<Fragment> all)
        {
            var fragments = all?.Where(f => f.StepNumber == step.Number).Select(f => FragmentToViewModel(f));
            if(fragments?.Count() == 0)
                fragments = null;

            return new { Name = step.Name,
                         Number = step.Number,
                         Start = step.Start,
                         End = step.End,
                         State = step.State,
                         Fragments = fragments
            };
        }

        private object FragmentToViewModel(Fragment fragment)
        {
            return new {
                Id = fragment.Id,
                Created = fragment.Created,
                Name = fragment.Name,
                Filename = fragment.Filename,
                Type = fragment.Type
            };
        }

        #endregion    
    }
}