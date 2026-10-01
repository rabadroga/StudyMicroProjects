using BeginCsh.PackageDimensionsValidator;
    static class Program
    {
    static void Main()
    {
        (Dimensions handle, Dimensions box) = PackageDimensionsPrinter.ReadDimensions();
        ValidationResult fitResult = DimensionsValidator.ValidateDirect(handle, box);

        if (fitResult.IsFit) PackageDimensionsPrinter.PrintValidation(handle, box);
        else
        {
            PackageDimensionsPrinter.PrintWarning(handle, box, fitResult);

            Console.WriteLine("Желаете проверить возможность переукладки вашей ручки? \n [Enter] для продолжения или любая другая клавиша для отмены");
            ConsoleKeyInfo keyInput = Console.ReadKey();
            if (keyInput.Key == ConsoleKey.Enter)
            {
                (Dimensions rotatedHandle, Dimensions rotatedBox) = DimensionsValidator.ValidateWithRotation(handle, box);
                ValidationResult rotatedFitResult = DimensionsValidator.ValidateDirect(rotatedHandle, rotatedBox);

                if (rotatedFitResult.IsFit)
                {
                    PackageDimensionsPrinter.PrintValidation(rotatedHandle, box);
                } 
                else Console.WriteLine("Переукладка не дала результата");
            }
            else Console.WriteLine("Выход...");
        }
        //потом закомменчу этот код и вставлю в текстовый файл в папке микропроекта для того, чтобы запустить его сразу, если нужно будет (в програм всегда разные коды для запуска же)
    }

    }