namespace BeginCsh.WarehouseBinGrid;

public abstract record BlockOutcome
{
    private BlockOutcome(){}

    public record SuccessEmpty() : BlockOutcome;

    public record SuccessOccupied(BinContent Content) : BlockOutcome;

    public record AlreadyBlocked() : BlockOutcome;
}