namespace BeginCsh.WarehouseBinGrid;

public abstract record BlockOutcome
{
    private BlockOutcome(){}

    public sealed record SuccessEmpty() : BlockOutcome;

    public sealed record SuccessOccupied(BinContent.Blocked BlockedContent) : BlockOutcome;

    public sealed record AlreadyBlocked() : BlockOutcome;

    public sealed record CellNotFound(BinCoordinate Coordinate) : BlockOutcome;
}