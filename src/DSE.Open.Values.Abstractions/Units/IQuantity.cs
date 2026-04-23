// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Units;

/// <summary>
/// Represents a physical quantity with an <see cref="Amount"/> measured in specific
/// <see cref="Units"/>.
/// </summary>
/// <typeparam name="TAmount">The numeric type used to express the amount.</typeparam>
/// <typeparam name="TUnits">The unit of measure type.</typeparam>
public interface IQuantity<TAmount, TUnits> : IFormattable
    where TAmount : IEquatable<TAmount>, IComparable<TAmount>
    where TUnits : IUnitOfMeasure<TAmount>
{
    /// <summary>Gets the numeric amount of this quantity.</summary>
    TAmount Amount { get; }

    /// <summary>Gets the unit of measure for this quantity.</summary>
    TUnits Units { get; }

    /// <summary>
    /// Formats the quantity as a string, converting the amount to the specified
    /// <paramref name="unitOfMass"/>.
    /// </summary>
    string ToString(string? format, IFormatProvider? formatProvider, TUnits unitOfMass);
}
