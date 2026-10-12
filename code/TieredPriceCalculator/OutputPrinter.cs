namespace BeginCsh.TieredPriceCalculator;

public static class OutputPrinter
{
    public static void PrintComparisonReport(PricingResult flat, PricingResult progressive)
    {
        Console.WriteLine("=======================================================================");
        Console.WriteLine("                    СРАВНИТЕЛЬНЫЙ ОТЧЕТ РАСЧЕТОВ                       ");
        Console.WriteLine("=======================================================================");
        Console.WriteLine($"| {"Параметр",-28} | {"Flat (Фикс.)",16} | {"Progressive",16} |");
        Console.WriteLine("|------------------------------|------------------|------------------|");
        Console.WriteLine($"| {"Количество (Quantity)",-28} | {flat.Quantity,16} | {progressive.Quantity,16} |");
        Console.WriteLine($"| {"Базовая цена (BasePrice)",-28} | {flat.BasePrice,16:C2} | {progressive.BasePrice,16:C2} |");
        Console.WriteLine($"| {"Сумма без скидки (Subtotal)",-28} | {flat.Subtotal,16:C2} | {progressive.Subtotal,16:C2} |");
        Console.WriteLine($"| {"Ставка скидки (DiscountRate)",-28} | {flat.DiscountRate,15:P2} | {progressive.DiscountRate,15:P2} |");
        Console.WriteLine($"| {"Сумма скидки (DiscountAmount)",-28} | {flat.DiscountAmount,16:C2} | {progressive.DiscountAmount,16:C2} |");
        Console.WriteLine($"| {"ИТОГО (TotalPrice)",-28} | {flat.TotalPrice,16:C2} | {progressive.TotalPrice,16:C2} |");
        Console.WriteLine($"| {"Эфф. цена/шт (EffectiveUnit)",-28} | {flat.EffectiveUnitPrice,16:C4} | {progressive.EffectiveUnitPrice,16:C4} |");
        Console.WriteLine("=======================================================================");
    }

    public static void PrintWholesaleAnomalyReport(int plusQuantity, decimal savedMoney)
    {
        Console.WriteLine($"Внимание! Обнаружена аномалия оптовика. \n Приобретите еще {plusQuantity}, чтобы сэкономить {savedMoney}");
    }
}