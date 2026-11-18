namespace BeginCsh.Tests.WarehouseBinGrid;

using BeginCsh.WarehouseBinGrid;

public class RastTest
{
    private static Rack CreateBasicRack() =>
        new Rack(50);

    [Fact]
    public void Rack_ValidCreation_CreateCorrectBinGrinInDictionary()
    {
        int maxQuantity = 50;

        var rack = new Rack(maxQuantity);
        var bins =
            from shelf in new[] { 'A', 'B', 'C', 'D' }
            from cell in Enumerable.Range(1, 6)
            select new BinCoordinate(shelf, cell);

        Assert.Equal(24, rack.RackStructure.Count);
        Assert.True(bins.All(bin => rack.RackStructure.ContainsKey(bin)));
        Assert.All(rack.RackStructure.Values, bin =>
        {
            Assert.Equal(maxQuantity, bin.MaxQuantity);
            Assert.IsType<BinContent.Empty>(bin.Content);
        });
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Rack_NegativeOrZeroMaxQuantity_ThrowException(int maxQuantity)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Rack(maxQuantity));
    }

    [Fact]
    public void AutoDeposit_InEmptyBin_ReturnSuccessOutcomeAndSingleOccupiedBin()
    {
        var rack = CreateBasicRack();
        string sku = "handle";
        int amount = 10;

        var outcome = rack.AutoDeposit(sku, amount);

        Assert.IsType<DepositOutcome.Success>(outcome);
        var currentBin = Assert.Single(rack.RackStructure.Values, bin => bin.Content is BinContent.Occupied);
        var currentContent = Assert.IsType<BinContent.Occupied>(currentBin.Content);
        Assert.Equal(sku, currentContent.Sku);
        Assert.Equal(amount, currentContent.Quantity);
    }

    [Fact]
    public void AutoDeposit_InOccupiedBin_ReturnSuccessOutcomeAndSingleOccupiedBin()
    {
        var rack = CreateBasicRack();
        string sku = "handle";
        int amount = 10;
        var targetCoord = new BinCoordinate('B', 4);
        rack.Deposit(targetCoord, sku, amount);

        var outcome = rack.AutoDeposit(sku, amount);

        Assert.IsType<DepositOutcome.Success>(outcome);
        var currentBin = Assert.Single(rack.RackStructure.Values, bin => bin.Content is BinContent.Occupied);
        Assert.Equal(targetCoord, currentBin.Coordinate);
        var currentContent = Assert.IsType<BinContent.Occupied>(currentBin.Content);
        Assert.Equal(amount * 2, currentContent.Quantity);
    }

    [Fact]
    public void AutoDeposit_InOccupiedBinBoundaryQuantity_ReturnSuccessOutcomeAndSingleOccupiedBin()
    {
        var rack = CreateBasicRack();
        string sku = "handle";
        int amount = 10;
        var targetCoord = new BinCoordinate('B', 4);
        rack.Deposit(targetCoord, sku, 40);

        var outcome = rack.AutoDeposit(sku, amount);

        Assert.IsType<DepositOutcome.Success>(outcome);
        rack.RackStructure.TryGetValue(targetCoord, out var bin);
        var currentContent = Assert.IsType<BinContent.Occupied>(bin?.Content);
        Assert.Equal(40 + amount, currentContent.Quantity);
    }

    [Fact]
    public void AutoDeposit_OccupiedButQuantityExcess_DepositInOtherEmptyBin()
    {
        var rack = CreateBasicRack();
        string sku = "handle";
        int amount = 10;
        var otherCoord = new BinCoordinate('B', 4);
        rack.Deposit(otherCoord, sku, 45);

        var outcome = rack.AutoDeposit(sku, amount);

        var successOccupied = Assert.IsType<DepositOutcome.Success>(outcome);
        Assert.Equal(amount, successOccupied.Content.Quantity);
        Assert.Equal(2, rack.RackStructure.Values.Count(bin => bin.Content is BinContent.Occupied));
        rack.RackStructure.TryGetValue(otherCoord, out var otherBin);
        var otherContent = Assert.IsType<BinContent.Occupied>(otherBin?.Content);
        Assert.Equal(45, otherContent.Quantity);
        Assert.Equal(otherContent.Sku, successOccupied.Content.Sku);
    }

    [Fact]
    public void AutoDeposit_AllBinsOccupiedWithOtherSku_ReturnAutoDepositFailed()
    {
        // Given
        var rack = CreateBasicRack();
        string sku = "handle";
        int amount = 10;
        string otherSku = "knob";

        foreach (var coord in rack.RackStructure.Keys)
        {
            rack.Deposit(coord, otherSku, amount);
        }
        // When
        var outcome = rack.AutoDeposit(sku, amount);

        // Then
        var failed = Assert.IsType<DepositOutcome.AutoDepositFailed>(outcome);
        Assert.Equal(sku, failed.Sku);
        Assert.All(rack.RackStructure.Values, bin =>
        {
            var otherContent = Assert.IsType<BinContent.Occupied>(bin.Content);
            Assert.Equal(otherSku, otherContent.Sku);
            Assert.Equal(amount, otherContent.Quantity);
        });
    }

    [Fact]
    public void AutoDeposit_AllBinsOccupiedAndQuantityExcess_ReturnAutoDepositFailed()
    {
        // Given
        var rack = CreateBasicRack();
        string sku = "handle";
        int amount = 10;

        foreach (var coord in rack.RackStructure.Keys)
        {
            rack.Deposit(coord, sku, 45);
        }
        // When
        var outcome = rack.AutoDeposit(sku, amount);

        // Then
        var failed = Assert.IsType<DepositOutcome.AutoDepositFailed>(outcome);
        Assert.Equal(sku, failed.Sku);
        Assert.All(rack.RackStructure.Values, bin =>
        {
            var otherContent = Assert.IsType<BinContent.Occupied>(bin.Content);
            Assert.Equal(failed.Sku, otherContent.Sku);
            Assert.Equal(45, otherContent.Quantity);
        });
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void AutoDeposit_InvalidSku_ThrowsArgumentException(string? invalidSku)
    {
        var rack = CreateBasicRack();

        Assert.ThrowsAny<ArgumentException>(() => rack.AutoDeposit(invalidSku!, 10));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void AutoDeposit_InvalidAmount_ThrowsArgumentOutOfRangeException(int invalidAmount)
    {
        var rack = CreateBasicRack();

        Assert.Throws<ArgumentOutOfRangeException>(() => rack.AutoDeposit("handle", invalidAmount));
    }

    [Fact]
    public void Deposit_ValidEmptyBinCoord_ReturnsSuccessDeposit()
    {
        // Given
        var rack = CreateBasicRack();
        string sku = "handle";
        int amount = 10;
        var coord = new BinCoordinate('B', 4);

        // When
        var outcome = rack.Deposit(coord, sku, amount);

        // Then
        var success = Assert.IsType<DepositOutcome.Success>(outcome);

        var currentBin = Assert.Single(rack.RackStructure.Values, bin => bin.Content is BinContent.Occupied);
        Assert.Equal(coord, currentBin.Coordinate);
        var currentContent = Assert.IsType<BinContent.Occupied>(currentBin.Content);
        Assert.Equal(success.Content.Quantity, currentContent.Quantity);
    }

    [Fact]
    public void Deposit_ValidOccupiedBinCoord_ReturnsSuccessDepositAndOneOccupiedBin()
    {
        // Given
        var rack = CreateBasicRack();
        string sku = "handle";
        int amount = 10;
        var coord = new BinCoordinate('B', 4);
        rack.Deposit(coord, sku, amount);

        // When
        var outcome = rack.Deposit(coord, sku, amount);

        // Then
        var success = Assert.IsType<DepositOutcome.Success>(outcome);
        Assert.Equal(2 * amount, success.Content.Quantity);

        var currentBin = Assert.Single(rack.RackStructure.Values, bin => bin.Content is BinContent.Occupied);
        Assert.Equal(coord, currentBin.Coordinate);
        var currentContent = Assert.IsType<BinContent.Occupied>(currentBin.Content);
        Assert.Equal(success.Content.Quantity, currentContent.Quantity);
    }

    [Theory]
    [InlineData('E', 1)]
    [InlineData('A', 7)]
    public void Deposit_NotExistentBin_ReturnsOutcomeCellNotFound(char shelf, int cell)
    {
        var rack = CreateBasicRack();
        var coord = new BinCoordinate(shelf, cell);

        var outcome = rack.Deposit(coord, "handle", 10);

        var notFound = Assert.IsType<DepositOutcome.CellNotFound>(outcome);
        Assert.Equal(coord, notFound.Coordinate);
        Assert.DoesNotContain(rack.RackStructure.Values, bin => bin.Content is BinContent.Occupied);
    }

    [Fact]
    public void Withdraw_ValidOccupiedBinCoord_ReturnsSuccessWithdrawAndOneOccupiedBin()
    {
        // Given
        var rack = CreateBasicRack();
        string sku = "handle";
        int amount = 10;
        var coord = new BinCoordinate('B', 4);
        rack.Deposit(coord, sku, 20);

        // When
        var outcome = rack.Withdraw(coord, sku, amount);

        // Then
        var success = Assert.IsType<WithdrawOutcome.Success>(outcome);
        Assert.Equal(20 - amount, success.Content.Quantity);

        var currentBin = Assert.Single(rack.RackStructure.Values, bin => bin.Content is BinContent.Occupied);
        Assert.Equal(coord, currentBin.Coordinate);
        var currentContent = Assert.IsType<BinContent.Occupied>(currentBin.Content);
        Assert.Equal(success.Content.Quantity, currentContent.Quantity);
    }

    [Theory]
    [InlineData('E', 1)]
    [InlineData('A', 7)]
    public void Withdraw_NotExistentBin_ReturnsOutcomeCellNotFound(char shelf, int cell)
    {
        var rack = CreateBasicRack();
        var invalidCoord = new BinCoordinate(shelf, cell);
        var validCoord = new BinCoordinate('B', 4);
        string sku = "handle";
        rack.Deposit(validCoord, sku, 20);

        var outcome = rack.Withdraw(invalidCoord, sku, 10);

        var notFound = Assert.IsType<WithdrawOutcome.CellNotFound>(outcome);
        Assert.Equal(invalidCoord, notFound.Coordinate);
        var bin = Assert.Single(rack.RackStructure.Values, bin => bin.Content is BinContent.Occupied);
        Assert.Equal(validCoord, bin.Coordinate);
        var currentContent = Assert.IsType<BinContent.Occupied>(bin.Content);
        Assert.Equal(20, currentContent.Quantity);
    }

    [Fact]
    public void Block_ValidBinCoord_ReturnsSuccessOutcomeBlock()
    {
        // Given
        var rack = CreateBasicRack();
        var coord = new BinCoordinate('B', 4);

        // When
        var outcome = rack.Block(coord, "test");

        // Then
        var success = Assert.IsType<BlockOutcome.SuccessEmpty>(outcome);

        var currentBin = Assert.Single(rack.RackStructure.Values, bin => bin.Content is BinContent.Blocked);
        Assert.Equal(coord, currentBin.Coordinate);
    }

    [Theory]
    [InlineData('E', 1)]
    [InlineData('A', 7)]
    public void Block_NotExistentBin_ReturnsOutcomeCellNotFound(char shelf, int cell)
    {
        var rack = CreateBasicRack();
        var coord = new BinCoordinate(shelf, cell);

        var outcome = rack.Block(coord, "test");

        var notFound = Assert.IsType<BlockOutcome.CellNotFound>(outcome);
        Assert.Equal(coord, notFound.Coordinate);
        Assert.DoesNotContain(rack.RackStructure.Values, bin => bin.Content is BinContent.Blocked);
    }

    [Fact]
    public void Unblock_ValidBinCoord_ReturnsSuccessOutcomeBlock()
    {
        // Given
        var rack = CreateBasicRack();
        var coord = new BinCoordinate('B', 4);
        rack.Block(coord, "test");

        // When
        var outcome = rack.Unblock(coord);

        // Then
        Assert.IsType<UnblockOutcome.SuccessEmpty>(outcome);
        Assert.DoesNotContain(rack.RackStructure.Values, bin => bin.Content is BinContent.Blocked);
    }

    [Theory]
    [InlineData('E', 1)]
    [InlineData('A', 7)]
    public void Unblock_NotExistentBin_ReturnsOutcomeCellNotFound(char shelf, int cell)
    {
        var rack = CreateBasicRack();
        var invalidCoord = new BinCoordinate(shelf, cell);
        var validCoord = new BinCoordinate('B', 4);
        rack.Block(validCoord, "test");

        var outcome = rack.Unblock(invalidCoord);

        var notFound = Assert.IsType<UnblockOutcome.CellNotFound>(outcome);
        Assert.Equal(invalidCoord, notFound.Coordinate);
        Assert.Single(rack.RackStructure.Values, bin => bin.Content is BinContent.Blocked);
    }

    [Fact]
    public void GetEmptyBinsByShelf_FewEmptyBins_ReturnOnlyEmptyBinsInEnumerable()
    {
        // Given
        var rack = CreateBasicRack();
        var coord1 = new BinCoordinate('B', 4);
        var coord2 = new BinCoordinate('B', 6);
        var otherCoord = new BinCoordinate('A', 1);
        rack.Deposit(coord1, "handle", 10);
        rack.Deposit(coord2, "handle", 10);
        rack.Deposit(otherCoord, "handle", 10);
    
        // When
        var emptyBinsCollection = rack.GetEmptyBinsByShelf('B').ToList();
    
        // Then
        Assert.Equal(4, emptyBinsCollection.Count);
        Assert.All(emptyBinsCollection, bin =>
        {
            Assert.Equal('B', bin.Coordinate.Shelf);
            Assert.NotEqual(coord1, bin.Coordinate);
            Assert.NotEqual(coord2, bin.Coordinate);
        });
    }

    [Fact]
    public void GetEmptyBinsByShelf_NoEmptyBins_ReturnEmptyCollection()
    {
        // Given
        var rack = CreateBasicRack();
        foreach(var bin in rack.RackStructure.Keys.Where(bin => bin.Shelf == 'B'))
        {
                rack.Deposit(bin, "handle", 10);
        }
    
        // When
        var emptyBinsCollection = rack.GetEmptyBinsByShelf('B');
    
        // Then
        Assert.Empty(emptyBinsCollection);
    }

    [Fact]
    public void GetEmptyBinsByShelf_NotExistedShelf_ReturnEmptyCollection()
    {
        // Given
        var rack = CreateBasicRack();
    
        // When
        var emptyBinsCollection = rack.GetEmptyBinsByShelf('Z');
    
        // Then
        Assert.Empty(emptyBinsCollection);
    }

    [Fact]
    public void FindBinsBySku_FewTargetSkuAndOtherSku_ReturnCollectionOnlyWithTargetSku()
    {
        // Given
        var rack = CreateBasicRack();

        var coord1 = new BinCoordinate('B', 4);
        var coord2 = new BinCoordinate('C', 6);
        var otherCoord = new BinCoordinate('A', 1);

        string targetSku = "handle";
        string otherSku = "knob";

        rack.Deposit(coord1, targetSku, 10);
        rack.Deposit(coord2, targetSku, 10);
        rack.Deposit(otherCoord, otherSku, 10);
    
        // When
        var binsBySkuCollection = rack.FindBinsBySku(targetSku).ToList();
    
        // Then
        Assert.Equal(2, binsBySkuCollection.Count);
        Assert.All(binsBySkuCollection, bin =>
        {
            var occupied = Assert.IsType<BinContent.Occupied>(bin.Content);
            Assert.Equal(targetSku, occupied.Sku);
        });
    }

    [Fact]
    public void FindBinsBySku_NoBinsWithTargetSku_ReturnEmptyCollection()
    {
        // Given
        var rack = CreateBasicRack();
    
        // When
        var binsBySkuCollection = rack.FindBinsBySku("handle");
    
        // Then
        Assert.Empty(binsBySkuCollection);
    }

    [Theory]
    [InlineData(" ")]
    [InlineData("    ")]
    [InlineData(null)]
    public void FindBinsBySku_NullOrWhiteSpaceSku_ThrowsException(string? sku)
    {
        // Given
        var rack = CreateBasicRack();
    
        // When & Then
        Assert.ThrowsAny<ArgumentException>(() => rack.FindBinsBySku(sku!));
    }

    [Fact]
    public void GetRackStatistic_SeveralBinsWithAnyStates_PreciseRackStatistic()
    {
        // Given
        var rack = CreateBasicRack();

        rack.Deposit(new('B', 4), "handle", 10);
        rack.Deposit(new('C', 2), "handle", 10);
        rack.Deposit(new('D', 5), "handle", 10);

        rack.Block(new('B', 4), "test");
        rack.Block(new('A', 1), "test");
    
        // When
        var statistic = rack.GetRackStatistic();

        var expectedStatistic = new RackStatistic(24, 20, 2, 2);
    
        // Then
        Assert.Equal(expectedStatistic, statistic);
    }

    [Fact]
    public void GetBinsGroupedAndSorted_BasicRackStructure_CorrectSort()
    {
        // Given
        var rack = CreateBasicRack();
    
        // When
        var row = rack.GetBinsGroupedAndSorted().ToList();

        char[] shelves = ['D', 'C', 'B', 'A'];
        int[] cells = [1, 2, 3, 4, 5, 6];
    
        // Then
        Assert.Equal(shelves, row.Select(r => r.Shelf));
        Assert.All(row, r =>
        {
            Assert.All(r.Bins, bin => Assert.Equal(bin.Coordinate.Shelf, r.Shelf));
            Assert.Equal(cells, r.Bins.Select(b => b.Coordinate.Cell));
        });
    }
    
    //потом переход к консоли
}