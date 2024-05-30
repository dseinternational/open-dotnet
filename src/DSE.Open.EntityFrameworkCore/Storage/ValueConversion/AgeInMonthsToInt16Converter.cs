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

    private static short ConvertToInt16(AgeInMonths code)
    {
        return (short)code.TotalMonths;
    }

    private static AgeInMonths ConvertFromInt16(short code)
    {
        return new(code);
    }
}
