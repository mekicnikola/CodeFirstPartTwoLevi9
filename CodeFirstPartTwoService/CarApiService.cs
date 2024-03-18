using CodeFirstPartTwoApp.Models;
using Newtonsoft.Json;

namespace CodeFirstPartTwoService
{
    public class CarApiService
    {
        private readonly HttpClient _httpClient = new();
        private readonly string _apiKey = GetKey("carApiKey.txt");
        private readonly string _apiHost = GetKey("carApiHost.txt");

        public async Task<bool> IsModelAvailableAsync(string model, int year, string brand)
        {
            var requestUri = $"https://car-api2.p.rapidapi.com/api/models?model={Uri.EscapeDataString(model)}&year={year}&make={Uri.EscapeDataString(brand)}";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUri),
                Headers =
                {
                    { "X-RapidAPI-Key", _apiKey },
                    { "X-RapidAPI-Host", _apiHost },
                },
            };

            using (var response = await _httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                
                var body = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CarApiResponse.CarApiResponse>(body);

                return result.Data.Any(d => d.Name.Equals(model, StringComparison.OrdinalIgnoreCase));
                ;

            }
        }

        private static string GetKey(string file)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var relativePath = @$"..\..\..\..\CodeFirstPartTwoService\{file}";
            var filePath = Path.Combine(baseDirectory, relativePath);
            System.Diagnostics.Debug.WriteLine("Putanja" + filePath);
            try
            {
                return File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($@"Could not read the connection string file: {ex.Message}");
                return string.Empty;
            }
        }
    }
}