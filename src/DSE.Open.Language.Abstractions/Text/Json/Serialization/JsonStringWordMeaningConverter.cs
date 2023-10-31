// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Language.Text.Json.Serialization;

public class JsonStringWordMeaningConverter : SpanParsableCharWritingJsonConverter<WordMeaning>
{
    public static readonly JsonStringSignConverter Default = new();

    protected override int GetMaxCharCountToWrite(WordMeaning value)
    {
        return WordMeaning.MaxSerializedCharLength;
    }
}
