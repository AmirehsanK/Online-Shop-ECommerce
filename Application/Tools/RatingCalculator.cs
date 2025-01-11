namespace Application.Tools;

public static class RatingCalculator
{
    public static int CalculateBasedOnFive(float value)
    {
        var percentage = (value / 5.0f) * 100;
        percentage = Math.Max(0, Math.Min(100, percentage));
        return (int)percentage;
    }
}