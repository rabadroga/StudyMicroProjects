namespace BeginCsh.WarehouseInventorySimulator;

public abstract record RestockmentOutcome
{
    private RestockmentOutcome(){}

    public sealed record Success(OperationLog Log) : RestockmentOutcome;
    public sealed record ItemNotFound(string Sku) : RestockmentOutcome; 
}