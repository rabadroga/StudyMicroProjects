using System.Reflection.Metadata;

namespace BeginCsh.PackageDimensionsValidator;

public sealed record ExceedanceInfo(string name, double itemsize, double limit)
{
    public double excess => itemsize - limit;
};

public static class DimensionsValidator
{
    public static IReadOnlyList<ExceedanceInfo> ValidateDimensions(Dimensions handle, Dimensions box)
    {
        var exceedances = new List<ExceedanceInfo>();

        if (handle.Length > box.Length)
        {
            exceedances.Add(new("длине", handle.Length, box.Length));
        }
        if (handle.Width > box.Width)
        {
            exceedances.Add(new("ширине", handle.Width, box.Width));
        }
        if (handle.Height > box.Height)
        {
            exceedances.Add(new("высоте", handle.Height, box.Height));
        }

        return exceedances;
    }

    //мб стоит закинуть это в отдельный класс. Это же не валидатор, а крутитель
    public static (Dimensions rotatedHandle, Dimensions rotatedBox) RotateTry(Dimensions handle, Dimensions box)
    {
        double[] hRotated = [handle.Length, handle.Width, handle.Height];
        Array.Sort(hRotated);
        Array.Reverse(hRotated);
        double[] bRotated = [box.Length, box.Width, box.Height];
        Array.Sort(bRotated);
        Array.Reverse(bRotated);
          
        var rotatedHandle = new Dimensions(hRotated[0], hRotated[1], hRotated[2]);
        var rotatedBox = new Dimensions(bRotated [0], bRotated [1], bRotated [2]);
        return (rotatedHandle, rotatedBox);
    }
}