// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class AgeInMonthsToInt16Converter : ValueConverter<AgeInMonths, short>
{
    public static readonly AgeInMonthsToInt16Converter Default = new();

    public AgeInMonthsToInt16Converter()
        : base(c => ConvertToInt16(c), s => ConvertFromInt16(s))
    {
    }

    // keep public for EF Core compiled models
    public static short ConvertToInt16(AgeInMonths code)
    {
        return (short)code.TotalMonths;
    }

    // keep public for EF Core compiled models
    public static AgeInMonths ConvertFromInt16(short code)
    {
        return new(code);
    }
}
