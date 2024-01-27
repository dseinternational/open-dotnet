// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

/// <summary>
/// A path containing only lowercase ASCII letters and digits, hyphens ('-') and
/// forward slashes ('/') at locations other than the first and last character.
/// </summary>
[ComparableValue(AllowDefault = true)]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<UriPath, CharSequence>))]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct UriPath : IComparableValue<UriPath, CharSequence>
{
    public const char Separator = '/';
    public const char Dash = '-';

    public static readonly UriPath Empty = new(default, true);

    public const int MaxLength = 256;

    public static int MaxSerializedCharLength => MaxLength;

    public UriPath(CharSequence path) : this(path, false)
    {
    }

    public UriPath(string path) : this(CharSequence.Parse(path, CultureInfo.InvariantCulture), false)
    {
    }

    public UriPath(ReadOnlyMemory<char> path) : this(new CharSequence(path), false)
    {
    }

    public char this[int index] => _value[index];

    public UriPath Slice(int start, int length)
    {
        return new UriPath(_value.Slice(start, length));
    }

    public ReadOnlySpan<char> Span => _value.AsSpan();

    public bool IsEmpty => _value.IsEmpty;

    public int Length => _value.Length;

    public bool EndsWith(ReadOnlySpan<char> value)
    {
        return _value.EndsWith(value, StringComparison.Ordinal);
    }

    public bool EndsWith(UriPath value)
    {
        return _value.EndsWith(value._value, StringComparison.Ordinal);
    }

    public bool EndsWith(CharSequence value)
    {
        return _value.EndsWith(value, StringComparison.Ordinal);
    }

    public bool EndsWith(char value)
    {
        return !_value.IsEmpty && _value[_value.Length] == value;
    }

    public bool Equals(string value)
    {
        return _value.Equals(value, StringComparison.Ordinal);
    }

    public bool Equals(ReadOnlySpan<char> value)
    {
        return _value.Equals(value, StringComparison.Ordinal);
    }

    public int IndexOf(char c)
    {
        return _value.IndexOf(c);
    }

    public int LastIndexOf(char c)
    {
        return _value.LastIndexOf(c);
    }

    public UriPath? GetParent()
    {
        if (!_value.IsEmpty)
        {
            var lastSlashIndex = _value.LastIndexOf(Separator);

            if (lastSlashIndex > 0)
            {
                return new UriPath(_value.AsSpan()[..lastSlashIndex].ToArray());
            }
        }

        return null;
    }

    public int GetSegmentCount()
    {
        return Length == 0 ? 0 : _value.AsSpan().Count('/') + 1;
    }

    public bool StartsWith(ReadOnlySpan<char> value)
    {
        return _value.StartsWith(value, StringComparison.Ordinal);
    }

    public bool StartsWith(UriPath value)
    {
        return _value.StartsWith(value._value, StringComparison.Ordinal);
    }

    public bool StartsWith(CharSequence value)
    {
        return _value.StartsWith(value, StringComparison.Ordinal);
    }

    public bool StartsWith(char value)
    {
        return !_value.IsEmpty && _value[0] == value;
    }

    private static string GetString(ReadOnlySpan<char> s)
    {
        return UriPathStringPool.Shared.GetOrAdd(s);
    }

    // TODO
    private static bool IsValidOuterChar(char c)
    {
        return c != Separator;
    }

    // TODO
    private static bool IsValidInnerChar(char c)
    {
        return IsValidOuterChar(c) || c == Separator;
    }

    public static bool IsValidValue(CharSequence value)
    {
        return IsValidValue(value, false);
    }

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
            return char.IsLetterOrDigit(value[0]);
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

        return inner.AsSpan().All(IsValidInnerChar);
    }

    public static bool IsValidValue(ReadOnlySpan<char> value)
    {
        return IsValidValue(value, false);
    }

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
            return char.IsLetterOrDigit(value[0]);
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

    public static bool IsValidValue(string value)
    {
        return IsValidValue(value.AsSpan());
    }

    public static bool TryParseSanitised(ReadOnlySpan<char> s, out UriPath value)
    {
        if (s.IsEmpty)
        {
            value = Empty;
            return true;
        }

        while (s.Length > 0 && s[0] == '/')
        {
            s = s[1..];
        }

        while (s.Length > 0 && s[^1] == '/')
        {
            s = s[..^1];
        }

        if (s.Length <= MaxLength)
        {
            char[]? rentedBuffer = null;

            try
            {
                Span<char> span = s.Length <= StackallocThresholds.MaxCharLength
                    ? stackalloc char[s.Length]
                    : rentedBuffer = ArrayPool<char>.Shared.Rent(s.Length);

                var written = s.ToLowerInvariant(span);

                if (written > -1)
                {
                    return TryParse(span, out value);
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

        _value.AsSpan().CopyTo(combined);
        combined[_value.Length] = Separator;
        path._value.AsSpan().CopyTo(combined.AsSpan(_value.Length + 1));

        return new UriPath(combined);
    }

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

        _value.AsSpan().CopyTo(combined);
        combined[_value.Length] = Separator;
        path1._value.AsSpan().CopyTo(combined.AsSpan(_value.Length + 1));
        combined[_value.Length + path1.Length + 1] = Separator;
        path2._value.AsSpan().CopyTo(combined.AsSpan(_value.Length + path1.Length + 2));

        return new UriPath(combined);
    }

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

        _value.AsSpan().CopyTo(combined);
        combined[_value.Length] = Separator;
        path1._value.AsSpan().CopyTo(combined.AsSpan(_value.Length + 1));
        combined[_value.Length + path1.Length + 1] = Separator;
        path2._value.AsSpan().CopyTo(combined.AsSpan(_value.Length + path1.Length + 2));
        combined[_value.Length + path1.Length + path2.Length + 2] = Separator;
        path3._value.AsSpan().CopyTo(combined.AsSpan(_value.Length + path1.Length + path2.Length + 3));

        return new UriPath(combined);
    }

    public UriPath AppendSegment(string path)
    {
        return Append(new UriPath(path));
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

        var sub = _value.AsSpan()[startIndex..];

        if (sub.Length > 0 && sub[0] == '/')
        {
            sub = sub[1..];
        }

        return new UriPath(new CharSequence(sub.ToArray()), true);
    }

    /// <summary>
    /// Creates an absolute path by prepending and appending '/' characters to the current path.
    /// </summary>
    public string ToAbsolutePath()
    {
        char[]? rented = null;
        var requiredLength = _value.Length + 2;

        try
        {
            var span = requiredLength <= StackallocThresholds.MaxCharLength
                ? stackalloc char[requiredLength]
                : rented = ArrayPool<char>.Shared.Rent(requiredLength);

            if (rented is not null)
            {
                span = span[..requiredLength];
            }

            span[0] = '/';
            _value.AsSpan().CopyTo(span[1..]);
            span[^1] = '/';

            return span.ToString();
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<char>.Shared.Return(rented);
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
        return new UriPath(CharSequence.FromAsciiString((AsciiString)uriAsciiPath), skipValidation: true);
    }
}
