using BeginCsh.WarehouseBinGrid;
static class Program
{
    static void Main()
    {
        //написал максимально упрощенный юи, чтобы не тратить лишнее время. Уже хотелось наконец-то закончить
        var rack = new Rack(maxQuantity: 100);

        Console.WriteLine("=== ДО ИЗМЕНЕНИЙ ===");
        SimpleGridRenderer.RenderBinGrid(rack.GetBinsGroupedAndSorted(), rack.GetRackStatistic());

        Console.WriteLine($"1. Deposit: {rack.Deposit(new BinCoordinate('A', 1), "SKU-101", 40)}");
        Console.WriteLine($"2. AutoDeposit: {rack.AutoDeposit("SKU-202", 25)}");
        Console.WriteLine($"3. Block: {rack.Block(new BinCoordinate('B', 2), "Сломан замок")}");
        Console.WriteLine($"4. Withdraw (ошибка): {rack.Withdraw(new BinCoordinate('B', 2), "SKU-101", 10)}");
        Console.WriteLine($"5. Withdraw (успех): {rack.Withdraw(new BinCoordinate('A', 1), "SKU-101", 15)}");

        Console.WriteLine("\n=== ПОСЛЕ ИЗМЕНЕНИЙ ===");
        SimpleGridRenderer.RenderBinGrid(rack.GetBinsGroupedAndSorted(), rack.GetRackStatistic());


    }
}