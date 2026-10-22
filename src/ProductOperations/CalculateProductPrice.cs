namespace BeginCsh.ProductOperations;
using static BeginCsh.ProductOperations.GetProductInfo;
using System;
static class CalculateProductPrice
{
    //Решил закинуть калькуляцию и вывод в один класс, так как они слишком маленькие, код перегружен.
    public static void DisplayingTotalPrice()
    {
         decimal totalPriceNoNDA = quantity * price;
         decimal ndaOfTotalPrice = totalPriceNoNDA * 0.16m; // НДС -- 16%
         decimal totalPriceWithNDA = totalPriceNoNDA * 1.16m;
        //Может можно сделать оформление покруче, но пока так, лучше не умею.
        Console.WriteLine($"Артикул товара: {article}");
        Console.WriteLine($"Количество товара: {quantity}");
        Console.WriteLine($"Общая стоимость товара без НДС: {totalPriceNoNDA:C} тенге");
        Console.WriteLine($"Сумма НДС: {ndaOfTotalPrice:C} тенге");
        Console.WriteLine($"Общая стоимость товара с НДС: {totalPriceWithNDA:C} тенге");
    }
    
}