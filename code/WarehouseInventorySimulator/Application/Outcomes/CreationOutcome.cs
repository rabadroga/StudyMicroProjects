namespace BeginCsh.WarehouseInventorySimulator;

public abstract record CreationOutcome
{
    private CreationOutcome(){}

    public sealed record AlreadyExists(string Sku) : CreationOutcome;
    public sealed record Success(OperationLog Log) : CreationOutcome;
}