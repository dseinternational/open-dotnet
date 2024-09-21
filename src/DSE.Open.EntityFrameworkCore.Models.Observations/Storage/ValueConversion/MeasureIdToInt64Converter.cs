// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Observations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Observations.Storage.ValueConversion;

public sealed class MeasureIdToInt64Converter : ValueConverter<MeasureId, long>
{
    public static readonly MeasureIdToInt64Converter Default = new();

    public MeasureIdToInt64Converter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    private static long ConvertTo(MeasureId value)
    {
        return (long)value;
    }

    private static MeasureId ConvertFrom(long value)
    {
        if (MeasureId.TryFromInt64(value, out var id))
        {
            return id;
        }

        ValueConversionException.Throw(
            $"Unable to convert {nameof(Int64)} value '{value}' to {nameof(MeasureId)}.", value, null);
        return default;
    }
}
