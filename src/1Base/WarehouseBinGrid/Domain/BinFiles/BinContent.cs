namespace BeginCsh.WarehouseBinGrid;

public abstract record BinContent
{
    private BinContent(){}

    public record Empty() : BinContent;

    public record Occupied(string Sku, int Quantity) : BinContent;

    public record Blocked(string Reason, Occupied? OccupiedContent = null) : BinContent;
}