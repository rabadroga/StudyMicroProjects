namespace BeginCsh.WarehouseBinGrid;

public readonly record struct BinCoordinate
(
    char Shelf, 
    int Cell
)
{
    public override string ToString() => $"{Shelf}-{Cell:D2}";
}