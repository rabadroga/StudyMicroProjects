namespace BeginCsh.Tests.WarehouseBinGrid;

using BeginCsh.WarehouseBinGrid;
using Newtonsoft.Json.Converters;

public class StorageBinTest
{
    [Fact]
    public void StorageBin_ValidItemCreation_ValidValues()
    {
        var coordinate = new BinCoordinate('A', 1);
        int maxQuantity = 10;

        var bin = new StorageBin(coordinate, maxQuantity);

        Assert.Equal(coordinate, bin.Coordinate);
        Assert.Equal(maxQuantity, bin.MaxQuantity);
        Assert.IsType<BinContent.Empty>(bin.Content);
    }

    [Fact]
    public void StorageBin_DefaultBinCoordinate_ThrowExeption()
    {
        BinCoordinate coordinate = default;
        int maxQuantity = 10;

        var exeption = Assert.Throws<ArgumentException>(() => new StorageBin(coordinate, maxQuantity));

        Assert.Equal("coordinate", exeption.ParamName);
        Assert.Contains("Пустые координаты", exeption.Message);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void StorageBin_MaxQuantityIsNullOrNegative_ThrowOutOfRangeExeption(int maxQuantity)
    {
        var coordinate = new BinCoordinate('A', 1);

        Assert.Throws<ArgumentOutOfRangeException>(() => new StorageBin(coordinate, maxQuantity));
    }

    private StorageBin CreateBasicBin() => 
        new StorageBin(new BinCoordinate('A', 1), 50);

    [Fact]
    public void Deposit_ValidEmptyValues_ChangeContentToOccupiedAndReturnSuccessOutcome()
    {
        string sku = "handle";
        int amount = 10;
        var bin = CreateBasicBin();
    
        var outcome = bin.Deposit(sku, amount);
    
        var occupied = Assert.IsType<BinContent.Occupied>(bin.Content); //стоит ли как-то разделять для читаемости?
        Assert.Equal(amount, occupied.Quantity);
        Assert.Equal(sku, occupied.Sku);
        var success = Assert.IsType<DepositOutcome.Success>(outcome);
        Assert.Equal(bin.Content, success.Content);
    }

    [Fact]
    public void Deposit_ValidOccupiedValues_AddAmountToQuantityOfEqualSku()
    {
        string sku = "handle";
        int amount = 10;
        var bin = CreateBasicBin();
        bin.Deposit("handle", 10);

        var outcome = bin.Deposit(sku, amount);

        var occupied = Assert.IsType<BinContent.Occupied>(bin.Content);
        Assert.Equal(amount + 10, occupied.Quantity);
        Assert.Equal(sku, occupied.Sku);
        var success = Assert.IsType<DepositOutcome.Success>(outcome);
        Assert.Equal(bin.Content, success.Content);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(50)]
    public void Deposit_EmptyAmountBoundaryValues_SuccessOutcome(int amount)
    {
        var bin = CreateBasicBin();

        var outcome = bin.Deposit("handle", amount);

        var occupied = Assert.IsType<BinContent.Occupied>(bin.Content);
        Assert.Equal(amount, occupied.Quantity);
        var success = Assert.IsType<DepositOutcome.Success>(outcome);
        Assert.Equal(bin.Content, success.Content);
    }

    [Fact]
    public void Deposit_OccupiedAmountBoundaryValues_SuccessOutcome()
    {
        var bin = CreateBasicBin();
        string sku = "handle";
        bin.Deposit(sku, 30);

        var outcome = bin.Deposit(sku, 20);

        var occupied = Assert.IsType<BinContent.Occupied>(bin.Content);
        Assert.Equal(30 + 20, occupied.Quantity);
        var success = Assert.IsType<DepositOutcome.Success>(outcome);
        Assert.Equal(bin.Content, success.Content);
    }

    [Fact]
    public void Deposit_EmptyMaxQuantityExcess_ReturnOutcomeQuantityExcess()
    {
        string sku = "handle";
        int amount = 51;
        var bin = CreateBasicBin();

        var outcome = bin.Deposit(sku, amount);

        var excess = Assert.IsType<DepositOutcome.QuantityExcess>(outcome);
        Assert.Equal(sku, excess.Sku);
        Assert.Equal(bin.MaxQuantity, excess.MaxQuantity);
        Assert.Equal(amount, excess.RequestedAmount);
        Assert.Equal(0, excess.CurrentQuantity);
        Assert.IsType<BinContent.Empty>(bin.Content);
    }

    [Fact]
    public void Deposit_OccupiedMaxQuantityExcess_ReturnOutcomeQuantityExcess()
    {
        string sku = "handle";
        int amount = 50;
        var bin = CreateBasicBin();
        bin.Deposit(sku, 1);

        var outcome = bin.Deposit(sku, amount);

        var excess = Assert.IsType<DepositOutcome.QuantityExcess>(outcome);
        Assert.Equal(sku, excess.Sku);
        Assert.Equal(bin.MaxQuantity, excess.MaxQuantity);
        Assert.Equal(amount, excess.RequestedAmount);
        Assert.Equal(1, excess.CurrentQuantity);
        var occupied = Assert.IsType<BinContent.Occupied>(bin.Content);
        Assert.Equal(sku, occupied.Sku);
        Assert.Equal(1, occupied.Quantity);
    }

    [Fact]
    public void Deposit_OccupiedWithOtherSku_ReturnOutcomeOccupiedWithOtherSku()
    {
        string sku = "handle";
        string sku2 = "knob";
        int amount = 5;
        var bin = CreateBasicBin();
        bin.Deposit(sku, 10);

        var outcome = bin.Deposit(sku2, amount);

        var otherSku = Assert.IsType<DepositOutcome.OccupiedWithOtherSku>(outcome);
        Assert.Equal(sku2, otherSku.InputSku);
        Assert.Equal(sku, otherSku.Occupied.Sku);
        Assert.Equal(10, otherSku.Occupied.Quantity);
        var occupied = Assert.IsType<BinContent.Occupied>(bin.Content);
        Assert.Equal(sku, occupied.Sku);
        Assert.Equal(10, occupied.Quantity);
    }

    [Fact]
    public void Deposit_BlockedBin_ReturnOutcomeBlocked()
    {
        var bin = CreateBasicBin();
        bin.Block("test");

        var outcome = bin.Deposit("handle", 10);

        var blocked = Assert.IsType<DepositOutcome.Blocked>(outcome);
        Assert.Equal("handle", blocked.Sku);
        Assert.Equal(bin.Content, blocked.BlockedContent);
        var binBlocked = Assert.IsType<BinContent.Blocked>(bin.Content);
        Assert.Equal("test", binBlocked.Reason);
    }

    [Theory]
    [InlineData("   ")]
    [InlineData(null)]
    public void Deposit_SkuIsNullOrWhiteSpace_ThrowException(string? sku)
    {
        var bin = CreateBasicBin();
        
        Assert.ThrowsAny<ArgumentException>(() => bin.Deposit(sku!, 10));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Deposit_AmountIsNegativeOrZero_ThrowException(int amount)
    {
        var bin = CreateBasicBin();

        Assert.Throws<ArgumentOutOfRangeException>(() => bin.Deposit("handle", amount));
    }

    [Fact]
    public void Withdraw_ValidValues_ReduceQuantityAndReturnSuccessOutcome()
    {
        var bin = CreateBasicBin();
        string sku = "handle";
        bin.Deposit(sku, 10);

        var outcome = bin.Withdraw(sku, 5);
    
        var occupied = Assert.IsType<BinContent.Occupied>(bin.Content);
        Assert.Equal(10 - 5, occupied.Quantity);
        Assert.Equal(sku, occupied.Sku);
        var success = Assert.IsType<WithdrawOutcome.Success>(outcome);
        Assert.Equal(bin.Content, success.Content);
    }

    [Fact]
    public void Withdraw_ReduceToEmpty_ReturnSuccessOutcomeAndChangeContentToEmpty()
    {
        var bin = CreateBasicBin();
        string sku = "handle";
        int amount = 10;
        bin.Deposit(sku, 10);

        var outcome = bin.Withdraw(sku, amount);

        Assert.IsType<BinContent.Empty>(bin.Content);
        var success = Assert.IsType<WithdrawOutcome.SuccessEmpty>(outcome);
        Assert.Equal(sku, success.Sku);
        Assert.Equal(amount, success.Amount);
    }

    [Fact]
    public void Withdraw_IsufficientStock_ReturnOutcomeInsufficientStock()
    {
        var bin = CreateBasicBin();
        string sku = "handle";
        int amount = 11;
        bin.Deposit(sku, 10);

        var outcome = bin.Withdraw(sku, amount);

        var insufficient = Assert.IsType<WithdrawOutcome.InsufficientStock>(outcome);
        Assert.Equal(bin.Content, insufficient.OccupiedContent);
        Assert.Equal(amount, insufficient.Amount);
        var occupied = Assert.IsType<BinContent.Occupied>(bin.Content);
        Assert.Equal(10, occupied.Quantity); //только щас подумал -- может при депозите тоже создавать переменную, что обращаться к ней и сверять с ее значением amount? Или это будет оверхед для тестов?
    }

    [Fact]
    public void Withdraw_OccupiedItemNotFound_ReturnOutcomeItemNotFound()
    {
        var bin = CreateBasicBin();
        string sku = "handle";
        string sku2 = "knob";
        bin.Deposit(sku, 1);

        var outcome = bin.Withdraw(sku2, 10);

        var notFound = Assert.IsType<WithdrawOutcome.ItemNotFound>(outcome);
        Assert.Equal(sku2, notFound.Sku);
        var occupied = Assert.IsType<BinContent.Occupied>(bin.Content);
        Assert.Equal(1, occupied.Quantity);
        Assert.Equal(sku, occupied.Sku);
    }

    [Fact]
    public void Withdraw_EmptyBin_ReturnOutcomeEmpty()
    {
        var bin = CreateBasicBin();
        string sku = "handle";

        var outcome = bin.Withdraw(sku, 10);

        var empty = Assert.IsType<WithdrawOutcome.Empty>(outcome);
        Assert.Equal(sku, empty.Sku);
        Assert.IsType<BinContent.Empty>(bin.Content);
    }

    [Fact]
    public void Withdraw_BlockedBin_ReturnOutcomeBlocked()
    {
        var bin = CreateBasicBin();
        bin.Block("test");

        var outcome = bin.Withdraw("handle", 10);

        var blocked = Assert.IsType<WithdrawOutcome.Blocked>(outcome);
        Assert.Equal(bin.Content, blocked.BlockedContent);
        var binBlocked = Assert.IsType<BinContent.Blocked>(bin.Content);
        Assert.Equal("test", binBlocked.Reason);
    }

    [Theory]
    [InlineData("   ")]
    [InlineData(null)]
    public void Withdraw_SkuIsNullOrWhiteSpace_ThrowException(string? sku)
    {
        var bin = CreateBasicBin();
        
        Assert.ThrowsAny<ArgumentException>(() => bin.Withdraw(sku!, 10));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Withdraw_AmountIsNegativeOrZero_ThrowException(int amount)
    {
        var bin = CreateBasicBin();

        Assert.Throws<ArgumentOutOfRangeException>(() => bin.Withdraw("handle", amount));
    }

        [Fact]
    public void Block_ValidEmptyBin_ReturnOutcomeSuccessEmpty()
    {
        var bin = CreateBasicBin();
        string reason = "test";

        var outcome = bin.Block(reason);
    
        var blocked = Assert.IsType<BinContent.Blocked>(bin.Content);
        Assert.Equal(reason, blocked.Reason);
        Assert.Null(blocked.OccupiedContent);
        Assert.IsType<BlockOutcome.SuccessEmpty>(outcome);
    }

    [Fact]
    public void Block_ValidOccupiedin_ReturnOutcomeSuccessOccupied()
    {
        var bin = CreateBasicBin();
        bin.Deposit("handle", 10);
        var occupiedContent = bin.Content;
        string reason = "test";

        var outcome = bin.Block(reason);

        var blocked = Assert.IsType<BinContent.Blocked>(bin.Content);
        Assert.Equal(reason, blocked.Reason);
        Assert.Equal(occupiedContent, blocked.OccupiedContent);
        var success = Assert.IsType<BlockOutcome.SuccessOccupied>(outcome);
        Assert.Equal(bin.Content, success.Content);
    }

    [Fact]
    public void Block_BlockedBin_ReturnOutcomeAlreadyBlocked()
    {
        var bin = CreateBasicBin();
        string reason = "test";
        string notReason = "test2";
        bin.Block(reason);

        var outcome = bin.Block(notReason);

        var blocked = Assert.IsType<BinContent.Blocked>(bin.Content);
        Assert.Equal(reason, blocked.Reason);
        Assert.Null(blocked.OccupiedContent);
        Assert.IsType<BlockOutcome.AlreadyBlocked>(outcome);
    }

    [Theory]
    [InlineData("   ")]
    [InlineData(null)]
    public void Block_ReasonIsNullOrWhiteSpace_ThrowException(string? reason)
    {
        var bin = CreateBasicBin();
        
        Assert.ThrowsAny<ArgumentException>(() => bin.Block(reason!));
    }

        [Fact]
    public void Unblock_ValidBlockedOccupiedBin_ReturnOutcomeSuccessOccupied()
    {
        var bin = CreateBasicBin();
        string sku = "handle";
        int amount = 10;
        bin.Deposit(sku, amount);
        bin.Block("test");

        var outcome = bin.Unblock();

        var unblocked = Assert.IsType<BinContent.Occupied>(bin.Content);    
        Assert.Equal(sku, unblocked.Sku);
        Assert.Equal(amount, unblocked.Quantity);
        var success = Assert.IsType<UnblockOutcome.SuccessOccupied>(outcome);
        Assert.Equal(bin.Content, success.Content);
    }

    [Fact]
    public void Unblock_ValidBlockedEmptyBin_ReturnOutcomeSuccessEmpty()
    {
        var bin = CreateBasicBin();
        bin.Block("test");

        var outcome = bin.Unblock();
    
        Assert.IsType<BinContent.Empty>(bin.Content);
        Assert.IsType<UnblockOutcome.SuccessEmpty>(outcome);
    }

    [Fact]
    public void Unblock_NotBlocedOccupiedBin_ReturnOutcomeNotBlocked()
    {
        var bin = CreateBasicBin();
        string sku = "handle";
        int amount = 10;
        bin.Deposit(sku, amount);

        var outcome = bin.Unblock();

        var occupied = Assert.IsType<BinContent.Occupied>(bin.Content);    
        Assert.Equal(sku, occupied.Sku);
        Assert.Equal(amount, occupied.Quantity);
        Assert.IsType<UnblockOutcome.NotBlocked>(outcome);
    }

    [Fact]
    public void Unblock_NotBlocedEmptyBin_ReturnOutcomeNotBlocked()
    {
        var bin = CreateBasicBin();

        var outcome = bin.Unblock();

        Assert.IsType<BinContent.Empty>(bin.Content);
        Assert.IsType<UnblockOutcome.NotBlocked>(outcome);
    } //здесь еще получил опыт быстрого переписывания кода -- сначала написал все ассерты через True и внутри &&, потом осознал, какая это ошибка и сколько неудобств. Как будто за джуном прибрал))
}