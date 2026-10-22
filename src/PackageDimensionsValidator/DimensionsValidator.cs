namespace BeginCsh.PackageDimensionsValidator;

public static class DimensionsValidator
{
    public static ValidationResult ValidateDirect(Dimensions handle, Dimensions box)
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

        return new ValidationResult(exceedances.Count == 0, exceedances);
    }

    public static ValidationResult ValidateWithRotation(Dimensions handle, Dimensions box)
    {
        double[] hRotated = [handle.Length, handle.Width, handle.Height];
        Array.Sort(hRotated);
        Array.Reverse(hRotated);
        double[] bRotated = [box.Length, box.Width, box.Height];
        Array.Sort(bRotated);
        Array.Reverse(bRotated);
          
        return ValidateDirect
        (
            new Dimensions(hRotated[0], hRotated[1], hRotated[2]), 
            new Dimensions(bRotated[0], bRotated[1], bRotated[2])
        );
    }
}