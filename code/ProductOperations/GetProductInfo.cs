namespace BeginCsh.ProductOperations;
using System;
static class GetProductInfo
{
    public static void GettingProductInfo()
    {
        Console.WriteLine("Введите артикул товара:");
        string? article = Console.ReadLine();

        while (article is null or "")
        {
            Console.WriteLine("Артикул не может быть пустым. Пожалуйста, введите артикул:");
            article = Console.ReadLine();
        }

        //TODO Закончить получение данных от товаре по ТЗ. 
        // Добавить комменты к методам. 
        // Потом продолжить дальше делать прк1 -- калькулятор и вывод
    }
}