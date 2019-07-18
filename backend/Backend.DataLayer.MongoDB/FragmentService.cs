using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Backend.DataLayer.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using System.Linq;

namespace Backend.DataLayer.MongoDB
{
    public class FragmentService : BaseService<Fragment>, IFragmentService
    {
        public FragmentService(MongoSettings settings) : base(settings) { }

        private ObjectId ToObjectId(Guid id)
        {
            return new ObjectId(id.ToByteArray().Take(12).ToArray());
        }
        
        public Task Upload(Guid id, Stream stream)
        {
            return Bucket.UploadFromStreamAsync(ToObjectId(id), id.ToString(), stream);
        }

        public async Task<Stream> Download(Guid id)
        {
            return await Bucket.OpenDownloadStreamAsync(ToObjectId(id));
        }

        public Task<List<Fragment>> GetByJob(Guid jobId)
        {
            return Collection.Find(f => f.JobId == jobId).ToListAsync();
        }

        public Task<List<Fragment>> GetByJobStep(Guid jobId, int number)
        {
            return Collection.Find(f => f.JobId == jobId && f.StepNumber == number).ToListAsync();
        }

        public Task<Fragment> GetLatestByJob(Guid jobId, string name)
        {
            return Collection.Find(f => f.JobId == jobId && 
                                        f.Filename.ToLower() == name.ToLower())
                              .FirstOrDefaultAsync();
        }
    }
}