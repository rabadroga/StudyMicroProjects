using System.Diagnostics;

namespace BeginCsh.WarehouseInventorySimulator;

public static class OutputPrinter
{
    public static void PrintWelcome()
    {
        Console.WriteLine("Смена начата, добро пожаловать на склад!");
    }
    public static void PrintThresholdWarning()
    {
        Console.WriteLine("Внимание, достигнут минимальный порог! Необходимо сделать заказ для пополнения запасов товара");
    }
    public static void PrintCreationOutcome(CreationOutcome outcome)
    {
        switch (outcome)
        {
            case CreationOutcome.Success success:
                Console.WriteLine($"Успешно создан товар {success.Log.Sku} с количеством {success.Log.Quantity} штук");
                if (success.Log.IsLowStock)
                    PrintThresholdWarning();
                break;

            case CreationOutcome.AlreadyExists exists:
                Console.WriteLine($"Товар с артикулом {exists.Sku} уже существует!");
                break;

            default: throw new UnreachableException("Охвачены не все возможные исходы!");
        }
    }

    public static void PrintShipmentOutcome(ShipmentOutcome outcome)
    {
        switch (outcome)
        {
            case ShipmentOutcome.Success success:
                Console.WriteLine($"Успешно списано {success.Log.Amount} штук у товара {success.Log.Sku}, остаток: {success.Log.Quantity} штук");
                if (success.Log.IsLowStock)
                    PrintThresholdWarning();
                break;

            case ShipmentOutcome.ItemNotFound notFound:
                Console.WriteLine($"Товар с артикулом {notFound.Sku} не найден!");
                break;

            case ShipmentOutcome.InsufficientStock insufficient:
                Console.WriteLine($"Не удалось списать {insufficient.Amount} штук, на складе всего {insufficient.RemainingStock} штук");
                break;

            default: throw new UnreachableException("Охвачены не все возможные исходы!");
        }
    }

    public static void PrintRestockmentOutcome(RestockmentOutcome outcome)
    {
        switch (outcome)
        {
            case RestockmentOutcome.Success success:
                Console.WriteLine($"Успешно пополнен товар {success.Log.Sku} на {success.Log.Amount} штук, остаток: {success.Log.Quantity} штук");
                break;

            case RestockmentOutcome.ItemNotFound notFound:
                Console.WriteLine($"Товар с артикулом {notFound.Sku} не найден!");
                break;

            default: throw new UnreachableException("Охвачены не все возможные исходы!");
        }
    }

    public static void PrintLogReport(OperationLog log) //решил не делать здесь принятие словаря и прогон через цикл, чтобы не усложнять логику именно принтера. Но думаю, что можно сделать и здесь цикл, чтобы он вывел какие-то итоги смены (типо, общее количество циркуляции товаров, количество операций и тд)
    {
        switch (log.OperationType)
        {
            case OperationType.Creation:
                Console.WriteLine($"""
                    --==--
                    Создание нового товара: {log.Sku}
                    Начальное количество: {log.Quantity} шт.
                    Время: {log.DateTime:yyyy-MM-dd HH:mm:ss}
                    --==--
                    """);
                break;

            case OperationType.Shipment:
                Console.WriteLine($"""
                    --==--
                    Списание товара: {log.Sku}
                    Списано: {log.Amount} шт.
                    Остаток: {log.Quantity} шт.
                    Минимальный порог достигнут: {(log.IsLowStock ? "ДА" : "Нет")}
                    Время: {log.DateTime:yyyy-MM-dd HH:mm:ss}
                    --==--
                    """);
                break;

            case OperationType.Restock:
                Console.WriteLine($"""
                    --==--
                    Пополнение товара: {log.Sku}
                    Пополнено: {log.Amount} шт.
                    Остаток: {log.Quantity} шт.
                    Время: {log.DateTime:yyyy-MM-dd HH:mm:ss}
                    --==--
                    """);
                break;

            default:
                throw new UnreachableException("Охвачены не все возможные исходы!");
        }
    }

    public static void PrintEmptyLogReport()
    {
        Console.WriteLine("За смену не произошло ни одного события");
    }
}