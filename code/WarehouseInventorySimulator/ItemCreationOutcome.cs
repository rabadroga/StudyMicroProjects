namespace BeginCsh.WarehouseInventorySimulator;

public abstract record ItemCreationOutcome
{
    private ItemCreationOutcome(){}

    public sealed record AlreadyExists(string sku) : ItemCreationOutcome;
    public sealed record Success(InventoryItem item) : ItemCreationOutcome;
}