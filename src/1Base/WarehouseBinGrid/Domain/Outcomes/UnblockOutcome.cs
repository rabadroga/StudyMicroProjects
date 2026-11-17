namespace BeginCsh.WarehouseBinGrid;

public abstract record UnblockOutcome
{
    private UnblockOutcome(){}

    public sealed record SuccessOccupied(BinContent.Occupied Content) : UnblockOutcome;

    public sealed record SuccessEmpty() : UnblockOutcome;

    public sealed record NotBlocked() : UnblockOutcome;

    public sealed record CellNotFound(BinCoordinate Coordinate) : UnblockOutcome;
}