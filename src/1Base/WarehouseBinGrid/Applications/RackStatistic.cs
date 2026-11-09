namespace BeginCsh.WarehouseBinGrid;

public sealed record RackStatistic
(
    int BinQuantity,
    int EmptyBins,
    int OccupiedBins,
    int BlockedBins
)
{
    float OccupiedPercentage => BinQuantity == 0 ? 0f : (float)OccupiedBins / BinQuantity;
};