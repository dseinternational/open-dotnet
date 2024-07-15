// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values.Units;

[StructLayout(LayoutKind.Auto)]
[JsonConverter(typeof(JsonStringMassConverter))]
public readonly record struct Mass : IQuantity<double, UnitOfMass>, IComparable<Mass>
{
    private Mass(double valueInGrams)
    {
        Amount = valueInGrams;
    }

    /// <summary>
    /// Gets the amount of the quantity.
    /// </summary>
    public double Amount { get; }

    /// <summary>
    /// Gets the units of the amount of the quantity.
    /// </summary>
    public UnitOfMass Units => UnitOfMass.Gram;

    public double ConvertValueTo(UnitOfMass unitOfMass)
    {
        Guard.IsNotNull(unitOfMass);

        if (unitOfMass == UnitOfMass.Gram)
        {
            return Amount;
        }

        return Amount / unitOfMass.BaseUnits;
    }

    /// <summary>
    /// Gets the mass in grams.
    /// </summary>
    public double Grams => Amount;

    /// <summary>
    /// Gets the mass in kilograms.
    /// </summary>
    public double Kilograms => ConvertValueTo(UnitOfMass.Kilogram);

    public static readonly Mass Zero;

    public static Mass FromGrams(double g)
    {
        return new Mass(g);
    }

    public static Mass FromKilograms(double kg)
    {
        return new Mass(kg * UnitOfMass.Kilogram.BaseUnits);
    }

    public static Mass Parse(string? value)
    {
        if (TryParse(value, out var mass))
        {
            return mass;
        }

        throw new FormatException("Could not parse mass value: " + value);
    }

    public static bool TryParse([NotNullWhen(true)] string? value, out Mass mass)
    {
        return TryParse(value, NumberStyles.Number, null, out mass);
    }

    public static bool TryParse([NotNullWhen(true)] string? value, NumberStyles style, IFormatProvider? formatProvider, out Mass mass)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            var parts = GetParts(value);

            if (parts.Length == 2
                && double.TryParse(parts[0], style, formatProvider, out var amount)
                && UnitOfMass.TryParse(parts[1], out var unitOfMass))
            {
                mass = new Mass(amount * unitOfMass.BaseUnits);
                return true;
            }
        }

        mass = default;
        return false;
    }

    private static string[] GetParts(string value)
    {
        var sb = new StringBuilder(value.Length);

        var amount = string.Empty;
        var startedUnits = false;

        foreach (var c in value.Where(c => !char.IsWhiteSpace(c)))
        {
            if (IsNumeric(c))
            {
                _ = sb.Append(c);
            }
            else
            {
                if (startedUnits)
                {
                    _ = sb.Append(c);
                }
                else
                {
                    amount = sb.ToString();
                    _ = sb.Clear();
                    startedUnits = true;
                    _ = sb.Append(c);
                }
            }
        }

        var units = sb.ToString();

        return [amount, units];
    }

    private static bool IsNumeric(char c)
    {
        return c is (>= '0' and <= '9') or '-' or ',' or '.';
    }

    public override string ToString()
    {
        return ToString(null, null, UnitOfMass.Gram);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return ToString(format, formatProvider, UnitOfMass.Gram);
    }

    public string ToString(string? format, IFormatProvider? formatProvider, UnitOfMass unitOfMass)
    {
        Guard.IsNotNull(unitOfMass);

        return ConvertValueTo(unitOfMass).ToString(format, formatProvider) + unitOfMass.Abbreviation;
    }

    public int CompareTo(Mass other)
    {
        return Amount.CompareTo(other.Amount);
    }

    public static bool operator <(Mass left, Mass right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(Mass left, Mass right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(Mass left, Mass right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(Mass left, Mass right)
    {
        return left.CompareTo(right) >= 0;
    }
}
