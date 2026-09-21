using BeginCsh.ProductOperationsV2;
    static class Program
    {
    static void Main()
    {
        PriceCalculator.Order order = OrderReceiptPrinter.OrderRead();
        OrderReceiptPrinter.OrderPrint(order);
    }

    }