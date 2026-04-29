// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="Label"/> to a <see cref="string"/> for storage.
/// </summary>
public sealed class LabelToStringConverter : ValueConverter<Label, string>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly LabelToStringConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="LabelToStringConverter"/>.
    /// </summary>
    public LabelToStringConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    /// <summary>
    /// Converts a <see cref="Label"/> to its <see cref="string"/> storage form.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The string representation.</returns>
    // keep public for EF Core compiled models
    public static string ConvertTo(Label value)
    {
        return value.ToString();
    }

    /// <summary>
    /// Converts a <see cref="string"/> storage value back to a <see cref="Label"/>.
    /// </summary>
    /// <param name="value">The stored string value.</param>
    /// <returns>The reconstructed <see cref="Label"/>.</returns>
    // keep public for EF Core compiled models
    public static Label ConvertFrom(string value)
    {
        return new(value);
    }
}
