// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using CommunityToolkit.HighPerformance.Buffers;
using DSE.Open.Hashing;
using DSE.Open.Runtime.Helpers;
using DSE.Open.Runtime.InteropServices;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open;

/// <summary>
/// An immutable sequence of ASCII bytes.
/// </summary>
/// <remarks>
/// Implements <see cref="ISpanFormattable"/> and <see cref="ISpanParsable{TSelf}"/> over
/// a <see cref="ReadOnlyMemory{T}"/> of <see cref="AsciiChar"/>.
/// </remarks>
[JsonConverter(typeof(JsonStringAsciiStringConverter))]
[StructLayout(LayoutKind.Sequential)]
public readonly struct AsciiString
    : IEnumerable<AsciiChar>,
      IEquatable<AsciiString>,
      IEquatable<ReadOnlyMemory<AsciiChar>>,
      IComparable<AsciiString>,
      IEqualityOperators<AsciiString, AsciiString, bool>,
      ISpanFormattable,
      ISpanParsable<AsciiString>,
      IUtf8SpanFormattable,
      IUtf8SpanParsable<AsciiString>,
      ISpanFormattableCharCountProvider,
      IRepeatableHash64
{
    private readonly ReadOnlyMemory<AsciiChar> _value;

    /// <summary>
    /// Initializes a new <see cref="AsciiString"/> wrapping the specified sequence of ASCII characters.
    /// </summary>
    /// <param name="value">The backing sequence of ASCII characters.</param>
    public AsciiString(ReadOnlyMemory<AsciiChar> value)
    {
        _value = value;
    }

    /// <summary>
    /// Gets the <see cref="AsciiChar"/> at the specified zero-based position.
    /// </summary>
    public AsciiChar this[int i] => _value.Span[i];

    /// <summary>
    /// Returns a sub-section of the current sequence as a new <see cref="AsciiString"/>.
    /// </summary>
    public AsciiString Slice(int start, int length)
    {
        return new(_value.Slice(start, length));
    }

    /// <summary>
    /// Gets a value indicating whether the sequence is empty.
    /// </summary>
    public bool IsEmpty => _value.IsEmpty;

    /// <summary>
    /// Gets the number of ASCII characters in the sequence.
    /// </summary>
    public int Length => _value.Length;

    /// <summary>
    /// Returns the underlying <see cref="ReadOnlyMemory{T}"/> of <see cref="AsciiChar"/> values.
    /// </summary>
    public ReadOnlyMemory<AsciiChar> AsMemory()
    {
        return _value;
    }

    /// <summary>
    /// Returns the underlying <see cref="ReadOnlySpan{T}"/> of <see cref="AsciiChar"/> values.
    /// </summary>
    public ReadOnlySpan<AsciiChar> AsSpan()
    {
        return _value.Span;
    }

    /// <summary>
    /// Returns the underlying ASCII bytes as a <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/>.
    /// </summary>
    public ReadOnlySpan<byte> AsBytes()
    {
        return ValuesMarshal.AsBytes(_value.Span);
    }

    /// <inheritdoc/>
    public int GetCharCount(ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        return _value.Length;
    }

    /// <inheritdoc/>
    public int GetCharCount(string? format, IFormatProvider? formatProvider)
    {
        return _value.Length;
    }

    /// <summary>
    /// Returns a new array containing a copy of the sequence's <see cref="AsciiChar"/> values.
    /// </summary>
    public AsciiChar[] ToArray()
    {
        return _value.ToArray();
    }

    /// <summary>
    /// Returns a new array containing a copy of the underlying ASCII bytes.
    /// </summary>
    public byte[] ToByteArray()
    {
        if (_value.IsEmpty)
        {
            return [];
        }

        var result = new byte[_value.Length];

        ValuesMarshal.AsBytes(_value.Span).CopyTo(result);

        return result;
    }

    /// <summary>
    /// Returns a new array containing the UTF-16 representation of the ASCII characters.
    /// </summary>
    public char[] ToCharArray()
    {
        if (_value.IsEmpty)
        {
            return [];
        }

        var result = new char[_value.Length];

        var status = Ascii.ToUtf16(ValuesMarshal.AsBytes(_value.Span), result, out _);

        Debug.Assert(status == OperationStatus.Done);

        return result;
    }

    /// <summary>Parses the specified character span as an <see cref="AsciiString"/>.</summary>
    /// <exception cref="FormatException">Thrown if <paramref name="s"/> contains non-ASCII characters.</exception>
    public static AsciiString Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, default);
    }

    /// <inheritdoc/>
    public static AsciiString Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"'{s}' is not a valid {nameof(AsciiString)} value.");
        return default; // unreachable
    }

    /// <summary>Parses the specified string as an <see cref="AsciiString"/>.</summary>
    /// <exception cref="FormatException">Thrown if <paramref name="s"/> contains non-ASCII characters.</exception>
    public static AsciiString Parse(string s)
    {
        return Parse(s, default);
    }

    /// <inheritdoc/>
    public static AsciiString Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    /// <summary>Attempts to parse the specified character span as an <see cref="AsciiString"/>.</summary>
    public static bool TryParse(
        ReadOnlySpan<char> s,
        out AsciiString result)
    {
        return TryParse(s, default, out result);
    }

    /// <inheritdoc/>
    [SkipLocalsInit]
    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out AsciiString result)
    {
        var rented = SpanOwner<byte>.Empty;

        var buffer = MemoryThresholds.CanStackalloc<byte>(s.Length)
            ? stackalloc byte[MemoryThresholds.StackallocByteThreshold]
            : (rented = SpanOwner<byte>.Allocate(s.Length)).Span;

        using (rented)
        {
            var status = Ascii.FromUtf16(s, buffer, out var bytesWritten);

            if (status == OperationStatus.InvalidData)
            {
                result = default;
                return false;
            }

            Debug.Assert(status == OperationStatus.Done);

            result = new(ValuesMarshal.AsAsciiChars(buffer[..bytesWritten]).ToArray());
            return true;
        }
    }

    /// <summary>Attempts to parse the specified string as an <see cref="AsciiString"/>.</summary>
    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        out AsciiString result)
    {
        return TryParse(s, default, out result);
    }

    /// <inheritdoc/>
    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out AsciiString result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    /// <summary>Parses the specified UTF-8 byte span as an <see cref="AsciiString"/>.</summary>
    /// <exception cref="FormatException">Thrown if <paramref name="utf8Text"/> contains non-ASCII bytes.</exception>
    public static AsciiString Parse(ReadOnlySpan<byte> utf8Text)
    {
        return Parse(utf8Text, default);
    }

    /// <inheritdoc/>
    public static AsciiString Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
    {
        if (TryParse(utf8Text, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"'{Encoding.UTF8.GetString(utf8Text)}' is not a valid {nameof(AsciiString)} value.");
        return default; // unreachable
    }

    /// <inheritdoc/>
    public static bool TryParse(
        ReadOnlySpan<byte> utf8Text,
        IFormatProvider? provider,
        out AsciiString result)
    {
        if (!Ascii.IsValid(utf8Text))
        {
            result = default;
            return false;
        }

        var buffer = new AsciiChar[utf8Text.Length];

        utf8Text.CopyTo(ValuesMarshal.AsBytes((Span<AsciiChar>)buffer));

        result = new(buffer);
        return true;
    }

    /// <inheritdoc/>
    public int CompareTo(AsciiString other)
    {
        return _value.Span.SequenceCompareTo(other._value.Span);
    }

    /// <summary>
    /// Compares this instance to another <see cref="AsciiString"/> ignoring ASCII case.
    /// </summary>
    public int CompareToIgnoreCase(AsciiString other)
    {
        return CompareToIgnoreCase(other._value.Span);
    }

    /// <summary>
    /// Compares this instance to a span of <see cref="AsciiChar"/> values ignoring ASCII case.
    /// </summary>
    public int CompareToIgnoreCase(ReadOnlySpan<AsciiChar> asciiBytes)
    {
        var length = Math.Min(_value.Length, asciiBytes.Length);

        for (var i = 0; i < length; i++)
        {
            var c = AsciiChar.CompareToIgnoreCase(_value.Span[i], asciiBytes[i]);

            if (c != 0)
            {
                return c;
            }
        }

        return _value.Length - asciiBytes.Length;
    }

    /// <summary>Determines whether the sequence equals the specified span of <see cref="AsciiChar"/> values.</summary>
    public bool Equals(ReadOnlySpan<AsciiChar> other)
    {
        return _value.Span.SequenceEqual(other);
    }

    /// <inheritdoc/>
    public bool Equals(ReadOnlyMemory<AsciiChar> other)
    {
        return Equals(other.Span);
    }

    /// <inheritdoc/>
    public bool Equals(AsciiString other)
    {
        return Equals(other._value);
    }

    /// <summary>Determines whether the sequence equals the specified string (compared as ASCII).</summary>
    public bool Equals(string other)
    {
        return Equals(other.AsSpan());
    }

    /// <summary>Determines whether the sequence equals the specified character span (compared as ASCII).</summary>
    public bool Equals(ReadOnlySpan<char> other)
    {
        return Ascii.Equals(ValuesMarshal.AsBytes(_value.Span), other);
    }

    /// <summary>Determines whether two <see cref="AsciiString"/> values are equal ignoring ASCII case.</summary>
    public bool EqualsIgnoreCase(AsciiString other)
    {
        return _value.Span.SequenceEqualsIgnoreCase(other._value.Span);
    }

    /// <summary>Determines whether the sequence equals the specified character span ignoring ASCII case.</summary>
    public bool EqualsIgnoreCase(ReadOnlySpan<char> other)
    {
        return Ascii.EqualsIgnoreCase(ValuesMarshal.AsBytes(_value.Span), other);
    }

    /// <summary>Determines whether the sequence equals the specified string ignoring ASCII case.</summary>
    public bool EqualsIgnoreCase(string other)
    {
        ArgumentNullException.ThrowIfNull(other);
        return EqualsIgnoreCase(other.AsSpan());
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is AsciiString other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        var c = new HashCode();
        c.AddBytes(ValuesMarshal.AsBytes(_value.Span));
        return c.ToHashCode();
    }

    /// <summary>Returns a new <see cref="AsciiString"/> with all letters lowercased.</summary>
    public AsciiString ToLower()
    {
        var result = new AsciiChar[_value.Length];
        _ = TryFormat(ValuesMarshal.AsBytes((Span<AsciiChar>)result), out _, "L", default);
        return new(result);
    }

    /// <summary>Returns a new <see cref="AsciiString"/> with all letters uppercased.</summary>
    public AsciiString ToUpper()
    {
        var result = new AsciiChar[_value.Length];
        _ = TryFormat(ValuesMarshal.AsBytes((Span<AsciiChar>)result), out _, "U", default);
        return new(result);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return ToString(null, null);
    }

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (_value.IsEmpty)
        {
            return string.Empty;
        }

        return string.Create(_value.Length, (Value: this, format, formatProvider),
            (buffer, state) => state.Value.TryFormat(buffer, out _, state.format, state.formatProvider));
    }

    /// <summary>Returns the string representation with all letters lowercased.</summary>
    public string ToStringLower()
    {
        return ToString("L", null);
    }

    /// <summary>Returns the string representation with all letters uppercased.</summary>
    public string ToStringUpper()
    {
        return ToString("U", null);
    }

    /// <inheritdoc/>
    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (_value.IsEmpty)
        {
            charsWritten = 0;
            return true;
        }

        if (_value.Length > destination.Length)
        {
            charsWritten = 0;
            return false;
        }

        if (format.IsEmpty)
        {
            return Ascii.ToUtf16(ValuesMarshal.AsBytes(_value.Span), destination, out charsWritten) == OperationStatus.Done;
        }

        if (format.Length != 1)
        {
            ThrowHelper.ThrowFormatException($"The format '{format}' is not supported.");
        }

        switch (format[0] | 0x20)
        {
            case 'l':
                return Ascii.ToLower(ValuesMarshal.AsBytes(_value.Span), destination, out charsWritten) == OperationStatus.Done;
            case 'u':
                return Ascii.ToUpper(ValuesMarshal.AsBytes(_value.Span), destination, out charsWritten) == OperationStatus.Done;
            default:
                break;
        }

        ThrowHelper.ThrowFormatException($"The format '{format}' is not supported.");
        charsWritten = default;
        return false;
    }

    /// <inheritdoc/>
    public bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (_value.IsEmpty)
        {
            bytesWritten = 0;
            return true;
        }

        if (_value.Length > utf8Destination.Length)
        {
            bytesWritten = 0;
            return false;
        }

        if (format.IsEmpty)
        {
            bytesWritten = _value.Span.Length;
            return ValuesMarshal.AsBytes(_value.Span).TryCopyTo(utf8Destination);
        }

        switch (format[0] | 0x20)
        {
            case 'l':
                return Ascii.ToLower(ValuesMarshal.AsBytes(_value.Span), utf8Destination, out bytesWritten) == OperationStatus.Done;
            case 'u':
                return Ascii.ToUpper(ValuesMarshal.AsBytes(_value.Span), utf8Destination, out bytesWritten) == OperationStatus.Done;
            default:
                break;
        }

        ThrowHelper.ThrowFormatException($"The format '{format}' is not supported.");
        bytesWritten = default;
        return false;
    }

    /// <inheritdoc/>
    public IEnumerator<AsciiChar> GetEnumerator()
    {
        for (var i = 0; i < _value.Length; i++)
        {
            yield return _value.Span[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>Determines whether the sequence ends with the specified value.</summary>
    public bool EndsWith(AsciiString value)
    {
        return EndsWith(value._value.Span);
    }

    /// <summary>Determines whether the sequence ends with the specified ASCII bytes.</summary>
    public bool EndsWith(ReadOnlySpan<byte> value)
    {
        return EndsWith(ValuesMarshal.AsAsciiChars(value));
    }

    /// <summary>Determines whether the sequence ends with the specified value.</summary>
    public bool EndsWith(ReadOnlySpan<AsciiChar> value)
    {
        return _value.Span.EndsWith(value);
    }

    /// <summary>Determines whether the sequence ends with the specified value (compared as ASCII).</summary>
    public bool EndsWith(string value)
    {
        return EndsWith(value.AsSpan());
    }

    /// <summary>Determines whether the sequence ends with the specified value (compared as ASCII).</summary>
    public bool EndsWith(ReadOnlySpan<char> value)
    {
        if (value.Length > _value.Length)
        {
            return false;
        }

        return Ascii.Equals(ValuesMarshal.AsBytes(_value.Span[(_value.Length - value.Length)..]), value);
    }

    /// <summary>Determines whether the sequence starts with the specified value.</summary>
    public bool StartsWith(AsciiString value)
    {
        return StartsWith(value._value.Span);
    }

    /// <summary>Determines whether the sequence starts with the specified ASCII bytes.</summary>
    public bool StartsWith(ReadOnlySpan<byte> value)
    {
        return StartsWith(ValuesMarshal.AsAsciiChars(value));
    }

    /// <summary>Determines whether the sequence starts with the specified value.</summary>
    public bool StartsWith(ReadOnlySpan<AsciiChar> value)
    {
        return _value.Span.StartsWith(value);
    }

    /// <summary>Determines whether the sequence starts with the specified value (compared as ASCII).</summary>
    public bool StartsWith(string value)
    {
        return StartsWith(value.AsSpan());
    }

    /// <summary>Determines whether the sequence starts with the specified value (compared as ASCII).</summary>
    public bool StartsWith(ReadOnlySpan<char> value)
    {
        if (value.Length > _value.Length)
        {
            return false;
        }

        return Ascii.Equals(ValuesMarshal.AsBytes(_value.Span[..value.Length]), value);
    }

    /// <summary>
    /// Searches for the specified value and returns the index of its first occurrence. If not found,
    /// returns -1. Values are compared using <see cref="IEquatable{T}.Equals(T)"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public int IndexOf(AsciiChar value)
    {
        return _value.Span.IndexOf(value);
    }

    /// <summary>
    /// Searches for the specified value and returns the index of its last occurrence. If not found,
    /// returns -1. Values are compared using <see cref="IEquatable{T}.Equals(T)"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public int LastIndexOf(AsciiChar value)
    {
        return _value.Span.LastIndexOf(value);
    }

    /// <summary>Determines whether the sequence contains the specified value.</summary>
    public bool Contains(AsciiChar value)
    {
        return _value.Span.Contains(value);
    }

    /// <summary>Determines whether the sequence contains the specified subsequence.</summary>
    public bool Contains(AsciiString value)
    {
        return Contains(value._value.Span);
    }

    /// <summary>Determines whether the sequence contains the specified subsequence.</summary>
    public bool Contains(ReadOnlySpan<AsciiChar> value)
    {
        return _value.Span.IndexOf(value) >= 0;
    }

    /// <summary>Determines whether the sequence contains the specified subsequence of ASCII bytes.</summary>
    public bool Contains(ReadOnlySpan<byte> value)
    {
        return Contains(ValuesMarshal.AsAsciiChars(value));
    }

    /// <summary>Determines whether two <see cref="AsciiString"/> values are equal.</summary>
    public static bool operator ==(AsciiString left, AsciiString right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two <see cref="AsciiString"/> values are not equal.</summary>
    public static bool operator !=(AsciiString left, AsciiString right)
    {
        return !(left == right);
    }

    /// <summary>Determines whether an <see cref="AsciiString"/> equals the specified string (compared as ASCII).</summary>
    public static bool operator ==(AsciiString left, string right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether an <see cref="AsciiString"/> is not equal to the specified string (compared as ASCII).</summary>
    public static bool operator !=(AsciiString left, string right)
    {
        return !(left == right);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates (Parse)

    /// <summary>Parses the specified string as an <see cref="AsciiString"/> using the invariant culture.</summary>
    public static explicit operator AsciiString(string value)
    {
        return Parse(value, CultureInfo.InvariantCulture);
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

    /// <summary>Determines whether one <see cref="AsciiString"/> precedes another.</summary>
    public static bool operator <(AsciiString left, AsciiString right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>Determines whether one <see cref="AsciiString"/> precedes or equals another.</summary>
    public static bool operator <=(AsciiString left, AsciiString right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <summary>Determines whether one <see cref="AsciiString"/> follows another.</summary>
    public static bool operator >(AsciiString left, AsciiString right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <summary>Determines whether one <see cref="AsciiString"/> follows or equals another.</summary>
    public static bool operator >=(AsciiString left, AsciiString right)
    {
        return left.CompareTo(right) >= 0;
    }

    /// <summary>
    /// Converts the <see cref="AsciiString"/> to a <see cref="CharSequence"/>.
    /// </summary>
    public CharSequence ToCharSequence()
    {
        return CharSequence.FromAsciiString(this);
    }

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value.Span);
    }
}
