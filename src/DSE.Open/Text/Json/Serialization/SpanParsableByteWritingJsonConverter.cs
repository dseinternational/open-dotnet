// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
    protected virtual IFormatProvider FormatProvider => CultureInfo.InvariantCulture;

    protected override bool TryFormat(TValue value, Span<byte> data, out int bytesWritten)
        => value.TryFormat(data, out bytesWritten, default, FormatProvider);

    protected override bool TryParse(ReadOnlySpan<byte> data, [MaybeNullWhen(false)] out TValue value)
        => TValue.TryParse(data, FormatProvider, out value);
}
