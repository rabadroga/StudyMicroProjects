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
        CustomerTier customerTier = InputHelper.PromtEnum<CustomerTier>
        (
            "Выберите свою категорию покупателя: \n 1 -- Розничный \n 2 -- Дизайнер \n 3 -- Оптовик"
        );
        return customerTier;
    }
    }