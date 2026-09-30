namespace BeginCsh.PackageDimensionsValidator;
using BeginCsh.Base;

public static class PackageDimensionsPrinter
{
    public static (Dimensions handle, Dimensions box) ReadDimensions()
    {
        double handleLength = InputHelper.Prompt<double>("Введите длину ручки (мм): ", l => l > 0 && l <= 1800, "Недопустимый размер"); //Думал сделать отдельное сообщение для превышения длины, но как будто это будет скорее лишней работой
        double handleWidth = InputHelper.Prompt<double>("Введите ширину ручки (мм): ", w => w > 0 && w <= 500, "Недопустимый размер");
        double handleHeith = InputHelper.Prompt<double>("Введите высоту ручки (мм): ", h => h > 0 && h <= 500, "Недопустимый размер");

        double boxLength = InputHelper.Prompt<double>("Введите длину коробки (мм): ", l => l > 0 && l <= 3000, "Недопустимый размер");
        double boxWidth = InputHelper.Prompt<double>("Введите ширину коробки (мм): ", w => w > 0 && w <= 1000, "Недопустимый размер");
        double boxHeith = InputHelper.Prompt<double>("Введите высоту коробки (мм): ", h => h > 0 && h <= 900, "Недопустимый размер");

        var handle = new Dimensions(handleLength,handleWidth, handleHeith);
        var box = new Dimensions(boxLength, boxWidth, boxHeith);
        return (handle, box);
    }

    //Я тут подумал: хорошая же проверка: если в классе принтера есть метод не "void" (за исключением инпутсчитывателя) -- значит что-то здесь не так (в плане SRP)
    public static void PrintValidation(Dimensions handle, Dimensions box, IReadOnlyList<ExceedanceInfo> exceedances) 
    {
            Console.WriteLine("Мебельная ручка принята в доставку!");
            Console.WriteLine($"Параметры вашей ручки: {handle.Length} мм X {handle.Width} мм X {handle.Height} мм");
    }

    public static void PrintWarning(Dimensions handle, Dimensions box, IReadOnlyList<ExceedanceInfo> exceedances)
    {
        Console.WriteLine($"Параметры вашей ручки: {handle.Length} мм X {handle.Width} мм X {handle.Height} мм");
            Console.WriteLine("Внимание! Мебельная ручка не помещается в коробку по следующим параметрам: ");

            foreach (var ex in exceedances)
            {
                Console.WriteLine($"Превышение по {ex.name} в размере: {ex.excess} мм");
            }
    }
    }