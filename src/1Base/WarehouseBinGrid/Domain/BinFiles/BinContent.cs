namespace BeginCsh.WarehouseBinGrid;

public abstract record BinContent
{
    private BinContent(){}

    public sealed record Empty() : BinContent;

    public sealed record Occupied(string Sku, int Quantity) : BinContent;

    public sealed record Blocked(string Reason, Occupied? OccupiedContent = null) : BinContent;
}