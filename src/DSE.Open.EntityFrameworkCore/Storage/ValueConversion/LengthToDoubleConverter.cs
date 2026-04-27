// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values.Units;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// EF Core value converter that maps <see cref="Length"/> values to and from <see cref="double"/> millimetres.
/// </summary>
public sealed class LengthToDoubleConverter : ValueConverter<Length, double>
{
    public static readonly LengthToDoubleConverter Default = new();

    public LengthToDoubleConverter()
        : base(c => ConvertToDouble(c), s => ConvertFromDouble(s))
    {
    }

    // keep public for EF Core compiled models
    public static double ConvertToDouble(Length value)
    {
        return value.Amount;
    }

    // keep public for EF Core compiled models
    public static Length ConvertFromDouble(double value)
    {
        return Length.FromMillimetres(value);
    }
}
