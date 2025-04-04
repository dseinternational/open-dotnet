// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using CommunityToolkit.HighPerformance.Buffers;
using DSE.Open.Hashing;
using DSE.Open.Runtime.InteropServices;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

/// <summary>
///     A tag consisting only of ASCII letters or digits or dashes, forward slashes,
///     or colons ('-', '/', ':', with only letters or digits at beginning or end).
///     Tags are designed to be useable in URI path segments without escaping.
/// </summary>
/// <remarks>
///     See: <see href="https://www.ietf.org/rfc/rfc3986.html">RFC 3986: Uniform Resource
///     Identifier (URI): Generic Syntax</see>
/// </remarks>
[ComparableValue]
[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<Tag, AsciiString>))]
public readonly partial struct Tag
    : IComparableValue<Tag, AsciiString>,
      IUtf8SpanSerializable<Tag>,
      IRepeatableHash64
{
    public const int MinLength = 2;
    public const int MaxLength = 120;

    public static int MaxSerializedCharLength => MaxLength;

    public static int MaxSerializedByteLength => MaxLength;

    public static readonly Tag Empty;

    public Tag(string tag) : this(tag, false)
    {
        ArgumentNullException.ThrowIfNull(tag);
    }

    public Tag(ReadOnlySpan<char> tag) : this(tag, false)
    {
    }

    private Tag(ReadOnlySpan<char> tag, bool skipValidation)
    {
        var success = AsciiString.TryParse(tag, out var value);

        if (!success)
        {
            throw new ArgumentOutOfRangeException(nameof(tag), tag.ToString(), $"'{tag}' is not a valid {nameof(Tag)} value");
        }

        if (!skipValidation)
        {
            EnsureIsValidValue(value);
        }

        _value = value;
    }

    private static readonly FrozenSet<char> s_nonLetterDigitChars = ['-', ':', '/', '(', ')'];

    public static bool IsValidValue(AsciiString value)
    {
        if (value.Length is < MinLength or > MaxLength)
        {
            return false;
        }

        if (!AsciiChar.IsLetterOrDigit(value[0]) || !AsciiChar.IsLetterOrDigit(value[^1]))
        {
            return false;
        }

        for (var i = 1; i < value.Length - 1; i++)
        {
            var c = value[i];

            if (AsciiChar.IsLetterOrDigit(c) || s_nonLetterDigitChars.Contains(c))
            {
                continue;
            }

            return false;
        }

        return true;
    }

    public static string GetString(ReadOnlySpan<char> chars)
    {
        return TagStringPool.Shared.GetOrAdd(chars);
    }

    public static bool IsValidTag(ReadOnlySpan<char> tag)
    {
        return tag.Length is >= MinLength and <= MaxLength
            && char.IsAsciiLetterOrDigit(tag[0])
            && char.IsAsciiLetterOrDigit(tag[^1])
            && tag.All((char c) => char.IsAsciiLetterOrDigit(c) || s_nonLetterDigitChars.Contains(c));
    }

    public AsciiString AsAsciiString()
    {
        return _value;
    }

    public ReadOnlySpan<byte> AsBytes()
    {
        return ValuesMarshal.AsBytes(_value.AsSpan());
    }

    public ReadOnlySpan<AsciiChar> AsAsciiChars()
    {
        return _value.AsSpan();
    }

    public bool StartsWith(AsciiString value)
    {
        return _value.StartsWith(value);
    }

    public bool StartsWith(ReadOnlySpan<AsciiChar> value)
    {
        return _value.StartsWith(value);
    }

    public bool StartsWith(ReadOnlySpan<byte> value)
    {
        return _value.StartsWith(value);
    }

    public bool StartsWith(ReadOnlySpan<char> value)
    {
        return _value.StartsWith(value);
    }

    public bool EndsWith(AsciiString value)
    {
        return _value.EndsWith(value);
    }

    public bool EndsWith(ReadOnlySpan<AsciiChar> value)
    {
        return _value.EndsWith(value);
    }

    public bool EndsWith(ReadOnlySpan<byte> value)
    {
        return _value.EndsWith(value);
    }

    public bool EndsWith(ReadOnlySpan<char> value)
    {
        return _value.EndsWith(value);
    }

    public bool Contains(AsciiString value)
    {
        return _value.Contains(value);
    }

    public bool Contains(ReadOnlySpan<AsciiChar> value)
    {
        return _value.Contains(value);
    }

    public bool Contains(ReadOnlySpan<byte> value)
    {
        return _value.Contains(value);
    }

    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator Tag(string tag)
    {
        return new(tag);
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

    public static explicit operator string(Tag tag)
    {
        return tag.ToString();
    }

    private static class TagStringPool
    {
        public static readonly StringPool Shared = new();
    }
}
