// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Security;

[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(JsonStringSecureTokenConverter))]
public readonly record struct SecureToken : ISpanParsable<SecureToken>, ISpanFormattable
{
    public static readonly SecureToken Empty;

    public const int DefaultLength = 64;
    public const int MinTokenLength = 48;
    public const int MaxTokenLength = 256;

    // This must be sorted (Unicode/ASCII code order) to use binary search
    public static readonly ReadOnlyMemory<char> TokenChars
        = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".AsMemory();

    private readonly char[]? _token; // null if empty

    private SecureToken(char[] token)
    {
        _token = token;
    }

    [SkipLocalsInit]
    public static SecureToken New(int length = DefaultLength)
    {
        if (length is < MinTokenLength or > MaxTokenLength)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        Span<byte> data = stackalloc byte[MaxTokenLength];

        RandomNumberGenerator.Fill(data);

        Span<char> id = stackalloc char[MaxTokenLength / 2];

        for (var i = 0; i < length * 2; i += 2)
        {
            var f = MemoryMarshal.Read<ushort>(data.Slice(i, 2));
            var c = f % TokenChars.Length;
            id[i / 2] = TokenChars.Span[c];
        }

        return new(id[..length].ToArray());
    }

    public ReadOnlySpan<char> AsSpan()
    {
        return _token.AsSpan();
    }

    public bool Equals(SecureToken other)
    {
        return _token is null ? other._token is null : other._token is not null && _token.SequenceEqual(other._token);
    }

    public override int GetHashCode()
    {
        return string.GetHashCode(_token, StringComparison.Ordinal);
    }

    public override string ToString()
    {
        return ToString(null, null);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return new(_token);
    }

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

    public static bool IsValidToken(string? token)
    {
        return token is not null && IsValidToken(token.AsSpan());
    }

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

    public static bool IsValidTokenChar(char c)
    {
        return char.IsAsciiLetterOrDigit(c);
    }

    public static SecureToken Parse(string s)
    {
        return Parse(s, null);
    }

    public static SecureToken Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static SecureToken Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var token))
        {
            return token;
        }

        ThrowHelper.ThrowFormatException($"'{s}' is not a valid AccessToken.");
        return default; // unreachable
    }

    public static bool TryParse(string? s, out SecureToken result)
    {
        return TryParse(s, null, out result);
    }

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

    public static explicit operator ReadOnlyMemory<char>(SecureToken token)
    {
        return token._token;
    }

    public static explicit operator ReadOnlySpan<char>(SecureToken token)
    {
        return token._token;
    }

#pragma warning restore CA2225 // Operator overloads have named alternates
}
