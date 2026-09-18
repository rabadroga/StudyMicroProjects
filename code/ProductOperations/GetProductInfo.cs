namespace BeginCsh.ProductOperations;
using System;
static class GetProductInfo
{
    //Здесь мы получаем 3 параметра от пользователя: артикул (article), количество (quantity) и цену товара (price).
    public static void GettingProductInfo()
    {
        //Решил сделать все получение данных в одном методе, так как все данные связаны между собой и логично их получать вместе.
        Console.WriteLine("Введите артикул товара:");
            string? article = Console.ReadLine();

            while (article is null or "")
            {
                Console.WriteLine("Артикул не может быть пустым. Пожалуйста, введите артикул:");
                article = Console.ReadLine();
            }

        Console.WriteLine("Введите нужное количество:");
            string? quantityInput = Console.ReadLine();

            while (!int.TryParse(quantityInput, out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Вы ввели недопустимое количество:");
                quantityInput = Console.ReadLine();
            }
            
        Console.WriteLine("Введите цену соответствующего товара:");
            string? priceInput = Console.ReadLine();

            while (!decimal.TryParse(priceInput, out decimal price) || price <= 0)
            {
                Console.WriteLine("Вы ввели недопустимую цену:");
                priceInput = Console.ReadLine();
            }

        //TODO Потом продолжить дальше делать прк1 -- калькулятор и вывод
    }
}