﻿using CodeFirstPartTwoApp.Models;
using CodeFirstPartTwoService.Dto;

namespace CodeFirstPartTwoService.Service
{
    public interface ICarService
    {
        IEnumerable<Car> GetAllCars();
        Car? GetCarById(int id);
        Task<Car> AddCarAsync(CreateCarDto carDto);
        void UpdateCar(int id, CreateCarDto carDto);
        void DeleteCar(int id);
    }
}
