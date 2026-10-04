namespace BeginCsh.TieredPriceCalculator;

public static class FlatDiscountCalculator //Переименую просто в дисконт кальк, надо будет чекнуть как это сделать автоматом без замен
{
    //Изучил свитчи -- это имба, с ними обретается понимание и все становится проще
    public static decimal CalculateDiscount(int quantity) =>
        quantity switch
        {
            >= 500 => 0.18m,
            >= 200 => 0.10m,
            >= 50 => 0.05m,
            >  0 => 0m,
            _ => throw new ArgumentOutOfRangeException(nameof(quantity), "Недопустимое количество")
        };
}