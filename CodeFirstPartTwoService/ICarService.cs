using CodeFirstPartTwoApp.Models;
using CodeFirstPartTwoService.Dto;

namespace CodeFirstPartTwoService
{
    public interface ICarService
    {
        IEnumerable<Car> GetAllCars();
        Car GetCarById(int id);
        Car AddCar(CreateCarDto carDto); 
        void UpdateCar(int id, CreateCarDto carDto); 
        void DeleteCar(int id);
    }
}
