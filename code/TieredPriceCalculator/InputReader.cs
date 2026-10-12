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
        return promocode; //Я все не могу точно понять, куда мне внести проверку промокода -- сюда или в программ?
    }
}