using System.ComponentModel.DataAnnotations;
using CheesyMart.Core.DomainModels;
using CheesyMart.Core.Validators;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace CheesyMart.Test.Unit.Validators;

public class CheesyProductModelValidatorTest
{
    [Theory, MemberData(nameof(GetTestData))]
    public void ValidateCheesyProductTest(CheesyProductModel cheesyProductModel, int expectedErrors)
    {
        var cheesyProductModelValidator = new CheesyProductModelValidator();

        var validationResult = cheesyProductModelValidator.Validate(cheesyProductModel);

        Assert.Equal(expectedErrors, validationResult.Errors.Count);
    }

    public static IEnumerable<object[]> GetTestData()
    {
        return new[]
        {
            new object[]
            {
                new CheesyProductModel(), 3
            },
            new object[] // Type empty
            {
                new CheesyProductModel
                {
                    CheeseType = "DeepPurple",
                    Name = "My Test cheese",
                },
                2
            },
            new object[] // Type invalid color
            {
                new CheesyProductModel
                {
                    CheeseType = "DeepPurple",
                    Name = "My Test cheese",
                },
                2
            },
            new object[] // Type price
            {
                new CheesyProductModel
                {
                    CheeseType = "SemiSoft",
                    Name = "My Test cheese",
                    PricePerKilo = -12,
                },
                1
            },
            new object[] // Type price
            {
                new CheesyProductModel
                {
                    CheeseType = "SemiSoft",
                    Color = "GoldenYellow",
                    Name = "My Test cheese",
                    PricePerKilo = 12,
                },
                0
            },
        };
    }
}