namespace AnnPrepareLavni.API.Converters;

public static class UnitConversion
{
    public const double MetersToFeetUnit = 3.28084;
    public const double KilogramsToPoundsUnit = 2.20462;

    public static double MetersToFeet(double meters) => meters * MetersToFeetUnit;
    public static double FeetToMeters(double feet) => feet / MetersToFeetUnit;

    public static double KilogramsToPounds(double kilograms) => kilograms * KilogramsToPoundsUnit;
    public static double PoundsToKilograms(double pounds) => pounds / KilogramsToPoundsUnit;
}
