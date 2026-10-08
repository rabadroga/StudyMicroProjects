using BeginCsh.TieredPriceCalculator;
    static class Program
    {
    static void Main()
    {
        var quantity = InputReader.QuantityInput();
        var choicedLogic = InputReader.DiscountLogicChoiceInput();
        var promocode = InputReader.PromocodeInput();
        decimal discountRate = 0m;

        switch (promocode)
        {
            case "LOYALTY":
            discountRate += 0.03m;
            break;

            case var s when string.IsNullOrWhiteSpace(promocode): //решил оставить, чтобы не высвечивалось предупреждение лишний раз
            break;

            default:
            Console.WriteLine("Введен недействительный промокод!");
            break;
        }

        discountRate += choicedLogic switch
        {
            true => DiscountCalculator.CalculateFlatDiscount(quantity),
            false => DiscountCalculator.CalculateProgressiveDiscount(quantity, PricingResult.BasePrice)
        };
        //прочекать на возможность switch (как выработать в себе привычку быстро понимать, где лучше будет свитч?)
        discountRate = Math.Min(discountRate, 0.25m);  //Защита с макс уровнем скидки -- 25%        

        var result = new PricingResult(quantity, discountRate);

        Console.WriteLine(OutputPrinter.PrintOutput(result));
    }
    //Когда можно будет сделать себе задание, где я не буду сидеть ломать голову над каждой строчкой кода и реализацией функциональности, а напишу как рутину, чтобы увидеть, что я уже не хлебушек
    }