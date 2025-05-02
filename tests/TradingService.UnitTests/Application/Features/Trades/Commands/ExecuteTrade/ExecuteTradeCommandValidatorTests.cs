using FluentValidation.TestHelper;
using TradingService.Application.Features.Trades.Commands.ExecuteTrade;
using TradingService.Domain.Enums;

namespace TradingService.UnitTests.Application.Features.Trades.Commands.ExecuteTrade;

public class ExecuteTradeCommandValidatorTests
{
    private readonly ExecuteTradeCommandValidator _validator;

    public ExecuteTradeCommandValidatorTests()
    {
        _validator = new ExecuteTradeCommandValidator();
    }

    [Fact]
    public void Validate_InvalidTradeSide_ReturnsError()
    {
        // Arrange
        var command = new ExecuteTradeCommand((TradeSide)999, 1, 100);

        // Act
        var actual = _validator.TestValidate(command);

        // Assert
        actual.ShouldHaveValidationErrorFor(x => x.Side);
    }

    [Fact]
    public void Validate_ZeroQuantity_ReturnsError()
    {
        // Arrange
        var command = new ExecuteTradeCommand(TradeSide.Buy, 0, 100);

        // Act
        var actual = _validator.TestValidate(command);

        // Assert
        actual.ShouldHaveValidationErrorFor(x => x.Quantity);
    }

    [Fact]
    public void Validate_NegativeQuantity_ReturnsError()
    {
        // Arrange
        var command = new ExecuteTradeCommand(TradeSide.Buy, -1, 100);

        // Act
        var actual = _validator.TestValidate(command);

        // Assert
        actual.ShouldHaveValidationErrorFor(x => x.Quantity);
    }

    [Fact]
    public void Validate_ZeroPrice_ReturnsError()
    {
        // Arrange
        var command = new ExecuteTradeCommand(TradeSide.Buy, 1, 0);

        // Act
        var actual = _validator.TestValidate(command);

        // Assert
        actual.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void Validate_NegativePrice_ReturnsError()
    {
        // Arrange
        var command = new ExecuteTradeCommand(TradeSide.Buy, 1, -100);

        // Act
        var actual = _validator.TestValidate(command);

        // Assert
        actual.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void Validate_ValidCommand_ReturnsNoErrors()
    {
        // Arrange
        var command = new ExecuteTradeCommand(TradeSide.Buy, 1, 100);

        // Act
        var actual = _validator.TestValidate(command);

        // Assert
        actual.ShouldNotHaveAnyValidationErrors();
    }
}
