// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

[EquatableValue]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<Identifier, AsciiString>))]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct Identifier : IEquatableValue<Identifier, AsciiString>, IUtf8SpanSerializable<Identifier>
{
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

    public const int MaxLength = MaxPrefixLength + MaxIdLength;

    public static int MaxSerializedCharLength => MaxLength;

    public static int MaxSerializedByteLength => MaxLength;

    public const char PrefixDelimiter = '_';

    private static ReadOnlySpan<byte> ValidIdBytes => "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"u8;

    private static ReadOnlySpan<byte> ValidPrefixBytes => "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_"u8;

    private static readonly SearchValues<byte> s_validPrefixBytes = SearchValues.Create(ValidPrefixBytes);

    public static readonly Identifier Empty;

    private Identifier(ReadOnlyMemory<AsciiChar> value)
    {
        _value = new AsciiString(value);
        _initialized = true;
    }

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
        }

        var prefix = value.AsSpan()[..prefixEndIndex];
        var id = value.AsSpan()[(prefixEndIndex + 1)..];

        return prefix[0] != (AsciiChar)PrefixDelimiter
               && prefix.All(id => id == PrefixDelimiter || AsciiChar.IsLetterOrDigit(id))
               && id.Length is >= MinIdLength and <= MaxIdLength
               && id.ContainsOnlyAsciiLettersOrDigits();
    }

    public static bool IsValid(ReadOnlySpan<char> id)
    {
        if (!AsciiString.TryParse(id, out var value))
        {
            return false;
        }

        return IsValidValue(value);
    }

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
    public static Identifier New(int idLength, ReadOnlySpan<char> prefix)
    {
        Guard.IsBetweenOrEqualTo(idLength, MinIdLength, MaxIdLength);

        if (prefix.IsEmpty)
        {
            return New(idLength, ReadOnlySpan<byte>.Empty);
        }

        Span<byte> utf8 = stackalloc byte[prefix.Length];

        var status = Ascii.FromUtf16(prefix, utf8, out var bytesWritten);

        if (status is OperationStatus.Done)
        {
            Debug.Assert(bytesWritten == prefix.Length);
            return New(idLength, utf8);
        }

        Debug.Assert(status is OperationStatus.InvalidData);

        return ThrowHelper.ThrowArgumentException<Identifier>(nameof(prefix),
            $"Invalid {nameof(Identifier)} prefix '{prefix.ToString()}'");
    }

    /// <summary>
    /// Creates a new <see cref="Identifier"/> of the specified length with the given prefix and a random value
    /// </summary>
    /// <param name="idLength">The length of <see cref="Identifier"/> to create.</param>
    /// <param name="prefix">The prefix to use.</param>
    /// <returns>A new <see cref="Identifier"/> with the given prefix and a random value.</returns>
    public static Identifier New(int idLength, ReadOnlySpan<byte> prefix)
    {
        Guard.IsBetweenOrEqualTo(idLength, MinIdLength, MaxIdLength);

        if (!prefix.IsEmpty && !IsValidPrefix(prefix))
        {
            ThrowHelper.ThrowArgumentException(nameof(prefix),
                $"Invalid {nameof(Identifier)} prefix '{Encoding.UTF8.GetString(prefix)}'");
        }

        var idStartIndex = prefix.IsEmpty ? 0 : prefix.Length + 1;
        var idAndPrefixLength = idStartIndex + idLength;
        var id = new Memory<AsciiChar>(new AsciiChar[idAndPrefixLength]);

        if (idStartIndex > 0)
        {
            prefix.CopyTo(ValuesMarshal.AsBytes(id.Span));
            id.Span[idStartIndex - 1] = (AsciiChar)'_';
        }

        Span<byte> randomBuffer = stackalloc byte[idLength * 2];

        RandomNumberGenerator.Fill(randomBuffer);

        for (var i = 0; i < randomBuffer.Length; i += 2)
        {
            var f = BitConverter.ToUInt16(randomBuffer.Slice(i, 2));
            var c = f % ValidIdBytes.Length;
            id.Span[idStartIndex + (i / 2)] = (AsciiChar)ValidIdBytes[c];
        }

        return new Identifier(id);
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
}
