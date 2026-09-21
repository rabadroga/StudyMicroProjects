namespace BeginCsh.ProductOperationsV2;
using System;
using System.Globalization;

public static class OrderReceiptPrinter
{
    //Этот класс включает в себя прием и вывод инфы, он не делает никаких расчетов
    public static Order ReadOrder()
    {
        string articleRaw = OrderInfoExam<string> ("Введите артикул товара:");
        int quantityRaw = OrderInfoExam<int> ("Введите количество товара", q => q > 0);
        decimal priceRaw = OrderInfoExam<decimal> ("Введите цену товара: ", p => p > 0);

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

    static T OrderInfoExam<T> (string prompt, Func<T, bool>? validator = null, string errorMessage = "Вы ввели недопустимое значение")
        where T: IParsable<T>
        {
        while (true)
        {
            Console.WriteLine(prompt);
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine(errorMessage);
                continue;
            }
            if (!T.TryParse(input, CultureInfo.CurrentCulture, out T? result) || result is null)
            {
                Console.WriteLine(errorMessage);
                continue;
            }
            if (validator != null && !validator(result))
            {
                Console.WriteLine(errorMessage);
                continue;
            }
            return result;
        }
        }
}