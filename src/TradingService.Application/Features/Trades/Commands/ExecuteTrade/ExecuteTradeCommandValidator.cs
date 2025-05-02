using FluentValidation;

namespace TradingService.Application.Features.Trades.Commands.ExecuteTrade;

public class ExecuteTradeCommandValidator : AbstractValidator<ExecuteTradeCommand>
{
    public ExecuteTradeCommandValidator()
    {
        RuleFor(x => x.Side)
            .IsInEnum()
            .WithMessage("Trade side must be either Buy or Sell.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than zero.");
    }
}
