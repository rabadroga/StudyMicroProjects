using BeginCsh.TieredPriceCalculator;
    static class Program
    {
    static void Main()
    {
        //чекни InputHelper -- я его немного переписал и переместил
        var quantity = InputReader.QuantityInput();
        var promocode = InputReader.PromocodeInput();
        var flatResult = DiscountCalculator.CalculateFlatDiscount(quantity, promocode);
        var progressiveResult = DiscountCalculator.CalculateProgressiveDiscount(quantity, promocode);
        var anomalyResult = DiscountCalculator.CheckWholesaleAnomaly(quantity);

        OutputPrinter.PrintComparisonReport(flatResult, progressiveResult);

        if (anomalyResult is (int additionalQuantity, decimal savedMoney))
            OutputPrinter.PrintWholesaleAnomalyReport(additionalQuantity, savedMoney);
        //надо еще подумать, как правильно расчитывать объем работы, чтобы понять, когда я могу справедливо давать себе отдых до конца дня, если многое успел
    }
    }