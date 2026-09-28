using BeginCsh.PackageDimensionsValidator;
    static class Program
    {
    static void Main()
    {
        HandleItem handle = PackageDimensionsPrinter.ReadHandlesDimensions();
        var exceedances = new List<ExceedanceInfo>();
        
        DimensionsValidator.ValidateDimensions(handle, exceedances); //А вот как создается множество разных объектов рекорда, если их будут вызывать разные люди постоянно? Типо если будет создаваться база, они же не будут все как "handle"
        PackageDimensionsPrinter.PrintValidation(handle, exceedances);
    }

    }