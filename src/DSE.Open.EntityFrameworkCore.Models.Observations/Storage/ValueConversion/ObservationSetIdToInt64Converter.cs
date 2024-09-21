// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Observations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Observations.Storage.ValueConversion;

public sealed class ObservationSetIdToInt64Converter : ValueConverter<ObservationSetId, long>
{
    public static readonly ObservationSetIdToInt64Converter Default = new();

    public ObservationSetIdToInt64Converter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    private static long ConvertTo(ObservationSetId value)
    {
        return (long)value;
    }

    private static ObservationSetId ConvertFrom(long value)
    {
        if (ObservationSetId.TryFromInt64(value, out var id))
        {
            return id;
        }

        ValueConversionException.Throw(
            $"Unable to convert {nameof(Int64)} value '{value}' to {nameof(ObservationSetId)}.", value, null);
        return default;
    }
}
