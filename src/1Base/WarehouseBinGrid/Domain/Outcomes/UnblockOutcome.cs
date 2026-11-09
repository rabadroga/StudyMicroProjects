namespace BeginCsh.WarehouseBinGrid;

public abstract record UnblockOutcome
{
    private UnblockOutcome(){}

    public record SuccessOccupied(BinContent.Occupied Content) : UnblockOutcome;

    public record SuccessEmpty() : UnblockOutcome;

    public record NotBlocked() : UnblockOutcome;

    public record CellNotFound(BinCoordinate Coordinate) : UnblockOutcome;
}