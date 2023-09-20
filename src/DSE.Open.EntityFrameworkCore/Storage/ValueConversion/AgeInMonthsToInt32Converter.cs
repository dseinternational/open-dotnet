// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class AgeInMonthsToInt32Converter : ValueConverter<AgeInMonths, int>
{
    public static readonly AgeInMonthsToInt32Converter Default = new();

    public AgeInMonthsToInt32Converter()
        : base(c => ConvertToInt32(c), s => ConvertFromInt32(s))
    {
    }

    private static int ConvertToInt32(AgeInMonths code) => code.TotalMonths;

    private static AgeInMonths ConvertFromInt32(int code) => new(code);
}
