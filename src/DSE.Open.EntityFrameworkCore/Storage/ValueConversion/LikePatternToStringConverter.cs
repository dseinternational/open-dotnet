// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values.Text;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="LikePattern"/> to a <see cref="string"/> for storage.
/// </summary>
public sealed class LikePatternToStringConverter : ValueConverter<LikePattern, string>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly LikePatternToStringConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="LikePatternToStringConverter"/>.
    /// </summary>
    public LikePatternToStringConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    /// <summary>
    /// Converts a <see cref="LikePattern"/> to its <see cref="string"/> storage form.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The string representation.</returns>
    // keep public for EF Core compiled models
    public static string ConvertTo(LikePattern value)
    {
        return value.ToString();
    }

    /// <summary>
    /// Converts a <see cref="string"/> storage value back to a <see cref="LikePattern"/>.
    /// </summary>
    /// <param name="value">The stored string value.</param>
    /// <returns>The reconstructed <see cref="LikePattern"/>.</returns>
    // keep public for EF Core compiled models
    public static LikePattern ConvertFrom(string value)
    {
        return new(value);
    }
}
