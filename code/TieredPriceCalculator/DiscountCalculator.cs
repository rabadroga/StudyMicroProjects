namespace BeginCsh.TieredPriceCalculator;

public static class DiscountCalculator
{
    private static decimal validatePromocode(string? promocode)
    {
        decimal discountRate = 0m;
                
        switch (promocode)
            {
                case var s when string.Equals(s, "LOYALTY", StringComparison.OrdinalIgnoreCase):  //мб стоит потом реализовать это тоже в enum, прям напрашивается. И еще есть ощущение, что этой логики здесь быть не должно, но это не точно
                discountRate += 0.03m;
                break;

                case var s when string.IsNullOrWhiteSpace(s): //решил оставить, чтобы не высвечивалось предупреждение лишний раз
                break;

                default:
                Console.WriteLine("Введен недействительный промокод!");
                break;
            }

        return discountRate;
    }
    public static PricingResult CalculateFlatDiscount(int quantity, string? promocode = null, decimal basePrice = 1500m)
    {
        decimal discountRate = validatePromocode(promocode);
        discountRate += quantity switch
                            {
                                >= 500 => 0.18m,
                                >= 200 => 0.10m,
                                >= 49 => 0.05m,
                                >  0 => 0m,
                                _ => throw new ArgumentOutOfRangeException(nameof(quantity), "Недопустимое количество")
                            };
        discountRate = Math.Min(discountRate, 0.25m);

        decimal subtotal = decimal.Round(quantity * basePrice, 2, MidpointRounding.AwayFromZero);
        decimal discountAmount = decimal.Round(subtotal * discountRate, 2, MidpointRounding.AwayFromZero);
        decimal totalPrice = subtotal - discountAmount;
        decimal effectiveUnitPrice = decimal.Round(totalPrice / quantity, 2, MidpointRounding.AwayFromZero);

        return new PricingResult(quantity, basePrice, discountRate, subtotal, discountAmount, totalPrice, effectiveUnitPrice);
    }

    public static readonly (int Capacity, decimal DiscountRate)[] progressiveTiers =
        [
            (49, 0.00m),
            (150, 0.05m),
            (300, 0.10m),
            (int.MaxValue, 0.18m)
        ];
    public static PricingResult CalculateProgressiveDiscount(int quantity,  string? promocode = null, decimal basePrice = 1500m)
    {
        decimal discountRate = validatePromocode(promocode);
        decimal discountAmount = 0m;
        int tempQuantity = quantity;
        
        foreach((int C, decimal DR) in progressiveTiers)
        {
            if (tempQuantity <= 0) break;

            int currentQuantity = Math.Min(tempQuantity, C);
            discountAmount += basePrice * currentQuantity * DR;
            tempQuantity -= currentQuantity;
        }
        decimal subtotal = decimal.Round(quantity * basePrice, 2, MidpointRounding.AwayFromZero);
        discountRate += discountAmount / subtotal;

        discountAmount = decimal.Round(discountAmount, 2, MidpointRounding.AwayFromZero);
        decimal totalPrice = subtotal - discountAmount;
        decimal effectiveUnitPrice = decimal.Round(totalPrice / quantity, 2, MidpointRounding.AwayFromZero);

        return new PricingResult(quantity, basePrice, discountRate, subtotal, discountAmount, totalPrice, effectiveUnitPrice);
        //я тут немного запутался в округлениях, как будто лучше все посчитать точно и только потом округлять, чтобы сохранить точные значения на будущее, на потенциальную логику. Мб стоит реализовать округление в самом рекорде? типо, копии всех значений RoundedSubtotal и тд...
    }
}