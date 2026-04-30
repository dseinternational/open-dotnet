// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using CommunityToolkit.HighPerformance.Buffers;
using DSE.Open.Runtime.Helpers;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

/// <summary>
/// A path containing only lowercase ASCII letters and digits, hyphens ('-') and
/// forward slashes ('/') at locations other than the first and last character.
/// </summary>
/// <remarks>
/// This type has been renamed to <see cref="UriSlug"/>. The two types are
/// behaviourally identical; <see cref="UriPath"/> is retained as an obsolete
/// alias to avoid breaking existing consumers and will be removed in a future
/// release.
/// </remarks>
[Obsolete("UriPath has been renamed to UriSlug. Use UriSlug instead.")]
[ComparableValue]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<UriPath, CharSequence>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct UriPath : IComparableValue<UriPath, CharSequence>
{
    /// <summary>The path-segment separator ('/').</summary>
    public const char Separator = '/';
    /// <summary>The dash character ('-').</summary>
    public const char Dash = '-';

    /// <summary>
    /// An empty <see cref="UriPath"/>.
    /// </summary>
    public static readonly UriPath Empty = new(default, true);

    /// <summary>
    /// The maximum length, in characters, of a <see cref="UriPath"/>.
    /// </summary>
    public const int MaxLength = 512;

    /// <summary>
    /// Gets the maximum number of characters required when serializing a <see cref="UriPath"/> as text.
    /// </summary>
    public static int MaxSerializedCharLength => MaxLength;

    /// <summary>
    /// Initialises a new <see cref="UriPath"/> from the supplied <see cref="CharSequence"/>, validating its contents.
    /// </summary>
    public UriPath(CharSequence path) : this(path, false)
    {
    }

    /// <summary>
    /// Initialises a new <see cref="UriPath"/> by parsing the supplied string with the invariant culture.
    /// </summary>
    public UriPath(string path) : this(CharSequence.Parse(path, CultureInfo.InvariantCulture), false)
    {
    }

    /// <summary>
    /// Initialises a new <see cref="UriPath"/> from the supplied character memory region, validating its contents.
    /// </summary>
    public UriPath(ReadOnlyMemory<char> path) : this(new(path), false)
    {
    }

    /// <summary>
    /// Gets the character at the specified index in the path.
    /// </summary>
    public char this[int index] => _value[index];

    /// <summary>
    /// Returns a sub-range of the underlying value as a <see cref="CharSequence"/>.
    /// </summary>
    public CharSequence Slice(int start, int length)
    {
        return _value.Slice(start, length);
    }

    /// <summary>
    /// Gets a read-only span over the path's characters.
    /// </summary>
    public ReadOnlySpan<char> Span => _value.Span;

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
        return _value.EndsWith(value, StringComparison.Ordinal);
    }

    /// <summary>
    /// Determines whether this path ends with <paramref name="value"/>.
    /// </summary>
    public bool EndsWith(UriPath value)
    {
        return _value.EndsWith(value._value, StringComparison.Ordinal);
    }

    /// <summary>
    /// Determines whether this path ends with <paramref name="value"/>.
    /// </summary>
    public bool EndsWith(CharSequence value)
    {
        return _value.EndsWith(value, StringComparison.Ordinal);
    }

    /// <summary>
    /// Determines whether this path ends with the specified character.
    /// </summary>
    public bool EndsWith(char value)
    {
        return _value.EndsWith(value);
    }

    /// <summary>
    /// Determines whether this path is equal to the supplied string using an ordinal comparison.
    /// </summary>
    public bool Equals(string value)
    {
        return _value.Equals(value, StringComparison.Ordinal);
    }

    /// <summary>
    /// Determines whether this path is equal to the supplied character span using an ordinal comparison.
    /// </summary>
    public bool Equals(ReadOnlySpan<char> value)
    {
        return _value.Equals(value, StringComparison.Ordinal);
    }

    /// <summary>
    /// Returns the zero-based index of the first occurrence of <paramref name="c"/>, or -1 if not found.
    /// </summary>
    public int IndexOf(char c)
    {
        return _value.IndexOf(c);
    }

    /// <summary>
    /// Returns the zero-based index of the last occurrence of <paramref name="c"/>, or -1 if not found.
    /// </summary>
    public int LastIndexOf(char c)
    {
        return _value.LastIndexOf(c);
    }

    /// <summary>
    /// Returns the parent path (the portion before the last <see cref="Separator"/>),
    /// or <see langword="null"/> if there is no parent.
    /// </summary>
    public UriPath? GetParent()
    {
        if (!_value.IsEmpty)
        {
            var lastSlashIndex = _value.LastIndexOf(Separator);

            if (lastSlashIndex > 0)
            {
                return new UriPath(_value.Span[..lastSlashIndex].ToArray());
            }
        }

        return null;
    }

    /// <summary>
    /// Returns the number of segments in the path (segments are separated by <see cref="Separator"/>).
    /// </summary>
    public int GetSegmentCount()
    {
        return Length == 0 ? 0 : _value.Span.Count('/') + 1;
    }

    /// <summary>
    /// Determines whether this path begins with <paramref name="value"/>.
    /// </summary>
    public bool StartsWith(ReadOnlySpan<char> value)
    {
        return _value.StartsWith(value, StringComparison.Ordinal);
    }

    /// <summary>
    /// Determines whether this path begins with <paramref name="value"/>.
    /// </summary>
    public bool StartsWith(UriPath value)
    {
        return _value.StartsWith(value._value, StringComparison.Ordinal);
    }

    /// <summary>
    /// Determines whether this path begins with <paramref name="value"/>.
    /// </summary>
    public bool StartsWith(CharSequence value)
    {
        return _value.StartsWith(value, StringComparison.Ordinal);
    }

    /// <summary>
    /// Determines whether this path begins with the specified character.
    /// </summary>
    public bool StartsWith(char value)
    {
        return !_value.IsEmpty && _value[0] == value;
    }

    private static string GetString(ReadOnlySpan<char> s)
    {
        return UriPathStringPool.Shared.GetOrAdd(s);
    }

    private static bool IsValidOuterChar(char c)
    {
        return char.IsAsciiLetterLower(c) || char.IsAsciiDigit(c) || c == Dash;
    }

    private static bool IsValidInnerChar(char c)
    {
        return IsValidOuterChar(c) || c == Separator;
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> is a valid <see cref="UriPath"/>.
    /// </summary>
    public static bool IsValidValue(CharSequence value)
    {
        return IsValidValue(value, false);
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> is a valid <see cref="UriPath"/>,
    /// optionally permitting leading or trailing <see cref="Separator"/> characters.
    /// </summary>
    public static bool IsValidValue(CharSequence value, bool ignoreLeadingTrailingSlashes)
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

        return inner.Span.All(IsValidInnerChar);
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> is a valid <see cref="UriPath"/>.
    /// </summary>
    public static bool IsValidValue(ReadOnlySpan<char> value)
    {
        return IsValidValue(value, false);
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> is a valid <see cref="UriPath"/>,
    /// optionally permitting leading or trailing <see cref="Separator"/> characters.
    /// </summary>
    public static bool IsValidValue(ReadOnlySpan<char> value, bool ignoreLeadingTrailingSlashes)
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

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> is a valid <see cref="UriPath"/>.
    /// </summary>
    public static bool IsValidValue(string value)
    {
        return IsValidValue(value.AsSpan());
    }

    /// <summary>
    /// Attempts to parse the supplied character span as a <see cref="UriPath"/>, lower-casing the
    /// input and trimming a single leading or trailing '/'.
    /// </summary>
    [SkipLocalsInit]
    public static bool TryParseSanitised(ReadOnlySpan<char> s, out UriPath value)
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

                if (written > -1)
                {
                    return TryParse(buffer, out value);
                }
            }
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Attempts to parse the supplied string as a <see cref="UriPath"/>, lower-casing the
    /// input and trimming a single leading or trailing '/'.
    /// </summary>
    public static bool TryParseSanitised(string? s, out UriPath value)
    {
        return TryParseSanitised(s.AsSpan(), out value);
    }

    /// <summary>
    /// Creates a new <see cref="UriPath"/> by appending the <paramref name="path"/> to the current path.
    /// A <see cref="Separator"/> is placed between the two paths.
    /// </summary>
    /// <param name="path"></param>
    public UriPath Append(UriPath path)
    {
        if (_value.IsEmpty)
        {
            return path;
        }

        if (path.IsEmpty)
        {
            return this;
        }

        var combined = new char[_value.Length + path.Length + 1];

        _value.Span.CopyTo(combined);
        combined[_value.Length] = Separator;
        path._value.Span.CopyTo(combined.AsSpan(_value.Length + 1));

        return new(combined);
    }

    /// <summary>
    /// Creates a new <see cref="UriPath"/> by appending <paramref name="path1"/> and <paramref name="path2"/>
    /// to the current path, separated by <see cref="Separator"/>.
    /// </summary>
    public UriPath Append(UriPath path1, UriPath path2)
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

        var combined = new char[_value.Length + path1.Length + path2.Length + 2];

        _value.Span.CopyTo(combined);
        combined[_value.Length] = Separator;
        path1._value.Span.CopyTo(combined.AsSpan(_value.Length + 1));
        combined[_value.Length + path1.Length + 1] = Separator;
        path2._value.Span.CopyTo(combined.AsSpan(_value.Length + path1.Length + 2));

        return new(combined);
    }

    /// <summary>
    /// Creates a new <see cref="UriPath"/> by appending <paramref name="path1"/>, <paramref name="path2"/>
    /// and <paramref name="path3"/> to the current path, separated by <see cref="Separator"/>.
    /// </summary>
    public UriPath Append(UriPath path1, UriPath path2, UriPath path3)
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

        var combined = new char[_value.Length + path1.Length + path2.Length + path3.Length + 3];

        _value.Span.CopyTo(combined);
        combined[_value.Length] = Separator;
        path1._value.Span.CopyTo(combined.AsSpan(_value.Length + 1));
        combined[_value.Length + path1.Length + 1] = Separator;
        path2._value.Span.CopyTo(combined.AsSpan(_value.Length + path1.Length + 2));
        combined[_value.Length + path1.Length + path2.Length + 2] = Separator;
        path3._value.Span.CopyTo(combined.AsSpan(_value.Length + path1.Length + path2.Length + 3));

        return new(combined);
    }

    /// <summary>
    /// Creates a new <see cref="UriPath"/> by appending the supplied segment string, separated by <see cref="Separator"/>.
    /// </summary>
    public UriPath AppendSegment(string path)
    {
        return Append(new(path));
    }

    /// <summary>
    /// Returns a value representing the substring from the specified index to the end of the path.
    /// If the resulting substring were to start with a '/', it is removed.
    /// </summary>
    /// <param name="startIndex"></param>
    /// <returns></returns>
    public UriPath Subpath(int startIndex)
    {
        if (_value.IsEmpty)
        {
            return Empty;
        }

        var sub = _value.Span[startIndex..];

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

        var rented = SpanOwner<char>.Empty;

        Span<char> buffer = MemoryThresholds.CanStackalloc<char>(requiredLength)
            ? stackalloc char[requiredLength]
            : (rented = SpanOwner<char>.Allocate(requiredLength)).Span;

        using (rented)
        {
            if (rented.Length > 0)
            {
                buffer = buffer[..requiredLength];
            }

            buffer[0] = '/';
            _value.Span.CopyTo(buffer[1..]);
            buffer[^1] = '/';

            return buffer.ToString();
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
    /// Explicitly converts a string to a <see cref="UriPath"/>.
    /// </summary>
    public static explicit operator UriPath(string value)
    {
        return Parse(value, null);
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

    /// <summary>
    /// Converts the <paramref name="uriAsciiPath"/> to a <see cref="UriPath"/>.
    /// </summary>
    /// <param name="uriAsciiPath"></param>
    /// <returns></returns>
    public static UriPath FromUriAsciiPath(UriAsciiPath uriAsciiPath)
    {
        return new(CharSequence.FromAsciiString((AsciiString)uriAsciiPath), skipValidation: true);
    }
}
