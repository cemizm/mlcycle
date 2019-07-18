using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.DataLayer;
using Backend.DataLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.WebApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : BaseController<Project>
    {
        private readonly IProjectService service;

        public ProjectsController(IProjectService service) : base(service)
        {
            this.service = service;
        }

        internal override bool ValidateInsertModel(Project model)
        {
            if(string.IsNullOrEmpty(model.Name))
                return false;

            if(string.IsNullOrEmpty(model.GitRepository))
                return false;
            
            return true;
        }
    }
}