// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class TimeSpanToInt16HoursConverter : ValueConverter<TimeSpan, short>
{
    public static readonly TimeSpanToInt16HoursConverter Default = new();

    public TimeSpanToInt16HoursConverter() : base(
        t => (short)Math.Round(t.TotalHours, 0),
        m => TimeSpan.FromHours(m))
    {
    }
}
