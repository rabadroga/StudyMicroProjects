namespace BeginCsh.PackageDimensionsValidator;

public readonly record struct Dimensions (double Length, double Width, double Height);
public sealed record ExceedanceInfo(string Name, double ItemSize, double Limit)
{
    public double Excess => ItemSize - Limit;
};
public sealed record ValidationResult(bool IsFit, IReadOnlyList<ExceedanceInfo> Exceedances);

/*(Dimensions handle, Dimensions box) = PackageDimensionsPrinter.ReadDimensions();
        ValidationResult fitResult = DimensionsValidator.ValidateDirect(handle, box);

        if (fitResult.IsFit) PackageDimensionsPrinter.PrintValidation(handle, box);
        else
        {
            PackageDimensionsPrinter.PrintWarning(handle, box, fitResult);

            Console.WriteLine("Желаете проверить возможность переукладки вашей ручки? \n [Enter] для продолжения или любая другая клавиша для отмены");
            ConsoleKeyInfo keyInput = Console.ReadKey();
            if (keyInput.Key == ConsoleKey.Enter)
            {
                ValidationResult rotatedFitResult = DimensionsValidator.ValidateWithRotation(handle, box);

                if (rotatedFitResult.IsFit)
                {
                    PackageDimensionsPrinter.PrintValidation(handle, box);
                } 
                else Console.WriteLine("Переукладка не дала результата");
            }
            else Console.WriteLine("Выход...");
        }*/
        //Этот код нужно закинуть в програм.кс