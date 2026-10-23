using BeginCsh.WarehouseInventorySimulator;

namespace BeginCsh.Tests.WarehouseInventorySimulator;

public class WarehouseTests
{  
    private const string testSku = "item-test";
    private const int testQuantity = 10;
    private static Warehouse CreateWarehouseWithDefaultItem(int quantity = 10, int threshold = 0)
    {
        var warehouse = new Warehouse();
        warehouse.CreateItem(testSku, quantity, threshold);
        return warehouse;
    }
    
    [Fact]
    public void CreateItem_ValidParams_CreatedItemLogAndAddedToDictionaryAndList()
    {
        var warehouse = new Warehouse();
        var outcome = warehouse.CreateItem(testSku, testQuantity, 0);

        Assert.IsType<CreationOutcome.Success>(outcome); //возможно, сделать проверку через это значение

        var item = Assert.Contains(testSku, warehouse.WarehouseInventory);
        Assert.Equal(testSku, item.Sku);
        Assert.Equal(testQuantity, item.Quantity);
        Assert.Equal(0, item.WarningThreshold);

        var log = Assert.Single(warehouse.Logs);
        Assert.Equal(testSku, log.Sku);
        Assert.Equal(OperationType.Creation, log.OperationType);
        Assert.Equal(testQuantity, log.Quantity);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateItem_NullOrWhiteSpaceSku_ThrowsArgumentException(string? sku)
    {
        var warehouse = new Warehouse();

        Assert.ThrowsAny<ArgumentException>(() => warehouse.CreateItem(sku!, 1, 1));

        Assert.Empty(warehouse.WarehouseInventory);
        Assert.Empty(warehouse.Logs);
    }

    [Theory]
    [InlineData(1, -1)]
    [InlineData(-1, 1)]
    public void CreateItem_NegativeQuantityOrWarningThreshold_ThrowsArgumentOutOfRangeException(int quantity, int warningThreshold)
    {
        var warehouse = new Warehouse();

        Assert.Throws<ArgumentOutOfRangeException>(() => warehouse.CreateItem(testSku, quantity, warningThreshold));

        Assert.Empty(warehouse.WarehouseInventory);
        Assert.Empty(warehouse.Logs);
    }

    [Fact]
    public void CreateItem_ExistedSku_ReturnAlreadyExistOutcome()
    {
        var warehouse = CreateWarehouseWithDefaultItem();

        var outcome = warehouse.CreateItem(testSku, 20, 0);
        
        Assert.IsType<CreationOutcome.AlreadyExists>(outcome);

        Assert.Single(warehouse.WarehouseInventory);
        Assert.Single(warehouse.Logs);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    public void ShipItem_ValidAmount_CorrectReducing(int amount)
    {
        var warehouse = CreateWarehouseWithDefaultItem();

        var outcome = warehouse.ShipItem(testSku, amount);

        Assert.IsType<ShipmentOutcome.Success>(outcome);

        Assert.Collection(warehouse.Logs, 
            log1 => {},
            log2 =>
            {
                Assert.Equal(testSku, log2.Sku);
                Assert.Equal(OperationType.Shipment, log2.OperationType);
                Assert.Equal(testQuantity - amount, log2.Quantity);
            }
        );
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ShipItem_NullOrWhiteSpaceSku_ThrowsArgumentException(string? sku)
    {
        var warehouse = new Warehouse();

        Assert.ThrowsAny<ArgumentException>(() => warehouse.ShipItem(sku!, 10));
        Assert.Empty(warehouse.Logs);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void ShipItem_NegativeOrZeroAmount_ThrowsArgumentOutOfRangeException(int amount)
    {
        var warehouse = CreateWarehouseWithDefaultItem();

        Assert.Throws<ArgumentOutOfRangeException>(() => warehouse.ShipItem(testSku, amount));
        Assert.Single(warehouse.Logs);
    }

    [Fact]
    public void ShipItem_NotExistedSku_OutcomeIsItemNotFound()
    {
        var warehouse = new Warehouse();
        
        var outcome = warehouse.ShipItem(testSku, 10);
    
        Assert.IsType<ShipmentOutcome.ItemNotFound>(outcome);
        Assert.Empty(warehouse.Logs);
    }

    [Fact]
    public void ShipItem_AmountIsBiggerThanQuantity_ShipmentOutcomeIsInsufficientStock()
    {
        var warehouse = CreateWarehouseWithDefaultItem();

        var outcome = warehouse.ShipItem(testSku, 11);

        Assert.IsType<ShipmentOutcome.InsufficientStock>(outcome);
        Assert.Equal(testQuantity, warehouse.WarehouseInventory[testSku].Quantity);
        Assert.Single(warehouse.Logs);
    }

    [Fact]
    public void RestockItem_ValidAmount_CorrectIncreasing()
    {
        var warehouse = CreateWarehouseWithDefaultItem();

        var outcome = warehouse.RestockItem(testSku, 1);

        Assert.IsType<RestockmentOutcome.Success>(outcome);

        Assert.Collection(warehouse.Logs, 
            log1 => {},
            log2 =>
            {
                Assert.Equal(testSku, log2.Sku);
                Assert.Equal(OperationType.Restock, log2.OperationType);
                Assert.Equal(testQuantity + 1, log2.Quantity);
            }
        );
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void RestockItem_NullOrWhiteSpaceSku_ThrowsArgumentException(string? sku)
    {
        var warehouse = new Warehouse();

        Assert.ThrowsAny<ArgumentException>(() => warehouse.RestockItem(sku!, 10));
        Assert.Empty(warehouse.Logs);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void RestockItem_NegativeOrZeroAmount_ThrowsArgumentOutOfRangeException(int amount)
    {
        var warehouse = CreateWarehouseWithDefaultItem();

        Assert.Throws<ArgumentOutOfRangeException>(() => warehouse.RestockItem(testSku, amount));
        Assert.Single(warehouse.Logs);
    }

    [Fact]
    public void RestockItem_NotExistedSku_OutcomeIsItemNotFound()
    {
        var warehouse = new Warehouse();
        
        var outcome = warehouse.RestockItem(testSku, 10);
    
        Assert.IsType<RestockmentOutcome.ItemNotFound>(outcome);
        Assert.Empty(warehouse.Logs);
    }
}