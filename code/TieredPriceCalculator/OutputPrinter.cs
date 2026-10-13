namespace BeginCsh.TieredPriceCalculator;

public static class OutputPrinter
{
    public static void PrintComparisonReport(PricingResult flatResult, PricingResult progressiveResult)
    {
        Console.WriteLine("=======================================================================");
        Console.WriteLine("                    СРАВНИТЕЛЬНЫЙ ОТЧЕТ РАСЧЕТОВ                       ");
        Console.WriteLine("=======================================================================");
        Console.WriteLine($"| {"Параметр",-28} | {"Flat",16} | {"Progressive",16} |");
        Console.WriteLine("|------------------------------|------------------|------------------|");
        Console.WriteLine($"| {"Количество",-28} | {flatResult.Quantity,16} | {progressiveResult.Quantity,16} |");
        Console.WriteLine($"| {"Базовая цена",-28} | {flatResult.BasePrice,16:C2} | {progressiveResult.BasePrice,16:C2} |");
        Console.WriteLine($"| {"Сумма без скидки",-28} | {flatResult.Subtotal,16:C2} | {progressiveResult.Subtotal,16:C2} |");
        Console.WriteLine($"| {"Ставка скидки",-28} | {flatResult.DiscountRate,15:P2} | {progressiveResult.DiscountRate,15:P2} |");
        Console.WriteLine($"| {"Сумма скидки",-28} | {flatResult.DiscountAmount,16:C2} | {progressiveResult.DiscountAmount,16:C2} |");
        Console.WriteLine($"| {"ИТОГО",-28} | {flatResult.TotalPrice,16:C2} | {progressiveResult.TotalPrice,16:C2} |");
        Console.WriteLine($"| {"Эфф. цена/шт",-28} | {flatResult.EffectiveUnitPrice,16:C2} | {progressiveResult.EffectiveUnitPrice,16:C2} |");
        Console.WriteLine("=======================================================================");
    }

    public static void PrintWholesaleAnomalyReport(int additionalQuantity, decimal savedMoney)
    {
        Console.WriteLine($"Внимание! Обнаружена аномалия оптовика. \n Приобретите еще {additionalQuantity}, чтобы сэкономить {savedMoney:C2}");
    }
}