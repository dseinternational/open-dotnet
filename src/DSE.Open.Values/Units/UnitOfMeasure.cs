// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Units;

public abstract class UnitOfMeasure<T> : IUnitOfMeasure<T>, IEquatable<UnitOfMeasure<T>>, IComparable<UnitOfMeasure<T>>
    where T : IEquatable<T>, IComparable<T>
{
    protected UnitOfMeasure(T units, string name, string abbreviation)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentException.ThrowIfNullOrEmpty(abbreviation);

        BaseUnits = units;
        Name = name;
        Abbreviation = abbreviation;
    }

    public T BaseUnits { get; }

    public string Name { get; }

    public string Abbreviation { get; }

    public abstract UnitOfMeasure<T> BaseUnitOfMeasure { get; }

    IUnitOfMeasure<T> IUnitOfMeasure<T>.BaseUnitOfMeasure => ((IUnitOfMeasure<T>)BaseUnitOfMeasure).BaseUnitOfMeasure;

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

    public bool Equals(UnitOfMeasure<T>? other)
    {
        Guard.IsNotNull(other);
        return Equals(this, other);
    }

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

    public int CompareTo(UnitOfMeasure<T>? other)
    {
        Guard.IsNotNull(other);
        return Compare(this, other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(BaseUnits, Abbreviation, Name);
    }

    public static bool operator ==(UnitOfMeasure<T> left, UnitOfMeasure<T> right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(UnitOfMeasure<T> left, UnitOfMeasure<T> right)
    {
        return !(left == right);
    }

    public static bool operator <(UnitOfMeasure<T> left, UnitOfMeasure<T> right)
    {
        Guard.IsNotNull(left);
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(UnitOfMeasure<T> left, UnitOfMeasure<T> right)
    {
        Guard.IsNotNull(left);
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(UnitOfMeasure<T> left, UnitOfMeasure<T> right)
    {
        Guard.IsNotNull(left);
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(UnitOfMeasure<T> left, UnitOfMeasure<T> right)
    {
        Guard.IsNotNull(left);
        return left.CompareTo(right) >= 0;
    }
}

