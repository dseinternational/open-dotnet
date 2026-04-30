// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace DSE.Open.Text.Json.Serialization;

/// <summary>
/// Base implementation for a <see cref="JsonConverter"/> that reads and writes values
/// from <see cref="byte"/> buffers using the implementations provided by types
/// implementing <see cref="IUtf8SpanFormattable"/> and <see cref="IUtf8SpanParsable{TSelf}"/>.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public abstract class SpanParsableByteWritingJsonConverter<TValue> : ByteWritingJsonConverter<TValue>
    where TValue : IUtf8SpanFormattable, IUtf8SpanParsable<TValue>
{
    /// <summary>
    /// The format string passed to <see cref="IUtf8SpanFormattable.TryFormat"/>.
    /// Defaults to <see cref="string.Empty"/>.
    /// </summary>
    protected virtual string FormatString => string.Empty;

    /// <summary>
    /// The format provider used during formatting and parsing.
    /// Defaults to <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    protected virtual IFormatProvider FormatProvider => CultureInfo.InvariantCulture;

    /// <inheritdoc/>
    protected override bool TryFormat(TValue value, Span<byte> data, out int bytesWritten)
    {
        return value.TryFormat(data, out bytesWritten, FormatString, FormatProvider);
    }

    /// <inheritdoc/>
    protected override bool TryParse(ReadOnlySpan<byte> data, [MaybeNullWhen(false)] out TValue value)
    {
        return TValue.TryParse(data, FormatProvider, out value);
    }
}
