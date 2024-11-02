// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Observations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Observations.Storage.ValueConversion;

public sealed class ObservationIdToInt64Converter : ValueConverter<ObservationId, long>
{
    public static readonly ObservationIdToInt64Converter Default = new();

    public ObservationIdToInt64Converter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    // public for EF Core model compilation
    public static long ConvertTo(ObservationId value)
    {
        return (long)value;
    }

    // public for EF Core model compilation
    public static ObservationId ConvertFrom(long value)
    {
        if (ObservationId.TryFromInt64(value, out var id))
        {
            return id;
        }

        ValueConversionException.Throw(
            $"Unable to convert {nameof(Int64)} value '{value}' to {nameof(ObservationId)}.", value, null);
        return default;
    }
}
