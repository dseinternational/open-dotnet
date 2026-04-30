// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values.Units;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// EF Core value converter that maps <see cref="Length"/> values to and from <see cref="double"/> millimetres.
/// </summary>
public sealed class LengthToDoubleConverter : ValueConverter<Length, double>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly LengthToDoubleConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="LengthToDoubleConverter"/>.
    /// </summary>
    public LengthToDoubleConverter()
        : base(c => ConvertToDouble(c), s => ConvertFromDouble(s))
    {
    }

    /// <summary>
    /// Converts a <see cref="Length"/> to its <see cref="double"/> storage form (the amount in millimetres).
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The amount as a <see cref="double"/>.</returns>
    // keep public for EF Core compiled models
    public static double ConvertToDouble(Length value)
    {
        return value.Amount;
    }

    /// <summary>
    /// Converts a <see cref="double"/> storage value (millimetres) back to a <see cref="Length"/>.
    /// </summary>
    /// <param name="value">The stored amount in millimetres.</param>
    /// <returns>The reconstructed <see cref="Length"/>.</returns>
    // keep public for EF Core compiled models
    public static Length ConvertFromDouble(double value)
    {
        return Length.FromMillimetres(value);
    }
}
