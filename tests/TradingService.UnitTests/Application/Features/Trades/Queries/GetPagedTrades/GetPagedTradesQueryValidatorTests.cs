using FluentValidation.TestHelper;
using TradingService.Application.Features.Trades.Queries.GetPagedTrades;

namespace TradingService.UnitTests.Application.Features.Trades.Queries.GetPagedTrades;

public class GetPagedTradesQueryValidatorTests
{
    private readonly GetPagedTradesQueryValidator _validator;

    public GetPagedTradesQueryValidatorTests()
    {
        _validator = new GetPagedTradesQueryValidator();
    }

    [Fact]
    public void Validate_InvalidPageNumber_ReturnsError()
    {
        // Arrange
        var query = new GetPagedTradesQuery(0, 10);

        // Act
        var actual = _validator.TestValidate(query);

        // Assert
        actual.ShouldHaveValidationErrorFor(x => x.PageNumber);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(51)]
    public void Validate_InvalidPageSize_ReturnsError(int pageSize)
    {
        // Arrange
        var query = new GetPagedTradesQuery(1, pageSize);

        // Act
        var actual = _validator.TestValidate(query);

        // Assert
        actual.ShouldHaveValidationErrorFor(x => x.PageSize);
    }

    [Fact]
    public void Validate_ValidQuery_ReturnsNoErrors()
    {
        // Arrange
        var query = new GetPagedTradesQuery(1, 10);

        // Act
        var actual = _validator.TestValidate(query);

        // Assert
        actual.ShouldNotHaveAnyValidationErrors();
    }
}
