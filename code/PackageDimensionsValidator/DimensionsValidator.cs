namespace BeginCsh.PackageDimensionsValidator;

public sealed record ExceedanceInfo(string name, double itemsize, double limit)
{
    public double excess => itemsize - limit;
};

public static class DimensionsValidator
{
    public static IReadOnlyList<ExceedanceInfo> ValidateDimensions(HandleItem handle, List<ExceedanceInfo> exceedances)
    {
        //ExceedanceInfo[] exceedaances;

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

    public static IReadOnlyList<ExceedanceInfo> RotateTry(HandleItem handle, List<ExceedanceInfo> rotateExceedances)
    {
        double[] rotation = [handle.length, handle.width, handle.heigth];
        Array.Sort(rotation);
        Array.Reverse(rotation);
          
        var rotatedHandle = new HandleItem(rotation[0], rotation[1], rotation[2]);
        ValidateDimensions(rotatedHandle, rotateExceedances);
        return rotateExceedances;
    }
    //Здесь будет метод поворота для влезания
}