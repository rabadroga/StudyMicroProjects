using System.Diagnostics;

namespace BeginCsh.WarehouseBinGrid;

public static class SimpleGridRenderer
{
    public static void RenderBinGrid(IEnumerable<ShelfRow> row, RackStatistic statistic)
    {
        foreach (var shelf in row)
        {
            Console.WriteLine($"Полка {shelf.Shelf}: ");

            foreach (var bin in shelf.Bins)
            {
                Console.Write($"Ячейка: {bin.Coordinate}: ");

                var (color, content) = bin.Content switch
                {
                    BinContent.Empty => (ConsoleColor.Green, "Free"),

                    BinContent.Occupied occupied => (ConsoleColor.Yellow, $"[SKU: '{occupied.Sku}'x{occupied.Quantity}]"),

                    BinContent.Blocked blocked => (ConsoleColor.Red, $"{blocked.Reason}"),

                    _ => throw new UnreachableException()
                };

                Console.ForegroundColor = color;
                Console.Write(content);
                Console.ResetColor();

                Console.Write(" | ");
            }
            Console.WriteLine();
        }

        Console.WriteLine($"""
        ╔══════════════════════════════════════════════╗
        ║ СТАТИСТИКА СТЕЛЛАЖА                          ║
        ╠══════════════════════════════════════════════╣
        ║ Всего ячеек:    {statistic.BinQuantity,-6} Заполненность: {statistic.OccupiedPercentage,6:P1} ║
        ║ Свободно:       {statistic.EmptyBins,-6} Занято:        {statistic.OccupiedBins,-6} ║
        ║ Заблокировано:  {statistic.BlockedBins,-6}                       ║
        ╚══════════════════════════════════════════════╝
        """);

    }
}