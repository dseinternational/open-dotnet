// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Units;

/// <summary>
/// Base class for a unit of measure expressed as a scaling factor relative to the
/// base unit of measure for the quantity, together with a name and an abbreviation.
/// </summary>
/// <typeparam name="T">The numeric type used to express the scaling factor.</typeparam>
public abstract class UnitOfMeasure<T> : IUnitOfMeasure<T>, IEquatable<UnitOfMeasure<T>>, IComparable<UnitOfMeasure<T>>
    where T : IEquatable<T>, IComparable<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfMeasure{T}"/> class.
    /// </summary>
    /// <param name="units">The number of base units represented by one of these units.</param>
    /// <param name="name">The full name of the unit (for example, "kilometre").</param>
    /// <param name="abbreviation">The abbreviation for the unit (for example, "km").</param>
    protected UnitOfMeasure(T units, string name, string abbreviation)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentException.ThrowIfNullOrEmpty(abbreviation);

        BaseUnits = units;
        Name = name;
        Abbreviation = abbreviation;
    }

    /// <summary>
    /// Gets the number of base units represented by one of these units.
    /// </summary>
    public T BaseUnits { get; }

    /// <summary>
    /// Gets the full name of the unit.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the abbreviation for the unit.
    /// </summary>
    public string Abbreviation { get; }

    /// <summary>
    /// Gets the base unit of measure to which <see cref="BaseUnits"/> is relative.
    /// </summary>
    public abstract UnitOfMeasure<T> BaseUnitOfMeasure { get; }

    IUnitOfMeasure<T> IUnitOfMeasure<T>.BaseUnitOfMeasure => BaseUnitOfMeasure;

    /// <summary>
    /// Returns the <see cref="Name"/> of this unit of measure.
    /// </summary>
    public override string ToString()
    {
        return Name;
    }

    private static bool Equals(UnitOfMeasure<T>? unit1, UnitOfMeasure<T>? unit2)
    {
        return ReferenceEquals(unit1, unit2)
            || (unit1 is not null
                && unit2 is not null
                && unit1.BaseUnits.Equals(unit2.BaseUnits)
                && string.Equals(unit1.Abbreviation, unit2.Abbreviation, StringComparison.Ordinal)
                && string.Equals(unit1.Name, unit2.Name, StringComparison.Ordinal));
    }

    /// <summary>
    /// Returns a value indicating whether this unit of measure is equal to <paramref name="other"/>
    /// by comparing <see cref="BaseUnits"/>, <see cref="Abbreviation"/> and <see cref="Name"/>.
    /// </summary>
    public bool Equals(UnitOfMeasure<T>? other)
    {
        return other is not null && Equals(this, other);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is UnitOfMeasure<T> measure && Equals(this, measure);
    }

    private static int Compare(UnitOfMeasure<T> unit1, UnitOfMeasure<T> unit2)
    {
        if (ReferenceEquals(unit1, unit2))
        {
            return 0;
        }

        var abbrComparison = string.CompareOrdinal(unit1.Abbreviation, unit2.Abbreviation);

        return abbrComparison != 0 ? abbrComparison : unit1.BaseUnits.CompareTo(unit2.BaseUnits);
    }

    /// <summary>
    /// Compares this unit of measure to <paramref name="other"/>, ordering first by
    /// <see cref="Abbreviation"/> (ordinal) and then by <see cref="BaseUnits"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
    public int CompareTo(UnitOfMeasure<T>? other)
    {
        ArgumentNullException.ThrowIfNull(other);
        return Compare(this, other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(BaseUnits, Abbreviation, Name);
    }

    /// <summary>Indicates whether two units of measure are equal.</summary>
    public static bool operator ==(UnitOfMeasure<T> left, UnitOfMeasure<T> right)
    {
        return Equals(left, right);
    }

    /// <summary>Indicates whether two units of measure are not equal.</summary>
    public static bool operator !=(UnitOfMeasure<T> left, UnitOfMeasure<T> right)
    {
        return !(left == right);
    }

    /// <summary>Indicates whether <paramref name="left"/> sorts before <paramref name="right"/>.</summary>
    public static bool operator <(UnitOfMeasure<T> left, UnitOfMeasure<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        return left.CompareTo(right) < 0;
    }

    /// <summary>Indicates whether <paramref name="left"/> sorts before or equal to <paramref name="right"/>.</summary>
    public static bool operator <=(UnitOfMeasure<T> left, UnitOfMeasure<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        return left.CompareTo(right) <= 0;
    }

    /// <summary>Indicates whether <paramref name="left"/> sorts after <paramref name="right"/>.</summary>
    public static bool operator >(UnitOfMeasure<T> left, UnitOfMeasure<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        return left.CompareTo(right) > 0;
    }

    /// <summary>Indicates whether <paramref name="left"/> sorts after or equal to <paramref name="right"/>.</summary>
    public static bool operator >=(UnitOfMeasure<T> left, UnitOfMeasure<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        return left.CompareTo(right) >= 0;
    }
}
