using BeginCsh.WarehouseInventorySimulator;

namespace BeginCsh.Tests.WarehouseInventorySimulator;

public class InventoryItemTests
{
    private static InventoryItem CreateItemWithQuantity(int quantity) => new InventoryItem("test-item", quantity, 0);

    //нужно ли делать тест валидного создания item?

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void InventoryItem_NullOrWhiteSpaceSku_ThrowsArgumentException(string? sku)
    {
        Assert.ThrowsAny<ArgumentException>(() => new InventoryItem(sku!, 1, 1));
    }

    [Theory]
    [InlineData(1, -1)]
    [InlineData(-1, 1)]
    public void InventoryItem_NegativeQuantityOrWarningThreshold_ThrowsArgumentOutOfRangeException(int quantity, int warningThreshold)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new InventoryItem("item-test", quantity, warningThreshold));
    }

    [Theory]
    [InlineData(10, 5, 5)]
    [InlineData(10, 10, 0)]
    public void Ship_ValidAmount_CorrectReducing(int quantity, int amount, int expected)
    {
        var item = CreateItemWithQuantity(quantity);

        item.Ship(amount);

        Assert.Equal(expected, item.Quantity);
    }

    [Theory]
    [InlineData(10, 5, 2, false)]
    [InlineData(10, 5, 5, true)]
    [InlineData(10, 5, 6, true)]
    public void IsLowStock_ValidAmount_CorrectStatusShowing(int quantity, int amount, int warningThreshold, bool isLowStock)
    {
        var item = new InventoryItem("item-test", quantity, warningThreshold);

        item.Ship(amount);

        Assert.Equal(isLowStock, item.IsLowStock);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Ship_InvalidAmount_ThrowsInvalidArgumentOutOfRangeException(int amount)
    {
        var item = CreateItemWithQuantity(10);

        Assert.Throws<ArgumentOutOfRangeException>(() => item.Ship(amount));
        Assert.Equal(10, item.Quantity);           
    }

    [Fact]
    public void Ship_AmountExceedsAvailableQuantity_ThrowsInvalidOperationException()
    {
        var item = CreateItemWithQuantity(10);

        Assert.Throws<InvalidOperationException>(() => item.Ship(11));
        Assert.Equal(10, item.Quantity); 
    }

    [Fact]
    public void Restock_ValidAmount_CorrectAdding()
    {
        var item = CreateItemWithQuantity(10);

        item.Restock(10);

        Assert.Equal(20, item.Quantity);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Restock_InvalidAmount_ThrowsInvalidArgumentOutOfRangeException(int amount)
    {
        var item = CreateItemWithQuantity(10);

        Assert.Throws<ArgumentOutOfRangeException>(() => item.Restock(amount));
        Assert.Equal(10, item.Quantity);           
    }
}