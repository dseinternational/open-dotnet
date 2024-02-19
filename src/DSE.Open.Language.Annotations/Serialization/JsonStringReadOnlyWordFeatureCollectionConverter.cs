// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Language.Annotations.Serialization;

public class JsonStringReadOnlyWordFeatureCollectionConverter
    : SpanParsableCharWritingJsonConverter<ReadOnlyWordFeatureCollection>
{
    public static readonly JsonStringWordFeatureCollectionConverter Default = new();

    protected override int GetMaxCharCountToWrite(ReadOnlyWordFeatureCollection value)
    {
        Guard.IsNotNull(value);
        return value.Count * 16;
    }
}
