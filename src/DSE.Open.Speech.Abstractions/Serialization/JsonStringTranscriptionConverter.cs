// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Speech.Serialization;

public class JsonStringTranscriptionConverter : SpanParsableCharWritingJsonConverter<SpeechTranscription>
{
    public static readonly JsonStringTranscriptionConverter Default = new();

    protected override int GetMaxCharCountToWrite(SpeechTranscription value)
    {
        return ((ISpanFormatableCharCountProvider)value).GetCharCount(null,null);
    }
}
