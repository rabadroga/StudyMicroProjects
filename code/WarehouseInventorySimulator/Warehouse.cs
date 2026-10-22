namespace BeginCsh.WarehouseInventorySimulator;

public class Warehouse
{
    private readonly Dictionary<string, InventoryItem> _warehouseInventory = new();
    public IReadOnlyDictionary<string, InventoryItem> WarehouseInventory => _warehouseInventory;

    private readonly List<OperationLog> _logs = new();
    public IReadOnlyList<OperationLog> Logs => _logs;

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
    public ItemCreationOutcome CreateItem(string sku, int quantity, int warningThreshold)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sku);
        ArgumentOutOfRangeException.ThrowIfNegative(quantity);
        ArgumentOutOfRangeException.ThrowIfNegative(warningThreshold);

        if (_warehouseInventory.ContainsKey(sku))
        {
            return new ItemCreationOutcome.AlreadyExists(sku);
        }

        var item = new InventoryItem(sku, quantity, warningThreshold);
        _warehouseInventory.Add(sku, item);

        return new ItemCreationOutcome.Success(item);
    }

    public ShipmentOutcome ShipItem(string sku, int amount)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sku);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        if (!_warehouseInventory.TryGetValue(sku, out var item))
        {
            return new ShipmentOutcome.ItemNotFound(sku);
        }

        if (amount > item.Quantity)
        {
            return new ShipmentOutcome.InsufficientStock(amount, item.Quantity);
        }
        
        DateTimeOffset dateTime = DateTimeOffset.UtcNow;
        item.Ship(amount);

        var log = new OperationLog
        (
            item.Sku,
            OperationType.Shipment,
            amount,
            item.Quantity,
            item.IsLowStock,
            dateTime
        );
        _logs.Add(log);

        return new ShipmentOutcome.Success(log);
        //не забыть про уведомление достижения порога предупреждения.
            //по идее, реализовать несложно, за счет активного булевого поля, только вот не уверен куда: по идее, это просто предупреждение и думаю, что его нужно ввести уже в программ через проверку
    }

    public RestockmentOutcome RestockItem(string sku, int amount)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sku);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        if (!_warehouseInventory.TryGetValue(sku, out var item))
        {
            return new RestockmentOutcome.ItemNotFound(sku);
        }

        DateTimeOffset dateTime = DateTimeOffset.UtcNow;
        item.Restock(amount);

        var log = new OperationLog
        (
            item.Sku,
            OperationType.Restock,
            amount,
            item.Quantity,
            item.IsLowStock,
            dateTime
        );
        _logs.Add(log);

        return new RestockmentOutcome.Success(log);
    }
}