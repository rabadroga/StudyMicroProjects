namespace BeginCsh.WarehouseInventorySimulator;

public class InventoryItem
{
    public string Sku { get; }
    public int WarningThreshold { get; } //возможно потом сделать priv set если нужно будет обновление порогов
    public int Quantity { get; private set; }

    public bool IsLowStock => Quantity <= WarningThreshold;

    public InventoryItem(string sku, int quantity, int warningThreshold)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sku);
        ArgumentOutOfRangeException.ThrowIfNegative(quantity);
        ArgumentOutOfRangeException.ThrowIfNegative(warningThreshold);

        Sku = sku;
        Quantity = quantity;
        WarningThreshold = warningThreshold;
    }

    internal void Ship(int amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
        
        if (Quantity < amount)
        {
            throw new InvalidOperationException("Недостаточное количество для списания!");
        }

        Quantity -= amount;
    }

    internal void Restock(int amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
        
        Quantity += amount;
    }
}