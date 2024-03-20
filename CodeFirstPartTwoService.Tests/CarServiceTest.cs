using CodeFirstPartTwoApp.Models;
using CodeFirstPartTwoService.Service;

namespace CodeFirstPartTwoService.Tests
{
    [TestClass]
    public class CarServiceTests
    {
        private TestApplicationContext _context;

        [TestInitialize]
        public void TestInitialize()
        {
            _context = new TestApplicationContext();
            _context.ResetState();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _context.Dispose();
        }


        [TestMethod]
        public void GetAllCars_ReturnsCars_WhenCarsExist()
        {

            // Arrange
            _context.Cars.Add(new Car { CarId = 1, Brand = "Toyota", Model = "Corolla", Year = 2020, Color = "Red" });
            _context.Cars.Add(new Car { CarId = 2, Brand = "Honda", Model = "Civic", Year = 2019, Color = "Blue" });
            _context.SaveChanges();

            var service = new CarService(_context);

            // Act
            var cars = service.GetAllCars();

            // Assert
            Assert.AreEqual(2, cars.Count());
            Assert.IsTrue(cars.Any(car => car is { Brand: "Toyota", Model: "Corolla" }));
            Assert.IsTrue(cars.Any(car => car is { Brand: "Honda", Model: "Civic" }));
        }

        [TestMethod]
        public void GetCarById_ReturnsCar_WhenCarExists()
        {
            // Arrange
            var testCar = new Car { Brand = "Peugeot", Model = "407", Year = 2005, Color = "Red" };
            _context.Cars.Add(testCar);
            _context.SaveChanges();

            var service = new CarService(_context);

            // Act
            var car = service.GetCarById(testCar.CarId);

            // Assert
            Assert.IsNotNull(car);
            Assert.AreEqual("Peugeot", car.Brand);
            Assert.AreEqual("407", car.Model);
            Assert.AreEqual(2005, car.Year);
            Assert.AreEqual("Red", car.Color);
        }

        [TestMethod]
        public void GetCarById_ReturnsNull_WhenCarNotExists()
        {
            // Arrange
            var service = new CarService(_context);

            // Act
            var car = service.GetCarById(1
                );

            // Assert
            Assert.IsNull(car);
        }

    }
}