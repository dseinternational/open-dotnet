// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="Tag"/> to a <see cref="string"/> for storage.
/// </summary>
public sealed class TagToStringConverter : ValueConverter<Tag, string>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly TagToStringConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="TagToStringConverter"/>.
    /// </summary>
    public TagToStringConverter()
        : base(v => ConvertToString(v), v => ConvertToUniqueId(v))
    {
    }

    /// <summary>
    /// Converts a <see cref="Tag"/> to its <see cref="string"/> storage form.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The string representation.</returns>
    // keep public for EF Core compiled models
    public static string ConvertToString(Tag value)
    {
        return value.ToString();
    }

    /// <summary>
    /// Converts a <see cref="string"/> storage value back to a <see cref="Tag"/>.
    /// </summary>
    /// <param name="value">The stored string value.</param>
    /// <returns>The reconstructed <see cref="Tag"/>.</returns>
    // keep public for EF Core compiled models
    public static Tag ConvertToUniqueId(string value)
    {
        return new(value);
    }
}
