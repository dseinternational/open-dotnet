// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using CommunityToolkit.HighPerformance.Buffers;
using DSE.Open.Runtime.Helpers;
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
[StructLayout(LayoutKind.Auto)]
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
      ISpanFormatableCharCountProvider
{
    private readonly ReadOnlyMemory<AsciiChar> _value;

    public AsciiString(ReadOnlyMemory<AsciiChar> value)
    {
        _value = value;
    }

    public AsciiChar this[int i] => _value.Span[i];

    public AsciiString Slice(int start, int length)
    {
        return new(_value.Slice(start, length));
    }

    public bool IsEmpty => _value.IsEmpty;

    public int Length => _value.Length;

    public ReadOnlyMemory<AsciiChar> AsMemory()
    {
        return _value;
    }

    public ReadOnlySpan<AsciiChar> AsSpan()
    {
        return _value.Span;
    }

    public int GetCharCount(ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        return _value.Length;
    }

    public int GetCharCount(string? format, IFormatProvider? formatProvider)
    {
        return _value.Length;
    }

    public AsciiChar[] ToArray()
    {
        return _value.ToArray();
    }

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

    public static AsciiString Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, default);
    }

    public static AsciiString Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"'{s}' is not a valid {nameof(AsciiString)} value.");
        return default; // unreachable
    }

    public static AsciiString Parse(string s)
    {
        return Parse(s, default);
    }

    public static AsciiString Parse(string s, IFormatProvider? provider)
    {
        Guard.IsNotNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out AsciiString result)
    {
        return TryParse(s, default, out result);
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out AsciiString result)
    {
        var rented = SpanOwner<byte>.Empty;

        Span<byte> buffer = MemoryThresholds.CanStackalloc<byte>(s.Length)
            ? stackalloc byte[s.Length]
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

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        out AsciiString result)
    {
        return TryParse(s, default, out result);
    }

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

    public static AsciiString Parse(ReadOnlySpan<byte> utf8Text)
    {
        return Parse(utf8Text, default);
    }

    public static AsciiString Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
    {
        if (TryParse(utf8Text, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"'{Encoding.UTF8.GetString(utf8Text)}' is not a valid {nameof(AsciiString)} value.");
        return default; // unreachable
    }

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

        utf8Text.CopyTo(ValuesMarshal.AsBytes(buffer));

        result = new(buffer);
        return true;
    }

    public int CompareTo(AsciiString other)
    {
        return _value.Span.SequenceCompareTo(other._value.Span);
    }

    public int CompareToCaseInsensitive(AsciiString other)
    {
        return CompareToCaseInsensitive(other._value.Span);
    }

    public int CompareToCaseInsensitive(ReadOnlySpan<AsciiChar> asciiBytes)
    {
        var length = Math.Min(_value.Length, asciiBytes.Length);

        for (var i = 0; i < length; i++)
        {
            var c = AsciiChar.CompareToCaseInsensitive(_value.Span[i], asciiBytes[i]);

            if (c != 0)
            {
                return c;
            }
        }

        return _value.Length - asciiBytes.Length;
    }

    public bool Equals(ReadOnlySpan<AsciiChar> other)
    {
        return _value.Span.SequenceEqual(other);
    }

    public bool Equals(ReadOnlyMemory<AsciiChar> other)
    {
        return Equals(other.Span);
    }

    public bool Equals(AsciiString other)
    {
        return Equals(other._value);
    }

    public bool Equals(string other)
    {
        return Equals(other.AsSpan());
    }

    public bool Equals(ReadOnlySpan<char> other)
    {
        return Ascii.Equals(ValuesMarshal.AsBytes(_value.Span), other);
    }

    public bool EqualsCaseInsensitive(AsciiString other)
    {
        return _value.Span.SequenceEqualsCaseInsensitive(other._value.Span);
    }

    public bool EqualsCaseInsensitive(ReadOnlySpan<char> other)
    {
        return Ascii.EqualsIgnoreCase(ValuesMarshal.AsBytes(_value.Span), other);
    }

    public bool EqualsCaseInsensitive(string other)
    {
        return EqualsCaseInsensitive(other.AsSpan());
    }

    public override bool Equals(object? obj)
    {
        return obj is AsciiString other && Equals(other);
    }

    public override int GetHashCode()
    {
        var c = new HashCode();
        c.AddBytes(ValuesMarshal.AsBytes(_value.Span));
        return c.ToHashCode();
    }

    public AsciiString ToLower()
    {
        var result = new AsciiChar[_value.Length];
        _ = TryFormat(ValuesMarshal.AsBytes(result), out _, "L", default);
        return new(result);
    }

    public AsciiString ToUpper()
    {
        var result = new AsciiChar[_value.Length];
        _ = TryFormat(ValuesMarshal.AsBytes(result), out _, "U", default);
        return new(result);
    }

    public override string ToString()
    {
        return ToString(null, null);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (_value.IsEmpty)
        {
            return string.Empty;
        }

        return string.Create(_value.Length, (Value: this, format, formatProvider),
            (buffer, state) => state.Value.TryFormat(buffer, out _, state.format, state.formatProvider));
    }

    public string ToStringLower()
    {
        return ToString("L", null);
    }

    public string ToStringUpper()
    {
        return ToString("U", null);
    }

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
            ThrowHelper.ThrowFormatException($"The format '{format.ToString()}' is not supported.");
        }

        switch (format[0] | 0x20)
        {
            case 'l':
                return Ascii.ToLower(ValuesMarshal.AsBytes(_value.Span), destination, out charsWritten) == OperationStatus.Done;
            case 'u':
                return Ascii.ToUpper(ValuesMarshal.AsBytes(_value.Span), destination, out charsWritten) == OperationStatus.Done;
        }

        ThrowHelper.ThrowFormatException($"The format '{format.ToString()}' is not supported.");
        charsWritten = default;
        return false;
    }

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
        }

        ThrowHelper.ThrowFormatException($"The format '{format.ToString()}' is not supported.");
        bytesWritten = default;
        return false;
    }

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

    public bool EndsWith(AsciiString value)
    {
        return EndsWith(value._value.Span);
    }

    public bool EndsWith(ReadOnlySpan<byte> value)
    {
        return EndsWith(ValuesMarshal.AsAsciiChars(value));
    }

    public bool EndsWith(ReadOnlySpan<AsciiChar> value)
    {
        return _value.Span.EndsWith(value);
    }

    public bool EndsWith(string value)
    {
        return EndsWith(value.AsSpan());
    }

    public bool EndsWith(ReadOnlySpan<char> value)
    {
        if (value.Length > _value.Length)
        {
            return false;
        }

        return Ascii.Equals(ValuesMarshal.AsBytes(_value.Span[(_value.Length - value.Length)..]), value);
    }

    public bool StartsWith(AsciiString value)
    {
        return StartsWith(value._value.Span);
    }

    public bool StartsWith(ReadOnlySpan<byte> value)
    {
        return StartsWith(ValuesMarshal.AsAsciiChars(value));
    }

    public bool StartsWith(ReadOnlySpan<AsciiChar> value)
    {
        return _value.Span.StartsWith(value);
    }

    public bool StartsWith(string value)
    {
        return StartsWith(value.AsSpan());
    }

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

    public bool Contains(AsciiChar value)
    {
        return _value.Span.Contains(value);
    }

    public bool Contains(AsciiString value)
    {
        return Contains(value._value.Span);
    }

    public bool Contains(ReadOnlySpan<AsciiChar> value)
    {
        return _value.Span.IndexOf(value) >= 0;
    }

    public bool Contains(ReadOnlySpan<byte> value)
    {
        return Contains(ValuesMarshal.AsAsciiChars(value));
    }

    public static bool operator ==(AsciiString left, AsciiString right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(AsciiString left, AsciiString right)
    {
        return !(left == right);
    }

    public static bool operator ==(AsciiString left, string right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(AsciiString left, string right)
    {
        return !(left == right);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates (Parse)

    public static explicit operator AsciiString(string value)
    {
        return Parse(value, CultureInfo.InvariantCulture);
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

    public static bool operator <(AsciiString left, AsciiString right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(AsciiString left, AsciiString right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(AsciiString left, AsciiString right)
    {
        return left.CompareTo(right) > 0;
    }

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
}
