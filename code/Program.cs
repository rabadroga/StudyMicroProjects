using BeginCsh.TieredPriceCalculator;
    static class Program
    {
    static void Main()
    {
        /*
        Что по плану:
        В рекорде уже вычисляются сабтотал, размер скидки, сумма и цена за штуку
        Нужно понять размер скидки, создать градацию х2
        И потом вывести
        */
        var quantity = InputReader.ReadInput();
        var discount = DiscountCalculator.CalculateFlatDiscount(quantity);
        var result = new PricingResult(quantity,discount);

        Console.WriteLine(OutputPrinter.PrintOutput(result));
    }
    }