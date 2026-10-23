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

    public void Ship(int amount) //с одной стороны -- понятно, почему паблик метод, другой класс должен иметь к нему доступ, но с другой -- по сути, только один класс и должен иметь этот доступ. Тут в идеале должно быть что-то вроде наследования?
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
        
        if (Quantity < amount)
        {
            throw new InvalidOperationException("Недостаточное количество для списания!");
        }

        Quantity -= amount;
    }

    public void Restock(int amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
        
        Quantity += amount;
    }
}