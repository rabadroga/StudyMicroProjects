namespace BeginCsh.TieredPriceCalculator;
using BeginCsh.Base;

public static class InputReader
{
    public static int ReadInput()
    {
        int quantity = InputHelper.Prompt<int>("Введите количество товара", q => q > 0);
        return quantity;
    }
}