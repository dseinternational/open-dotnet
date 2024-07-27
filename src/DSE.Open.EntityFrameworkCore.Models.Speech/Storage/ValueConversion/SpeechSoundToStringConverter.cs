// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Speech;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Speech.Storage.ValueConversion;

public sealed class SpeechSoundToStringConverter : ValueConverter<SpeechSound, string>
{
    public static readonly SpeechSoundToStringConverter Default = new();

    public SpeechSoundToStringConverter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    private static string ConvertTo(SpeechSound value)
    {
        return value.ToStringInvariant();
    }

    private static SpeechSound ConvertFrom(string value)
    {
        if (SpeechSound.TryParse(value, CultureInfo.InvariantCulture, out var result))
        {
            return result;
        }

        ValueConversionException.Throw($"Could not convert value {value} to {nameof(SpeechSound)}");
        return default; // unreachable
    }
}
