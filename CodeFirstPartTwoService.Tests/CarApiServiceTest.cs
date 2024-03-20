using CodeFirstPartTwoService.CarApiResponse;
using Moq.Protected;
using Moq;
using Newtonsoft.Json;
using System.Net;


namespace CodeFirstPartTwoService.Tests
{
    [TestClass]
    public class CarApiServiceTest
    {
        [TestMethod]
        public async Task IsModelAvailableAsync_ReturnsTrue_WhenModelIsAvailable()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new CarApiResponse.CarApiResponse
                {
                    Data = [new CarData { Name = "TestModel", Make = new Make { Name = "TestBrand" } }]
                }))
            };

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var carApiService = new CarApiService(httpClient);

            // Act
            var result = await carApiService.IsModelAvailableAsync("TestModel", 2020, "TestBrand");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task IsModelAvailableAsync_ReturnsFalse_WhenModelIsNotAvailable()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new CarApiResponse.CarApiResponse
                {
                    Data = []
                }))
            };

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var carApiService = new CarApiService(httpClient);

            // Act
            var result = await carApiService.IsModelAvailableAsync("NonExistentModel", 2020, "TestBrand");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task IsModelAvailableAsync_ThrowsException_WhenApiReturnsError()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("")
            };

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var carApiService = new CarApiService(httpClient);

            // Act Assert
            await Assert.ThrowsExceptionAsync<HttpRequestException>(() => carApiService.IsModelAvailableAsync("TestModel", 2020, "TestBrand"));
        }
    }
}
