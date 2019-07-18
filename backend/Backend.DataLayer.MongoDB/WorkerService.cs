using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DataLayer.Models;
using MongoDB.Driver;

namespace Backend.DataLayer.MongoDB
{
    public class WorkerService : BaseService<Worker>, IWorkerService
    {
        public WorkerService(MongoSettings settings) : base(settings) { }
    }
}