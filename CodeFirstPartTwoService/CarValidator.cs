using CodeFirstPartTwoService.Dto;

namespace CodeFirstPartTwoService
{
    public class CarValidator(CarApiService carApiService)
    {
        public async Task<bool> ValidateCarAsync(CreateCarDto carDto)
        {
            return await carApiService.IsModelAvailableAsync(carDto.Model, carDto.Year, carDto.Brand);
        }
    }
}
