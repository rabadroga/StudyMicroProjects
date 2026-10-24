namespace BeginCsh.WarehouseInventorySimulator;
using BeginCsh.Base;

public static class InputReader
{
    public static ChoiceValue ChoiceInput()
    {
        var choice = InputHelper.PromptEnum<ChoiceValue>("Выберите пункт: : \n 1 -- Создание товара \n 2 -- Списание товара \n 3 -- Пополнение товара \n 0 -- Завершить смену");

        return choice;
    }
    public static string SkuInput()
    {
        string sku = InputHelper.Prompt<string>("Введите артикул товара: ");

        return sku;
    }

    public static int QuantityInput()
    {
        int quantity = InputHelper.Prompt<int>("Введите количество товара:", q => q >= 0);

        return quantity;
    }

    public static int WarningThresholdInput()
    {
        int warningThreshold = InputHelper.Prompt<int>("Введите порог предупреждения для количества товара (для его отсутствия введите '0'");

        return warningThreshold;
    }

    public static int AmountInput()
    {
        int amount = InputHelper.Prompt<int>("Введите необходимое количество: ", q => q > 0);

        return amount;
    }
}