namespace BeginCsh.TieredPriceCalculator;

public static class DiscountCalculator
{
    public static readonly (int Capacity, decimal DiscountRate)[] progressiveTiers =
        [
            (49, 0.00m),
            (150, 0.05m),
            (300, 0.10m),
            (int.MaxValue, 0.18m)
        ];
    public static readonly (int FlatCapacity, decimal FlatDiscountRate)[] FlatDiscountTiers =
        [
            (500, 0.18m),
            (200, 0.10m),
            (50, 0.05m)
        ];
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
        foreach ((int FC, decimal FDR) in FlatDiscountTiers)
        {
            if (quantity >= FC)
            {
                discountRate += FDR;
            }
        }
        /*discountRate += quantity switch
            {
             >= 500 => 0.18m,
             >= 200 => 0.10m,
             >= 50 => 0.05m,
             >  0 => 0m,
             _ => throw new ArgumentOutOfRangeException(nameof(quantity), "Недопустимое количество")
            };*/
        discountRate = Math.Min(discountRate, 0.25m);

        decimal subtotal = decimal.Round(quantity * basePrice, 2, MidpointRounding.AwayFromZero);
        decimal discountAmount = decimal.Round(subtotal * discountRate, 2, MidpointRounding.AwayFromZero);
        decimal totalPrice = subtotal - discountAmount;
        decimal effectiveUnitPrice = decimal.Round(totalPrice / quantity, 2, MidpointRounding.AwayFromZero);

        return new PricingResult(quantity, basePrice, discountRate, subtotal, discountAmount, totalPrice, effectiveUnitPrice);
    }
    public static (int plusQuantity, decimal savedMoney)? CheckWholesaleAnomaly(int quantity)
    {
        int upperLimitQuantity = 0;
        foreach ((int FC, _) in FlatDiscountTiers)
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
              plusQuantity: upperLimitQuantity - quantity,
              savedMoney: currentQuantityResult.TotalPrice - upperLimitQuantityResult.TotalPrice
              );
            } else return null;
    }
    public static PricingResult CalculateProgressiveDiscount(int quantity,  string? promocode = null, decimal basePrice = 1500m)
    {
        decimal subtotal = decimal.Round(quantity * basePrice, 2, MidpointRounding.AwayFromZero);
        int tempQuantity = quantity; //до этого я сам додумался))
        decimal discountAmount = 0m;

        foreach((int C, decimal DR) in progressiveTiers)
        {
            if (tempQuantity <= 0) break;

            int currentQuantity = Math.Min(tempQuantity, C);
            discountAmount += basePrice * currentQuantity * DR;
            tempQuantity -= currentQuantity;
        }

        decimal promoDiscountRate = validatePromocode(promocode);
        decimal promoDiscountAmount = subtotal * promoDiscountRate;
        discountAmount += promoDiscountAmount;
        discountAmount = Math.Min(discountAmount, subtotal*0.25m);

        decimal discountRate = discountAmount / subtotal;
        discountAmount = decimal.Round(discountAmount, 2, MidpointRounding.AwayFromZero);
        decimal totalPrice = subtotal - discountAmount;
        decimal effectiveUnitPrice = decimal.Round(totalPrice / quantity, 2, MidpointRounding.AwayFromZero);

        return new PricingResult(quantity, basePrice, discountRate, subtotal, discountAmount, totalPrice, effectiveUnitPrice);
        //я тут немного запутался в округлениях, как будто лучше все посчитать точно и только потом округлять, чтобы сохранить точные значения на будущее, на потенциальную логику. Мб стоит реализовать округление в самом рекорде? типо, копии всех значений RoundedSubtotal и тд...
        //да и в целом этот метод оказался каким-то замудренным для меня, я как будто нормально его в памяти не отложил
    }
}

//Далее: создать метод парадокса оптовика
//       переписать АутпутПринтер
//      переписать Програм
//ревью