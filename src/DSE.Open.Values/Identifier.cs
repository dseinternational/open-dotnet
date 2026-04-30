// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Runtime.InteropServices;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

/// <summary>
/// An ASCII identifier, optionally prefixed by a short tag separated with <see cref="PrefixDelimiter"/>.
/// </summary>
[EquatableValue]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<Identifier, AsciiString>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct Identifier : IEquatableValue<Identifier, AsciiString>,
    IIdentifier<Identifier>,
    IEquatable<string>,
    IUtf8SpanSerializable<Identifier>,
    IRepeatableHash64
{
    /// <summary>
    /// The default length used for newly generated identifiers, excluding any prefix.
    /// </summary>
    public const int DefaultLength = 48;

    /// <summary>
    /// The minimum length of an identifier, excluding the prefix.
    /// </summary>
    public const int MinIdLength = 12; // e.g. Stripe customer id is 14 (cus_MBvF2uhJkOAcKF)

    /// <summary>
    /// The maximum length of an identifier, excluding the prefix.
    /// </summary>
    public const int MaxIdLength = 256;

    /// <summary>
    /// The minimum length of a prefix.
    /// </summary>
    public const int MinPrefixLength = 2;

    /// <summary>
    /// The maximum length of a prefix.
    /// </summary>
    public const int MaxPrefixLength = 23;

    /// <summary>
    /// The maximum total length of an identifier, including any prefix and delimiter.
    /// </summary>
    public const int MaxLength = MaxPrefixLength + MaxIdLength;

    /// <summary>
    /// Gets the maximum number of characters required when serializing an <see cref="Identifier"/> as text.
    /// </summary>
    public static int MaxSerializedCharLength => MaxLength;

    /// <summary>
    /// Gets the maximum number of bytes required when serializing an <see cref="Identifier"/> as UTF-8.
    /// </summary>
    public static int MaxSerializedByteLength => MaxLength;

    /// <summary>
    /// The character used to separate the prefix from the identifier value.
    /// </summary>
    public const char PrefixDelimiter = '_';

    private const int ValidIdByteCount = 62;

    private const int RandomIdByteUpperBound = byte.MaxValue + 1 - ((byte.MaxValue + 1) % ValidIdByteCount);

    private static ReadOnlySpan<byte> ValidIdBytes => "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"u8;

    private static ReadOnlySpan<byte> ValidPrefixBytes => "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_"u8;

    private static readonly SearchValues<byte> s_validPrefixBytes = SearchValues.Create(ValidPrefixBytes);

    /// <summary>
    /// An empty <see cref="Identifier"/>.
    /// </summary>
    public static readonly Identifier Empty;

    private Identifier(ReadOnlyMemory<AsciiChar> value)
    {
        _value = new(value);
    }

    /// <summary>
    /// Gets the total number of ASCII characters in the identifier, including any prefix.
    /// </summary>
    public int Length => _value.Length;

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> satisfies the prefix and identifier length and character rules for an <see cref="Identifier"/>.
    /// </summary>
    public static bool IsValidValue(AsciiString value)
    {
        if (value.Length is < MinIdLength or > MaxLength)
        {
            return false;
        }

        var prefixEndIndex = value.AsSpan().LastIndexOf((AsciiChar)PrefixDelimiter);

        switch (prefixEndIndex)
        {
            case -1:
                return value.AsSpan().ContainsOnlyAsciiLettersOrDigits();
            case 0:
            case > MaxPrefixLength:
                // '_' is not valid in id (beyond prefix) and cannot start with '_'
                return false;
            default:
                break;
        }

        var prefix = value.AsSpan()[..prefixEndIndex];
        var id = value.AsSpan()[(prefixEndIndex + 1)..];

        return prefix[0] != (AsciiChar)PrefixDelimiter
               && prefix.All(id => id == PrefixDelimiter || AsciiChar.IsLetterOrDigit(id))
               && id.Length is >= MinIdLength and <= MaxIdLength
               && id.ContainsOnlyAsciiLettersOrDigits();
    }

    /// <summary>
    /// Returns <see langword="true"/> if the supplied character span is a valid <see cref="Identifier"/>.
    /// </summary>
    public static bool IsValid(ReadOnlySpan<char> id)
    {
        if (!AsciiString.TryParse(id, out var value))
        {
            return false;
        }

        return IsValidValue(value);
    }

    /// <summary>
    /// Splits the supplied identifier into its prefix and identifier portions on the last <see cref="PrefixDelimiter"/>;
    /// when no delimiter is present the prefix is empty and the entire input is treated as the id.
    /// </summary>
    public static (ReadOnlyMemory<char> prefix, ReadOnlyMemory<char> id) Split(ReadOnlyMemory<char> uid)
    {
        if (uid.IsEmpty)
        {
            return (null, uid);
        }

        var i = uid.Span.LastIndexOf(PrefixDelimiter);

        if (i < 0)
        {
            return (null, uid);
        }

        return (prefix: uid[..i], id: uid[(i + 1)..]);
    }

    private static bool IsValidPrefix(ReadOnlySpan<byte> prefix)
    {
        return prefix.Length is >= MinPrefixLength and <= MaxPrefixLength && prefix.IndexOfAnyExcept(s_validPrefixBytes) < 0;
    }

    /// <summary>
    /// Creates a new <see cref="Identifier"/> of default length with a random value.
    /// </summary>
    public static Identifier New()
    {
        return New(DefaultLength);
    }

    /// <summary>
    /// Creates a new <see cref="Identifier"/> of the specified length with a random value.
    /// </summary>
    /// <param name="length">The length of <see cref="Identifier"/> to create.</param>
    public static Identifier New(int length)
    {
        return New(length, ReadOnlySpan<byte>.Empty);
    }

    /// <summary>
    /// Creates a new <see cref="Identifier"/> with the given prefix and a random value.
    /// </summary>
    /// <param name="prefix">The prefix to use.</param>
    public static Identifier New(ReadOnlySpan<char> prefix)
    {
        return New(DefaultLength, prefix);
    }

    /// <summary>
    /// Creates a new <see cref="Identifier"/> of the specified length with the given prefix and a random value
    /// </summary>
    /// <param name="idLength">The length of <see cref="Identifier"/> to create.</param>
    /// <param name="prefix">The prefix to use.</param>
    /// <returns>A new <see cref="Identifier"/> with the given prefix and a random value.</returns>
    [SkipLocalsInit]
    public static Identifier New(int idLength, ReadOnlySpan<char> prefix)
    {
        Guard.IsBetweenOrEqualTo(idLength, MinIdLength, MaxIdLength);

        if (prefix.IsEmpty)
        {
            return New(idLength, ReadOnlySpan<byte>.Empty);
        }

        if (prefix.Length > MaxPrefixLength)
        {
            return ThrowHelper.ThrowArgumentException<Identifier>(nameof(prefix),
                $"Invalid {nameof(Identifier)} prefix '{prefix.ToString()}'");
        }

        Span<byte> utf8 = stackalloc byte[MaxPrefixLength];

        var status = Ascii.FromUtf16(prefix, utf8, out var bytesWritten);

        if (status is OperationStatus.Done)
        {
            Debug.Assert(bytesWritten == prefix.Length);
            return New(idLength, utf8[..bytesWritten]);
        }

        Debug.Assert(status is OperationStatus.InvalidData);

        return ThrowHelper.ThrowArgumentException<Identifier>(nameof(prefix),
            $"Invalid {nameof(Identifier)} prefix '{prefix.ToString()}'");
    }

    /// <summary>
    /// Creates a new <see cref="Identifier"/> of the specified length with the given prefix and a random value
    /// </summary>
    /// <param name="idLength">The length of <see cref="Identifier"/> to create.</param>
    /// <param name="utf8Prefix">The prefix to use.</param>
    /// <returns>A new <see cref="Identifier"/> with the given prefix and a random value.</returns>
    public static Identifier New(int idLength, ReadOnlySpan<byte> utf8Prefix)
    {
        Guard.IsBetweenOrEqualTo(idLength, MinIdLength, MaxIdLength);

        if (!utf8Prefix.IsEmpty && !IsValidPrefix(utf8Prefix))
        {
            ThrowHelper.ThrowArgumentException(nameof(utf8Prefix),
                $"Invalid {nameof(Identifier)} prefix '{Encoding.UTF8.GetString(utf8Prefix)}'");
        }

        var idStartIndex = utf8Prefix.IsEmpty ? 0 : utf8Prefix.Length + 1;
        var idAndPrefixLength = idStartIndex + idLength;
        var id = new Memory<AsciiChar>(new AsciiChar[idAndPrefixLength]);

        if (idStartIndex > 0)
        {
            utf8Prefix.CopyTo(ValuesMarshal.AsBytes(id.Span));
            id.Span[idStartIndex - 1] = (AsciiChar)'_';
        }

        Debug.Assert(ValidIdBytes.Length == ValidIdByteCount);

        Span<byte> randomBuffer = stackalloc byte[MaxIdLength];
        var charsWritten = 0;

        while (charsWritten < idLength)
        {
            RandomNumberGenerator.Fill(randomBuffer);

            foreach (var randomByte in randomBuffer)
            {
                if (randomByte >= RandomIdByteUpperBound)
                {
                    continue;
                }

                id.Span[idStartIndex + charsWritten] = (AsciiChar)ValidIdBytes[randomByte % ValidIdByteCount];
                charsWritten++;

                if (charsWritten == idLength)
                {
                    break;
                }
            }
        }

        return new(id);
    }

    /// <summary>
    /// Determines whether this <see cref="Identifier"/> starts with the given sequence of characters.
    /// </summary>
    /// <param name="value">The characters to compare.</param>
    /// <returns>True if the <see cref="Identifier"/> starts with the given sequence of characters; otherwise, false.</returns>
    public bool StartsWith(ReadOnlySpan<char> value)
    {
        return _value.StartsWith(value);
    }

    /// <summary>
    /// Determines whether this <see cref="Identifier"/> starts with the given sequence of bytes.
    /// </summary>
    /// <param name="value">The bytes to compare.</param>
    /// <returns>True if the <see cref="Identifier"/> starts with the given sequence of bytes; otherwise, false.</returns>
    public bool StartsWith(ReadOnlySpan<byte> value)
    {
        return _value.StartsWith(value);
    }

    /// <summary>
    /// Determines whether this <see cref="Identifier"/> ends with the given sequence of characters.
    /// </summary>
    /// <param name="value">The characters to compare.</param>
    /// <returns>True if the <see cref="Identifier"/> ends with the given sequence of characters; otherwise, false.</returns>
    public bool EndsWith(ReadOnlySpan<char> value)
    {
        return _value.EndsWith(value);
    }

    /// <summary>
    /// Determines whether this <see cref="Identifier"/> ends with the given sequence of bytes.
    /// </summary>
    /// <param name="value">The bytes to compare.</param>
    /// <returns>True if the <see cref="Identifier"/> ends with the given sequence of bytes; otherwise, false.</returns>
    public bool EndsWith(ReadOnlySpan<byte> value)
    {
        return _value.EndsWith(value);
    }

    /// <summary>
    /// Determines whether this Identifier is equal to <paramref name="other"/> by comparing bytes.
    /// </summary>
    /// <param name="other"></param>
    public bool Equals(ReadOnlySpan<byte> other)
    {
        return _value.Equals(ValuesMarshal.AsAsciiChars(other));
    }

    /// <summary>
    /// Determines whether this Identifier is equal to <paramref name="other"/> by comparing chars.
    /// </summary>
    /// <param name="other"></param>
    public bool Equals(ReadOnlySpan<char> other)
    {
        return _value.Equals(other);
    }

    /// <inheritdoc/>
    public bool Equals(string? other)
    {
        return other is not null && Equals(other.AsSpan());
    }

    /// <summary>
    /// Returns a read-only span over the underlying ASCII bytes of the identifier.
    /// </summary>
    public ReadOnlySpan<byte> AsBytes()
    {
        return ValuesMarshal.AsBytes(_value.AsSpan());
    }

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }
}
