using CodeFirstPartTwoApp.Models;
using CodeFirstPartTwoService.Dto;
using CodeFirstPartTwoService.Service;
using Microsoft.AspNetCore.Mvc;
using CodeFirstPartTwoService;
using FluentValidation.Results;

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

        [HttpPost]
        public async Task<IActionResult> PostCar([FromBody] CreateCarDto carDto)
        {
            HttpClient httpClient = new();
            var carApiService = new CarApiService(httpClient);
            var validator = new CarValidator(carApiService);

            var validationResult = await validator.ValidateAsync(carDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Errors = ReturnValidationError(validationResult) });
            }
            var car = await carService.AddCarAsync(carDto);
            return CreatedAtAction(nameof(GetCar), new { id = car.CarId }, car);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCar(int id, CreateCarDto carDto)
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
            HttpClient httpClient = new();
            var carApiService = new CarApiService(httpClient);
            var validator = new CarValidator(carApiService);

            var validationResult = await validator.ValidateAsync(carDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Errors = ReturnValidationError(validationResult) });
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

        private static List<ValidationError> ReturnValidationError(ValidationResult validationResult)
        {
            var validationErrors = validationResult.Errors
                .Select(e => new ValidationError { PropertyName = e.PropertyName, ErrorMessage = e.ErrorMessage })
                .ToList();

            return validationErrors;
        }
    }
}
