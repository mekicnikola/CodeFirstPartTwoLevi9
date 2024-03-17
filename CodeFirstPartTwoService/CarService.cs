using System.Data.Entity;
using CodeFirstPartTwoApp.Models;
using CodeFirstPartTwoApplication.Data;
using CodeFirstPartTwoService.Dto;

namespace CodeFirstPartTwoService
{
    public class CarService(ApplicationContext context) : ICarService
    {
        public IEnumerable<Car> GetAllCars()
        {
            //return _context.Cars.ToList();
            return context.Cars.Include(c => c.Engine).ToList();
        }

        public Car GetCarById(int id)
        {
            //return _context.Cars.Find(id);
            var carWithEngine = context.Cars
                .Include(c => c.Engine)
                .FirstOrDefault(c => c.CarId == id);
            return carWithEngine;
        }

        public Car AddCar(CreateCarDto carDto)
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
            context.SaveChanges();

            return car;
        }

        public void UpdateCar(int id, CreateCarDto carDto)
        {
            var car = context.Cars.Include(c => c.Engine).SingleOrDefault(c => c.CarId == id);
            if (car != null)
            {
                car.Color = carDto.Color;
                car.Year = carDto.Year;
                car.ChassisNumber = carDto.ChassisNumber;
                car.Brand = carDto.Brand;
                car.Model = carDto.Model;
                if (car.Engine != null)
                {
                    car.Engine.Year = carDto.Engine.Year;
                    car.Engine.Brand = carDto.Engine.Brand;
                    car.Engine.SerialNumber = carDto.Engine.SerialNumber;
                    car.Engine.Type = carDto.Engine.Type;
                    car.Engine.EngineTypeId = carDto.Engine.EngineTypeId;
                }
                context.SaveChanges();
            }
            // not exists
        }

        public void DeleteCar(int id)
        {
            var car = context.Cars.Include(c => c.Engine).SingleOrDefault(c => c.CarId == id);
            if (car != null)
            {
                if (car.Engine != null)
                {
                    context.Engines.Remove(car.Engine);
                }

                context.Cars.Remove(car);
                context.SaveChanges();
            }
            // not exists
        }
    }
}
