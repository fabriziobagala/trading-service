using FluentValidation.TestHelper;
using TradingService.Application.Features.Trades.Queries.GetTradeById;

namespace TradingService.UnitTests.Application.Features.Trades.Queries.GetTradeById;

public class GetTradeByIdQueryValidatorTests
{
    private readonly GetTradeByIdQueryValidator _validator;

    public GetTradeByIdQueryValidatorTests()
    {
        _validator = new GetTradeByIdQueryValidator();
    }

    [Fact]
    public void Validate_EmptyId_ReturnsError()
    {
        // Arrange
        var query = new GetTradeByIdQuery(Guid.Empty);

        // Act
        var actual = _validator.TestValidate(query);

        // Assert
        actual.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Validate_ValidId_ReturnsNoErrors()
    {
        // Arrange
        var query = new GetTradeByIdQuery(Guid.NewGuid());

        // Act
        var actual = _validator.TestValidate(query);

        // Assert
        actual.ShouldNotHaveAnyValidationErrors();
    }
}
