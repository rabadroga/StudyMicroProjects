namespace BeginCsh.WarehouseInventorySimulator;

public class Warehouse
{
    private readonly Dictionary<string, InventoryItem> _warehouseInventory = new();
    public IReadOnlyDictionary<string, InventoryItem> WarehouseInventory => _warehouseInventory;

    private readonly List<WarehouseOperationLog> _logs = new();
    public List<WarehouseOperationLog> Logs => _logs;

    /*
    надо будет потом все реализовать в отдельном классе интпута -- ввод нового товара, операций с ним
    потом класс печати отчета смена и мб списка товаров и тд

    А также реализовать консоль юи в плане небольшого меню, как в тг ботах --
    у чела выходят сообщения, он нажимает нужные символы, чтобы выполнять команды
    типо:
    идет сначала пополнение, он вводит, потом может добавить еще и выйти из режима пополнения (скорее всего, это будет цикл в программ)
    потом идет просто смена, он нажимает ввод, вывод товара
    и отдельная кнопка завершить смену
    */
    public InventoryItem CreateItem(string sku, int quantity, int warningThreshold)
    {
        var item = new InventoryItem(sku, quantity, warningThreshold);

        if (!_warehouseInventory.TryAdd(sku, item))
        {
            throw new InvalidOperationException($"Артикул {sku} уже существует!");
        }

        return item;
    }

    public void ShipItem(string sku, int amount)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sku);

        if (!_warehouseInventory.TryGetValue(sku, out var item))
        {
            throw new KeyNotFoundException("Артикул не найден");
        }

        DateTimeOffset dateTime = DateTimeOffset.UtcNow;

        item.Ship(amount);

        var log = new WarehouseOperationLog
        (
            item.Sku,
            OperationType.Shipment,
            amount,
            item.Quantity,
            item.IsLowStock,
            dateTime
        );
        _logs.Add(log);
    }

    public void RestockItem(string sku, int amount)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sku);

        if (!_warehouseInventory.TryGetValue(sku, out var item))
        {
            throw new KeyNotFoundException("Артикул не найден");
        }

        DateTimeOffset dateTime = DateTimeOffset.UtcNow;

        item.Restock(amount);

        var log = new WarehouseOperationLog
        (
            item.Sku,
            OperationType.Restock,
            amount,
            item.Quantity,
            item.IsLowStock,
            dateTime
        );
    }
}