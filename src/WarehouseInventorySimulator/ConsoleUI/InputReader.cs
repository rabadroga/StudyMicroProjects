namespace BeginCsh.WarehouseInventorySimulator;
using BeginCsh.Base;

public static class InputReader
{
    public static ChoiceValue ChoiceInput()
    {
        const string prompt = """
        Выберите пункт:
        1 -- Создание товара
        2 -- Списание товара
        3 -- Пополнение товара
        0 -- Завершить смену
        """;

        return InputHelper.PromptEnum<ChoiceValue>(prompt);
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