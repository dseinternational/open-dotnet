// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values.Units;

/// <summary>
/// Represents a mass quantity, stored internally in grams.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(JsonStringMassConverter))]
public readonly record struct Mass : IQuantity<double, UnitOfMass>, IComparable<Mass>, IRepeatableHash64
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

    /// <summary>
    /// Returns the mass expressed in the given <paramref name="unitOfMass"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="unitOfMass"/> is <see langword="null"/>.</exception>
    public double ConvertValueTo(UnitOfMass unitOfMass)
    {
        ArgumentNullException.ThrowIfNull(unitOfMass);

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

    /// <summary>
    /// A <see cref="Mass"/> of zero grams.
    /// </summary>
    public static readonly Mass Zero;

    /// <summary>
    /// Creates a <see cref="Mass"/> from a value in grams.
    /// </summary>
    public static Mass FromGrams(double g)
    {
        return new Mass(g);
    }

    /// <summary>
    /// Creates a <see cref="Mass"/> from a value in kilograms.
    /// </summary>
    public static Mass FromKilograms(double kg)
    {
        return new Mass(kg * UnitOfMass.Kilogram.BaseUnits);
    }

    /// <summary>
    /// Parses a string of the form "&lt;amount&gt;&lt;unit-abbreviation&gt;" (for
    /// example, "2.5kg") into a <see cref="Mass"/>.
    /// </summary>
    /// <exception cref="FormatException">The value could not be parsed.</exception>
    public static Mass Parse(string? value)
    {
        if (TryParse(value, out var mass))
        {
            return mass;
        }

        throw new FormatException("Could not parse mass value: " + value);
    }

    /// <summary>
    /// Attempts to parse a string of the form "&lt;amount&gt;&lt;unit-abbreviation&gt;"
    /// into a <see cref="Mass"/> using <see cref="NumberStyles.Number"/> and the current
    /// culture.
    /// </summary>
    /// <returns><see langword="true"/> if the value was parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse([NotNullWhen(true)] string? value, out Mass mass)
    {
        return TryParse(value, NumberStyles.Number, null, out mass);
    }

    /// <summary>
    /// Attempts to parse a string of the form "&lt;amount&gt;&lt;unit-abbreviation&gt;"
    /// into a <see cref="Mass"/>, using the given number style and format provider for
    /// the numeric portion.
    /// </summary>
    /// <returns><see langword="true"/> if the value was parsed successfully; otherwise, <see langword="false"/>.</returns>
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

    /// <summary>
    /// Returns a string of the form "&lt;amount&gt;g" using the value in grams.
    /// </summary>
    public override string ToString()
    {
        return ToString(null, null, UnitOfMass.Gram);
    }

    /// <summary>
    /// Formats the mass in grams using the given format and format provider.
    /// </summary>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return ToString(format, formatProvider, UnitOfMass.Gram);
    }

    /// <summary>
    /// Formats the mass in the given <paramref name="unitOfMass"/> using the supplied
    /// format and format provider, appending the unit's abbreviation.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="unitOfMass"/> is <see langword="null"/>.</exception>
    public string ToString(string? format, IFormatProvider? formatProvider, UnitOfMass unitOfMass)
    {
        ArgumentNullException.ThrowIfNull(unitOfMass);

        return ConvertValueTo(unitOfMass).ToString(format, formatProvider) + unitOfMass.Abbreviation;
    }

    /// <inheritdoc/>
    public int CompareTo(Mass other)
    {
        return Amount.CompareTo(other.Amount);
    }

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        var h0 = RepeatableHash64Provider.Default.GetRepeatableHashCode(Amount);
        var h1 = Units.GetRepeatableHashCode();
        return RepeatableHash64Provider.Default.CombineHashCodes(h0, h1);
    }

    /// <summary>Indicates whether <paramref name="left"/> is less than <paramref name="right"/>.</summary>
    public static bool operator <(Mass left, Mass right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>Indicates whether <paramref name="left"/> is less than or equal to <paramref name="right"/>.</summary>
    public static bool operator <=(Mass left, Mass right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <summary>Indicates whether <paramref name="left"/> is greater than <paramref name="right"/>.</summary>
    public static bool operator >(Mass left, Mass right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <summary>Indicates whether <paramref name="left"/> is greater than or equal to <paramref name="right"/>.</summary>
    public static bool operator >=(Mass left, Mass right)
    {
        return left.CompareTo(right) >= 0;
    }
}
