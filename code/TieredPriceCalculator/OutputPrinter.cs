namespace BeginCsh.TieredPriceCalculator;

public static class OutputPrinter
{
    public static string PrintOutput(PricingResult result) => 
            $"""
            -------------------------------------------
            Позиция: Товар [Цена: {PricingResult.BasePrice:C2}]
            Промежуточный итог:            {result.Subtotal,12:C2}
            Скидка ({result.DiscoutRate,3:P0})                 -{result.DiscoutAmount,12:C2}
            -------------------------------------------
            ИТОГО:                                {result.TotalPrice,12:C2}
            (Цена за шт. со скидкой: {result.TotalUnitPrice:C2})
            """;
}