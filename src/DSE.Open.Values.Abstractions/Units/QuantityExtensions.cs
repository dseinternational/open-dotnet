// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Units;

/// <summary>
/// Provides extension methods for formatting <see cref="IQuantity{TAmount, TUnits}"/> values.
/// </summary>
public static class QuantityExtensions
{
    /// <summary>
    /// Formats <paramref name="quantity"/> using the current culture, converting the amount
    /// to the specified <paramref name="units"/>.
    /// </summary>
    public static string ToString<TAmount, TUnits>(this IQuantity<TAmount, TUnits> quantity, TUnits units)
        where TAmount : IEquatable<TAmount>, IComparable<TAmount>
        where TUnits : IUnitOfMeasure<TAmount>
    {
        ArgumentNullException.ThrowIfNull(quantity);
        return quantity.ToString(null, units);
    }

    /// <summary>
    /// Formats <paramref name="quantity"/> using the specified <paramref name="format"/> and
    /// the current culture, converting the amount to the specified <paramref name="units"/>.
    /// </summary>
    public static string ToString<TAmount, TUnits>(this IQuantity<TAmount, TUnits> quantity, string? format, TUnits units)
        where TAmount : IEquatable<TAmount>, IComparable<TAmount>
        where TUnits : IUnitOfMeasure<TAmount>
    {
        ArgumentNullException.ThrowIfNull(quantity);
        return quantity.ToString(format, CultureInfo.CurrentCulture, units);
    }

    /// <summary>
    /// Formats <paramref name="quantity"/> using the invariant culture and the quantity's own units.
    /// </summary>
    public static string ToStringInvariant<TAmount, TUnits>(this IQuantity<TAmount, TUnits> quantity)
        where TAmount : IEquatable<TAmount>, IComparable<TAmount>
        where TUnits : IUnitOfMeasure<TAmount>
    {
        ArgumentNullException.ThrowIfNull(quantity);
        return quantity.ToString(null, CultureInfo.InvariantCulture, quantity.Units);
    }

    /// <summary>
    /// Formats <paramref name="quantity"/> using the invariant culture, converting the amount
    /// to the specified <paramref name="units"/>.
    /// </summary>
    public static string ToStringInvariant<TAmount, TUnits>(this IQuantity<TAmount, TUnits> quantity, TUnits units)
        where TAmount : IEquatable<TAmount>, IComparable<TAmount>
        where TUnits : IUnitOfMeasure<TAmount>
    {
        ArgumentNullException.ThrowIfNull(quantity);
        return quantity.ToString(null, CultureInfo.InvariantCulture, units);
    }
}
