namespace BeginCsh.PackageDimensionsValidator;

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
}