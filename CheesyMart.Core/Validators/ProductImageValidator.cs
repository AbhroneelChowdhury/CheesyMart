using CheesyMart.Core.DomainModels;
using FluentValidation;

namespace CheesyMart.Core.Validators;

public class ProductImageValidator : AbstractValidator<ProductImageModel>
{
    public ProductImageValidator()
    {
        RuleFor(model => model.ImageData).NotEmpty();
        RuleFor(model => model.AltText).NotEmpty();
    }
}