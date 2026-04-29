// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Security;

/// <summary>
/// A cryptographically random alphanumeric token of bounded length suitable for use as an access
/// token, API key, or other opaque secret. Equality is computed in constant time.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(JsonStringSecureTokenConverter))]
public readonly record struct SecureToken : ISpanParsable<SecureToken>, ISpanFormattable
{
    /// <summary>
    /// The empty (default) <see cref="SecureToken"/>.
    /// </summary>
    public static readonly SecureToken Empty;

    /// <summary>
    /// The default length, in characters, of a newly generated token.
    /// </summary>
    public const int DefaultLength = 64;

    /// <summary>
    /// The minimum allowed length, in characters, of a token.
    /// </summary>
    public const int MinTokenLength = 48;

    /// <summary>
    /// The maximum allowed length, in characters, of a token.
    /// </summary>
    public const int MaxTokenLength = 256;

    // This must be sorted (Unicode/ASCII code order) to use binary search
    /// <summary>
    /// The set of characters used by tokens, in ASCII (sorted) order: digits, then upper case
    /// letters, then lower case letters.
    /// </summary>
    public static readonly ReadOnlyMemory<char> TokenChars
        = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".AsMemory();

    private readonly char[]? _token; // null if empty

    private SecureToken(char[] token)
    {
        _token = token;
    }

    /// <summary>
    /// Generates a new <see cref="SecureToken"/> of the specified length using
    /// <see cref="RandomNumberGenerator"/>.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="length"/> is outside
    /// the range <see cref="MinTokenLength"/>..<see cref="MaxTokenLength"/>.</exception>
    [SkipLocalsInit]
    public static SecureToken New(int length = DefaultLength)
    {
        if (length is < MinTokenLength or > MaxTokenLength)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        Span<char> id = stackalloc char[MaxTokenLength];

        for (var i = 0; i < length; i++)
        {
            id[i] = TokenChars.Span[RandomNumberGenerator.GetInt32(TokenChars.Length)];
        }

        return new(id[..length].ToArray());
    }

    /// <summary>
    /// Returns a read-only span over the underlying characters of the token.
    /// </summary>
    public ReadOnlySpan<char> AsSpan()
    {
        return _token.AsSpan();
    }

    /// <summary>
    /// Determines whether this token equals <paramref name="other"/> using a constant-time comparison.
    /// </summary>
    public bool Equals(SecureToken other)
    {
        return CryptographicOperations.FixedTimeEquals(
            MemoryMarshal.AsBytes(_token.AsSpan()),
            MemoryMarshal.AsBytes(other._token.AsSpan()));
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return string.GetHashCode(_token, StringComparison.Ordinal);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return ToString(null, null);
    }

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return new(_token);
    }

    /// <inheritdoc/>
    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        var span = _token.AsSpan();

        if (span.TryCopyTo(destination))
        {
            charsWritten = span.Length;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    /// <summary>
    /// Determines whether the specified string is a valid <see cref="SecureToken"/>.
    /// </summary>
    public static bool IsValidToken(string? token)
    {
        return token is not null && IsValidToken(token.AsSpan());
    }

    /// <summary>
    /// Determines whether the specified span of characters is a valid <see cref="SecureToken"/>.
    /// </summary>
    public static bool IsValidToken(ReadOnlySpan<char> token)
    {
        if (token.Length is < MinTokenLength or > MaxTokenLength)
        {
            return false;
        }

        foreach (var @char in token)
        {
            if (!IsValidTokenChar(@char))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Determines whether the specified character is valid in a token (an ASCII letter or digit).
    /// </summary>
    public static bool IsValidTokenChar(char c)
    {
        return char.IsAsciiLetterOrDigit(c);
    }

    /// <summary>
    /// Parses the specified string into a <see cref="SecureToken"/>.
    /// </summary>
    public static SecureToken Parse(string s)
    {
        return Parse(s, null);
    }

    /// <inheritdoc/>
    public static SecureToken Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    /// <inheritdoc/>
    public static SecureToken Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var token))
        {
            return token;
        }

        ThrowHelper.ThrowFormatException($"'{s}' is not a valid AccessToken.");
        return default; // unreachable
    }

    /// <summary>
    /// Attempts to parse the specified string into a <see cref="SecureToken"/>.
    /// </summary>
    public static bool TryParse(string? s, out SecureToken result)
    {
        return TryParse(s, null, out result);
    }

    /// <inheritdoc/>
    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out SecureToken result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out SecureToken result)
    {
        if (s.IsEmpty)
        {
            result = default;
            return true;
        }

        if (!IsValidToken(s))
        {
            result = default;
            return false;
        }

        result = new(s.ToArray());
        return true;
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    /// <summary>
    /// Explicitly converts a <see cref="SecureToken"/> to a <see cref="ReadOnlyMemory{Char}"/>
    /// over its underlying characters.
    /// </summary>
    public static explicit operator ReadOnlyMemory<char>(SecureToken token)
    {
        return token._token;
    }

    /// <summary>
    /// Explicitly converts a <see cref="SecureToken"/> to a <see cref="ReadOnlySpan{Char}"/>
    /// over its underlying characters.
    /// </summary>
    public static explicit operator ReadOnlySpan<char>(SecureToken token)
    {
        return token._token;
    }

#pragma warning restore CA2225 // Operator overloads have named alternates
}
