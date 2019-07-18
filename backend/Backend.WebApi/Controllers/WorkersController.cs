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
    public class WorkersController : BaseController<Worker>
    {
        private readonly IWorkerService service;

        public WorkersController(IWorkerService service) : base(service)
        {
            this.service = service;
        }

        internal override bool ValidateInsertModel(Worker model)
        {
            if(string.IsNullOrEmpty(model.Name))
                return false;
            
            return true;
        }
    }
}