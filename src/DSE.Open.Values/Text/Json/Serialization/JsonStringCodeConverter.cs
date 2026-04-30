// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Values.Text.Json.Serialization;

/// <summary>
/// A <see cref="System.Text.Json.Serialization.JsonConverter{T}"/> that reads and writes
/// <see cref="Code"/> as a JSON string using its <see cref="ISpanParsable{TSelf}"/>
/// and <see cref="ISpanFormattable"/> implementations.
/// </summary>
public class JsonStringCodeConverter : SpanParsableCharWritingJsonConverter<Code>
{
    /// <summary>
    /// A shared default instance of the converter.
    /// </summary>
    public static readonly JsonStringCodeConverter Default = new();

    /// <inheritdoc/>
    protected override int GetMaxCharCountToWrite(Code value)
    {
        return Code.MaxLength;
    }
}
