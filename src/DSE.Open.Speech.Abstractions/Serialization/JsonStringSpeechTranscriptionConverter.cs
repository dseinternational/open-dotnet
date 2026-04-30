// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Speech.Serialization;

/// <summary>
/// A <see cref="System.Text.Json.Serialization.JsonConverter"/> that serializes
/// <see cref="SpeechTranscription"/> values as JSON strings.
/// </summary>
public class JsonStringSpeechTranscriptionConverter : SpanParsableCharWritingJsonConverter<SpeechTranscription>
{
    /// <summary>
    /// The default <see cref="JsonStringSpeechTranscriptionConverter"/> instance.
    /// </summary>
    public static readonly JsonStringSpeechTranscriptionConverter Default = new();

    /// <inheritdoc/>
    protected override int GetMaxCharCountToWrite(SpeechTranscription value)
    {
        return ((ISpanFormattableCharCountProvider)value).GetCharCount(null, null);
    }
}
