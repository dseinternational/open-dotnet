// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json.Serialization;

namespace DSE.Open.Text.Json.Serialization;

/// <summary>
/// Base implementation for a <see cref="JsonConverter"/> that reads and writes values
/// from <see cref="char"/> buffers using the implementations provided by types
/// implementing <see cref="ISpanFormattable"/> and <see cref="ISpanParsable{TSelf}"/>.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public abstract class SpanParsableCharWritingJsonConverter<TValue> : CharWritingJsonConverter<TValue>
    where TValue : ISpanFormattable, ISpanParsable<TValue>
{
    protected virtual IFormatProvider FormatProvider => CultureInfo.InvariantCulture;

    protected override bool TryFormat(TValue value, Span<char> data, out int charsWritten)
        => value.TryFormat(data, out charsWritten, default, FormatProvider);

    protected override bool TryParse(ReadOnlySpan<char> data, [MaybeNullWhen(false)] out TValue value)
        => TValue.TryParse(data, FormatProvider, out value);
}
