using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Backend.DataLayer.Models;

namespace Backend.DataLayer
{
    public interface IFragmentService : IBaseService<Fragment>
    {
        Task Upload(Guid id, Stream stream);

        Task<Stream> Download(Guid id);

        Task<List<Fragment>> GetByJob(Guid jobId);

        Task<List<Fragment>> GetByJobStep(Guid jobId, int number);

        Task<Fragment> GetLatestByJob(Guid jobId, string name);
    }
}