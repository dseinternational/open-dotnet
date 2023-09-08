// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

/// <summary>
/// A randomly-generated sequence of digits and letters, with an optional prefix,
/// used to identify something.
/// </summary>
[StructLayout(LayoutKind.Auto)]
[JsonConverter(typeof(JsonStringIdentifierConverter))]
public readonly record struct Identifier
    : ISpanFormattable,
      ISpanParsable<Identifier>,
      IEquatable<Identifier>
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

    public const char PrefixDelimiter = '_';

    // This must be sorted (Unicode/ASCII code order) to use binary search
    private static readonly char[] s_idChars
        = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();

    private static readonly int s_idCharsLength = s_idChars.Length;

    private readonly ReadOnlyMemory<char> _id;

    private Identifier(ReadOnlyMemory<char> id)
    {
        _id = id;
    }

    public static readonly Identifier Empty;

    public static Identifier New() => New(DefaultLength);

    public static Identifier New(int length) => New(length, null);

    public static Identifier New(ReadOnlySpan<char> prefix) => New(DefaultLength, prefix);

    public static Identifier New(int idLength, ReadOnlySpan<char> prefix)
    {
        Guard.IsBetweenOrEqualTo(idLength, MinIdLength, MaxIdLength);

        if (!prefix.IsEmpty)
        {
            if (!IsValidPrefix(prefix))
            {
                ThrowHelper.ThrowArgumentException(nameof(prefix),
                    $"Invalid {nameof(Identifier)} value: {prefix}");
            }
        }

        var idStartIndex = prefix.IsEmpty ? 0 : prefix.Length + 1;
        var idAndPrefixLength = idStartIndex + idLength;

        var id = new Memory<char>(new char[idAndPrefixLength]);

        if (idStartIndex > 0)
        {
            Debug.Assert(prefix.Length == idStartIndex - 1);
            prefix.CopyTo(id.Span);
            id.Span[idStartIndex - 1] = '_';
        }

        Span<byte> randomBuffer = stackalloc byte[idLength * 2];

        RandomNumberGenerator.Fill(randomBuffer);

        for (var i = 0; i < randomBuffer.Length; i += 2)
        {
            var f = BitConverter.ToUInt16(randomBuffer.Slice(i, 2));
            var c = f % s_idCharsLength;
            id.Span[idStartIndex + (i / 2)] = s_idChars[c];
        }

        return new Identifier(id);
    }

    /// <summary>
    /// Verifies if the supplied value is a valid identifier - optionally including a prefix.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool IsValid(ReadOnlySpan<char> id)
    {
        if (id.Length is < MinIdLength or > MaxLength)
        {
            return false;
        }

        var prefixEndIndex = id.LastIndexOf(PrefixDelimiter);

        switch (prefixEndIndex)
        {
            case -1:
                return id.All(IsAllowedIdentifierCharacter);
            case 0:
            case > MaxPrefixLength:
                // '_' is not valid in id, beyond prefix
                // and cannot start with '_'
                return false;
        }

        var prefix = id[..prefixEndIndex];

        return prefix[0] != PrefixDelimiter
            && prefix.All(IsPrefixAllowedCharacter)
            && IsValidIdPart(id[(prefixEndIndex + 1)..]);
    }

    public static bool IsValidIdPart(ReadOnlySpan<char> idPart)
        => idPart.Length is >= MinIdLength and <= MaxIdLength && idPart.All(IsAllowedIdentifierCharacter);

    public static bool IsValidPrefix(ReadOnlySpan<char> prefix)
        => prefix.Length is >= MinPrefixLength and <= MaxPrefixLength && prefix.All(IsPrefixAllowedCharacter);

    public static bool IsAllowedIdentifierCharacter(char c) => char.IsAsciiLetterOrDigit(c);

    public static bool IsPrefixAllowedCharacter(char c) => char.IsAsciiLetterOrDigit(c) || c == PrefixDelimiter;

    public bool StartsWith(ReadOnlySpan<char> value) => _id.Span.StartsWith(value, StringComparison.Ordinal);

    public static (ReadOnlyMemory<char> prefix, ReadOnlyMemory<char> id) Split(ReadOnlyMemory<char> uid)
    {
        if (uid.IsEmpty)
        {
            return (null, uid);
        }

        var i = uid.Span.LastIndexOf('_');

        if (i < 0)
        {
            return (null, uid);
        }

        var prefix = uid[..i];
        var id = uid[(i + 1)..];

        return (prefix, id);
    }

    public bool Equals(string? other) => Equals(other.AsSpan());

    public bool Equals(ReadOnlyMemory<char> other) => Equals(other.Span);

    public bool Equals(ReadOnlySpan<char> other) => _id.Span.SequenceEqual(other);

    public bool Equals(Identifier other) => Equals(other._id.Span);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        for (var i = 0; i < _id.Length; i++)
        {
            hash.Add(_id.Span[i]);
        }

        return hash.ToHashCode();
    }

    public static Identifier Parse(string s) => Parse(s, null);

    public static Identifier Parse(string s, IFormatProvider? provider) => Parse(s.AsSpan(), provider);

    public static Identifier Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"Failed to parse {nameof(Identifier)} value: {s}");
        return default;
    }

    public static bool TryParse(ReadOnlySpan<char> s, out Identifier result) => TryParse(s, null, out result);

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Identifier result)
    {
        if (s.IsEmpty)
        {
            result = Empty;
            return true;
        }

        if (!IsValid(s))
        {
            result = default;
            return false;
        }

        result = new Identifier(s.ToArray());
        return true;
    }

    public static bool TryParse([NotNullWhen(true)] string? s, out Identifier result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s, null, out result);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out Identifier result)
        => TryParse(s.AsSpan(), provider, out result);

    public bool TryFormat(Span<char> destination, out int charsWritten)
        => TryFormat(destination, out charsWritten, default, null);

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (_id.Span.TryCopyTo(destination))
        {
            charsWritten = _id.Length;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    public override string ToString() => ToString(null, null);

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        Span<char> destination = stackalloc char[_id.Length];
        _ = TryFormat(destination, out _, format, formatProvider);
        return destination.ToString();
    }

    public static explicit operator string(Identifier identifier) => identifier.ToString();

    public static explicit operator Identifier(string identifier) => FromString(identifier);

    public static Identifier FromString(string identifier) => Parse(identifier);
}
