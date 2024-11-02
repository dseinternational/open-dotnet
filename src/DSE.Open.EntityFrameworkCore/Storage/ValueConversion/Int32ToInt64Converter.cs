// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class Int32ToInt64Converter : ValueConverter<int, long>
{
    public static readonly Int32ToInt64Converter Default = new();

    public Int32ToInt64Converter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    // keep public for EF Core compiled models
    public static long ConvertTo(int value)
    {
        return value;
    }

    // keep public for EF Core compiled models
    public static int ConvertFrom(long value)
    {
        checked
        {
            return (int)value;
        }
    }
}
