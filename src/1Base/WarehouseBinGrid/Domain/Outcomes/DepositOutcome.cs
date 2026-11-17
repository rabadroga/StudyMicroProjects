namespace BeginCsh.WarehouseBinGrid;

public abstract record DepositOutcome
{
    private DepositOutcome(){}

    public sealed record Success(BinContent.Occupied Content) : DepositOutcome;

    public sealed record QuantityExcess(string Sku, int MaxQuantity, int RequestedAmount, int CurrentQuantity) : DepositOutcome;

    public sealed record OccupiedWithOtherSku(string InputSku, BinContent.Occupied Occupied) : DepositOutcome;

    public sealed record Blocked(string Sku, BinContent.Blocked BlockedContent) : DepositOutcome;

    public sealed record CellNotFound(BinCoordinate Coordinate) : DepositOutcome;

    public sealed record AutoDepositFailed(string Sku) : DepositOutcome;
}