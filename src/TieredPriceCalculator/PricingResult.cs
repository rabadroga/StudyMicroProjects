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

//Метод инициализации:
/*
        var quantity = InputReader.QuantityInput();
        var promocode = InputReader.PromocodeInput();
        var customerTier = InputReader.CustomerTierInput();

        var flatResult = DiscountCalculator.CalculateFlatDiscount(quantity, customerTier, promocode);
        var progressiveResult = DiscountCalculator.CalculateProgressiveDiscount(quantity, customerTier, promocode);
        var anomalyResult = DiscountCalculator.CheckWholesaleAnomaly(quantity, customerTier);

        OutputPrinter.PrintComparisonReport(flatResult, progressiveResult, customerTier);

        if (anomalyResult is (int additionalQuantity, decimal savedMoney))
            OutputPrinter.PrintWholesaleAnomalyReport(additionalQuantity, savedMoney);
    */