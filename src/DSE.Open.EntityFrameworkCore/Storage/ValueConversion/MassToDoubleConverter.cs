// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values.Units;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// EF Core value converter that maps <see cref="Mass"/> values to and from <see cref="double"/> grams.
/// </summary>
public sealed class MassToDoubleConverter : ValueConverter<Mass, double>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly MassToDoubleConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="MassToDoubleConverter"/>.
    /// </summary>
    public MassToDoubleConverter()
        : base(c => ConvertToDouble(c), s => ConvertFromDouble(s))
    {
    }

    /// <summary>
    /// Converts a <see cref="Mass"/> to its <see cref="double"/> storage form (the amount in grams).
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The amount as a <see cref="double"/>.</returns>
    // keep public for EF Core compiled models
    public static double ConvertToDouble(Mass value)
    {
        return value.Amount;
    }

    /// <summary>
    /// Converts a <see cref="double"/> storage value (grams) back to a <see cref="Mass"/>.
    /// </summary>
    /// <param name="value">The stored amount in grams.</param>
    /// <returns>The reconstructed <see cref="Mass"/>.</returns>
    // keep public for EF Core compiled models
    public static Mass ConvertFromDouble(double value)
    {
        return Mass.FromGrams(value);
    }
}
