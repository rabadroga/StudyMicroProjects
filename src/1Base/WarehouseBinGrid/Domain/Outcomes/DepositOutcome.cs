namespace BeginCsh.WarehouseBinGrid;

public abstract record DepositOutcome
{
    private DepositOutcome(){}

    public record Success(BinContent.Occupied Content) : DepositOutcome;

    public record QuantityExcess(string Sku, int MaxQuantity, int RequestedAmount, int CurrentQuantity) : DepositOutcome;

    public record OccupiedWithOtherSku(string InputSku, BinContent.Occupied Occupied) : DepositOutcome;

    public record Blocked(string Sku, BinContent.Blocked BlockedContent) : DepositOutcome;

    public record CellNotFound(BinCoordinate Coordinate) : DepositOutcome;

    public record AutoDepositFailed(string sku) : DepositOutcome;
}