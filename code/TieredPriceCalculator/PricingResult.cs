namespace BeginCsh.TieredPriceCalculator;

public sealed record PricingResult
(
    int Quantity,
    decimal BasePrice,
    decimal DiscountRate,
    decimal Subtotal,
    decimal DiscountAmount,
    decimal TotalPrice,
    decimal EffectiveUnitPrice
);