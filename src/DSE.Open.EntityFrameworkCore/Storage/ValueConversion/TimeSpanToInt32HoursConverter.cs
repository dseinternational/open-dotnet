// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class TimeSpanToInt32HoursConverter : ValueConverter<TimeSpan, int>
{
    public static readonly TimeSpanToInt32HoursConverter Default = new();

    public TimeSpanToInt32HoursConverter() : base(
        t => (int)Math.Round(t.TotalHours, 0),
        m => TimeSpan.FromHours(m))
    {
    }
}
