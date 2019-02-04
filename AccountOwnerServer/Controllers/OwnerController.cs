using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Entities.Extensions;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountOwnerServer.Controllers
{
    [Route("api/owner")]
    public class OwnerController : Controller
    {
         private ILoggerManager _logger;
        private IRepositoryWrapper _repository;

        public OwnerController(ILoggerManager logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAllOwners()
        {
            try
            {
                IEnumerable<Owner> owners = _repository.Owner.GetAllOwners();
                _logger.LogInfo($"Returned all owners from database.");
                return Ok(owners);
            }
            catch(Exception e)
            {
                _logger.LogError($"Something went wrong inside GetAllOwners action: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "OwnerById")]
        public IActionResult GetOwnerById(Guid id)
        {
            try
            {
                Owner owner = _repository.Owner.GetOwnerById(id);

                if(owner.IsEmptyObject())
                {
                    _logger.LogError($"Owner with id: {id}, hasn't benn found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned owner with id:{id}");
                    return Ok(owner);
                }
            }
            catch(Exception e)
            {
                _logger.LogError($"Something went wrong inside GetOwnerById action: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult CreateOwner([FromBody]Owner owner)
        {
            try
            {
                if(owner.IsObjectNull())
                {
                    _logger.LogError("Owner object sent from client is null");
                    return BadRequest("Owner object is null");
                }

                if(!ModelState.IsValid)
                {
                    _logger.LogError("Invalid owner object sent from client.");
                    return BadRequest("Invalid model objetc");
                }

                _repository.Owner.CreateOwner(owner);

                return CreatedAtRoute("OwnerById", new {id = owner.Id}, owner);
            }
            catch(Exception e)
            {
                _logger.LogError($"Something went wrong inside CreateOwner action: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}/account")]
        public IActionResult GetOwnerWithDetails(Guid id)
        {
            try
            {
                OwnerExtended owner = _repository.Owner.GetOwnerWithDetails(id);

                if(owner.Id.Equals(Guid.Empty))
                {
                    _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned owner with details for id: {id}");
                    return Ok(owner);
                }
            }
            catch(Exception e)
            {
                _logger.LogError($"Something went wrong inside GetOwnerWithDetails action: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOwner(Guid id, [FromBody]Owner owner)
        {
            try
            {
                if(owner.IsObjectNull())
                {
                    _logger.LogError("Owner object sent from client is null");
                    return BadRequest("Owner object is null");
                }

                if(!ModelState.IsValid)
                {
                    _logger.LogError("Invalid owner object sent from client.");
                    return BadRequest("Invalid model object");
                }

                Owner dbOwner = _repository.Owner.GetOwnerById(id);
                if(dbOwner.IsEmptyObject())
                {
                    _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.Owner.UpdateOwner(dbOwner, owner);

                return NoContent();
            }
            catch(Exception e)
            {
                _logger.LogError($"Something went wrong inside UpdateOwner action: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOwner(Guid id)
        {
            try
            {
                Owner owner = _repository.Owner.GetOwnerById(id);
                if(owner.IsEmptyObject())
                {
                    _logger.LogError($"Owner with id: {id}, hasn't been found in db");
                    return NotFound();
                }

                if(_repository.Account.AccountsByOwner(id).Any())
                {
                    _logger.LogError($"Cannot delete owner with id: {id}. It has related accounts. Delete those accounts first");
                    return BadRequest("Cannot delete owner. It has related accounts. Delete those accounts first");
                }

                _repository.Owner.DeleteOwner(owner);

                return NoContent();
            }
            catch(Exception e)
            {
                _logger.LogError($"Something went wrong inside DeleteOwner action: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}