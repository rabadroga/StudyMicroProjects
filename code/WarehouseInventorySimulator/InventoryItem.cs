namespace BeginCsh.WarehouseInventorySimulator;

public class InventoryItem
{
    public string Article { get; }
    public int WarningThreshold { get; } //возможно потом сделать priv set если нужно будет обновление порогов

    public int Quantity {get; private set;}

    public InventoryItem(string article, int quantity, int warningThreshold)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(article);
        ArgumentOutOfRangeException.ThrowIfNegative(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(warningThreshold);

        Article = article;
        Quantity = quantity;
        WarningThreshold = warningThreshold;
    }

    //здесь будет метод списания --
    /*
    Он выводит рекорд (надо создать в отдельном файле?) с параметрами (остаток, достигнут ли порог) -- они пойдут в инфу для истории отгрузок
    и потом меняет значения количества

    мб еще создать метод для пополнения товара, на всякий случай?

    что такое discriminated union?
    */
}