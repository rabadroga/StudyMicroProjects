namespace BeginCsh.TieredPriceCalculator;

using System.Runtime.InteropServices;
using BeginCsh.Base;

public static class InputReader
{
    public static int QuantityInput()
    {
        int quantity = InputHelper.Prompt<int>("Введите количество товара:", q => q > 0);
        return quantity;
    }
    public static bool DiscountLogicChoiceInput() //мб стоит реализовать в enum
    {
        char logicChoice = InputHelper.Prompt<char>("Выберите тип логики скидок: \n 1 -- плоская логика \n 2 -- прогрессивная шкала", c => c == '1' || c == '2');
        return(logicChoice == '1');
    }
    public static string? PromocodeInput()
    {
        Console.WriteLine("Введите промокод или оставьте поле пустым: ");
        string? promocode = Console.ReadLine();
        return promocode; //Я все не могу точно понять, куда мне внести проверку промокода -- сюда или в программ?
    }
}