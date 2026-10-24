namespace BeginCsh.WarehouseInventorySimulator;

public class Warehouse
{
    private readonly Dictionary<string, InventoryItem> _warehouseInventory = new(StringComparer.OrdinalIgnoreCase);
    public IReadOnlyDictionary<string, InventoryItem> WarehouseInventory => _warehouseInventory;

    private readonly List<OperationLog> _logs = new();
    public IReadOnlyList<OperationLog> Logs => _logs;

    public CreationOutcome CreateItem(string sku, int quantity, int warningThreshold)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sku);
        ArgumentOutOfRangeException.ThrowIfNegative(quantity);
        ArgumentOutOfRangeException.ThrowIfNegative(warningThreshold);

        DateTimeOffset dateTime = DateTimeOffset.UtcNow;
        
        if (_warehouseInventory.ContainsKey(sku))
        {
            return new CreationOutcome.AlreadyExists(sku);
        }

        var item = new InventoryItem(sku, quantity, warningThreshold);
        _warehouseInventory.Add(sku, item);

        var log = new OperationLog
        (
            sku,
            OperationType.Creation,
            quantity,
            quantity,
            item.IsLowStock,
            dateTime
        );
        _logs.Add(log);

        return new CreationOutcome.Success(log);
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