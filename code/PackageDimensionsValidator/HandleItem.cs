namespace BeginCsh.PackageDimensionsValidator;

public sealed record HandleItem (double length, double width, double heigth)
{
    public const double BoxLength = 300;
    public const double BoxWidth = 200;
    public const double BoxHeigth = 150;
};