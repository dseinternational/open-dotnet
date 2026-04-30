// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Observations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Observations.Storage.ValueConversion;

/// <summary>
/// Converts <see cref="ObservationId"/> values to and from <see cref="long"/> for storage.
/// </summary>
public sealed class ObservationIdToInt64Converter : ValueConverter<ObservationId, long>
{
    /// <summary>
    /// Gets a default, shared instance of <see cref="ObservationIdToInt64Converter"/>.
    /// </summary>
    public static readonly ObservationIdToInt64Converter Default = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ObservationIdToInt64Converter"/> class.
    /// </summary>
    public ObservationIdToInt64Converter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    /// <summary>
    /// Converts an <see cref="ObservationId"/> to a <see cref="long"/> for storage.
    /// </summary>
    /// <remarks>Kept public for EF Core model compilation.</remarks>
    public static long ConvertTo(ObservationId value)
    {
        return (long)value;
    }

    /// <summary>
    /// Converts a stored <see cref="long"/> back to an <see cref="ObservationId"/>.
    /// </summary>
    /// <remarks>Kept public for EF Core model compilation.</remarks>
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
