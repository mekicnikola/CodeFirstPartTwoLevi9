using CodeFirstPartTwoApp.Models;
using CodeFirstPartTwoService;
using CodeFirstPartTwoService.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CodeFirstPartTwoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EngineTypeController(IEngineTypeService engineTypeService) : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<EngineType>> GetEngineTypes()
        {
            return Ok(engineTypeService.GetAllEngineTypes());
        }

        [HttpGet("{id}")]
        public ActionResult<EngineType> GetEngineType(int id)
        {
            var engineType = engineTypeService.GetEngineTypeById(id);
            return engineType == null ? NotFound() : Ok(engineType);
        }

        [HttpPost]
        public ActionResult<EngineType> PostEngineType(CreateEngineTypeDto engineTypeDto)
        {
            var engineType = engineTypeService.AddEngineType(engineTypeDto);
            return CreatedAtAction(nameof(GetEngineType), new { id = engineType.EngineTypeId }, engineType);
        }

        [HttpPut("{id}")]
        public IActionResult PutEngineType(int id, CreateEngineTypeDto engineTypeDto)
        {
            var modelStatusStateIsNotValid = !ModelState.IsValid;
            if (modelStatusStateIsNotValid)
            {
                return BadRequest(ModelState);
            }

            var engineType = engineTypeService.GetEngineTypeById(id);
            if (engineType == null)
            {
                return NotFound();
            }
            engineTypeService.UpdateEngineType(id, engineTypeDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEngineType(int id)
        {
            var engineType = engineTypeService.GetEngineTypeById(id);
            if (engineType == null)
            {
                return NotFound();
            }
            engineTypeService.DeleteEngineType(id);
            return NoContent();
        }
    }
}
