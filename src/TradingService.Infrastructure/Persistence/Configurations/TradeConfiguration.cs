using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradingService.Domain.Entities;
using TradingService.Domain.Enums;

namespace TradingService.Infrastructure.Persistence.Configurations;

public class TradeConfiguration : IEntityTypeConfiguration<Trade>
{
    public void Configure(EntityTypeBuilder<Trade> builder)
    {
        builder.ToTable("trades", tb => 
        {
            tb.HasCheckConstraint("CK_Trade_Quantity_GreaterThanZero", "quantity > 0");
            tb.HasCheckConstraint("CK_Trade_Price_GreaterThanZero", "price > 0");
        });

        builder.HasKey(t => t.Id);

        ConfigureProperties(builder);
        SeedData(builder);
    }

    private void ConfigureProperties(EntityTypeBuilder<Trade> builder)
    {
        builder.Property(t => t.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(t => t.Side)
            .HasColumnName("side")
            .HasMaxLength(4)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(t => t.Quantity)
            .HasColumnName("quantity")
            .IsRequired();

        builder.Property(t => t.Price)
            .HasColumnName("price")
            .HasPrecision(9, 2)
            .IsRequired();

        builder.Property(t => t.TotalAmount)
            .HasColumnName("total_amount")
            .HasPrecision(9, 2)
            .IsRequired();

        builder.Property(t => t.ExecutedAt)
            .HasColumnName("executed_at")
            .IsRequired();
    }

    private void SeedData(EntityTypeBuilder<Trade> builder)
    {
        builder.HasData(
            new Trade
            {
                Id = Guid.Parse("866bee10-bb20-4d6b-aab2-0df998d52b4f"),
                Side = TradeSide.Buy,
                Quantity = 100,
                Price = 50.0m,
                TotalAmount = 5000.0m,
                ExecutedAt = new DateTime(2025, 03, 1, 12, 0, 0, DateTimeKind.Utc)
            },
            new Trade
            {
                Id = Guid.Parse("c4ae962b-1cf5-498e-92ed-2db397b883c0"),
                Side = TradeSide.Sell,
                Quantity = 50,
                Price = 75.0m,
                TotalAmount = 3750.0m,
                ExecutedAt = new DateTime(2025, 03, 2, 14, 30, 0, DateTimeKind.Utc)
            },
            new Trade
            {
                Id = Guid.Parse("1570b546-7456-4358-bcc3-38b08b428bde"),
                Side = TradeSide.Buy,
                Quantity = 200,
                Price = 25.0m,
                TotalAmount = 5000.0m,
                ExecutedAt = new DateTime(2025, 03, 3, 9, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
