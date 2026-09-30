using BeginCsh.PackageDimensionsValidator;
    static class Program
    {
    static void Main()
    {
        (Dimensions handle, Dimensions box) = PackageDimensionsPrinter.ReadDimensions();
        IReadOnlyList<ExceedanceInfo> exceedances = DimensionsValidator.ValidateDimensions(handle, box);

        if (exceedances is []) PackageDimensionsPrinter.PrintValidation(handle, box, exceedances);
        else
        {
            PackageDimensionsPrinter.PrintWarning(handle, box, exceedances);

            Console.WriteLine("Желаете проверить возможность переукладки вашей ручки? \n [Enter] для продолжения или любая другая клавиша для отмены");
            ConsoleKeyInfo keyInput = Console.ReadKey();
            if (keyInput.Key == ConsoleKey.Enter)
            {
                (Dimensions rotatedHandle, Dimensions rotatedBox) = ItemRotator.RotateTry(handle, box);
                IReadOnlyList<ExceedanceInfo> rotatedExceedances = DimensionsValidator.ValidateDimensions(rotatedHandle, rotatedBox);

                if (rotatedExceedances is [])
                {
                    PackageDimensionsPrinter.PrintValidation(rotatedHandle, rotatedBox, rotatedExceedances);
                } 
                else Console.WriteLine("Переукалдка не дала результата");
            }
            else Console.WriteLine("Выход...");
        }
    }

    }