namespace BeginCsh.TieredPriceCalculator;

public static class DiscountCalculator
{
    const decimal DefaultBasePrice = 1500m;
    const decimal MaxDiscountCap = 0.25m;

    private static readonly (int Capacity, decimal DiscountRate)[] progressiveTiers =
        [
            (49, 0.00m),
            (150, 0.05m),
            (300, 0.10m),
            (int.MaxValue, 0.18m)
        ];
    private static readonly (int Capacity, decimal DiscountRate)[] progressiveWholesaleTiers =
        [
            (99, 0.00m),
            (400, 0.10m),
            (500, 0.20m),
            (int.MaxValue, 0.25m)
        ];
    private static readonly (int Capacity, decimal DiscountRate)[] flatTiers =
        [
            (50, 0.05m),
            (200, 0.10m),
            (500, 0.18m),
        ];
    private static readonly (int Capacity, decimal DiscountRate)[] flatWholesaleTiers =
    [
        (100, 0.10m),
            (500, 0.20m),
            (1000, 0.25m),
        ];

    private static decimal ValidatePromoCode(string? promoCode)
    {
        decimal discountRate = 0m;

        switch (promoCode)
        {
            case var s when string.Equals(s, "LOYALTY", StringComparison.OrdinalIgnoreCase):  //мб стоит потом реализовать это тоже в enum, прям напрашивается. И еще есть ощущение, что этой логики здесь быть не должно, но это не точно
                discountRate += 0.03m;
                break;

            /*case var s when string.IsNullOrWhiteSpace(s):
            break;*/

            default:
                break;
        }

        return discountRate;
    }
    private static decimal ValidateTierBonus(CustomerTier customerTier) => customerTier switch
    {
        CustomerTier.Designer => 0.05m,
        CustomerTier.Retail or CustomerTier.Wholesaler => 0m,
        _ => throw new ArgumentOutOfRangeException(nameof(customerTier), $"Необработанный тип: {customerTier}")
    };
    public static PricingResult CalculateFlatDiscount(int quantity, CustomerTier customerTier, string? promoCode = null, decimal basePrice = DefaultBasePrice)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegative(basePrice);

        decimal discountRate = ValidatePromoCode(promoCode)
                             + ValidateTierBonus(customerTier);

        switch (customerTier)
        {
            case CustomerTier.Wholesaler:
                discountRate += quantity switch
                {
                    //Этот массив связан с массивом проверки аномалии оптовика
                    >= 1000 => 0.25m,
                    >= 500 => 0.20m,
                    >= 100 => 0.10m,
                    > 0 => 0m,
                    _ => throw new ArgumentOutOfRangeException(nameof(quantity), "Недопустимое количество")
                };
                break;

            case CustomerTier.Retail or CustomerTier.Designer:
                discountRate += quantity switch
                {
                    >= 500 => 0.18m,
                    >= 200 => 0.10m,
                    >= 50 => 0.05m,
                    > 0 => 0m,
                    _ => throw new ArgumentOutOfRangeException(nameof(quantity), "Недопустимое количество")
                };
                break;

            default: throw new ArgumentOutOfRangeException(nameof(customerTier), $"Необработанный тип: {customerTier}");
                //Я не стал делать if, потому что оставил прицел на будущее расширение тиров
        }

        discountRate = Math.Min(discountRate, MaxDiscountCap);
        decimal subtotal = decimal.Round(quantity * basePrice, 2, MidpointRounding.AwayFromZero);
        decimal discountAmount = decimal.Round(subtotal * discountRate, 2, MidpointRounding.AwayFromZero);
        decimal totalPrice = subtotal - discountAmount;
        decimal effectiveUnitPrice = decimal.Round(totalPrice / quantity, 2, MidpointRounding.AwayFromZero);

        return new PricingResult(quantity, basePrice, discountRate, subtotal, discountAmount, totalPrice, effectiveUnitPrice);
    }
    public static PricingResult CalculateProgressiveDiscount(int quantity, CustomerTier customerTier, string? promoCode = null, decimal basePrice = DefaultBasePrice)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegative(basePrice);

        decimal subtotal = quantity * basePrice;
        int tempQuantity = quantity;
        decimal discountAmount = 0m;
        decimal bonusRate = ValidateTierBonus(customerTier) + ValidatePromoCode(promoCode);

        var currentTier = customerTier switch
        {
            CustomerTier.Wholesaler => progressiveWholesaleTiers,
            CustomerTier.Retail or CustomerTier.Designer => progressiveTiers,
            _ => throw new ArgumentOutOfRangeException(nameof(customerTier), $"Необработанный тип: {customerTier}")
        };
        foreach ((int C, decimal DR) in currentTier)
        {
            if (tempQuantity <= 0) break;

            int currentQuantity = Math.Min(tempQuantity, C);
            decimal bonusAndTierRate = Math.Min(bonusRate + DR, MaxDiscountCap);
            discountAmount += basePrice * currentQuantity * bonusAndTierRate;
            tempQuantity -= currentQuantity;
        }
        decimal discountRate = discountAmount / subtotal;
        discountAmount = decimal.Round(discountAmount, 2, MidpointRounding.AwayFromZero);
        subtotal = decimal.Round(subtotal, 2, MidpointRounding.AwayFromZero);
        decimal totalPrice = subtotal - discountAmount;
        decimal effectiveUnitPrice = decimal.Round(totalPrice / quantity, 2, MidpointRounding.AwayFromZero);

        return new PricingResult(quantity, basePrice, discountRate, subtotal, discountAmount, totalPrice, effectiveUnitPrice);
    }

    public static (int AdditionalQuantity, decimal SavedMoney)? CheckWholesaleAnomaly(int quantity, CustomerTier customerTier)
    {
        var currentTier = customerTier switch
        {
            CustomerTier.Wholesaler => flatWholesaleTiers,
            CustomerTier.Retail or CustomerTier.Designer => flatTiers,
            _ => throw new ArgumentOutOfRangeException(nameof(customerTier), $"Необработанный тип: {customerTier}")
        };
        int upperLimitQuantity = 0;

        foreach ((int FC, _) in currentTier)
        {
            if (quantity < FC)
            {
                upperLimitQuantity = FC;
                break;
            }
        }

        if (upperLimitQuantity == 0) return null;

        var currentQuantityResult = CalculateFlatDiscount(quantity, customerTier);
        var upperLimitQuantityResult = CalculateFlatDiscount(upperLimitQuantity, customerTier);
        if (currentQuantityResult.TotalPrice > upperLimitQuantityResult.TotalPrice)
        {
            return
            (
            AdditionalQuantity: upperLimitQuantity - quantity,
            SavedMoney: currentQuantityResult.TotalPrice - upperLimitQuantityResult.TotalPrice
            );
        }
        else return null;
    }
}