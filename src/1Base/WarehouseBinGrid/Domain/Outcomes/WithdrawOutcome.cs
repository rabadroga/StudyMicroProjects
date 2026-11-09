namespace BeginCsh.WarehouseBinGrid;

public abstract record WithdrawOutcome
{
    private WithdrawOutcome(){}

    public record Success(BinContent.Occupied Content) : WithdrawOutcome;

    public record SuccessEmpty(string Sku, int amount) : WithdrawOutcome;

    public record InsufficientStock(BinContent.Occupied OccupiedContent, int Amount) : WithdrawOutcome;

    public record ItemNotFound(string Sku) : WithdrawOutcome;

    public record Empty(string Sku) : WithdrawOutcome;

    public record Blocked(BinContent.Blocked BlockedContent) : WithdrawOutcome;

    public record CellNotFound(BinCoordinate Coordinate) : WithdrawOutcome;
}