// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Speech;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Speech.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="SpeechTranscription"/> to and from its <see cref="string"/> representation
/// for storage by Entity Framework Core.
/// </summary>
public sealed class SpeechTranscriptionToStringConverter : ValueConverter<SpeechTranscription, string>
{
    /// <summary>
    /// A default, shared instance of <see cref="SpeechTranscriptionToStringConverter"/>.
    /// </summary>
    public static readonly SpeechTranscriptionToStringConverter Default = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="SpeechTranscriptionToStringConverter"/> class.
    /// </summary>
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
