// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class UInt64ToInt64Converter : ValueConverter<ulong, long>
{
    public static readonly UInt64ToInt64Converter Default = new();

    public UInt64ToInt64Converter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    // keep public for EF Core compiled models
    public static long ConvertTo(ulong value)
    {
        checked
        {
            return (long)value;
        }
    }

    // keep public for EF Core compiled models
    public static ulong ConvertFrom(long value)
    {
        return (ulong)value;
    }
}
