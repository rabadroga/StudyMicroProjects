namespace BeginCsh.TieredPriceCalculator;

using BeginCsh.Base;

public static class InputReader
{
    public static int QuantityInput()
    {
        int quantity = InputHelper.Prompt<int>("Введите количество товара:", q => q > 0);
        return quantity;
    }
    public static string? PromocodeInput()
    {
        Console.WriteLine("Введите промокод или оставьте поле пустым: ");
        string? promocode = Console.ReadLine();
        return promocode;
    }
    public static CustomerTier CustomerTierInput()
    {
        CustomerTier customerTier = InputHelper.PromptEnum<CustomerTier>
        (
            "Выберите свою категорию покупателя: \n 0 -- Розничный \n 1 -- Дизайнер \n 2 -- Оптовик"
        );
        return customerTier;
    }
    }