using BeginCsh.ProductOperationsV2;
    static class Program
    {
    static void Main()
    {
        Order order = OrderReceiptPrinter.ReadOrder();
        OrderReceiptPrinter.PrintOrder(order);
    }

    }