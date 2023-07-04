// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class TimeSpanToInt64MinutesConverter : ValueConverter<TimeSpan, long>
{
    public static readonly TimeSpanToInt64MinutesConverter Default = new();

    public TimeSpanToInt64MinutesConverter() : base(
        t => (long)Math.Round(t.TotalMinutes, 0),
        m => TimeSpan.FromMinutes(m))
    {
    }
}
