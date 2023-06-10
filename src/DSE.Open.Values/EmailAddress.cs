// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using CommunityToolkit.HighPerformance;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

[JsonConverter(typeof(JsonStringEmailAddressConverter))]
[StructLayout(LayoutKind.Auto)]
public readonly record struct EmailAddress : IComparable<EmailAddress>, ISpanParsable<EmailAddress>,
    ISpanFormattable, IEquatable<string>, IEquatable<ReadOnlyMemory<char>>
{
    // https://datatracker.ietf.org/doc/html/rfc5322#section-3.2.3
    private const string ATextSymbolChars = "!#$%&'*+-/=?^_`{|}~";

    public const int MaxLocalPartLength = 64;

    public const char AtChar = '@';

    // https://stackoverflow.com/questions/386294/what-is-the-maximum-length-of-a-valid-email-address
    public const int MaxLength = 254;

    public static readonly EmailAddress Empty;

    private readonly string? _value; // null if empty
    private readonly int _splitIndex;

    private EmailAddress(ReadOnlySpan<char> email, int splitIndex) : this(email.ToString(), splitIndex)
    {
    }

    private EmailAddress(string email, int splitIndex)
    {
        _value = email;
        _splitIndex = splitIndex;
    }

    public EmailAddress(ReadOnlySpan<char> email) : this(email.ToString())
    {
    }

    public EmailAddress(string email)
    {
        Guard.IsNotNullOrWhiteSpace(email);

        email = email.Trim();

        if (!IsValid(email.AsSpan(), out var splitIndex))
        {
            ThrowHelper.ThrowFormatException($"Invalid {nameof(EmailAddress)}: '{email}'");
        }

        _value = email;
        _splitIndex = splitIndex;
    }

    public static bool IsValid(ReadOnlySpan<char> email) => IsValid(email, out _);

    public static bool IsValid(ReadOnlySpan<char> email, out int splitIndex)
    {
        if (email.Length > MaxLength)
        {
            splitIndex = -1;
            return false;
        }

        splitIndex = email.IndexOf(AtChar);

        if (splitIndex is < 0 or > MaxLocalPartLength)
        {
            return false;
        }

        var localPart = email[..splitIndex];

        if (!AllAreValidDotAtomChars(localPart))
        {
            return false;
        }

        var domain = email[(splitIndex + 1)..];

        return AllAreValidDotAtomChars(domain) && domain.IndexOf('.') >= 0;
    }

    // https://datatracker.ietf.org/doc/html/rfc5322#section-3.2.3

    private static bool AllAreValidDotAtomChars(ReadOnlySpan<char> value)
    {
        foreach (var @char in value)
        {
            if (!(char.IsAsciiLetterOrDigit(@char)
                  || @char == '.'
                  || ATextSymbolChars.IndexOf(@char, StringComparison.Ordinal) > -1))
            {
                return false;
            }
        }

        return true;
    }

    public int CompareTo(EmailAddress other) => string.CompareOrdinal(_value, other._value);

    /// <summary>
    /// Determines if the email address contains the specified sequence of characters. Compared
    /// using the <see cref="StringComparison.InvariantCultureIgnoreCase"/> comparison type.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Contains(string value) => Contains(value.AsSpan(), StringComparison.Ordinal);

    /// <summary>
    /// Determines if the email address contains the specified sequence of characters. Compared
    /// using the specified comparison type.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="comparisonType"></param>
    /// <returns></returns>
    public bool Contains(string value, StringComparison comparisonType)
    {
        Guard.IsNotNull(value);
        return Contains(value.AsSpan(), comparisonType);
    }

    /// <summary>
    /// Determines if the email address contains the specified sequence of characters. Compared
    /// using the <see cref="StringComparison.InvariantCultureIgnoreCase"/> comparison type.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Contains(ReadOnlySpan<char> value) => Contains(value, StringComparison.InvariantCultureIgnoreCase);

    /// <summary>
    /// Determines if the email address contains the specified sequence of characters. Compared
    /// using the specified comparison type.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="comparisonType"></param>
    /// <returns></returns>
    public bool Contains(ReadOnlySpan<char> value, StringComparison comparisonType)
        => _value.AsSpan().Contains(value, comparisonType);

    public bool Equals(EmailAddress other) => Equals(other._value.AsSpan());

    public bool Equals(string? other) => Equals(other.AsSpan());

    public bool Equals(ReadOnlyMemory<char> other) => Equals(other.Span);

    public bool Equals(ReadOnlySpan<char> other) => _value.AsSpan().SequenceEqual(other);

    public override int GetHashCode() => string.GetHashCode(_value, StringComparison.Ordinal);

    public static EmailAddress Parse(string s) => Parse(s, null);

    public static EmailAddress Parse(string s, IFormatProvider? provider)
    {
        Guard.IsNotNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static EmailAddress Parse(ReadOnlySpan<char> s) => Parse(s, null);

    public static EmailAddress Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : ThrowHelper.ThrowFormatException<EmailAddress>($"Failed to parse {nameof(EmailAddress)}: '{s}'");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        out EmailAddress result)
        => TryParse(s, null, out result);

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out EmailAddress result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    public static bool TryParse(ReadOnlySpan<char> s, out EmailAddress result)
        => TryParse(s, null, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out EmailAddress result)
    {
        s = s.Trim();

        if (s.IsEmpty)
        {
            result = default;
            return true;
        }

        if (IsValid(s, out var splitIndex))
        {
            result = new EmailAddress(s, splitIndex);
            return true;
        }

        result = default;
        return false;
    }

    public override string ToString() => ToString(null, null);

    public string ToString(string? format, IFormatProvider? formatProvider) => _value ?? string.Empty;

    public bool TryFormat(Span<char> destination, out int charsWritten)
        => TryFormat(destination, out charsWritten, default, default);

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (string.IsNullOrEmpty(_value))
        {
            charsWritten = 0;
            return true;
        }

        if (destination.Length >= _value.Length)
        {
            _value.CopyTo(destination);
            charsWritten = _value.Length;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    public static explicit operator EmailAddress(string value) => new(value);

    public static explicit operator string(EmailAddress value) => value.ToString();

    public static explicit operator ReadOnlySpan<char>(EmailAddress value) => value._value;

    public static explicit operator ReadOnlyMemory<char>(EmailAddress value) => value._value.AsMemory();

    public static bool operator <(EmailAddress left, EmailAddress right) => left.CompareTo(right) < 0;

    public static bool operator <=(EmailAddress left, EmailAddress right) => left.CompareTo(right) <= 0;

    public static bool operator >(EmailAddress left, EmailAddress right) => left.CompareTo(right) > 0;

    public static bool operator >=(EmailAddress left, EmailAddress right) => left.CompareTo(right) >= 0;

    public static EmailAddress ToEmailAddress(EmailAddress left, EmailAddress right) => throw new NotImplementedException();

    public static ReadOnlySpan<char> ToReadOnlySpan(EmailAddress left, EmailAddress right) => throw new NotImplementedException();

    public static ReadOnlyMemory<char> ToReadOnlyMemory(EmailAddress left, EmailAddress right) => throw new NotImplementedException();
}
