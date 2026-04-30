// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Language.Annotations.Serialization;

/// <summary>
/// JSON converter that serializes <see cref="ReadOnlyWordFeatureCollection"/> values as their parsable string representation.
/// </summary>
public class JsonStringReadOnlyWordFeatureCollectionConverter
    : SpanParsableCharWritingJsonConverter<ReadOnlyWordFeatureCollection>
{
    /// <summary>
    /// A shared default instance of the converter.
    /// </summary>
    public static readonly JsonStringReadOnlyWordFeatureCollectionConverter Default = new();

    /// <inheritdoc/>
    protected override int GetMaxCharCountToWrite(ReadOnlyWordFeatureCollection value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value.Count == 0
            ? 0
            : value.Sum(v => v.GetCharCount()) + value.Count - 1;
    }
}
