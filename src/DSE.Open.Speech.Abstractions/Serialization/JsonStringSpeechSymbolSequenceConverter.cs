// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Speech.Serialization;

/// <summary>
/// A <see cref="System.Text.Json.Serialization.JsonConverter"/> that serializes
/// <see cref="SpeechSymbolSequence"/> values as JSON strings.
/// </summary>
public class JsonStringSpeechSymbolSequenceConverter : SpanParsableCharWritingJsonConverter<SpeechSymbolSequence>
{
    /// <summary>
    /// The default <see cref="JsonStringSpeechSymbolSequenceConverter"/> instance.
    /// </summary>
    public static readonly JsonStringSpeechSymbolSequenceConverter Default = new();

    /// <inheritdoc/>
    protected override int GetMaxCharCountToWrite(SpeechSymbolSequence value)
    {
        return value.Length;
    }
}
