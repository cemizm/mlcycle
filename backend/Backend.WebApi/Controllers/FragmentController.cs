using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Backend.DataLayer;
using Backend.DataLayer.Models;
using Backend.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Backend.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FragmentsController : Controller
    {
        private readonly IFragmentService fragmentService;
        private readonly IJobService jobService;

        public FragmentsController(IFragmentService fragmentService, IJobService jobService)
        {
            this.fragmentService = fragmentService;
            this.jobService = jobService;
        }

        [HttpGet("job/{job}")]
        public async Task<IActionResult> GetAll(Guid job)
        {
            if(job == Guid.Empty)
                return BadRequest();

            var fragments = await fragmentService.GetByJob(job);

            return Ok(fragments);
        }

        [HttpGet("job/{job}/step/{number}")]
        public async Task<IActionResult> GetAll(Guid job, int number)
        {
            if(job == Guid.Empty)
                return BadRequest();

            var fragments = await fragmentService.GetByJobStep(job, number);

            return Ok(fragments);
        }

        #region Downloads

        private async Task<IActionResult> Download(Fragment fragment)
        {
            if(fragment == null)
                return NotFound();
            
            var fecp = new FileExtensionContentTypeProvider();

            string contentType;
            if(!fecp.TryGetContentType(fragment.Filename, out contentType))
                contentType = "application/octet-stream";

            var stream = await fragmentService.Download(fragment.Id);

            return File(stream, contentType, fragment.Filename);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if(id == Guid.Empty)
                return BadRequest();

            var fragment = await fragmentService.GetById(id);

            return await Download(fragment);
        }

        [HttpGet("job/{job}/name/{name}")]
        public async Task<IActionResult> GetLatestByJob(Guid job, string name)
        {
            if(job == Guid.Empty)
                return BadRequest();

            var fragment = await fragmentService.GetLatestByJob(job, name);

            return await Download(fragment);
        }

        [HttpGet("project/{project}/name/{name}")]
        public async Task<IActionResult> GetLatestByProject(Guid project, string name)
        {
            if(project == Guid.Empty)
                return BadRequest();

            var job = await jobService.GetLatestJob(project);
            if(job == null)
                return NotFound();
                
            return await GetLatestByJob(job.Id, name);
        }

        #endregion 

        #region Upload

        [HttpPost("job/{jobId}/step/{number}")]
        [RequestSizeLimit(long.MaxValue)]
        [RequestFormLimits(MultipartBodyLengthLimit=long.MaxValue)]
        public async Task<IActionResult> Upload(Guid jobId, int number, [FromForm]FragmentUpload upload)
        {
            if(jobId == Guid.Empty)
                return BadRequest("jobId not set");

            if(string.IsNullOrEmpty(upload.Filename))
                return BadRequest("filename not set");

            if(string.IsNullOrEmpty(upload.Name))
                return BadRequest("name not set");

            var job = await jobService.GetById(jobId);
            if(job == null)
                return NotFound("Job not found");
            
            var step = job.Steps.Find(s => s.Number == number);
            if(step == null)
                return NotFound("Step not found");

            if(step.State != ProcessingState.InProgress)
                return StatusCode(406, "Step is not in progress");

            var fragment = await fragmentService.GetLatestByJob(jobId, upload.Filename);
            if(fragment != null)
                return Conflict("Filename already exists");
            
            fragment = new Fragment{
                Id = Guid.NewGuid(),
                JobId = jobId,
                StepNumber = number,
                Created = DateTime.UtcNow,
                Name = upload.Name,
                Filename = upload.Filename,
                Type = upload.Type,
            };

            using(Stream s = upload.BinaryData.OpenReadStream())
                await fragmentService.Upload(fragment.Id, s);

            await fragmentService.Add(fragment);

            return Ok(fragment);
        }

        #endregion
    
    }
}