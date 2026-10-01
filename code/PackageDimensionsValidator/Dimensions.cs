namespace BeginCsh.PackageDimensionsValidator;

public readonly record struct Dimensions (double Length, double Width, double Height);
public sealed record ExceedanceInfo(string Name, double Itemsize, double Limit)
{
    public double Excess => Itemsize - Limit;
};
public sealed record ValidationResult(bool IsFit, IReadOnlyList<ExceedanceInfo> Exceedances);