// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Units;

/// <summary>
/// Defines a unit of measure that quantities of type <typeparamref name="T"/> can be
/// expressed in, identified by name and abbreviation and convertible to a base unit.
/// </summary>
/// <typeparam name="T">The numeric type used to express amounts.</typeparam>
public interface IUnitOfMeasure<T>
    where T : IEquatable<T>, IComparable<T>
{
    /// <summary>
    /// Gets the quantity represented by the unit of measure in base units.
    /// </summary>
    T BaseUnits { get; }

    /// <summary>
    /// Gets the standard name (in English) of the unit of measure.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the standard abbreviation of the unit of measure.
    /// </summary>
    string Abbreviation { get; }

    /// <summary>
    /// Gets the base unit of measure (where <see cref="BaseUnits"/> typically equals 1).
    /// </summary>
    IUnitOfMeasure<T> BaseUnitOfMeasure { get; }
}
