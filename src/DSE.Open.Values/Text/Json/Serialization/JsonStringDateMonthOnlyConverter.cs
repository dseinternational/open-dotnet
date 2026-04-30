// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Values.Text.Json.Serialization;

/// <summary>
/// A <see cref="System.Text.Json.Serialization.JsonConverter{T}"/> that reads and writes
/// <see cref="DateMonthOnly"/> as a JSON string using its <see cref="ISpanParsable{TSelf}"/>
/// and <see cref="ISpanFormattable"/> implementations.
/// </summary>
public class JsonStringDateMonthOnlyConverter : SpanParsableCharWritingJsonConverter<DateMonthOnly>
{
    /// <summary>
    /// A shared default instance of the converter.
    /// </summary>
    public static readonly JsonStringDateMonthOnlyConverter Default = new();

    /// <inheritdoc/>
    protected override int GetMaxCharCountToWrite(DateMonthOnly value)
    {
        return 16;
    }
}
