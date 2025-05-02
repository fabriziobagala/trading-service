using FluentValidation;

namespace TradingService.Application.Features.Trades.Queries.GetPagedTrades;

public class GetPagedTradesQueryValidator : AbstractValidator<GetPagedTradesQuery>
{
    public GetPagedTradesQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0.")
            .LessThanOrEqualTo(50).WithMessage("Page size must not exceed 50.");
    }
}
