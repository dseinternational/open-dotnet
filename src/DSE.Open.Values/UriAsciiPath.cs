// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using CommunityToolkit.HighPerformance;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

/// <summary>
/// A path containing only <see href="https://datatracker.ietf.org/doc/html/rfc3986#section-2.3">
/// Unreserved Characters</see> - ASCII letters, digits, '-', '.', '_' and '~'.
/// Forward slashes ('/') are interpreted to define segments and may only occur at locations other
/// than the first and last character.
/// </summary>
[ComparableValue]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<UriAsciiPath, AsciiString>))]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct UriAsciiPath : IComparableValue<UriAsciiPath, AsciiString>
{
    public static readonly AsciiChar Separator = (AsciiChar)'/';
    public static readonly AsciiChar Dash = (AsciiChar)'-';
    public static readonly AsciiChar Stop = (AsciiChar)'.';
    public static readonly AsciiChar Underscore = (AsciiChar)'_';
    public static readonly AsciiChar Tilde = (AsciiChar)'~';

    public static readonly UriAsciiPath Empty = new(default, true);

    public const int MaxLength = 256;

    public static int MaxSerializedCharLength => MaxLength;

    public UriAsciiPath(AsciiString path) : this(path, false)
    {
    }

    public UriAsciiPath(string path) : this(AsciiString.Parse(path), false)
    {
    }

    public UriAsciiPath(ReadOnlyMemory<AsciiChar> path) : this(new AsciiString(path), false)
    {
    }

    public AsciiChar this[int index] => _value[index];

    public UriAsciiPath Slice(int start, int length) => new(_value.Slice(start, length));

    public ReadOnlySpan<AsciiChar> Span => _value.Span;

    public bool IsEmpty => _value.IsEmpty;

    public int Length => _value.Length;

    public bool EndsWith(ReadOnlySpan<char> value) => _value.EndsWith(value);

    public bool EndsWith(string value) => _value.EndsWith(value);

    public bool EndsWith(UriAsciiPath value) => _value.EndsWith(value._value);

    public bool EndsWith(AsciiString value) => _value.EndsWith(value);

    public bool EndsWith(AsciiChar value) => !_value.IsEmpty && _value[_value.Length] == value;

    public bool Equals(string value) => _value.Equals(value);

    public bool Equals(ReadOnlySpan<char> value) => _value.Equals(value);

    public bool Equals(ReadOnlySpan<AsciiChar> value) => _value.Equals(value);

    public bool EqualsCaseInsensitive(UriAsciiPath other) => _value.EqualsCaseInsensitive(other._value);

    public int IndexOf(AsciiChar c) => _value.IndexOf(c);

    public int LastIndexOf(AsciiChar c) => _value.LastIndexOf(c);

    public UriAsciiPath? GetParent()
    {
        if (!_value.IsEmpty)
        {
            var lastSlashIndex = _value.LastIndexOf(Separator);

            if (lastSlashIndex > 0)
            {
                return new UriAsciiPath(_value.Span[..lastSlashIndex].ToArray());
            }
        }

        return null;
    }

    public int GetSegmentCount() => Length == 0 ? 0 : _value!.Count(c => c == '/') + 1;

    public bool StartsWith(ReadOnlySpan<char> value) => _value.StartsWith(value);

    public bool StartsWith(string value) => _value.StartsWith(value);

    public bool StartsWith(UriAsciiPath value) => _value.StartsWith(value._value);

    public bool StartsWith(AsciiString value) => _value.StartsWith(value);

    public bool StartsWith(AsciiChar value) => !_value.IsEmpty && _value[0] == value;

    public UriAsciiPath ToLower() => new(_value.ToLower(), false);

    public UriAsciiPath ToUpper() => new(_value.ToUpper(), false);

    public string ToStringLower() => _value.ToStringLower();

    public string ToStringUpper() => _value.ToStringUpper();

    private static string GetString(ReadOnlySpan<char> s) => UriPathStringPool.Shared.GetOrAdd(s);

    private static bool IsValidOuterChar(AsciiChar c)
        => AsciiChar.IsLetterOrDigit(c) || c == Dash || c == Stop || c == Underscore || c == Tilde;

    private static bool IsValidOuterChar(char c)
        => AsciiChar.IsLetterOrDigit(c) || c == Dash || c == Stop || c == Underscore || c == Tilde;

    private static bool IsValidInnerChar(AsciiChar c)
        => IsValidOuterChar(c) || c == Separator;

    private static bool IsValidInnerChar(char c)
        => IsValidOuterChar(c) || c == Separator;

    public static bool IsValidValue(AsciiString value) => IsValidValue(value, false);

    public static bool IsValidValue(AsciiString value, bool ignoreLeadingTrailingSlashes)
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

    public static bool IsValidValue(ReadOnlySpan<char> value) => IsValidValue(value, false);

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

    public static bool IsValidValue(string value) => IsValidValue(value.AsSpan());

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

    public static bool TryParseSanitised(string? s, out UriAsciiPath value) => TryParseSanitised(s.AsSpan(), out value);

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

        _value.Span.CopyTo(combined);
        combined[_value.Length] = Separator;
        path._value.Span.CopyTo(combined.AsSpan(_value.Length + 1));

        return new UriAsciiPath(combined);
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

        _value.Span.CopyTo(combined);
        combined[_value.Length] = Separator;
        path1._value.Span.CopyTo(combined.AsSpan(_value.Length + 1));
        combined[_value.Length + path1.Length + 1] = Separator;
        path2._value.Span.CopyTo(combined.AsSpan(_value.Length + path1.Length + 2));

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

        _value.Span.CopyTo(combined);
        combined[_value.Length] = Separator;
        path1._value.Span.CopyTo(combined.AsSpan(_value.Length + 1));
        combined[_value.Length + path1.Length + 1] = Separator;
        path2._value.Span.CopyTo(combined.AsSpan(_value.Length + path1.Length + 2));
        combined[_value.Length + path1.Length + path2.Length + 2] = Separator;
        path3._value.Span.CopyTo(combined.AsSpan(_value.Length + path1.Length + path2.Length + 3));

        return new UriAsciiPath(combined);
    }

    public UriAsciiPath AppendSegment(string path) => Append(new UriAsciiPath(path));

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

        var sub = _value.Span[startIndex..];

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
        try
        {
            var span = _value.Length < Open.StackallocThresholds.MaxCharLength - 2
                ? stackalloc AsciiChar[_value.Length + 2]
                : rented = ArrayPool<AsciiChar>.Shared.Rent(_value.Length + 2);

            var separator = (AsciiChar)'/';

            span[0] = separator;
            _value.Span.CopyTo(span[1..]);
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
    public string Substring(int startIndex) => Subpath(startIndex).ToString();

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator UriAsciiPath(string value) => Parse(value);

#pragma warning restore CA2225 // Operator overloads have named alternates

    /// <summary>
    /// Creates a new <see cref="UriPath"/> from this <see cref="UriAsciiPath"/>.
    /// </summary>
    /// <returns></returns>
    public UriPath ToUriPath() => UriPath.FromUriAsciiPath(this);
}
