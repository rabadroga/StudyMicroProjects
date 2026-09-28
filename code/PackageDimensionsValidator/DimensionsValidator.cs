using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace BeginCsh.PackageDimensionsValidator;

public sealed record ExceedanceInfo(string name, double itemsize, double limit);

public static class DimensionsValidator
{
    public static IReadOnlyList<ExceedanceInfo> ValidateDimensions(HandleItem handle)
    {
        var exceedances = new List<ExceedanceInfo>();

        if (handle.length > HandleItem.BoxLength)
        {
            exceedances.Add(new("Длина", handle.length, HandleItem.BoxLength));
        }
        else if (handle.width > HandleItem.BoxWidth)
        {
            exceedances.Add(new("Ширина", handle.width, HandleItem.BoxWidth));
        }
        else if (handle.heigth > HandleItem.BoxHeigth)
        {
            exceedances.Add(new("Высота", handle.heigth, HandleItem.BoxHeigth));
        }

        return exceedances;
    }

    //Здесь будет метод поворота для влезания
}