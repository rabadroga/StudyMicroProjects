namespace BeginCsh.WarehouseBinGrid;

public readonly record struct ShelfRow(char Shelf, IReadOnlyList<StorageBin> Bins); //небольшой рекорд для сортировки и группировки ячеек

public class Rack
{
    int MaxQuantity { get; }

    private readonly Dictionary<BinCoordinate, StorageBin> _rackStructure;
    public IReadOnlyDictionary<BinCoordinate, StorageBin> RackStructure => _rackStructure;

    public Rack(int maxQuantity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxQuantity);

        MaxQuantity = maxQuantity;

        char[] shelves = ['A', 'B', 'C', 'D'];

        _rackStructure = shelves
                        .SelectMany(shelf => Enumerable.Range(1, 6), (shelf, cell) => new BinCoordinate(shelf, cell))
                        .ToDictionary(coord => coord, coord => new StorageBin(coord, maxQuantity)); //мб стоило реализовать ввод произвольного количестваа полок и ячеек, но как будто это лишнее
    }

    private StorageBin? FindBinForDeposit(string sku, int amount)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sku);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        var occupiedBin = _rackStructure.Values.FirstOrDefault(bin => bin.Content is BinContent.Occupied occ && occ.Sku == sku && bin.MaxQuantity >= (occ.Quantity + amount));
        if (occupiedBin is not null)
        {
            return occupiedBin;
        }

        var emptyBin = _rackStructure.Values.FirstOrDefault(bin => bin.Content is BinContent.Empty);
        if (emptyBin is not null)
        {
            return emptyBin;
        }

        return null;
    }

    public DepositOutcome AutoDeposit(string sku, int amount)
    {
        var bin = FindBinForDeposit(sku, amount);
        if (bin is not null)
        {
            return bin.Deposit(sku, amount);
        }

        return new DepositOutcome.AutoDepositFailed(sku);
    }

    public DepositOutcome Deposit(BinCoordinate coordinate, string sku, int amount)
    {
        if (!_rackStructure.TryGetValue(coordinate, out var bin))
        {
            return new DepositOutcome.CellNotFound(coordinate);
        }

        var outcome = bin.Deposit(sku, amount);
        return outcome;
    }

    public WithdrawOutcome Withdraw(BinCoordinate coordinate, string sku, int amount) //возможно еще стоило сделать списание по артикулу, а не только по корде
    {
        if (!_rackStructure.TryGetValue(coordinate, out var bin))
        {
            return new WithdrawOutcome.CellNotFound(coordinate);
        }

        var outcome = bin.Withdraw(sku, amount);
        return outcome;
    }

    public BlockOutcome Block(BinCoordinate coordinate, string reason)
    {
        if (!_rackStructure.TryGetValue(coordinate, out var bin))
        {
            return new BlockOutcome.CellNotFound(coordinate);
        }

        var outcome = bin.Block(reason);
        return outcome;
    }

    public UnblockOutcome Unblock(BinCoordinate coordinate)
    {
        if (!_rackStructure.TryGetValue(coordinate, out var bin))
        {
            return new UnblockOutcome.CellNotFound(coordinate);
        }

        var outcome = bin.Unblock();
        return outcome;
    }

    //может стоило оставить все базовые операции с ячейками здесь, а уже группировку, нахождение пустых и тд -- в другом классе?
    public IEnumerable<StorageBin> GetEmptyBinsByShelf(char shelf) => //а здесь же если будет пустое, то я обработаю в консоли вывода просто? Типо, если эмпти, то будет вывод, что нет такого
        _rackStructure.Values
            .Where(bin => bin.Coordinate.Shelf == shelf
                        && bin.Content is BinContent.Empty);

    public IEnumerable<StorageBin> FindBinsBySku(string sku)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sku);

        return _rackStructure.Values
            .Where(bin => bin.Content is BinContent.Occupied occBin 
                && occBin.Sku == sku);
    }

    public RackStatistic GetRackStatistic()
    {
        var counts = 
            _rackStructure.Values
                .GroupBy(group => group.Content.GetType())
                    .ToDictionary(group => group.Key, group => group.Count());

        int empty = counts.GetValueOrDefault(typeof(BinContent.Empty), 0);
        int occupied = counts.GetValueOrDefault(typeof(BinContent.Occupied), 0);
        int blocked = counts.GetValueOrDefault(typeof(BinContent.Blocked), 0); //Кстати, получается эта сказка .GetType и typeof позволяют бесшовно и без оверхеда (если это понятие тут применимо) сделать из рекорда или ДУ -- енум?

        return new RackStatistic
            (
                BinQuantity: _rackStructure.Count,
                EmptyBins: empty,
                OccupiedBins: occupied,
                BlockedBins: blocked
            );
    }

    public IEnumerable<ShelfRow> GetBinsGroupedAndSorted() => 
        _rackStructure.Values
            .GroupBy(group => group.Coordinate.Shelf)
            .OrderByDescending(group => group.Key)
            .Select(group => new ShelfRow (group.Key, group.OrderBy(bin => bin.Coordinate.Cell).ToList())); //для многочисленных ячеек может и перегруженно, но для моего случая -- оптимальный вариант
}