namespace BeginCsh.TieredPriceCalculator;

public readonly record struct PricingResult(
    int Quantity,
    decimal DiscoutRate  //скидка в процентах
)
{
    public const decimal BasePrice = 1500;   //по заданию она фиксированная. Мог бы сделать конст, но думаю, что скорее привяжу ее к товару. Мб можно бы сделать отдельный класс с ручками, названиями и ценами
    public decimal Subtotal => decimal.Round(Quantity * BasePrice, 2, MidpointRounding.AwayFromZero);
    public decimal DiscoutAmount => decimal.Round(Subtotal * DiscoutRate, 2, MidpointRounding.AwayFromZero); //скидка в деньгах
    public decimal TotalPrice => Subtotal + DiscoutAmount;
    public decimal TotalUnitPrice => decimal.Round(TotalPrice / Quantity, 2, MidpointRounding.AwayFromZero); //цена за штуку, итоговая
};

//Далее нужно будет заняться логикой. Изучить, что такое свитч, продумать градацию скидок, там вроде свитчи прям замешаны. Потом сделать ступрасчет