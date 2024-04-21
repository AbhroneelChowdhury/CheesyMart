using CheesyMart.Core.CommandModels;
using CheesyMart.Core.DomainModels;
using CheesyMart.Core.Validators;

namespace CheesyMart.Test.Unit.Validators;

public class ProductImageValidatorTest
{
    [Theory, MemberData(nameof(GetTestData))]
    public void ValidateProductImageTest(ProductImageCommandModel productImageCommandModel, int expectedErrors)
    {
        var productImageValidator = new ProductImageCommandModelValidator();

        var validationResult = productImageValidator.Validate(productImageCommandModel);

        Assert.Equal(expectedErrors, validationResult.Errors.Count);
    }

    public static IEnumerable<object[]> GetTestData()
    {
        return new[]
        {
            new object[]
            {
                new ProductImageCommandModel(), 3
            },
            new object[] // Type empty
            {
                new ProductImageCommandModel
                {
                    Data = "dasdsd",
                    AlternateText = "test",
                },
                1
            },
            new object[] // Type invalid color
            {
                new ProductImageCommandModel
                {
                    Data = "dasdsd",
                    AlternateText = "test",
                    MimeType = "Text/Jpeg"
                },
                0
            },
            
        };
    }
}