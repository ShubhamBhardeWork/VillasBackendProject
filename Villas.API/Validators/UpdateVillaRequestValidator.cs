using FluentValidation;
using Villas.API.DTOs;

namespace Villas.API.Validators
{
    public class UpdateVillaRequestValidator : AbstractValidator<UpdateVillaRequest>
    {
        public UpdateVillaRequestValidator()
        {
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(3).WithMessage("Name must be at least 3 characters long.")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

            RuleFor(v => v.Details)
                .MaximumLength(200).WithMessage("Details must not exceed 200 charcaters.");

            RuleFor(v => v.Rate)
                .GreaterThan(0).WithMessage("Rate must be greater than 0.");

            RuleFor(v => v.Sqft)
                .GreaterThan(0).WithMessage("Sqft must be greater than 0.");

            RuleFor(v => v.Occupancy)
                .GreaterThan(0).WithMessage("Occupancy must be greater than 0.");
        }
    }
}
