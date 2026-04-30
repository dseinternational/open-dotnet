// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Language.Annotations.Serialization;

/// <summary>
/// JSON converter that serializes <see cref="WordFeature"/> values as their parsable string representation.
/// </summary>
public class JsonStringWordFeatureConverter : SpanParsableCharWritingJsonConverter<WordFeature>
{
    /// <summary>
    /// A shared default instance of the converter.
    /// </summary>
    public static readonly JsonStringWordFeatureConverter Default = new();

    /// <inheritdoc/>
    protected override int GetMaxCharCountToWrite(WordFeature value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value.GetCharCount();
    }
}
