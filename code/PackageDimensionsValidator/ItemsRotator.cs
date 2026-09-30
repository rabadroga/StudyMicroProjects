namespace BeginCsh.PackageDimensionsValidator;

    //мб стоит закинуть это в отдельный класс. Это же не валидатор, а крутитель
    public static class ItemRotator{
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