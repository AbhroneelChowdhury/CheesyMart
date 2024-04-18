using CheesyMart.Core.DomainModels;
using CheesyMart.Data.Enums;
using FluentValidation;

namespace CheesyMart.Core.Validators;

public class CheesyProductModelValidator : AbstractValidator<CheesyProductModel>
{
    public CheesyProductModelValidator()
    {
        RuleFor(model => model.Name).NotEmpty();
        RuleFor(model => model.PricePerKilo).NotNull().GreaterThan(0m);
        RuleFor(model => model.CheeseType).NotEmpty().IsEnumName(typeof(CheeseType));
    }
    
}