// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Language.Annotations.Serialization;

public class JsonStringWordFeatureCollectionConverter
    : SpanParsableCharWritingJsonConverter<WordFeatureCollection>
{
    public static readonly JsonStringWordFeatureCollectionConverter Default = new();

    protected override int GetMaxCharCountToWrite(WordFeatureCollection value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value.Count * 16;
    }
}
