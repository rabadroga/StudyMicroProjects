namespace BeginCsh.PackageDimensionsValidator;

public readonly record struct Dimensions (double Length, double Width, double Height);
//Получается, этот рекорд небольшой и ему не нужна особая логика -- поэтому он структура?
public sealed record ExceedanceInfo(string name, double itemsize, double limit)
{
    public double excess => itemsize - limit;
};