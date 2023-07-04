// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class TimeSpanToInt64HoursConverter : ValueConverter<TimeSpan, long>
{
    public static readonly TimeSpanToInt64HoursConverter Default = new();

    public TimeSpanToInt64HoursConverter() : base(
        t => (long)Math.Round(t.TotalHours, 0),
        m => TimeSpan.FromHours(m))
    {
    }
}
