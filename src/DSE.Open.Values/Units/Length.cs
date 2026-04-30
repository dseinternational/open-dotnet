// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values.Units;

/// <summary>
/// Represents a length quantity, stored internally in millimetres.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(JsonStringLengthConverter))]
public readonly record struct Length
    : IQuantity<double, UnitOfLength>,
      IComparable<Length>,
      IRepeatableHash64
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

    /// <summary>
    /// Returns the length expressed in the given <paramref name="unitOfLength"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="unitOfLength"/> is <see langword="null"/>.</exception>
    public double ConvertValueTo(UnitOfLength unitOfLength)
    {
        ArgumentNullException.ThrowIfNull(unitOfLength);

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

    /// <summary>
    /// A <see cref="Length"/> of zero millimetres.
    /// </summary>
    public static readonly Length Zero;

    /// <summary>
    /// Creates a <see cref="Length"/> from a value in millimetres.
    /// </summary>
    public static Length FromMillimetres(double mm)
    {
        return new Length(mm);
    }

    /// <summary>
    /// Creates a <see cref="Length"/> from a value in centimetres.
    /// </summary>
    public static Length FromCentimetres(double cm)
    {
        return new Length(cm * UnitOfLength.Centimetre.BaseUnits);
    }

    /// <summary>
    /// Creates a <see cref="Length"/> from a value in metres.
    /// </summary>
    public static Length FromMetres(double cm)
    {
        return new Length(cm * UnitOfLength.Metre.BaseUnits);
    }

    private static readonly char[] s_separator = [' '];

    /// <summary>
    /// Attempts to parse a string of the form "&lt;amount&gt; &lt;unit-abbreviation&gt;"
    /// (for example, "5 cm") into a <see cref="Length"/>.
    /// </summary>
    /// <returns><see langword="true"/> if the value was parsed successfully; otherwise, <see langword="false"/>.</returns>
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

    /// <summary>
    /// Returns a string of the form "&lt;amount&gt; mm" using the value in millimetres.
    /// </summary>
    public override string ToString()
    {
        return ToString(null, null, UnitOfLength.Millimetre);
    }

    /// <summary>
    /// Formats the length in millimetres using the given format and format provider.
    /// </summary>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return ToString(format, formatProvider, UnitOfLength.Millimetre);
    }

    /// <summary>
    /// Formats the length in the given <paramref name="unitOfMass"/> using the supplied
    /// format and format provider, appending the unit's abbreviation.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="unitOfMass"/> is <see langword="null"/>.</exception>
    public string ToString(string? format, IFormatProvider? formatProvider, UnitOfLength unitOfMass)
    {
        ArgumentNullException.ThrowIfNull(unitOfMass);

        return ConvertValueTo(unitOfMass).ToString(format, formatProvider) + " " + unitOfMass.Abbreviation;
    }

    /// <inheritdoc/>
    public int CompareTo(Length other)
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
    public static bool operator <(Length left, Length right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>Indicates whether <paramref name="left"/> is less than or equal to <paramref name="right"/>.</summary>
    public static bool operator <=(Length left, Length right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <summary>Indicates whether <paramref name="left"/> is greater than <paramref name="right"/>.</summary>
    public static bool operator >(Length left, Length right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <summary>Indicates whether <paramref name="left"/> is greater than or equal to <paramref name="right"/>.</summary>
    public static bool operator >=(Length left, Length right)
    {
        return left.CompareTo(right) >= 0;
    }
}
