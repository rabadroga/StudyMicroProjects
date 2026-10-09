namespace BeginCsh.TieredPriceCalculator;

public static class OutputPrinter
{
    public static string PrintOutput(PricingResult result) => 
            $"""
            -------------------------------------------
            Позиция: Товар [Цена: {result.BasePrice:C2}]
            Промежуточный итог:            {result.Subtotal,12:C2}
            Скидка ({result.DiscountRate,3:P0})                 -{result.DiscountAmount,12:C2}
            -------------------------------------------
            ИТОГО:                                {result.TotalPrice,12:C2}
            (Цена за шт. со скидкой: {result.EffectiveUnitPrice:C2})
            """;
}