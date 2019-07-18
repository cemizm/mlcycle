using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.DataLayer.Models;

namespace Backend.DataLayer
{
    public interface IJobService : IBaseService<Job>
    {
        Task<List<Job>> GetScheduledJobs();

        Task<Job> GetLatestJob(Guid projectId);
    }
}