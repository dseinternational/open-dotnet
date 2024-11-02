// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class DateTimeOffsetToUtcDateTimeConverter : ValueConverter<DateTimeOffset, DateTime>
{
    public static readonly DateTimeOffsetToUtcDateTimeConverter Default = new();

    public DateTimeOffsetToUtcDateTimeConverter()
        : base(v => ToUtcTicks(v), v => FromUtcTicks(v))
    {
    }

    // keep public for EF Core compiled models
    public static DateTime ToUtcTicks(DateTimeOffset value)
    {
        return value.UtcDateTime;
    }

    // keep public for EF Core compiled models
    public static DateTimeOffset FromUtcTicks(DateTime value)
    {
        return new(value, TimeSpan.Zero);
    }
}
