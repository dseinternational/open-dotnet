// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class TimeSpanToDoubleHoursConverter : ValueConverter<TimeSpan, double>
{
    public static readonly TimeSpanToDoubleHoursConverter Default = new();

    public TimeSpanToDoubleHoursConverter() : base(
        t => t.TotalHours,
        m => TimeSpan.FromHours(m))
    {
    }
}
