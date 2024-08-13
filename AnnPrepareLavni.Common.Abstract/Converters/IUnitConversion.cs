namespace AnnPrepareLavni.API.Common.Converters;

public interface IUnitConversion
{
    static double MetersToFeetUnit { get; }
    static double KilogramsToPoundsUnit { get; }

    static abstract double MetersToFeet(double meters);
    static abstract double FeetToMeters(double feet);

    static abstract double KilogramsToPounds(double kilograms);
    static abstract double PoundsToKilograms(double pounds);
}
