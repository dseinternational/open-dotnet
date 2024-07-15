// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Units;

public static class QuantityExtensions
{
    public static string ToString<TAmount, TUnits>(this IQuantity<TAmount, TUnits> quantity, TUnits units)
        where TAmount : IEquatable<TAmount>, IComparable<TAmount>
        where TUnits : IUnitOfMeasure<TAmount>
    {
        Guard.IsNotNull(quantity);
        return quantity.ToString(null, units);
    }

    public static string ToString<TAmount, TUnits>(this IQuantity<TAmount, TUnits> quantity, string? format, TUnits units)
        where TAmount : IEquatable<TAmount>, IComparable<TAmount>
        where TUnits : IUnitOfMeasure<TAmount>
    {
        Guard.IsNotNull(quantity);
        return quantity.ToString(format, CultureInfo.CurrentCulture, units);
    }

    public static string ToStringInvariant<TAmount, TUnits>(this IQuantity<TAmount, TUnits> quantity)
        where TAmount : IEquatable<TAmount>, IComparable<TAmount>
        where TUnits : IUnitOfMeasure<TAmount>
    {
        Guard.IsNotNull(quantity);
        return quantity.ToString(null, CultureInfo.InvariantCulture, quantity.Units);
    }

    public static string ToStringInvariant<TAmount, TUnits>(this IQuantity<TAmount, TUnits> quantity, TUnits units)
        where TAmount : IEquatable<TAmount>, IComparable<TAmount>
        where TUnits : IUnitOfMeasure<TAmount>
    {
        Guard.IsNotNull(quantity);
        return quantity.ToString(null, CultureInfo.InvariantCulture, units);
    }
}
