// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using DSE.Open.Hashing;

namespace DSE.Open.Values.Units;

/// <summary>
/// Represents a volume quantity, stored as an <see cref="Amount"/> together with the
/// <see cref="UnitOfVolume"/> in which the amount is expressed.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public readonly record struct Volume : IQuantity<double, UnitOfVolume>, IComparable<Volume>, IRepeatableHash64
{
    private Volume(double amount, UnitOfVolume measure)
    {
        Amount = amount;
        Units = measure;
    }

    /// <summary>
    /// Creates a <see cref="Volume"/> with the given amount expressed in cubic metres.
    /// </summary>
    public static Volume CubicMetre(double amount)
    {
        return new Volume(amount, UnitOfVolume.CubicMetre);
    }

    /// <summary>
    /// Creates a <see cref="Volume"/> with the given amount expressed in cubic centimetres.
    /// </summary>
    public static Volume CubicCentimetre(double amount)
    {
        return new Volume(amount, UnitOfVolume.CubicCentimetre);
    }

    /// <summary>
    /// Creates a <see cref="Volume"/> with the given amount expressed in litres.
    /// </summary>
    public static Volume Litre(double amount)
    {
        return new Volume(amount, UnitOfVolume.Litre);
    }

    /// <summary>
    /// Creates a <see cref="Volume"/> with the given amount expressed in millilitres.
    /// </summary>
    public static Volume Millilitre(double amount)
    {
        return new Volume(amount, UnitOfVolume.Millilitre);
    }

    /// <summary>
    /// Gets the amount of the quantity in <see cref="Units"/>.
    /// </summary>
    public double Amount { get; }

    /// <summary>
    /// Gets the units in which <see cref="Amount"/> is expressed.
    /// </summary>
    public UnitOfVolume Units { get; }

    /// <summary>
    /// Returns the volume expressed in the given <paramref name="unitOfVolume"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="unitOfVolume"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">This <see cref="Volume"/> is the default value and has no units.</exception>
    public double ConvertValueTo(UnitOfVolume unitOfVolume)
    {
        ArgumentNullException.ThrowIfNull(unitOfVolume);

        var units = GetRequiredUnits();

        if (unitOfVolume == units)
        {
            return Amount;
        }

        return (Amount * units.BaseUnits) / unitOfVolume.BaseUnits;
    }

    /// <summary>
    /// Compares this volume to <paramref name="other"/> by converting both amounts to
    /// the common base unit before comparing.
    /// </summary>
    public int CompareTo(Volume other)
    {
        return (Amount * GetRequiredUnits().BaseUnits).CompareTo(other.Amount * other.GetRequiredUnits().BaseUnits);
    }

    /// <summary>
    /// Returns a string of the form "&lt;amount&gt; &lt;unit-abbreviation&gt;" using
    /// <see cref="Amount"/> and <see cref="Units"/>.
    /// </summary>
    public override string ToString()
    {
        return ToString(null, null, GetRequiredUnits());
    }

    /// <summary>
    /// Formats the volume in its current <see cref="Units"/> using the given format and format provider.
    /// </summary>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return ToString(format, formatProvider, GetRequiredUnits());
    }

    /// <summary>
    /// Formats the volume in the given <paramref name="unitOfMass"/> using the supplied
    /// format and format provider, appending the unit's abbreviation.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="unitOfMass"/> is <see langword="null"/>.</exception>
    public string ToString(string? format, IFormatProvider? formatProvider, UnitOfVolume unitOfMass)
    {
        ArgumentNullException.ThrowIfNull(unitOfMass);
        return ConvertValueTo(unitOfMass).ToString(format, formatProvider) + " " + unitOfMass.Abbreviation;
    }

    private UnitOfVolume GetRequiredUnits()
    {
        var units = Units;

        if (ReferenceEquals(units, null))
        {
            throw new InvalidOperationException("Cannot convert or format a default Volume because units are not set.");
        }

        return units;
    }

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        var h0 = RepeatableHash64Provider.Default.GetRepeatableHashCode(Amount);
        var h1 = GetRequiredUnits().GetRepeatableHashCode();
        return RepeatableHash64Provider.Default.CombineHashCodes(h0, h1);
    }

    /// <summary>Indicates whether <paramref name="left"/> is less than <paramref name="right"/>.</summary>
    public static bool operator <(Volume left, Volume right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>Indicates whether <paramref name="left"/> is less than or equal to <paramref name="right"/>.</summary>
    public static bool operator <=(Volume left, Volume right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <summary>Indicates whether <paramref name="left"/> is greater than <paramref name="right"/>.</summary>
    public static bool operator >(Volume left, Volume right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <summary>Indicates whether <paramref name="left"/> is greater than or equal to <paramref name="right"/>.</summary>
    public static bool operator >=(Volume left, Volume right)
    {
        return left.CompareTo(right) >= 0;
    }
}
