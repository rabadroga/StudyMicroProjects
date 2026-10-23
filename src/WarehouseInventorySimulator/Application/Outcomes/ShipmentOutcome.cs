namespace BeginCsh.WarehouseInventorySimulator;

public abstract record ShipmentOutcome
{
    private ShipmentOutcome(){}

    public sealed record Success(OperationLog Log) : ShipmentOutcome;
    public sealed record ItemNotFound(string Sku) : ShipmentOutcome; 
    public sealed record InsufficientStock(int Amount, int RemainingStock) : ShipmentOutcome;   
}