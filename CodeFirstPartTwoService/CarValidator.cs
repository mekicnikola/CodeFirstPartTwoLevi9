using CodeFirstPartTwoService.Dto;
using FluentValidation;

namespace CodeFirstPartTwoService
{
    public class CarValidator : AbstractValidator<CreateCarDto>
    {
        public CarValidator(CarApiService carApiService)
        {

            RuleFor(car => car.Model).NotEmpty();
            RuleFor(car => car.Brand).NotEmpty();
            RuleFor(car => car.Year).InclusiveBetween(1900, DateTime.UtcNow.Year);


            RuleFor(car => car).MustAsync(async (carDto, cancellation) =>
                    await carApiService.IsModelAvailableAsync(carDto.Model, carDto.Year, carDto.Brand))
                .WithMessage("Specified model is not available for the given brand and year.");
        }
    }
}
