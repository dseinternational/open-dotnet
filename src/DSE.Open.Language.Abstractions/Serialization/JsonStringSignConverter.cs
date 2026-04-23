// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Language.Serialization;

/// <summary>
/// A <see cref="System.Text.Json.Serialization.JsonConverter"/> that reads and
/// writes <see cref="Sign"/> values as JSON strings using
/// <see cref="Sign.Parse(ReadOnlySpan{char}, IFormatProvider)"/> and
/// <see cref="Sign.TryFormat(Span{char}, out int, ReadOnlySpan{char}, IFormatProvider?)"/>.
/// </summary>
public class JsonStringSignConverter : SpanParsableCharWritingJsonConverter<Sign>
{
    /// <summary>
    /// The default, cached <see cref="JsonStringSignConverter"/> instance.
    /// </summary>
    public static readonly JsonStringSignConverter Default = new();

    /// <inheritdoc/>
    protected override int GetMaxCharCountToWrite(Sign value)
    {
        return Sign.MaxSerializedCharLength;
    }
}
