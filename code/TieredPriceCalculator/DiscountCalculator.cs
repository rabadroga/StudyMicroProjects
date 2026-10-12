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
    private static readonly (int FlatCapacity, decimal FlatDiscountRate)[] flatDiscountTiers =
        [
            (50, 0.05m),
            (200, 0.10m),
            (500, 0.18m),
        ];
    private static decimal ValidatePromoCode(string? promoCode)
    {
        decimal discountRate = 0m;
                
        switch (promoCode)
            {
                case var s when string.Equals(s, "LOYALTY", StringComparison.OrdinalIgnoreCase):  //мб стоит потом реализовать это тоже в enum, прям напрашивается. И еще есть ощущение, что этой логики здесь быть не должно, но это не точно
                discountRate += 0.03m;
                break;

                /*case var s when string.IsNullOrWhiteSpace(s): //решил оставить, чтобы не высвечивалось предупреждение лишний раз
                break;*/

                default:
                break;
            }

        return discountRate;
    }
    public static PricingResult CalculateFlatDiscount(int quantity, string? promoCode = null, decimal basePrice = DefaultBasePrice)
    {
        decimal discountRate = ValidatePromoCode(promoCode);
        /*foreach ((int FC, decimal FDR) in FlatDiscountTiers)
        {
            if (quantity >= FC)
            {
                discountRate += FDR;
                break;
            }
        }*/
        discountRate += quantity switch
            {
             >= 500 => 0.18m,
             >= 200 => 0.10m,
             >= 50 => 0.05m,
             >  0 => 0m,
             _ => throw new ArgumentOutOfRangeException(nameof(quantity), "Недопустимое количество")
            };
        discountRate = Math.Min(discountRate, MaxDiscountCap);

        decimal subtotal = decimal.Round(quantity * basePrice, 2, MidpointRounding.AwayFromZero);
        decimal discountAmount = decimal.Round(subtotal * discountRate, 2, MidpointRounding.AwayFromZero);
        decimal totalPrice = subtotal - discountAmount;
        decimal effectiveUnitPrice = decimal.Round(totalPrice / quantity, 2, MidpointRounding.AwayFromZero);

        return new PricingResult(quantity, basePrice, discountRate, subtotal, discountAmount, totalPrice, effectiveUnitPrice);
    }
    public static (int AdditionalQuantity, decimal SavedMoney)? CheckWholesaleAnomaly(int quantity)
    {
        int upperLimitQuantity = 0;
        foreach ((int FC, _) in flatDiscountTiers)
        {
            if (quantity < FC) 
            {
                upperLimitQuantity = FC;
                break;
            }
        }

        if (upperLimitQuantity == 0) return null;
        
        var currentQuantityResult = CalculateFlatDiscount(quantity);
        var upperLimitQuantityResult = CalculateFlatDiscount(upperLimitQuantity);
        if(currentQuantityResult.TotalPrice > upperLimitQuantityResult.TotalPrice)
            {
              return
              (
              AdditionalQuantity: upperLimitQuantity - quantity,
              SavedMoney: currentQuantityResult.TotalPrice - upperLimitQuantityResult.TotalPrice
              );
            } else return null;
    }
    public static PricingResult CalculateProgressiveDiscount(int quantity,  string? promoCode = null, decimal basePrice = DefaultBasePrice)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegative(basePrice);

        decimal subtotal = decimal.Round(quantity * basePrice, 2, MidpointRounding.AwayFromZero);
        int tempQuantity = quantity;
        decimal discountAmount = 0m;

        foreach((int C, decimal DR) in progressiveTiers)
        {
            if (tempQuantity <= 0) break;

            int currentQuantity = Math.Min(tempQuantity, C);
            discountAmount += basePrice * currentQuantity * DR;
            tempQuantity -= currentQuantity;
        }

        decimal promoDiscountRate = ValidatePromoCode(promoCode);
        decimal promoDiscountAmount = subtotal * promoDiscountRate;
        discountAmount += promoDiscountAmount;
        discountAmount = Math.Min(discountAmount, subtotal*MaxDiscountCap);

        decimal discountRate = discountAmount / subtotal;
        discountAmount = decimal.Round(discountAmount, 2, MidpointRounding.AwayFromZero);
        decimal totalPrice = subtotal - discountAmount;
        decimal effectiveUnitPrice = decimal.Round(totalPrice / quantity, 2, MidpointRounding.AwayFromZero);

        return new PricingResult(quantity, basePrice, discountRate, subtotal, discountAmount, totalPrice, effectiveUnitPrice);
    }
}