namespace BeginCsh.PackageDimensionsValidator;
using BeginCsh.Base;

public static class PackageDimensionsPrinter
{
    public static HandleItem ReadHandlesDimensions()
    {
        double handleLength = InputHelper.Prompt<double>("Введите длину ручки (мм): ", l => l > 0 && l <= 1800, "Недопустимый размер"); //Думал сделать отдельное сообщение для превышения длины, но как будто это будет скорее лишней работой
        double handleWidth = InputHelper.Prompt<double>("Введите ширину ручки (мм): ", w => w > 0 && w <= 500, "Недопустимый размер");
        double handleHeith = InputHelper.Prompt<double>("Введите высоту ручки (мм): ", h => h > 0 && h <= 500, "Недопустимый размер");

        var handle = new HandleItem(handleLength,handleWidth, handleHeith);
        return handle;
    }

    public static void PrintValidation(HandleItem handle, IReadOnlyList<ExceedanceInfo> exceedances)
    {
        if (exceedances is [])
        {
            Console.WriteLine("Мебельная ручка принята в доставку!");
            Console.WriteLine($"Параметры вашей ручки: {handle.length} мм X {handle.width} мм X {handle.heigth} мм");
        }
        else
        {
            Console.WriteLine($"Параметры вашей ручки: {handle.length} мм X {handle.width} мм X {handle.heigth} мм");
            Console.WriteLine("Внимание! Мебельная ручка не помещается в коробку по следующим параметрам: ");

            foreach (var ex in exceedances)
            {
                Console.WriteLine($"Превышение по {ex.name} в размере: {ex.excess} мм");
            }

            Console.WriteLine("Желаете проверить возможность переукладки вашей ручки? \n [Enter] для продолжения или любая другая клавиша для отмены");
            ConsoleKeyInfo keyInput = Console.ReadKey();
            if (keyInput.Key == ConsoleKey.Enter)
            {
                var rotateExceedances = new List<ExceedanceInfo>();
                DimensionsValidator.RotateTry(handle, rotateExceedances);

                if (rotateExceedances is [])
                {
                    Console.WriteLine("Мебельная ручка уложена правильно и принята в доставку!");
                    Console.WriteLine($"Параметры вашей ручки: {handle.length} мм X {handle.width} мм X {handle.heigth} мм");
                } 
                else Console.WriteLine("Переукалдка не дала результата");
            }
            else Console.WriteLine("Выход...");
    }
}
}