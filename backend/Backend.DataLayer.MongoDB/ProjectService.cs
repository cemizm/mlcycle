using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Backend.DataLayer.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Backend.DataLayer.MongoDB
{
    public class ProjectService : BaseService<Project>, IProjectService
    {
        public ProjectService(MongoSettings settings) : base(settings) { }

    }
}