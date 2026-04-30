// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Observations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Observations.Storage.ValueConversion;

/// <summary>
/// Converts <see cref="Amount"/> values to and from <see cref="decimal"/> for storage.
/// </summary>
public sealed class AmountToDecimalConverter : ValueConverter<Amount, decimal>
{
    /// <summary>
    /// Gets a default, shared instance of <see cref="AmountToDecimalConverter"/>.
    /// </summary>
    public static readonly AmountToDecimalConverter Default = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="AmountToDecimalConverter"/> class.
    /// </summary>
    public AmountToDecimalConverter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    /// <summary>
    /// Converts an <see cref="Amount"/> to a <see cref="decimal"/> for storage.
    /// </summary>
    /// <remarks>Kept public for EF Core compiled models.</remarks>
    public static decimal ConvertTo(Amount value)
    {
        return value;
    }

    /// <summary>
    /// Converts a stored <see cref="decimal"/> back to an <see cref="Amount"/>.
    /// </summary>
    /// <remarks>Kept public for EF Core compiled models.</remarks>
    public static Amount ConvertFrom(decimal value)
    {
        if (Amount.TryFromValue(value, out var result))
        {
            return result;
        }

        ValueConversionException.Throw($"Could not convert value {value} to {nameof(Amount)}");
        return default; // unreachable
    }
}
