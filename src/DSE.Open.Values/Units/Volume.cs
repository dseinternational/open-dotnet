// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;

namespace DSE.Open.Values.Units;

[StructLayout(LayoutKind.Sequential)]
public readonly record struct Volume : IQuantity<double, UnitOfVolume>, IComparable<Volume>
{
    private Volume(double amount, UnitOfVolume measure)
    {
        Amount = amount;
        Units = measure;
    }

    public static Volume CubicMetre(double amount)
    {
        return new Volume(amount, UnitOfVolume.CubicMetre);
    }

    public static Volume CubicCentimetre(double amount)
    {
        return new Volume(amount, UnitOfVolume.CubicCentimetre);
    }

    public static Volume Litre(double amount)
    {
        return new Volume(amount, UnitOfVolume.Litre);
    }

    public static Volume Millilitre(double amount)
    {
        return new Volume(amount, UnitOfVolume.Millilitre);
    }

    public double Amount { get; }

    public UnitOfVolume Units { get; }

    public int CompareTo(Volume other)
    {
        return (Amount * Units.BaseUnits).CompareTo(other.Amount * other.Units.BaseUnits);
    }

    public override string ToString()
    {
        return ToString(null, null, Units);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return ToString(format, formatProvider, Units);
    }

    public string ToString(string? format, IFormatProvider? formatProvider, UnitOfVolume unitOfMass)
    {
        ArgumentNullException.ThrowIfNull(unitOfMass);
        return Amount.ToString(format, formatProvider) + " " + unitOfMass.Abbreviation;
    }

    public static bool operator <(Volume left, Volume right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(Volume left, Volume right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(Volume left, Volume right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(Volume left, Volume right)
    {
        return left.CompareTo(right) >= 0;
    }
}
