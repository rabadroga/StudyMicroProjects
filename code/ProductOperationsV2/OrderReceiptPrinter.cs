namespace BeginCsh.ProductOperationsV2;
using System;
using System.Globalization;
using BeginCsh.Base;

public static class OrderReceiptPrinter
{
    //Этот класс включает в себя прием и вывод инфы, он не делает никаких расчетов
    public static Order ReadOrder()
    {
        string articleRaw = InputExam.ExamInput<string> ("Введите артикул товара:");
        int quantityRaw = InputExam.ExamInput<int> ("Введите количество товара", q => q > 0);
        decimal priceRaw = InputExam.ExamInput<decimal> ("Введите цену товара: ", p => p > 0);

        var order = new Order(articleRaw, quantityRaw, priceRaw);

        return order;
    }

    public static void PrintOrder(Order order)
    {
        Console.WriteLine("\n=============== ЧЕК ПОЗИЦИИ ===============");
        Console.WriteLine($"Артикул товара:            {order.Article}");
        Console.WriteLine($"Количество:                {order.Quantity}");
        Console.WriteLine($"Цена за единицу:           {order.Price:C2}");
        Console.WriteLine($"Стоимость без НДС:         {order.Subtotal:C2}");
        Console.WriteLine($"Сумма НДС:                 {order.Vatamount:C2}");
        Console.WriteLine($"Итого к оплате с НДС:      {order.TotalPrice:C2}");
        Console.WriteLine("===============================================");
    }

}