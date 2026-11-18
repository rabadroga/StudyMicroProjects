namespace BeginCsh.WarehouseBinGrid;

public abstract record WithdrawOutcome
{
    private WithdrawOutcome(){}

    public sealed record Success(BinContent.Occupied Content) : WithdrawOutcome;

    public sealed record SuccessEmpty(string Sku, int Amount) : WithdrawOutcome;

    public sealed record InsufficientStock(BinContent.Occupied OccupiedContent, int Amount) : WithdrawOutcome;

    public sealed record ItemNotFound(string Sku) : WithdrawOutcome;

    public sealed record Empty(string Sku) : WithdrawOutcome;

    public sealed record Blocked(BinContent.Blocked BlockedContent) : WithdrawOutcome;

    public sealed record CellNotFound(BinCoordinate Coordinate) : WithdrawOutcome;
}