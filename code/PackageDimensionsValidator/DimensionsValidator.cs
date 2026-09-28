namespace BeginCsh.PackageDimensionsValidator;

public sealed record ExceedanceInfo(string name, double itemsize, double limit)
{
    public double excess => itemsize - limit;
};

public static class DimensionsValidator
{
    public static IReadOnlyList<ExceedanceInfo> ValidateDimensions(HandleItem handle, List<ExceedanceInfo> exceedances)
    {
        //var exceedances = new List<ExceedanceInfo>();

        if (handle.length > HandleItem.BoxLength)
        {
            exceedances.Add(new("длине", handle.length, HandleItem.BoxLength));
        }
        if (handle.width > HandleItem.BoxWidth)
        {
            exceedances.Add(new("ширине", handle.width, HandleItem.BoxWidth));
        }
        if (handle.heigth > HandleItem.BoxHeigth)
        {
            exceedances.Add(new("высоте", handle.heigth, HandleItem.BoxHeigth));
        }

        return exceedances;
    }

    //Здесь будет метод поворота для влезания
}