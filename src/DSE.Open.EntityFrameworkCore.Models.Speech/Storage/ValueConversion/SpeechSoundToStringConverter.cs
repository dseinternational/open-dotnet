// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Speech;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Speech.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="SpeechSound"/> to and from its <see cref="string"/> representation
/// for storage by Entity Framework Core.
/// </summary>
public sealed class SpeechSoundToStringConverter : ValueConverter<SpeechSound, string>
{
    /// <summary>
    /// A default, shared instance of <see cref="SpeechSoundToStringConverter"/>.
    /// </summary>
    public static readonly SpeechSoundToStringConverter Default = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="SpeechSoundToStringConverter"/> class.
    /// </summary>
    public SpeechSoundToStringConverter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    /// <summary>
    /// Converts the specified <see cref="SpeechSound"/> to its invariant <see cref="string"/> representation.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The string representation of <paramref name="value"/>.</returns>
    // public for EF Core model compilation
    public static string ConvertTo(SpeechSound value)
    {
        return value.ToStringInvariant();
    }

    /// <summary>
    /// Parses the specified <see cref="string"/> into a <see cref="SpeechSound"/>.
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <returns>The parsed <see cref="SpeechSound"/>.</returns>
    // public for EF Core model compilation
    public static SpeechSound ConvertFrom(string value)
    {
        if (SpeechSound.TryParse(value, CultureInfo.InvariantCulture, out var result))
        {
            return result;
        }

        ValueConversionException.Throw($"Could not convert value {value} to {nameof(SpeechSound)}");
        return default; // unreachable
    }
}
