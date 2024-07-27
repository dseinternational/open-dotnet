// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class UInt32ToInt64Converter : ValueConverter<uint, long>
{
    public static readonly UInt32ToInt64Converter Default = new();

    public UInt32ToInt64Converter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    private static long ConvertTo(uint value)
    {
        return value;
    }

    private static uint ConvertFrom(long value)
    {
        checked
        {
            return (uint)value;
        }
    }
}
