// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values.Units;

[StructLayout(LayoutKind.Auto)]
[JsonConverter(typeof(JsonStringLengthConverter))]
public readonly record struct Length : IQuantity<double, UnitOfLength>, IComparable<Length>
{
    private Length(double valueInMillimetres)
    {
        Amount = valueInMillimetres;
    }

    /// <summary>
    /// Gets the amount of the quantity.
    /// </summary>
    public double Amount { get; }

    /// <summary>
    /// Gets the units of the amount of the quantity.
    /// </summary>
    public UnitOfLength Units => UnitOfLength.Millimetre;

    public double ConvertValueTo(UnitOfLength unitOfLength)
    {
        Guard.IsNotNull(unitOfLength);

        if (unitOfLength == UnitOfLength.Millimetre)
        {
            return Amount;
        }

        return Amount / unitOfLength.BaseUnits;
    }

    /// <summary>
    /// Gets the length in millimetres.
    /// </summary>
    public double Millimetres => Amount;

    /// <summary>
    /// Gets the length in centimetres.
    /// </summary>
    public double Centimetres => ConvertValueTo(UnitOfLength.Centimetre);

    /// <summary>
    /// Gets the length in metres.
    /// </summary>
    public double Metres => ConvertValueTo(UnitOfLength.Metre);

    /// <summary>
    /// Gets the length in kilometres.
    /// </summary>
    public double Kilometres => ConvertValueTo(UnitOfLength.Kilometre);

    public static readonly Length Zero;

    public static Length FromMillimetres(double mm)
    {
        return new Length(mm);
    }

    public static Length FromCentimetres(double cm)
    {
        return new Length(cm * UnitOfLength.Centimetre.BaseUnits);
    }

    public static Length FromMetres(double cm)
    {
        return new Length(cm * UnitOfLength.Metre.BaseUnits);
    }

    private static readonly char[] s_separator = [' '];

    public static bool TryParse([NotNullWhen(true)] string? value, out Length length)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            var parts = value.Split(s_separator, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 2
                && double.TryParse(parts[0], out var amount)
                && UnitOfLength.TryParse(parts[1], out var unitOfMass))
            {
                length = new Length(amount * unitOfMass.BaseUnits);
                return true;
            }
        }

        length = default;
        return false;
    }

    public override string ToString()
    {
        return ToString(null, null, UnitOfLength.Millimetre);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return ToString(format, formatProvider, UnitOfLength.Millimetre);
    }

    public string ToString(string? format, IFormatProvider? formatProvider, UnitOfLength unitOfMass)
    {
        Guard.IsNotNull(unitOfMass);

        return ConvertValueTo(unitOfMass).ToString(format, formatProvider) + " " + unitOfMass.Abbreviation;
    }

    public int CompareTo(Length other)
    {
        return Amount.CompareTo(other.Amount);
    }

    public static bool operator <(Length left, Length right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(Length left, Length right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(Length left, Length right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(Length left, Length right)
    {
        return left.CompareTo(right) >= 0;
    }
}
