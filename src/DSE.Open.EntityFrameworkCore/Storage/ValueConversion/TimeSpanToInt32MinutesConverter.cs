// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class TimeSpanToInt32MinutesConverter : ValueConverter<TimeSpan, int>
{
    public static readonly TimeSpanToInt32MinutesConverter Default = new();

    public TimeSpanToInt32MinutesConverter() : base(
        t => (int)Math.Round(t.TotalMinutes, 0),
        m => TimeSpan.FromMinutes(m))
    {
    }
}
