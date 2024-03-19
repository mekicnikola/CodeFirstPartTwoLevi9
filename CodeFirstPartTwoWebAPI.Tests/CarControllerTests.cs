using CodeFirstPartTwoService.Service;
using CodeFirstPartTwoApp.Models;
using CodeFirstPartTwoService;
using CodeFirstPartTwoService.Dto;
using Moq;
using CodeFirstPartTwoWebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace CodeFirstPartTwoWebAPI.Tests
{
    [TestClass]
    public class CarControllerTests
    {
        private Mock<ICarService> _mockCarService;
        private CarController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _mockCarService = new Mock<ICarService>();
            _controller = new CarController(_mockCarService.Object);
        }

        [TestMethod]
        public void GetCars_ReturnsAllCars()
        {
            // Arrange
            var cars = new List<Car>
            {
                new() { CarId = 1, Brand = "Toyota", Model = "Corolla" },
                new() { CarId = 2, Brand = "Honda", Model = "Civic" }
            };

            _mockCarService.Setup(service => service.GetAllCars())
                .Returns(cars);

            // Act
            var result = _controller.GetCars();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var model = okResult.Value as IEnumerable<Car>;
            Assert.AreEqual(2, model.Count());
        }

        [TestMethod]
        public void GetCar_ReturnsCar_WhenCarExists()
        {
            // Arrange
            var car = new Car { CarId = 1, Brand = "Toyota", Model = "Corolla" };
            _mockCarService.Setup(service => service.GetCarById(1))
                .Returns(car);

            // Act
            var result = _controller.GetCar(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var model = okResult.Value as Car;
            Assert.IsNotNull(model);
            Assert.AreEqual("Toyota", model.Brand);
        }

        [TestMethod]
        public async Task PostCar_ReturnsCreatedAtAction_WhenCarIsValid()
        {
            // Arrange
            var carDto = new CreateCarDto
            {
                Brand = "Tesla",
                Model = "Model S",
                Year = 2020,
                Color = "Red",
                ChassisNumber = "123ABC",
                Engine = new CreateEngineDto()
                {
                    Year = 2020,
                    Brand = "Tesla",
                    SerialNumber = "ENG123",
                    Type = "Electric",
                    EngineTypeId = 1
                }
            };

            var car = new Car
            {
                CarId = 3,
                Brand = carDto.Brand,
                Model = carDto.Model,
                Year = carDto.Year,
                Color = carDto.Color,
                ChassisNumber = carDto.ChassisNumber,
            };

            _mockCarService.Setup(service => service.AddCarAsync(It.IsAny<CreateCarDto>()))
                .ReturnsAsync(car);

            // Act
            var result = await _controller.PostCar(carDto);

            // Assert
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual("GetCar", createdAtActionResult.ActionName);
            Assert.AreEqual(3, ((Car)createdAtActionResult.Value).CarId);
        }

        [TestMethod]
        public async Task PutCar_ReturnsNoContent_WhenCarExistsAndDataIsValid()
        {
            // Arrange
            var carId = 1;
            var carDto = new CreateCarDto
            {
                Brand = "Tesla",
                Model = "Model 3",
                Year = 2019,
                Color = "Black",
                ChassisNumber = "123DEF",
                Engine = new CreateEngineDto()
                {
                    Year = 2019,
                    Brand = "Tesla",
                    SerialNumber = "ENG456",
                    Type = "Electric",
                    EngineTypeId = 1
                }
            };

            _mockCarService.Setup(service => service.GetCarById(carId))
                .Returns(new Car { CarId = carId });

            // Act
            var result = await _controller.PutCar(carId, carDto);

            // Assert
            var noContentResult = result as NoContentResult;
            Assert.IsNotNull(noContentResult);
            _mockCarService.Verify(service => service.UpdateCar(carId, It.IsAny<CreateCarDto>()), Times.Once);
        }

        [TestMethod]
        public void DeleteCar_ReturnsNoContent_WhenCarExists()
        {
            // Arrange
            const int carId = 1;
            _mockCarService.Setup(service => service.GetCarById(carId))
                .Returns(new Car { CarId = carId, Brand = "Toyota", Model = "Corolla" });

            // Act
            var result = _controller.DeleteCar(carId);

            // Assert
            var noContentResult = result as NoContentResult;
            Assert.IsNotNull(noContentResult);
            _mockCarService.Verify(service => service.DeleteCar(carId), Times.Once);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        [DataRow("-3")]
        public async Task Validator_Should_Fail_When_Model_Is_Empty(string model)
        {
            // Arrange
            var mockCarApiService = new Mock<ICarApiService>();
            mockCarApiService.Setup(service => service.IsModelAvailableAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var validator = new CarValidator(mockCarApiService.Object);


            var carDto = CreateCarDto(model, "Toyota", 2020);


            // Act
            var result = await validator.ValidateAsync(carDto);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "Model" && !string.IsNullOrEmpty(e.ErrorMessage)));
        }

        [TestMethod]
        [DataRow(2050)]
        [DataRow(-1)]
        [DataRow(1054)]
        public async Task Validator_Should_Fail_When_Year_Is_Out_Of_Range(int year)
        {
            // Arrange
            var mockCarApiService = new Mock<ICarApiService>();
            mockCarApiService.Setup(service => service.IsModelAvailableAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var validator = new CarValidator(mockCarApiService.Object);

            var carDto = CreateCarDto("Corolla", "Toyota", year);

            // Act
            var validationResult = await validator.ValidateAsync(carDto);

            // Assert
            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationResult.Errors.Any(e => e.PropertyName == "Year" && !string.IsNullOrEmpty(e.ErrorMessage)));
        }


        [TestMethod]
        public async Task Validator_Should_Fail_When_Model_Is_Empty_And_Year_Is_Out_Of_Range()
        {
            // Arrange
            var mockCarApiService = new Mock<ICarApiService>();
            mockCarApiService.Setup(service => service.IsModelAvailableAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var validator = new CarValidator(mockCarApiService.Object);

            var carDto = CreateCarDto("","Toyota",2050);

            // Act
            var result = await validator.ValidateAsync(carDto);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "Model" && !string.IsNullOrEmpty(e.ErrorMessage)));
            Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "Year" && !string.IsNullOrEmpty(e.ErrorMessage)));
        }

        private static CreateCarDto CreateCarDto(string model, string brand, int year)
        {
            var carDto = new CreateCarDto
            {
                Model = model,
                Brand = brand,
                Year = year,
            };
            return carDto;
        }

    }
}
