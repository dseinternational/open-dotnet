// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using CommunityToolkit.HighPerformance.Buffers;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

/// <summary>
///     A tag consisting only of ASCII letters or digits or dashes, forward slashes,
///     or colons ('-', '/', ':', with only letters or digits at beginning or end).
///     Tags are designed to be useable in URIs without escaping. A related, but
///     more permissive, type is <see cref="Label" />.
/// </summary>
/// <remarks>
///     See: <see href="https://www.ietf.org/rfc/rfc3986.html">RFC 3986: Uniform Resource
///     Identifier (URI): Generic Syntax</see>
/// </remarks>
[ComparableValue]
[StructLayout(LayoutKind.Auto)]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<Tag, AsciiString>))]
public readonly partial struct Tag : IComparableValue<Tag, AsciiString>, IUtf8SpanSerializable<Tag>
{
    public const int MinLength = 2;
    public const int MaxLength = 120;

    public static int MaxSerializedCharLength => MaxLength;

    public static int MaxSerializedByteLength => MaxLength;

    public static readonly Tag Empty;

    public Tag(string tag) : this(tag, false)
    {
        Guard.IsNotNull(tag);
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
            EnsureIsValidArgumentValue(AsciiString.Parse(tag));
        }

        _value = value;
        _initialized = true;
    }

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

            if (AsciiChar.IsLetterOrDigit(c) || c == '-' || c == '/' || c == ':')
            {
                continue;
            }

            return false;
        }

        return true;
    }

    public static string GetString(ReadOnlySpan<char> chars) => TagStringPool.Shared.GetOrAdd(chars);

    public static bool IsValidTag(ReadOnlySpan<char> tag)
    {
        return tag.Length is >= MinLength and <= MaxLength
            && char.IsAsciiLetterOrDigit(tag[0])
            && char.IsAsciiLetterOrDigit(tag[^1])
            && tag.All((char c) => char.IsAsciiLetterOrDigit(c) || c == '-' || c == '/' || c == ':');
    }

    public AsciiString AsAsciiString() => _value;

    public ReadOnlySpan<byte> AsBytes() => ValuesMarshal.AsBytes(_value.Span);

    public ReadOnlySpan<AsciiChar> AsAsciiChars() => _value.Span;

    public bool StartsWith(AsciiString value) => _value.StartsWith(value);

    public bool StartsWith(ReadOnlySpan<AsciiChar> value) => _value.StartsWith(value);

    public bool StartsWith(ReadOnlySpan<byte> value) => _value.StartsWith(value);

    [Obsolete("Use StartsWith(ReadOnlySpan<AsciiChar>) instead.")]
    public bool StartsWith(ReadOnlySpan<char> value) => _value.StartsWith(value);

    public bool EndsWith(AsciiString value) => _value.EndsWith(value);

    public bool EndsWith(ReadOnlySpan<AsciiChar> value) => _value.EndsWith(value);

    public bool EndsWith(ReadOnlySpan<byte> value) => _value.EndsWith(value);

    [Obsolete("Use EndsWith(ReadOnlySpan<AsciiChar>) instead.")]
    public bool EndsWith(ReadOnlySpan<char> value) => _value.EndsWith(value);

    public bool Contains(AsciiString value) => _value.Contains(value);

    public bool Contains(ReadOnlySpan<AsciiChar> value) => _value.Contains(value);

    public bool Contains(ReadOnlySpan<byte> value) => _value.Contains(value);

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator Tag(string tag) => new(tag);

    public static explicit operator string(Tag tag) => tag.ToString();

#pragma warning restore CA2225 // Operator overloads have named alternates

    private static class TagStringPool
    {
        public static readonly StringPool Shared = new();
    }
}
