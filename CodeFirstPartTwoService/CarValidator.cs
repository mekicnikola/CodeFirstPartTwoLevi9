using CodeFirstPartTwoService.Dto;
using FluentValidation;

namespace CodeFirstPartTwoService
{
    public class CarValidator : AbstractValidator<CreateCarDto>
    {
        public CarValidator(ICarApiService carApiService)
        {

            RuleFor(car => car.Model)
                .NotEmpty().WithMessage("Model cannot be empty")
                .Matches(@"^[a-zA-Z0-9 ]+$").WithMessage("Model contains invalid characters");
            RuleFor(car => car.Brand).NotEmpty().WithMessage("Brand cannot be empty");
            RuleFor(car => car.Year)
                .InclusiveBetween(1900, DateTime.UtcNow.Year)
                .WithMessage("Year must be between 1900 and the current year");


            RuleFor(car => car).MustAsync(async (carDto, cancellation) =>
                    await carApiService.IsModelAvailableAsync(carDto.Model, carDto.Year, carDto.Brand))
                .WithMessage("Specified model is not available for the given brand and year.");

        }
    }
}
