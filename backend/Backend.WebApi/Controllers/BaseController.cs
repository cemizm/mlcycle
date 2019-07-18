using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.DataLayer;
using Backend.DataLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.WebApi.Controllers
{
    public abstract class BaseController<T> : Controller where T : BaseModel
    {
        private readonly IBaseService<T> service;

        public BaseController(IBaseService<T> service)
        {
            this.service = service;
        }

        #region CRUD Operations

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var models = await service.GetAll();

            return Ok(models);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if(id == Guid.Empty)
                return BadRequest();

            var model = await service.GetById(id);
            if(model == null)
                return NotFound();

            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] T model)
        {
            if(model == null)
                return BadRequest();

            if(model.Id != Guid.Empty)
                return BadRequest();

            if(!ValidateInsertModel(model))
                return BadRequest();

            await service.Add(model);

            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] T model)
        {
            if(id == Guid.Empty)
                return BadRequest();

            if(model == null)
                return BadRequest();
            
            if(model.Id != Guid.Empty)
                return BadRequest();

            if(!ValidateUpdateModel(model))
                return BadRequest();

            model.Id = id;

            await service.Update(id, model);

            return Ok(model);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if(id == Guid.Empty)
                return BadRequest();
            
            await service.Delete(id);

            return Ok();
        }

        #endregion

        internal virtual bool ValidateInsertModel(T model) 
        {
            return true;
        }

        internal virtual bool ValidateUpdateModel(T model)
        {
            return ValidateInsertModel(model);
        }
    }
}