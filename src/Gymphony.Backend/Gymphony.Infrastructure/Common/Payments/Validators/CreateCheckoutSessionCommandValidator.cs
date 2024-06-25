using FluentValidation;
using Gymphony.Application.Common.Payments.Commands;
using Gymphony.Domain.Enums;

namespace Gymphony.Infrastructure.Common.Payments.Validators;

public class CreateCheckoutSessionCommandValidator : AbstractValidator<CreateCheckoutSessionCommand>
{
    public CreateCheckoutSessionCommandValidator()
    {
        RuleFor(command => command.ProductId)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(command => command.ProductType)
            .Must(BeProductTypeEnum)
            .WithMessage("Product Type must be convertible to an enum type ProductType!");
    }

    private bool BeProductTypeEnum(string productType) =>
        Enum.TryParse(productType, false, out ProductType _);
}