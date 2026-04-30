// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Speech.Serialization;

/// <summary>
/// A <see cref="System.Text.Json.Serialization.JsonConverter"/> that serializes
/// <see cref="SpeechSound"/> values as JSON strings.
/// </summary>
public class JsonStringSpeechSoundConverter : SpanParsableCharWritingJsonConverter<SpeechSound>
{
    /// <summary>
    /// The default <see cref="JsonStringSpeechSoundConverter"/> instance.
    /// </summary>
    public static readonly JsonStringSpeechSoundConverter Default = new();

    /// <inheritdoc/>
    protected override int GetMaxCharCountToWrite(SpeechSound value)
    {
        return value.Length;
    }
}
