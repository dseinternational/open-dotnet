// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class TimeSpanToInt16MinutesConverter : ValueConverter<TimeSpan, short>
{
    public static readonly TimeSpanToInt16MinutesConverter Default = new();

    public TimeSpanToInt16MinutesConverter() : base(
        t => (short)Math.Round(t.TotalMinutes, 0),
        m => TimeSpan.FromMinutes(m))
    {
    }
}
