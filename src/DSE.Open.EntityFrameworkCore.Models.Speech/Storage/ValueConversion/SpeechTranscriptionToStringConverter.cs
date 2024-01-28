// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Speech;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Speech.Storage.ValueConversion;

public sealed class SpeechTranscriptionToStringConverter : ValueConverter<SpeechTranscription, string>
{
    public static readonly SpeechTranscriptionToStringConverter Default = new();

    public SpeechTranscriptionToStringConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    private static string ConvertTo(SpeechTranscription value)
    {
        return value.ToString();
    }

    private static SpeechTranscription ConvertFrom(string value)
    {
        return SpeechTranscription.Parse(value, CultureInfo.InvariantCulture);
    }
}
