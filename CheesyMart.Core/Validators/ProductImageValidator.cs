using CheesyMart.Core.CommandModels;
using CheesyMart.Core.DomainModels;
using FluentValidation;

namespace CheesyMart.Core.Validators;

public class ProductImageCommandModelValidator : AbstractValidator<ProductImageCommandModel>
{
    public ProductImageCommandModelValidator()
    {
        RuleFor(model => model.Data).NotEmpty();
        RuleFor(model => model.AlternateText).NotEmpty();
        RuleFor(model => model.MimeType).NotEmpty();
    }
}