using CodeFirstPartTwoApp.Models;
using CodeFirstPartTwoService.Dto;
using CodeFirstPartTwoService.Service;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstPartTwoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController(ICarService carService) : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Car>> GetCars()
        {
            return Ok(carService.GetAllCars());
        }

        [HttpGet("{id}")]
        public ActionResult<Car> GetCar(int id)
        {
            var car = carService.GetCarById(id);
            return car == null ? NotFound() : Ok(car);
        }

        //[HttpPost]
        //public ActionResult<Car> PostCar(CreateCarDto carDto)
        //{
        //    var car = carService.AddCarAsync(carDto);
        //    return CreatedAtAction(nameof(GetCar), new { id = car.CarId }, car);
        //}

        [HttpPost]
        public async Task<ActionResult<Car>> PostCar(CreateCarDto carDto)
        {
            try
            {
                var car = await carService.AddCarAsync(carDto);
                return CreatedAtAction(nameof(GetCar), new { id = car.CarId }, car);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult PutCar(int id, CreateCarDto carDto)
        {
            var modelStatusStateIsNotValid = !ModelState.IsValid;
            if (modelStatusStateIsNotValid)
            {
                return BadRequest(ModelState);
            }
            var car = carService.GetCarById(id);
            if (car == null)
            {
                return NotFound();
            }
            carService.UpdateCar(id, carDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCar(int id)
        {
            var car = carService.GetCarById(id);
            if (car == null)
            {
                return NotFound();
            }
            carService.DeleteCar(id);
            return NoContent();
        }
    }
}
