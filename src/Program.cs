using BeginCsh.WarehouseInventorySimulator;
static class Program
{
    static void Main()
    {
        var warehouse = new Warehouse();
        var menu = new ConsoleMenu(warehouse);
        menu.Run();
    }
}