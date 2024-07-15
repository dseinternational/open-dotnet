// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Units;

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
