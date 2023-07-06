// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using CommunityToolkit.HighPerformance;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

/// <summary>
/// A path containing only lowercase ASCII letters and digits, hyphens ('-') and
/// forward slashes ('/') at locations other than the first and last character.
/// </summary>
[ComparableValue]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<UriPath, CharSequence>))]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct UriPath : IComparableValue<UriPath, CharSequence>
{
    public static readonly char Separator = '/';
    public static readonly char Dash = '-';

    public static readonly UriPath Empty = new(default, true);

    public const int MaxLength = 256;

    public static int MaxSerializedCharLength => MaxLength;

    public UriPath(CharSequence path) : this(path, false)
    {
    }

    public UriPath(string path) : this(CharSequence.Parse(path), false)
    {
    }

    public UriPath(ReadOnlyMemory<char> path) : this(new CharSequence(path), false)
    {
    }

    public char this[int index] => _value[index];

    public UriPath Slice(int start, int length) => new(_value.Slice(start, length));

    public ReadOnlySpan<char> Span => _value.Span;

    public bool IsEmpty => _value.IsEmpty;

    public int Length => _value.Length;

    public bool EndsWith(ReadOnlySpan<char> value) => _value.EndsWith(value, StringComparison.Ordinal);

    public bool EndsWith(UriPath value) => _value.EndsWith(value._value, StringComparison.Ordinal);

    public bool EndsWith(CharSequence value) => _value.EndsWith(value, StringComparison.Ordinal);

    public bool EndsWith(char value) => !_value.IsEmpty && _value[_value.Length] == value;

    public bool Equals(string value) => _value.Equals(value, StringComparison.Ordinal);

    public bool Equals(ReadOnlySpan<char> value) => _value.Equals(value, StringComparison.Ordinal);

    public int IndexOf(char c) => _value.IndexOf(c);

    public int LastIndexOf(char c) => _value.LastIndexOf(c);

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

    public int GetSegmentCount() => Length == 0 ? 0 : _value.Span.Count('/') + 1;

    public bool StartsWith(ReadOnlySpan<char> value) => _value.StartsWith(value, StringComparison.Ordinal);

    public bool StartsWith(UriPath value) => _value.StartsWith(value._value, StringComparison.Ordinal);

    public bool StartsWith(CharSequence value) => _value.StartsWith(value, StringComparison.Ordinal);

    public bool StartsWith(char value) => !_value.IsEmpty && _value[0] == value;

    private static string GetString(ReadOnlySpan<char> s) => UriPathStringPool.Shared.GetOrAdd(s);

    // TODO
    private static bool IsValidOuterChar(char c) => c != Separator;

    // TODO
    private static bool IsValidInnerChar(char c) => IsValidOuterChar(c) || c == Separator;

    public static bool IsValidValue(CharSequence value) => IsValidValue(value, false);

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

        return inner.Span.All(IsValidInnerChar);
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

    public static bool IsValidValue(string value) => IsValidValue(value.AsSpan());

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

    public static bool TryParseSanitised(string? s, out UriPath value) => TryParseSanitised(s.AsSpan(), out value);

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

        var combined = new char[_value.Length + path.Length + 1];

        _value.Span.CopyTo(combined);
        combined[_value.Length] = Separator;
        path._value.Span.CopyTo(combined.AsSpan(_value.Length + 1));

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

        _value.Span.CopyTo(combined);
        combined[_value.Length] = Separator;
        path1._value.Span.CopyTo(combined.AsSpan(_value.Length + 1));
        combined[_value.Length + path1.Length + 1] = Separator;
        path2._value.Span.CopyTo(combined.AsSpan(_value.Length + path1.Length + 2));

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

        _value.Span.CopyTo(combined);
        combined[_value.Length] = Separator;
        path1._value.Span.CopyTo(combined.AsSpan(_value.Length + 1));
        combined[_value.Length + path1.Length + 1] = Separator;
        path2._value.Span.CopyTo(combined.AsSpan(_value.Length + path1.Length + 2));
        combined[_value.Length + path1.Length + path2.Length + 2] = Separator;
        path3._value.Span.CopyTo(combined.AsSpan(_value.Length + path1.Length + path2.Length + 3));

        return new UriPath(combined);
    }

    public UriPath AppendSegment(string path) => Append(new UriPath(path));

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

        return new UriPath(new CharSequence(sub.ToArray()), true);
    }

    /// <summary>
    /// Returns a value representing the substring from the specified index to the end of the path.
    /// If the resulting substring were to start with a '/', it is removed.
    /// </summary>
    /// <param name="startIndex"></param>
    /// <returns></returns>
    public string Substring(int startIndex) => Subpath(startIndex).ToString();

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator UriPath(string value) => Parse(value);

#pragma warning restore CA2225 // Operator overloads have named alternates

}
