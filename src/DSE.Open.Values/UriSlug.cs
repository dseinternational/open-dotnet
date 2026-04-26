// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using CommunityToolkit.HighPerformance.Buffers;
using DSE.Open.Runtime.Helpers;
using DSE.Open.Runtime.InteropServices;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

/// <summary>
/// A path containing only lowercase ASCII letters and digits, hyphens ('-') and
/// forward slashes ('/') at locations other than the first and last character.
/// </summary>
[ComparableValue]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<UriSlug, AsciiString>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct UriSlug
    : IComparableValue<UriSlug, AsciiString>,
      IUtf8SpanSerializable<UriSlug>
{
    public static readonly AsciiChar Separator = (AsciiChar)'/';
    public static readonly AsciiChar Dash = (AsciiChar)'-';

    public static readonly UriSlug Empty = new(default, true);

    public const int MaxLength = 512;

    public static int MaxSerializedByteLength => MaxLength;

    public static int MaxSerializedCharLength => MaxLength;

    public UriSlug(AsciiString path) : this(path, false)
    {
    }

    public UriSlug(string path) : this(AsciiString.Parse(path, null), false)
    {
    }

    public UriSlug(ReadOnlyMemory<AsciiChar> path) : this(new(path), false)
    {
    }

    public AsciiChar this[int index] => _value[index];

    public AsciiString Slice(int start, int length)
    {
        return _value.Slice(start, length);
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

    public bool EndsWith(UriSlug value)
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
        return !_value.IsEmpty && _value[_value.Length - 1] == value;
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

    public int IndexOf(AsciiChar c)
    {
        return _value.IndexOf(c);
    }

    public int LastIndexOf(AsciiChar c)
    {
        return _value.LastIndexOf(c);
    }

    public UriSlug? GetParent()
    {
        if (!_value.IsEmpty)
        {
            var lastSlashIndex = _value.LastIndexOf(Separator);

            if (lastSlashIndex > 0)
            {
                return new UriSlug(_value.AsSpan()[..lastSlashIndex].ToArray());
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

    public bool StartsWith(UriSlug value)
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

    private static string GetString(ReadOnlySpan<char> s)
    {
        return UriPathStringPool.Shared.GetOrAdd(s);
    }

    private static bool IsValidOuterChar(AsciiChar c)
    {
        return AsciiChar.IsLetterLower(c) || AsciiChar.IsDigit(c) || c == Dash;
    }

    private static bool IsValidOuterChar(char c)
    {
        return AsciiChar.IsLetterLower(c) || char.IsAsciiDigit(c) || c == Dash;
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
        return IsValidValue(value.AsSpan(), false);
    }

    public static bool IsValidValue(ReadOnlySpan<AsciiChar> value)
    {
        return IsValidValue(value, false);
    }

    public static bool IsValidValue(ReadOnlySpan<AsciiChar> value, bool ignoreLeadingTrailingSlashes)
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

        if (!(IsValidOuterChar(value[0]) || (ignoreLeadingTrailingSlashes && value[0] == Separator)))
        {
            return false;
        }

        if (!(IsValidOuterChar(value[^1]) || (ignoreLeadingTrailingSlashes && value[^1] == Separator)))
        {
            return false;
        }

        var inner = value[1..^1];

        return inner.All(IsValidInnerChar);
    }

    [SkipLocalsInit]
    public static bool IsValidValue(ReadOnlySpan<char> value)
    {
        return IsValidValue(value, false);
    }

    [SkipLocalsInit]
    public static bool IsValidValue(ReadOnlySpan<char> value, bool ignoreLeadingTrailingSlashes)
    {
        var rented = SpanOwner<byte>.Empty;

        Span<byte> buffer = MemoryThresholds.CanStackalloc<byte>(value.Length)
            ? stackalloc byte[value.Length]
            : (rented = SpanOwner<byte>.Allocate(value.Length)).Span;

        using (rented)
        {
            return NarrowUtf16ToAscii(value, buffer)
                && IsValidValue(ValuesMarshal.AsAsciiChars(buffer[..value.Length]), ignoreLeadingTrailingSlashes);
        }
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

    [SkipLocalsInit]
    public static bool TryParseSanitised(ReadOnlySpan<char> s, out UriSlug value)
    {
        if (s.IsEmpty)
        {
            value = Empty;
            return true;
        }

        s = s.TrimOnce('/');

        if (s.IsEmpty)
        {
            value = Empty;
            return true;
        }

        if (s.Length <= MaxLength)
        {
            var rented = SpanOwner<char>.Empty;

            Span<char> buffer = MemoryThresholds.CanStackalloc<char>(s.Length)
                ? stackalloc char[s.Length]
                : (rented = SpanOwner<char>.Allocate(s.Length)).Span;

            using (rented)
            {
                var written = s.ToLowerInvariant(buffer);

                if (written >= 0)
                {
                    return TryParse(buffer[..written], out value);
                }
            }
        }

        value = default;
        return false;
    }

    public static bool TryParseSanitised(string? s, out UriSlug value)
    {
        return TryParseSanitised(s.AsSpan(), out value);
    }

    /// <summary>
    /// Creates a new <see cref="UriSlug"/> by appending the <paramref name="path"/> to the current path.
    /// A <see cref="Separator"/> is placed between the two paths.
    /// </summary>
    /// <param name="path"></param>
    public UriSlug Append(UriSlug path)
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

        return new(combined);
    }

    public UriSlug Append(UriSlug path1, UriSlug path2)
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

        return new(combined);
    }

    public UriSlug Append(UriSlug path1, UriSlug path2, UriSlug path3)
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

        return new(combined);
    }

    public UriSlug AppendSegment(string path)
    {
        return Append(new(path));
    }

    /// <summary>
    /// Returns a value representing the substring from the specified index to the end of the path.
    /// If the resulting substring were to start with a '/', it is removed.
    /// </summary>
    /// <param name="startIndex"></param>
    /// <returns></returns>
    public UriSlug Subpath(int startIndex)
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

        return new(new(sub.ToArray()), true);
    }

    /// <summary>
    /// Creates an absolute path by prepending and appending '/' characters to the current path.
    /// </summary>
    [SkipLocalsInit]
    public string ToAbsolutePath()
    {
        var requiredLength = _value.Length + 2;

        var rented = SpanOwner<AsciiChar>.Empty;

        Span<AsciiChar> buffer = MemoryThresholds.CanStackalloc<AsciiChar>(requiredLength)
            ? stackalloc AsciiChar[requiredLength]
            : (rented = SpanOwner<AsciiChar>.Allocate(requiredLength)).Span;

        using (rented)
        {
            if (rented.Length > 0)
            {
                buffer = buffer[..requiredLength];
            }

            buffer[0] = Separator;
            _value.AsSpan().CopyTo(buffer[1..]);
            buffer[^1] = Separator;

            return Encoding.UTF8.GetString(MemoryMarshal.AsBytes(buffer));
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

    public static explicit operator UriSlug(string value)
    {
        return Parse(value, CultureInfo.InvariantCulture);
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

    /// <summary>
    /// Creates a new <see cref="UriSlug"/> from the supplied <see cref="UriAsciiPath"/>.
    /// </summary>
    /// <remarks>
    /// The conversion does not validate the source path against <see cref="UriSlug"/>'s
    /// stricter character set (lowercase letters, digits, hyphens and slashes only).
    /// Callers should ensure the source is already a valid slug, or use
    /// <see cref="IsValidValue(AsciiString)"/> first.
    /// </remarks>
    public static UriSlug FromUriAsciiPath(UriAsciiPath uriAsciiPath)
    {
        return new((AsciiString)uriAsciiPath, skipValidation: true);
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

    public static UriSlug Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
    {
        if (TryParse(utf8Text, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"Cannot parse the value '{Encoding.UTF8.GetString(utf8Text)}' as a {nameof(UriSlug)}");
        return default; // unreachable
    }

    public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out UriSlug result)
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

        result = new(asciiString, skipValidation: true);
        return true;
    }
}
