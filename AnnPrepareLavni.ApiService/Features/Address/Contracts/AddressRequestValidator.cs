using FluentValidation;

namespace AnnPrepareLavni.ApiService.Features.Address.Contracts;

public class AddressRequestValidator : AbstractValidator<AddressRequest>
{
    public AddressRequestValidator()
    {
        RuleFor(a => a.Street1)
            .NotEmpty().WithMessage("Street is required.")
            .MaximumLength(100).WithMessage("Street cannot be longer than 100 characters.");

        RuleFor(a => a.Street2)
            .MaximumLength(100).WithMessage("Street2 cannot be longer than 100 characters.")
            .When(a => a.Street2 is not null);

        RuleFor(a => a.City)
            .NotEmpty().WithMessage("City is required.")
            .MaximumLength(50).WithMessage("City cannot be longer than 50 characters.");

        RuleFor(a => a.State)
            .NotEmpty().WithMessage("State is required.")
            .MaximumLength(50).WithMessage("State cannot be longer than 50 characters.");

        RuleFor(a => a.PostalCode)
            .NotEmpty().WithMessage("Postal code is required.")
            .MaximumLength(10).WithMessage("Postal code cannot be longer than 10 characters.");

        RuleFor(a => a.Country)
            .NotEmpty().WithMessage("Country is required.")
            .MaximumLength(50).WithMessage("Country cannot be longer than 50 characters.");
    }
}
