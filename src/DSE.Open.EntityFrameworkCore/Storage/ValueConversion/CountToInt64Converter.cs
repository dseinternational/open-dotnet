﻿// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class CountToInt64Converter : ValueConverter<Count, long>
{
    public static readonly CountToInt64Converter Default = new();

    public CountToInt64Converter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    private static long ConvertTo(Count value)
    {
        return value;
    }

    private static Count ConvertFrom(long value)
    {
        if (Count.TryFromValue(value, out var result))
        {
            return result;
        }

        ValueConversionException.Throw($"Could not convert value {value} to {nameof(Count)}");
        return default; // unreachable
    }
}
