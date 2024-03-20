using System.Data.Entity;
using CodeFirstPartTwoApp.Models;
using CodeFirstPartTwoApplication.Data;
using CodeFirstPartTwoService.Dto;

namespace CodeFirstPartTwoService.Service
{
    public class CarService(ApplicationContext context) : ICarService
    {
        public IEnumerable<Car> GetAllCars()
        {
            return context.Cars.Include(c => c.Engine).ToList();
        }

        public Car? GetCarById(int id)
        {
            var carWithEngine = context.Cars
                .Include(c => c.Engine)
                .FirstOrDefault(c => c.CarId == id);
            return carWithEngine;
        }

        public async Task<Car> AddCarAsync(CreateCarDto carDto)
        {
            var car = new Car
            {
                Color = carDto.Color,
                Year = carDto.Year,
                ChassisNumber = carDto.ChassisNumber,
                Brand = carDto.Brand,
                Model = carDto.Model,
                Engine = new Engine
                {
                    Year = carDto.Engine.Year,
                    Brand = carDto.Engine.Brand,
                    SerialNumber = carDto.Engine.SerialNumber,
                    Type = carDto.Engine.Type,
                    EngineTypeId = carDto.Engine.EngineTypeId
                }
            };

            context.Cars.Add(car);
            await context.SaveChangesAsync();

            return car;
        }

        public void UpdateCar(int id, CreateCarDto carDto)
        {
            var car = context.Cars.Include(c => c.Engine).SingleOrDefault(c => c.CarId == id);
            if (car == null) return;
            car.Color = carDto.Color;
            car.Year = carDto.Year;
            car.ChassisNumber = carDto.ChassisNumber;
            car.Brand = carDto.Brand;
            car.Model = carDto.Model;

                car.Engine.Year = carDto.Engine.Year;
                car.Engine.Brand = carDto.Engine.Brand;
                car.Engine.SerialNumber = carDto.Engine.SerialNumber;
                car.Engine.Type = carDto.Engine.Type;
                car.Engine.EngineTypeId = carDto.Engine.EngineTypeId;
            
            context.SaveChanges();
        }

        public void DeleteCar(int id)
        {
            var car = context.Cars.Include(c => c.Engine).SingleOrDefault(c => c.CarId == id);
            if (car == null) return;
          
                context.Engines.Remove(car.Engine);


            context.Cars.Remove(car);
            context.SaveChanges();
        }
    }
}
