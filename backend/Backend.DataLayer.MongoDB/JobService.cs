using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DataLayer.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Backend.DataLayer.MongoDB
{
    public class JobService : BaseService<Job>, IJobService
    {
        public JobService(MongoSettings settings) : base(settings) { }

        public Task<Job> GetLatestJob(Guid projectId)
        {
            return Collection.Find(j => j.ProjectId == projectId && j.State == ProcessingState.Done)
                             .SortByDescending(j => j.Created)
                             .FirstOrDefaultAsync();
        }

        public Task<List<Job>> GetScheduledJobs()
        {
            return Collection.Find(j => j.Steps.Any(s => s.State == ProcessingState.Scheduled))
                             .SortBy(j => j.Created)
                             .ToListAsync();
        }

        
    }
}