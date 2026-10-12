using BeginCsh.TieredPriceCalculator;
    static class Program
    {
    static void Main()
    {
        //после того, как написал код -- сверил потом с твоим рефактором, были затупы у меня
        var quantity = InputReader.QuantityInput();
        var promocode = InputReader.PromocodeInput(); //а надо ли писать именно вар? Не лучше ли для памяти обозначать четко тип, возвращаемого элемента? Это же хорошая практика для большой программы
        var flatResult = DiscountCalculator.CalculateFlatDiscount(quantity, promocode);
        var progressiveResult = DiscountCalculator.CalculateProgressiveDiscount(quantity, promocode);
        var anomalyResult = DiscountCalculator.CheckWholesaleAnomaly(quantity);

        OutputPrinter.PrintComparisonReport(flatResult, progressiveResult);

        if (anomalyResult is (int plusQuantity, decimal savedMoney))
            OutputPrinter.PrintWholesaleAnomalyReport(plusQuantity, savedMoney);

    }
    //А норм, что я тогда так реализовал выбор логики через char и булевое уравнение? Мне показалось, что это норм для 2-х вариантов, но это пришло чисто из головы
    //Когда можно будет сделать себе задание, где я не буду сидеть ломать голову над каждой строчкой кода и реализацией функциональности, а напишу как рутину, чтобы увидеть, что я уже не хлебушек
    }