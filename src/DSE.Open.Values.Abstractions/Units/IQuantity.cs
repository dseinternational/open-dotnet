// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Units;

public interface IQuantity<TAmount, TUnits> : IFormattable
    where TAmount : IEquatable<TAmount>, IComparable<TAmount>
    where TUnits : IUnitOfMeasure<TAmount>
{
    TAmount Amount { get; }

    TUnits Units { get; }

    string ToString(string? format, IFormatProvider? formatProvider, TUnits unitOfMass);
}
