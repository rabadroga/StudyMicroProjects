namespace BeginCsh.ProductOperationsV2;
    //Пока что заказ не допускает наличия нескольких позиций. Нет необходимости это прорабатывать в текущем проекте
    public sealed record Order (string Article, int Quantity, decimal Price)
    {
        const decimal Vat = 0.16m;  //НДС в РК -- 16%

        public decimal Subtotal => decimal.Round(Quantity * Price, 2, MidpointRounding.AwayFromZero);
        public decimal Vatamount => decimal.Round(Subtotal * Vat, 2, MidpointRounding.AwayFromZero);
        public decimal TotalPrice => Subtotal + Vatamount;
    }