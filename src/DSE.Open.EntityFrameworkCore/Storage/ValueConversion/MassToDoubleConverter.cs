// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values.Units;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// EF Core value converter that maps <see cref="Mass"/> values to and from <see cref="double"/> grams.
/// </summary>
public sealed class MassToDoubleConverter : ValueConverter<Mass, double>
{
    public static readonly MassToDoubleConverter Default = new();

    public MassToDoubleConverter()
        : base(c => ConvertToDouble(c), s => ConvertFromDouble(s))
    {
    }

    // keep public for EF Core compiled models
    public static double ConvertToDouble(Mass value)
    {
        return value.Amount;
    }

    // keep public for EF Core compiled models
    public static Mass ConvertFromDouble(double value)
    {
        return Mass.FromGrams(value);
    }
}
