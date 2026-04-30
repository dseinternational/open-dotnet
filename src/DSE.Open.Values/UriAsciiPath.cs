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
/// A path containing only <see href="https://datatracker.ietf.org/doc/html/rfc3986#section-2.3">
/// Unreserved Characters</see> - ASCII letters, digits, '-', '.', '_' and '~'.
/// Forward slashes ('/') are interpreted to define segments and may only occur at locations other
/// than the first and last character.
/// </summary>
[ComparableValue]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<UriAsciiPath, AsciiString>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct UriAsciiPath
    : IComparableValue<UriAsciiPath, AsciiString>,
      IUtf8SpanSerializable<UriAsciiPath>
{
    /// <summary>The path-segment separator ('/').</summary>
    public static readonly AsciiChar Separator = (AsciiChar)'/';
    /// <summary>The dash character ('-').</summary>
    public static readonly AsciiChar Dash = (AsciiChar)'-';
    /// <summary>The full-stop character ('.').</summary>
    public static readonly AsciiChar Stop = (AsciiChar)'.';
    /// <summary>The underscore character ('_').</summary>
    public static readonly AsciiChar Underscore = (AsciiChar)'_';
    /// <summary>The tilde character ('~').</summary>
    public static readonly AsciiChar Tilde = (AsciiChar)'~';

    /// <summary>
    /// An empty <see cref="UriAsciiPath"/>.
    /// </summary>
    public static readonly UriAsciiPath Empty = new(default, true);

    /// <summary>
    /// The maximum length, in characters, of a <see cref="UriAsciiPath"/>.
    /// </summary>
    public const int MaxLength = 1024;

    /// <summary>
    /// Gets the maximum number of bytes required when serializing a <see cref="UriAsciiPath"/> as UTF-8.
    /// </summary>
    public static int MaxSerializedByteLength => MaxLength;

    /// <summary>
    /// Gets the maximum number of characters required when serializing a <see cref="UriAsciiPath"/> as text.
    /// </summary>
    public static int MaxSerializedCharLength => MaxLength;

    /// <summary>
    /// Initialises a new <see cref="UriAsciiPath"/> from the supplied <see cref="AsciiString"/>, validating its contents.
    /// </summary>
    public UriAsciiPath(AsciiString path) : this(path, false)
    {
    }

    /// <summary>
    /// Initialises a new <see cref="UriAsciiPath"/> by parsing the supplied string as ASCII.
    /// </summary>
    public UriAsciiPath(string path) : this(AsciiString.Parse(path, null), false)
    {
    }

    /// <summary>
    /// Initialises a new <see cref="UriAsciiPath"/> from the supplied ASCII memory region, validating its contents.
    /// </summary>
    public UriAsciiPath(ReadOnlyMemory<AsciiChar> path) : this(new(path), false)
    {
    }

    /// <summary>
    /// Gets the <see cref="AsciiChar"/> at the specified index in the path.
    /// </summary>
    public AsciiChar this[int index] => _value[index];

    /// <summary>
    /// Returns a sub-range of the underlying value as an <see cref="AsciiString"/>.
    /// </summary>
    public AsciiString Slice(int start, int length)
    {
        return _value.Slice(start, length);
    }

    /// <summary>
    /// Gets a read-only span over the underlying ASCII characters.
    /// </summary>
    public ReadOnlySpan<AsciiChar> Span => _value.AsSpan();

    /// <summary>
    /// Gets a value indicating whether the path is empty.
    /// </summary>
    public bool IsEmpty => _value.IsEmpty;

    /// <summary>
    /// Gets the length of the path, in characters.
    /// </summary>
    public int Length => _value.Length;

    /// <summary>
    /// Determines whether this path ends with <paramref name="value"/>.
    /// </summary>
    public bool EndsWith(ReadOnlySpan<char> value)
    {
        return _value.EndsWith(value);
    }

    /// <summary>
    /// Determines whether this path ends with <paramref name="value"/>.
    /// </summary>
    public bool EndsWith(string value)
    {
        return _value.EndsWith(value);
    }

    /// <summary>
    /// Determines whether this path ends with <paramref name="value"/>.
    /// </summary>
    public bool EndsWith(UriAsciiPath value)
    {
        return _value.EndsWith(value._value);
    }

    /// <summary>
    /// Determines whether this path ends with <paramref name="value"/>.
    /// </summary>
    public bool EndsWith(AsciiString value)
    {
        return _value.EndsWith(value);
    }

    /// <summary>
    /// Determines whether this path ends with <paramref name="value"/>.
    /// </summary>
    public bool EndsWith(ReadOnlySpan<byte> value)
    {
        return _value.EndsWith(value);
    }

    /// <summary>
    /// Determines whether this path ends with the specified character.
    /// </summary>
    public bool EndsWith(AsciiChar value)
    {
        return !_value.IsEmpty && _value[_value.Length - 1] == value;
    }

    /// <summary>
    /// Determines whether this path is equal to the supplied string using an ordinal comparison.
    /// </summary>
    public bool Equals(string value)
    {
        return _value.Equals(value);
    }

    /// <summary>
    /// Determines whether this path is equal to the supplied character span using an ordinal comparison.
    /// </summary>
    public bool Equals(ReadOnlySpan<char> value)
    {
        return _value.Equals(value);
    }

    /// <summary>
    /// Determines whether this path is equal to the supplied ASCII span using an ordinal comparison.
    /// </summary>
    public bool Equals(ReadOnlySpan<AsciiChar> value)
    {
        return _value.Equals(value);
    }

    /// <summary>
    /// Determines whether this path is equal to <paramref name="other"/>, ignoring case.
    /// </summary>
    public bool EqualsIgnoreCase(UriAsciiPath other)
    {
        return _value.EqualsIgnoreCase(other._value);
    }

    /// <summary>
    /// Returns the zero-based index of the first occurrence of <paramref name="c"/>, or -1 if not found.
    /// </summary>
    public int IndexOf(AsciiChar c)
    {
        return _value.IndexOf(c);
    }

    /// <summary>
    /// Returns the zero-based index of the last occurrence of <paramref name="c"/>, or -1 if not found.
    /// </summary>
    public int LastIndexOf(AsciiChar c)
    {
        return _value.LastIndexOf(c);
    }

    /// <summary>
    /// Returns the parent path (the portion before the last <see cref="Separator"/>),
    /// or <see langword="null"/> if there is no parent.
    /// </summary>
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

    /// <summary>
    /// Returns the number of segments in the path (segments are separated by <see cref="Separator"/>).
    /// </summary>
    public int GetSegmentCount()
    {
        return Length == 0 ? 0 : _value.AsSpan().Count((AsciiChar)'/') + 1;
    }

    /// <summary>
    /// Determines whether this path begins with <paramref name="value"/>.
    /// </summary>
    public bool StartsWith(ReadOnlySpan<char> value)
    {
        return _value.StartsWith(value);
    }

    /// <summary>
    /// Determines whether this path begins with <paramref name="value"/>.
    /// </summary>
    public bool StartsWith(string value)
    {
        return _value.StartsWith(value);
    }

    /// <summary>
    /// Determines whether this path begins with <paramref name="value"/>.
    /// </summary>
    public bool StartsWith(UriAsciiPath value)
    {
        return _value.StartsWith(value._value);
    }

    /// <summary>
    /// Determines whether this path begins with <paramref name="value"/>.
    /// </summary>
    public bool StartsWith(AsciiString value)
    {
        return _value.StartsWith(value);
    }

    /// <summary>
    /// Determines whether this path begins with <paramref name="value"/>.
    /// </summary>
    public bool StartsWith(ReadOnlySpan<byte> value)
    {
        return _value.StartsWith(value);
    }

    /// <summary>
    /// Determines whether this path begins with the specified character.
    /// </summary>
    public bool StartsWith(AsciiChar value)
    {
        return !_value.IsEmpty && _value[0] == value;
    }

    /// <summary>
    /// Returns a new <see cref="UriAsciiPath"/> with all characters converted to lower case.
    /// </summary>
    public UriAsciiPath ToLower()
    {
        return new(_value.ToLower(), false);
    }

    /// <summary>
    /// Returns a new <see cref="UriAsciiPath"/> with all characters converted to upper case.
    /// </summary>
    public UriAsciiPath ToUpper()
    {
        return new(_value.ToUpper(), false);
    }

    /// <summary>
    /// Returns the path as a lower-case string.
    /// </summary>
    public string ToStringLower()
    {
        return _value.ToStringLower();
    }

    /// <summary>
    /// Returns the path as an upper-case string.
    /// </summary>
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

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> is a valid <see cref="UriAsciiPath"/>.
    /// </summary>
    public static bool IsValidValue(AsciiString value)
    {
        return IsValidValue(value.AsSpan());
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> is a valid <see cref="UriAsciiPath"/>.
    /// </summary>
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

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> is a valid <see cref="UriAsciiPath"/>.
    /// </summary>
    [SkipLocalsInit]
    public static bool IsValidValue(ReadOnlySpan<char> value)
    {
        var rented = SpanOwner<byte>.Empty;

        Span<byte> buffer = MemoryThresholds.CanStackalloc<byte>(value.Length)
            ? stackalloc byte[value.Length]
            : (rented = SpanOwner<byte>.Allocate(value.Length)).Span;

        using (rented)
        {
            return NarrowUtf16ToAscii(value, buffer)
                && IsValidValue(ValuesMarshal.AsAsciiChars(buffer[..value.Length]));
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

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> is a valid <see cref="UriAsciiPath"/>.
    /// </summary>
    public static bool IsValidValue(string value)
    {
        return IsValidValue(value.AsSpan());
    }

    /// <summary>
    /// Attempts to parse the supplied character span as a <see cref="UriAsciiPath"/>, lower-casing the
    /// input and trimming a single leading or trailing '/'.
    /// </summary>
    [SkipLocalsInit]
    public static bool TryParseSanitised(ReadOnlySpan<char> s, out UriAsciiPath value)
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
                ? stackalloc char[MemoryThresholds.StackallocCharThreshold]
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

    /// <summary>
    /// Attempts to parse the supplied string as a <see cref="UriAsciiPath"/>, lower-casing the
    /// input and trimming a single leading or trailing '/'.
    /// </summary>
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

        return new(combined);
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public static UriAsciiPath Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
    {
        if (TryParse(utf8Text, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"Cannot parse the value '{Encoding.UTF8.GetString(utf8Text)}' as a {nameof(UriAsciiPath)}");
        return default; // unreachable
    }

    /// <inheritdoc/>
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

        result = new(asciiString, skipValidation: true);
        return true;
    }

    /// <summary>
    /// Creates a new <see cref="UriAsciiPath"/> by appending <paramref name="path1"/> and <paramref name="path2"/>
    /// to the current path, separated by <see cref="Separator"/>.
    /// </summary>
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

        return new(combined);
    }

    /// <summary>
    /// Creates a new <see cref="UriAsciiPath"/> by appending <paramref name="path1"/>, <paramref name="path2"/>
    /// and <paramref name="path3"/> to the current path, separated by <see cref="Separator"/>.
    /// </summary>
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

        return new(combined);
    }

    /// <summary>
    /// Creates a new <see cref="UriAsciiPath"/> by appending the supplied segment string, separated by <see cref="Separator"/>.
    /// </summary>
    public UriAsciiPath AppendSegment(string path)
    {
        return Append(new(path));
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

            var separator = (AsciiChar)'/';

            buffer[0] = separator;
            _value.AsSpan().CopyTo(buffer[1..]);
            buffer[^1] = separator;

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

    /// <summary>
    /// Explicitly converts a string to a <see cref="UriAsciiPath"/> by parsing it with the invariant culture.
    /// </summary>
    public static explicit operator UriAsciiPath(string value)
    {
        return Parse(value, CultureInfo.InvariantCulture);
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

    /// <summary>
    /// Creates a new <see cref="UriPath"/> from this <see cref="UriAsciiPath"/>.
    /// </summary>
    /// <returns></returns>
    [Obsolete("UriPath has been renamed to UriSlug. Use ToUriSlug instead.")]
#pragma warning disable CS0618 // Type or member is obsolete
    public UriPath ToUriPath()
    {
        return UriPath.FromUriAsciiPath(this);
    }
#pragma warning restore CS0618 // Type or member is obsolete

    /// <summary>
    /// Creates a new <see cref="UriSlug"/> from this <see cref="UriAsciiPath"/>.
    /// </summary>
    /// <returns></returns>
    public UriSlug ToUriSlug()
    {
        return UriSlug.FromUriAsciiPath(this);
    }
}
