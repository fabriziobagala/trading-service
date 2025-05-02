using FluentValidation;

namespace TradingService.Application.Features.Trades.Queries.GetTradeById;

public class GetTradeByIdQueryValidator : AbstractValidator<GetTradeByIdQuery>
{
    public GetTradeByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("Trade ID must not be empty.");
    }
}
