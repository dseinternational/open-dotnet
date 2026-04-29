// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Observations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Observations.Storage.ValueConversion;

/// <summary>
/// Converts <see cref="Count"/> values to and from <see cref="long"/> for storage.
/// </summary>
public sealed class CountToInt64Converter : ValueConverter<Count, long>
{
    /// <summary>
    /// Gets a default, shared instance of <see cref="CountToInt64Converter"/>.
    /// </summary>
    public static readonly CountToInt64Converter Default = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="CountToInt64Converter"/> class.
    /// </summary>
    public CountToInt64Converter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    /// <summary>
    /// Converts a <see cref="Count"/> to a <see cref="long"/> for storage.
    /// </summary>
    /// <remarks>Kept public for EF Core compiled models.</remarks>
    public static long ConvertTo(Count value)
    {
        return value;
    }

    /// <summary>
    /// Converts a stored <see cref="long"/> back to a <see cref="Count"/>.
    /// </summary>
    /// <remarks>Kept public for EF Core compiled models.</remarks>
    public static Count ConvertFrom(long value)
    {
        if (Count.TryFromValue(value, out var result))
        {
            return result;
        }

        ValueConversionException.Throw($"Could not convert value {value} to {nameof(Count)}");
        return default; // unreachable
    }
}
