// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Speech.Serialization;

public class JsonStringSpeechSoundConverter : SpanParsableCharWritingJsonConverter<SpeechSound>
{
    public static readonly JsonStringSpeechSoundConverter Default = new();

    protected override int GetMaxCharCountToWrite(SpeechSound value)
    {
        return value.Length;
    }
}
