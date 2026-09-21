namespace BeginCsh.ProductOperationsV2;
public class PriceCalculator
{
    //Пока что заказ не допускает наличия нескольких позиций. Нет необходимости это прорабатывать в текущем проекте
    public record Order (string Article, int Quantity, decimal Price)
    {
        const decimal vat = 0.16m;  //НДС в РК -- 16%

        public decimal Subtotal => Quantity * Price;
        public decimal VATamount => Subtotal * vat;
        public decimal TotalPrice => Subtotal + VATamount;
    }
}