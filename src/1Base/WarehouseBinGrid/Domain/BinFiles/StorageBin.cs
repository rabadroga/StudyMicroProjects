namespace BeginCsh.WarehouseBinGrid;

internal class StorageBin
{
    internal BinCoordinate Coordinate { get; }
    internal BinStatus Status { get; private set; }
    internal int MaxQuantity { get; }

    //public string Sku;
    //public int CurrentQuantity;

    internal StorageBin(BinCoordinate coordinate, int maxQuantity, BinStatus status = BinStatus.Empty)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxQuantity);

        Coordinate = coordinate != default ? coordinate : throw new ArgumentException("Пустые координаты", nameof(coordinate)); //это же не избыточно?

        MaxQuantity = maxQuantity;
    }
}