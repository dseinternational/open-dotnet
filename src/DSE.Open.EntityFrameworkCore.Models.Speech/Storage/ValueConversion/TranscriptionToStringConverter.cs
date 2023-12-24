// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Speech;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Speech.Storage.ValueConversion;

public sealed class TranscriptionToStringConverter : ValueConverter<Transcription, string>
{
    public static readonly TranscriptionToStringConverter Default = new();

    public TranscriptionToStringConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    private static string ConvertTo(Transcription value)
    {
        return value.ToString();
    }

    private static Transcription ConvertFrom(string value)
    {
        return new Transcription(value);
    }
}
