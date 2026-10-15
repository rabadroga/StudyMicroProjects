using BeginCsh.TieredPriceCalculator;
static class Program
{
    static void Main()
    {
        //чекни InputHelper -- я его немного переписал и переместил
        var quantity = InputReader.QuantityInput();
        var promocode = InputReader.PromocodeInput();
        var customerTier = InputReader.CustomerTierInput();

        var flatResult = DiscountCalculator.CalculateFlatDiscount(quantity, customerTier, promocode);
        var progressiveResult = DiscountCalculator.CalculateProgressiveDiscount(quantity, customerTier, promocode);
        var anomalyResult = DiscountCalculator.CheckWholesaleAnomaly(quantity, customerTier);

        OutputPrinter.PrintComparisonReport(flatResult, progressiveResult, customerTier);

        if (anomalyResult is (int additionalQuantity, decimal savedMoney))
            OutputPrinter.PrintWholesaleAnomalyReport(additionalQuantity, savedMoney);
        //надо еще подумать, как правильно расчитывать объем работы, чтобы понять, когда я могу справедливо давать себе отдых до конца дня, если многое успел
    }
}