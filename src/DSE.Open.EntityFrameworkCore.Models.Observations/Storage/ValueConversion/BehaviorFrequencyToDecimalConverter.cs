// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Observations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Observations.Storage.ValueConversion;

/// <summary>
/// Converts <see cref="BehaviorFrequency"/> values to and from <see cref="decimal"/> for storage.
/// </summary>
public sealed class BehaviorFrequencyToDecimalConverter : ValueConverter<BehaviorFrequency, decimal>
{
    /// <summary>
    /// Gets a default, shared instance of <see cref="BehaviorFrequencyToDecimalConverter"/>.
    /// </summary>
    public static readonly BehaviorFrequencyToDecimalConverter Default = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="BehaviorFrequencyToDecimalConverter"/> class.
    /// </summary>
    public BehaviorFrequencyToDecimalConverter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    /// <summary>
    /// Converts a <see cref="BehaviorFrequency"/> to a <see cref="decimal"/> for storage.
    /// </summary>
    /// <remarks>Kept public for EF Core model compilation.</remarks>
    public static decimal ConvertTo(BehaviorFrequency value)
    {
        unchecked
        {
            return (uint)value;
        }
    }

    /// <summary>
    /// Converts a stored <see cref="decimal"/> back to a <see cref="BehaviorFrequency"/>.
    /// </summary>
    /// <remarks>Kept public for EF Core model compilation.</remarks>
    public static BehaviorFrequency ConvertFrom(decimal value)
    {
        return (BehaviorFrequency)(uint)Math.Round(value, 0);
    }
}
