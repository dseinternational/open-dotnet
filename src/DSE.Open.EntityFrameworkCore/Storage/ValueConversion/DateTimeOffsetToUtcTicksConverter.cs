// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class DateTimeOffsetToUtcTicksConverter : ValueConverter<DateTimeOffset, long>
{
    public static readonly DateTimeOffsetToUtcTicksConverter Default = new();

    public DateTimeOffsetToUtcTicksConverter()
        : base(v => ToUtcTicks(v), v => FromUtcTicks(v))
    {
    }

    private static long ToUtcTicks(DateTimeOffset value)
    {
        return value.UtcTicks;
    }

    private static DateTimeOffset FromUtcTicks(long value)
    {
        return new DateTimeOffset(value, TimeSpan.Zero);
    }
}
