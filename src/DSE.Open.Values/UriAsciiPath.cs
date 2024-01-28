// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

/// <summary>
/// A path containing only <see href="https://datatracker.ietf.org/doc/html/rfc3986#section-2.3">
/// Unreserved Characters</see> - ASCII letters, digits, '-', '.', '_' and '~'.
/// Forward slashes ('/') are interpreted to define segments and may only occur at locations other
/// than the first and last character.
/// </summary>
[ComparableValue]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<UriAsciiPath, AsciiString>))]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct UriAsciiPath
    : IComparableValue<UriAsciiPath, AsciiString>,
      IUtf8SpanSerializable<UriAsciiPath>
{
    public static readonly AsciiChar Separator = (AsciiChar)'/';
    public static readonly AsciiChar Dash = (AsciiChar)'-';
    public static readonly AsciiChar Stop = (AsciiChar)'.';
    public static readonly AsciiChar Underscore = (AsciiChar)'_';
    public static readonly AsciiChar Tilde = (AsciiChar)'~';

    public static readonly UriAsciiPath Empty = new(default, true);

    public const int MaxLength = 256;

    public static int MaxSerializedByteLength => MaxLength;

    public static int MaxSerializedCharLength => MaxLength;

    public UriAsciiPath(AsciiString path) : this(path, false)
    {
    }

    public UriAsciiPath(string path) : this(AsciiString.Parse(path, null), false)
    {
    }

    public UriAsciiPath(ReadOnlyMemory<AsciiChar> path) : this(new AsciiString(path), false)
    {
    }

    public AsciiChar this[int index] => _value[index];

    public UriAsciiPath Slice(int start, int length)
    {
        return new UriAsciiPath(_value.Slice(start, length));
    }

    public ReadOnlySpan<AsciiChar> Span => _value.AsSpan();

    public bool IsEmpty => _value.IsEmpty;

    public int Length => _value.Length;

    public bool EndsWith(ReadOnlySpan<char> value)
    {
        return _value.EndsWith(value);
    }

    public bool EndsWith(string value)
    {
        return _value.EndsWith(value);
    }

    public bool EndsWith(UriAsciiPath value)
    {
        return _value.EndsWith(value._value);
    }

    public bool EndsWith(AsciiString value)
    {
        return _value.EndsWith(value);
    }

    public bool EndsWith(ReadOnlySpan<byte> value)
    {
        return _value.EndsWith(value);
    }

    public bool EndsWith(AsciiChar value)
    {
        return !_value.IsEmpty && _value[_value.Length] == value;
    }

    public bool Equals(string value)
    {
        return _value.Equals(value);
    }

    public bool Equals(ReadOnlySpan<char> value)
    {
        return _value.Equals(value);
    }

    public bool Equals(ReadOnlySpan<AsciiChar> value)
    {
        return _value.Equals(value);
    }

    public bool EqualsCaseInsensitive(UriAsciiPath other)
    {
        return _value.EqualsCaseInsensitive(other._value);
    }

    public int IndexOf(AsciiChar c)
    {
        return _value.IndexOf(c);
    }

    public int LastIndexOf(AsciiChar c)
    {
        return _value.LastIndexOf(c);
    }

    public UriAsciiPath? GetParent()
    {
        if (!_value.IsEmpty)
        {
            var lastSlashIndex = _value.LastIndexOf(Separator);

            if (lastSlashIndex > 0)
            {
                return new UriAsciiPath(_value.AsSpan()[..lastSlashIndex].ToArray());
            }
        }

        return null;
    }

    public int GetSegmentCount()
    {
        return Length == 0 ? 0 : _value.AsSpan().Count((AsciiChar)'/') + 1;
    }

    public bool StartsWith(ReadOnlySpan<char> value)
    {
        return _value.StartsWith(value);
    }

    public bool StartsWith(string value)
    {
        return _value.StartsWith(value);
    }

    public bool StartsWith(UriAsciiPath value)
    {
        return _value.StartsWith(value._value);
    }

    public bool StartsWith(AsciiString value)
    {
        return _value.StartsWith(value);
    }

    public bool StartsWith(ReadOnlySpan<byte> value)
    {
        return _value.StartsWith(value);
    }

    public bool StartsWith(AsciiChar value)
    {
        return !_value.IsEmpty && _value[0] == value;
    }

    public UriAsciiPath ToLower()
    {
        return new UriAsciiPath(_value.ToLower(), false);
    }

    public UriAsciiPath ToUpper()
    {
        return new UriAsciiPath(_value.ToUpper(), false);
    }

    public string ToStringLower()
    {
        return _value.ToStringLower();
    }

    public string ToStringUpper()
    {
        return _value.ToStringUpper();
    }

    private static string GetString(ReadOnlySpan<char> s)
    {
        return UriPathStringPool.Shared.GetOrAdd(s);
    }

    private static bool IsValidOuterChar(AsciiChar c)
    {
        return AsciiChar.IsLetterOrDigit(c) || c == Dash || c == Stop || c == Underscore || c == Tilde;
    }

    private static bool IsValidOuterChar(char c)
    {
        return AsciiChar.IsLetterOrDigit(c) || c == Dash || c == Stop || c == Underscore || c == Tilde;
    }

    private static bool IsValidInnerChar(AsciiChar c)
    {
        return IsValidOuterChar(c) || c == Separator;
    }

    private static bool IsValidInnerChar(char c)
    {
        return IsValidOuterChar(c) || c == Separator;
    }

    public static bool IsValidValue(AsciiString value)
    {
        return IsValidValue(value.AsSpan());
    }

    public static bool IsValidValue(ReadOnlySpan<AsciiChar> value)
    {
        if (value.IsEmpty)
        {
            return true;
        }

        if (value.Length > MaxLength)
        {
            return false;
        }

        if (value.Length == 1)
        {
            return IsValidOuterChar(value[0]);
        }

        if (!IsValidOuterChar(value[0]))
        {
            return false;
        }

        if (!IsValidOuterChar(value[^1]))
        {
            return false;
        }

        var inner = value[1..^1];

        return inner.All(IsValidInnerChar);
    }

    public static bool IsValidValue(ReadOnlySpan<char> value)
    {
        byte[]? rented = null;

        try
        {
            Span<byte> buffer = value.Length <= StackallocThresholds.MaxByteLength
                ? stackalloc byte[value.Length]
                : (rented = ArrayPool<byte>.Shared.Rent(value.Length));

            if (NarrowUtf16ToAscii(value, buffer))
            {
                return IsValidValue(ValuesMarshal.AsAsciiChars(buffer[..value.Length]));
            }
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<byte>.Shared.Return(rented);
            }
        }

        return false;
    }

    private static bool NarrowUtf16ToAscii(ReadOnlySpan<char> value, Span<byte> destination)
    {
        if (value.IsEmpty)
        {
            return true;
        }

        var length = value.Length;

        for (var i = 0; i < length; i++)
        {
            var c = value[i];

            if (c > 127)
            {
                return false;
            }

            destination[i] = (byte)c;
        }

        return true;
    }

    public static bool IsValidValue(string value)
    {
        return IsValidValue(value.AsSpan());
    }

    public static bool TryParseSanitised(ReadOnlySpan<char> s, out UriAsciiPath value)
    {
        if (s.IsEmpty)
        {
            value = Empty;
            return true;
        }

        s = s.Trim();
        s = s.Trim('/');

        if (s.Length <= MaxLength)
        {
            char[]? rentedBuffer = null;

            try
            {
                Span<char> buffer = s.Length <= StackallocThresholds.MaxCharLength
                    ? stackalloc char[s.Length]
                    : rentedBuffer = ArrayPool<char>.Shared.Rent(s.Length);

                var written = s.ToLowerInvariant(buffer);

                if (written >= 0)
                {
                    return TryParse(buffer[..written], out value);
                }
            }
            finally
            {
                if (rentedBuffer is not null)
                {
                    ArrayPool<char>.Shared.Return(rentedBuffer);
                }
            }
        }

        value = default;
        return false;
    }

    public static bool TryParseSanitised(string? s, out UriAsciiPath value)
    {
        return TryParseSanitised(s.AsSpan(), out value);
    }

    /// <summary>
    /// Creates a new <see cref="UriAsciiPath"/> by appending the <paramref name="path"/> to the current path.
    /// A <see cref="Separator"/> is placed between the two paths.
    /// </summary>
    /// <param name="path"></param>
    public UriAsciiPath Append(UriAsciiPath path)
    {
        if (_value.IsEmpty)
        {
            return path;
        }

        if (path.IsEmpty)
        {
            return this;
        }

        var combined = new AsciiChar[_value.Length + path.Length + 1];

        _value.AsSpan().CopyTo(combined);
        combined[_value.Length] = Separator;
        path._value.AsSpan().CopyTo(combined.AsSpan(_value.Length + 1));

        return new UriAsciiPath(combined);
    }

    public bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (utf8Destination.Length >= _value.Length)
        {
            var bytes = ValuesMarshal.AsBytes(_value.AsSpan());
            bytes.CopyTo(utf8Destination);
            bytesWritten = bytes.Length;
            return true;
        }

        bytesWritten = 0;
        return false;
    }

    public static UriAsciiPath Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
    {
        if (TryParse(utf8Text, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"Cannot parse the value '{Encoding.UTF8.GetString(utf8Text)}' as a {nameof(UriAsciiPath)}");
        return default; // unreachable
    }

    public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out UriAsciiPath result)
    {
        if (!IsValidValue(ValuesMarshal.AsAsciiChars(utf8Text)))
        {
            result = default;
            return false;
        }

        if (!AsciiString.TryParse(utf8Text, provider, out var asciiString))
        {
            result = default;
            return false;
        }

        result = new UriAsciiPath(asciiString, skipValidation: true);
        return true;
    }

    public UriAsciiPath Append(UriAsciiPath path1, UriAsciiPath path2)
    {
        if (_value.IsEmpty)
        {
            return path1.Append(path2);
        }

        if (path1.IsEmpty)
        {
            return Append(path2);
        }

        if (path2.IsEmpty)
        {
            return Append(path1);
        }

        var combined = new AsciiChar[_value.Length + path1.Length + path2.Length + 2];

        _value.AsSpan().CopyTo(combined);
        combined[_value.Length] = Separator;
        path1._value.AsSpan().CopyTo(combined.AsSpan(_value.Length + 1));
        combined[_value.Length + path1.Length + 1] = Separator;
        path2._value.AsSpan().CopyTo(combined.AsSpan(_value.Length + path1.Length + 2));

        return new UriAsciiPath(combined);
    }

    public UriAsciiPath Append(UriAsciiPath path1, UriAsciiPath path2, UriAsciiPath path3)
    {
        if (_value.IsEmpty)
        {
            return path1.Append(path2, path3);
        }

        if (path1.IsEmpty)
        {
            return Append(path2, path3);
        }

        if (path2.IsEmpty)
        {
            return Append(path1, path3);
        }

        if (path3.IsEmpty)
        {
            return Append(path1, path2);
        }

        var combined = new AsciiChar[_value.Length + path1.Length + path2.Length + path3.Length + 3];

        _value.AsSpan().CopyTo(combined);
        combined[_value.Length] = Separator;
        path1._value.AsSpan().CopyTo(combined.AsSpan(_value.Length + 1));
        combined[_value.Length + path1.Length + 1] = Separator;
        path2._value.AsSpan().CopyTo(combined.AsSpan(_value.Length + path1.Length + 2));
        combined[_value.Length + path1.Length + path2.Length + 2] = Separator;
        path3._value.AsSpan().CopyTo(combined.AsSpan(_value.Length + path1.Length + path2.Length + 3));

        return new UriAsciiPath(combined);
    }

    public UriAsciiPath AppendSegment(string path)
    {
        return Append(new UriAsciiPath(path));
    }

    /// <summary>
    /// Returns a value representing the substring from the specified index to the end of the path.
    /// If the resulting substring were to start with a '/', it is removed.
    /// </summary>
    /// <param name="startIndex"></param>
    /// <returns></returns>
    public UriAsciiPath Subpath(int startIndex)
    {
        if (_value.IsEmpty)
        {
            return Empty;
        }

        var sub = _value.AsSpan()[startIndex..];

        if (sub.Length > 0 && sub[0] == '/')
        {
            sub = sub[1..];
        }

        return new UriAsciiPath(new AsciiString(sub.ToArray()), true);
    }

    /// <summary>
    /// Creates an absolute path by prepending and appending '/' characters to the current path.
    /// </summary>
    public string ToAbsolutePath()
    {
        AsciiChar[]? rented = null;
        var requiredLength = _value.Length + 2;

        try
        {
            var span = requiredLength <= StackallocThresholds.MaxCharLength
                ? stackalloc AsciiChar[requiredLength]
                : rented = ArrayPool<AsciiChar>.Shared.Rent(requiredLength);

            if (rented is not null)
            {
                span = span[..requiredLength];
            }

            var separator = (AsciiChar)'/';

            span[0] = separator;
            _value.AsSpan().CopyTo(span[1..]);
            span[^1] = separator;

            return Encoding.UTF8.GetString(MemoryMarshal.AsBytes(span));
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<AsciiChar>.Shared.Return(rented);
            }
        }
    }

    /// <summary>
    /// Returns a value representing the substring from the specified index to the end of the path.
    /// If the resulting substring were to start with a '/', it is removed.
    /// </summary>
    /// <param name="startIndex"></param>
    /// <returns></returns>
    public string Substring(int startIndex)
    {
        return Subpath(startIndex).ToString();
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator UriAsciiPath(string value)
    {
        return Parse(value, CultureInfo.InvariantCulture);
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

    /// <summary>
    /// Creates a new <see cref="UriPath"/> from this <see cref="UriAsciiPath"/>.
    /// </summary>
    /// <returns></returns>
    public UriPath ToUriPath()
    {
        return UriPath.FromUriAsciiPath(this);
    }
}
