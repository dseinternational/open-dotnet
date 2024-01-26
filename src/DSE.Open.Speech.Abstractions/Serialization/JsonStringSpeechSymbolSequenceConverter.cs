﻿// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Speech.Serialization;

public class JsonStringSpeechSymbolSequenceConverter : SpanParsableCharWritingJsonConverter<SpeechSymbolSequence>
{
    public static readonly JsonStringSpeechSymbolSequenceConverter Default = new();

    protected override int GetMaxCharCountToWrite(SpeechSymbolSequence value)
    {
        return value.Length;
    }
}
