namespace BeginCsh.TieredPriceCalculator;

public readonly record struct PricingResult
(
    int Quantity,
    decimal BasePrice,
    decimal DiscountRate,
    decimal Subtotal,
    decimal DiscountAmount,
    decimal TotalPrice,
    decimal EffectiveUnitPrice
);