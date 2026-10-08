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
    
    //Изучил свитчи -- это имба, с ними обретается понимание и все становится проще
    public static decimal CalculateFlatDiscount(int quantity) =>
        quantity switch
        {
            >= 500 => 0.18m,
            >= 200 => 0.10m,
            >= 50 => 0.05m,
            >  0 => 0m,
            _ => throw new ArgumentOutOfRangeException(nameof(quantity), "Недопустимое количество")
            //Далее нужно будет дописать safecap по лимиту скидки в 25%
            //Добавить сюда предупреждение о том, что чел может купить выгоднее
            //Под конец перечитать код
            //Когда можно будет сделать себе задание, где я не буду сидеть ломать голову над каждой строчкой кода и реализацией функциональности, а напишу как рутину, чтобы увидеть, что я уже не хлебушек
        };

    public static decimal CalculateProgressiveDiscount(int quantity, decimal basePrice)
    {
        decimal discountAmount = 0m;
        
        foreach((int C, decimal DR) in progressiveTiers)
        {
            if (quantity <= 0) break;

            int currentQuantity = Math.Min(quantity, C);
            discountAmount += basePrice * currentQuantity * DR;
            quantity -= currentQuantity;
        }

        return discountAmount;
    }
}