namespace BeginCsh.PackageDimensionsValidator;
using BeginCsh.ProductOperationsV2;

public static class PackageDimensionsPrinter
{
    public static HandleItem ReadHandlesDimensions()
    {
        double handleLength = InputExam.ExamInput<double>("Введите длину ручки (мм): ", l => l > 0 && l <= 1800, "Недопустимый размер"); //Думал сделать отдельное сообщение для превышения длины, но как будто это будет скорее лишней работой
        double handleWidth = InputExam.ExamInput<double>("Введите ширину ручки (мм): ", w => w > 0 && w <= 500, "Недопустимый размер");
        double handleHeith = InputExam.ExamInput<double>("Введите высоту ручки (мм): ", h => h > 0 && h <= 500, "Недопустимый размер");

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
        }
    }
}