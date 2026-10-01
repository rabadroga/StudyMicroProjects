namespace BeginCsh.PackageDimensionsValidator;

public readonly record struct Dimensions (double Length, double Width, double Height);
public sealed record ExceedanceInfo(string Name, double ItemSize, double Limit)
{
    public double Excess => ItemSize - Limit;
};
public sealed record ValidationResult(bool IsFit, IReadOnlyList<ExceedanceInfo> Exceedances);