// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Observations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Observations.Storage.ValueConversion;

/// <summary>
/// Converts <see cref="MeasureId"/> values to and from <see cref="long"/> for storage.
/// </summary>
public sealed class MeasureIdToInt64Converter : ValueConverter<MeasureId, long>
{
    /// <summary>
    /// Gets a default, shared instance of <see cref="MeasureIdToInt64Converter"/>.
    /// </summary>
    public static readonly MeasureIdToInt64Converter Default = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="MeasureIdToInt64Converter"/> class.
    /// </summary>
    public MeasureIdToInt64Converter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    /// <summary>
    /// Converts a <see cref="MeasureId"/> to a <see cref="long"/> for storage.
    /// </summary>
    /// <remarks>Kept public for EF Core model compilation.</remarks>
    public static long ConvertTo(MeasureId value)
    {
        return (long)value;
    }

    /// <summary>
    /// Converts a stored <see cref="long"/> back to a <see cref="MeasureId"/>.
    /// </summary>
    /// <remarks>Kept public for EF Core model compilation.</remarks>
    public static MeasureId ConvertFrom(long value)
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
