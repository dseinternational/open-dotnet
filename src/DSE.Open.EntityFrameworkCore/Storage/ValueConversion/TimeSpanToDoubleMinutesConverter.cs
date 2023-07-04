// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class TimeSpanToDoubleMinutesConverter : ValueConverter<TimeSpan, double>
{
    public static readonly TimeSpanToDoubleMinutesConverter Default = new();

    public TimeSpanToDoubleMinutesConverter() : base(
        t => t.TotalMinutes,
        m => TimeSpan.FromMinutes(m))
    {
    }
}
