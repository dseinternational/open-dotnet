// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Drawing.Text.Json.Serialization;

/// <summary>
/// A JSON converter that serializes and deserializes <see cref="Color"/> values
/// as hex strings (#RRGGBB or #RRGGBBAA).
/// </summary>
public class JsonStringColorConverter : SpanParsableCharWritingJsonConverter<Color>
{
    public static readonly JsonStringColorConverter Default = new();

    protected override int GetMaxCharCountToWrite(Color value)
    {
        return Color.MaxFormatLength;
    }
}
