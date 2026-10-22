namespace BeginCsh.WarehouseInventorySimulator;

public class InventoryItem
{
    public string Sku { get; }
    public int WarningThreshold { get; } //возможно потом сделать priv set если нужно будет обновление порогов
    public int Quantity { get; private set; }

    public bool IsLowStock => Quantity <= WarningThreshold; //я только щас догнал прикол лямбда выражений -- у него значение всегда плавающее и разное, поэтому оно так идеально для статуса

    public InventoryItem(string sku, int quantity, int warningThreshold)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sku);
        ArgumentOutOfRangeException.ThrowIfNegative(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(warningThreshold);

        Sku = sku;
        Quantity = quantity;
        WarningThreshold = warningThreshold;
    }

    public void Ship(int quantity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        
        if (Quantity < quantity)
        {
            throw new InvalidOperationException("Недостаточное количество для списания!");
        }

        Quantity -= quantity;
    }

    public void Restock(int quantity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        
        Quantity += quantity;
    }
}