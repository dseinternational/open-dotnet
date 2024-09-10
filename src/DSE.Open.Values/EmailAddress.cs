// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

/// <summary>
/// An email address with the following deviations from the RFC:
/// <list type="bullet">
/// <item>Quoted strings are not supported (e.g. <c>"john..doe"@example.org</c> is treated as invalid).</item>
/// <item>Domains must contain one or more periods (e.g. <c>user@localhost</c> is treated as invalid).</item>
/// <item>Domain literals are not supported (e.g. <c>user@[IPv6:2001:db8::1]</c>).</item>
/// <item>Domains must contain at least one ASCII letter (e.g. <c>email@123.123.123.123</c> is treated as invalid).</item>
/// <item>Top-level domains (TLDs) are not validated (e.g. <c>user@example.web</c> is treated as valid, although <c>.web</c> is not a valid TLD).</item>
/// </list>
/// </summary>
[JsonConverter(typeof(JsonStringEmailAddressConverter))]
[StructLayout(LayoutKind.Auto)]
public readonly record struct EmailAddress
    : IComparable<EmailAddress>,
      ISpanParsable<EmailAddress>,
      ISpanFormattable,
      IEquatable<string>,
      IEquatable<ReadOnlyMemory<char>>
{
    // https://datatracker.ietf.org/doc/html/rfc5322#section-3.2.3
    private const string ATextSymbolChars = "!#$%&'*+-/=?^_`{|}~";

    public const int MaxLocalPartLength = 64;

    public const int MaxDomainPartLength = MaxLength - MaxLocalPartLength - 1;

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
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        email = email.Trim();

        if (!IsValid(email.AsSpan(), out var splitIndex))
        {
            ThrowHelper.ThrowFormatException($"Invalid {nameof(EmailAddress)}: '{email}'");
        }

        _value = email;
        _splitIndex = splitIndex;
    }

    public static bool IsValid(ReadOnlySpan<char> email)
    {
        return IsValid(email, out _);
    }

    public static bool IsValid(ReadOnlySpan<char> email, out int splitIndex)
    {
        if (email.Length > MaxLength)
        {
            splitIndex = -1;
            return false;
        }

        splitIndex = email.IndexOf(AtChar);

        if (splitIndex is <= 0 or > MaxLocalPartLength)
        {
            return false;
        }

        return IsValidLocalPart(email[..splitIndex]) && IsValidDomainPart(email[(splitIndex + 1)..]);
    }

    // See https://datatracker.ietf.org/doc/html/rfc3696#section-3
    private static bool IsValidLocalPart(ReadOnlySpan<char> localPart)
    {
        if (localPart[0] == '.' || localPart[^1] == '.')
        {
            return false;
        }

        if (!AllAreValidDotAtomChars(localPart))
        {
            return false;
        }

        return !AnyConsecutiveDots(localPart);
    }

    [SkipLocalsInit]
    private static bool IsValidDomainPart(ReadOnlySpan<char> domain)
    {
        // labels (words or strings separated by periods) that make up a domain name must consist of only the ASCII [ASCII] alphabetic and numeric characters,
        // plus the hyphen. No other symbols or punctuation characters are permitted, nor is blank space.  If the hyphen is used, it is not permitted to appear
        // at either the beginning or end of a label. There is an additional rule that essentially requires that top-level domain names not be all-numeric.
        // https://datatracker.ietf.org/doc/html/rfc3696#section-2

        if (domain.Length > MaxDomainPartLength)
        {
            return false;
        }

        Span<Range> labels = stackalloc Range[10];
        var labelCount = domain.Split(labels, '.');
        var anyAsciiLetters = false;

        // We don't support domains with less than two labels (e.g. `fred@example`).
        if (labelCount < 2)
        {
            return false;
        }

        for (var i = 0; i < labelCount; i++)
        {
            var label = domain[labels[i]];

            if (label.Length < 1)
            {
                return false;
            }

            if (label[0] == '-' || label[^1] == '-')
            {
                return false;
            }

            foreach (var @char in label)
            {
                if (char.IsAsciiLetter(@char))
                {
                    anyAsciiLetters = true;
                    continue;
                }

                if (char.IsAsciiDigit(@char) || @char == '-')
                {
                    continue;
                }

                return false;
            }
        }

        if (!anyAsciiLetters)
        {
            return false;
        }

        return true;
    }

    private static bool AnyConsecutiveDots(ReadOnlySpan<char> localPart)
    {
        var previous = localPart[0];

        for (var i = 1; i < localPart.Length; i++)
        {
            var current = localPart[i];

            if (previous == '.' && current == '.')
            {
                return true;
            }

            previous = current;
        }

        return false;
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

    public int CompareTo(EmailAddress other)
    {
        return string.CompareOrdinal(_value, other._value);
    }

    /// <summary>
    /// Determines if the email address contains the specified sequence of characters. Compared
    /// using the <see cref="StringComparison.InvariantCultureIgnoreCase"/> comparison type.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Contains(string value)
    {
        return Contains(value.AsSpan(), StringComparison.Ordinal);
    }

    /// <summary>
    /// Determines if the email address contains the specified sequence of characters. Compared
    /// using the specified comparison type.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="comparisonType"></param>
    /// <returns></returns>
    public bool Contains(string value, StringComparison comparisonType)
    {
        ArgumentNullException.ThrowIfNull(value);
        return Contains(value.AsSpan(), comparisonType);
    }

    /// <summary>
    /// Determines if the email address contains the specified sequence of characters. Compared
    /// using the <see cref="StringComparison.InvariantCultureIgnoreCase"/> comparison type.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Contains(ReadOnlySpan<char> value)
    {
        return Contains(value, StringComparison.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// Determines if the email address contains the specified sequence of characters. Compared
    /// using the specified comparison type.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="comparisonType"></param>
    /// <returns></returns>
    public bool Contains(ReadOnlySpan<char> value, StringComparison comparisonType)
    {
        return _value.AsSpan().Contains(value, comparisonType);
    }

    public bool Equals(EmailAddress other)
    {
        return Equals(other._value.AsSpan());
    }

    public bool Equals(string? other)
    {
        return Equals(other.AsSpan());
    }

    public bool Equals(ReadOnlyMemory<char> other)
    {
        return Equals(other.Span);
    }

    public bool Equals(ReadOnlySpan<char> other)
    {
        return _value.AsSpan().SequenceEqual(other);
    }

    public override int GetHashCode()
    {
        return string.GetHashCode(_value, StringComparison.Ordinal);
    }

    public static EmailAddress Parse(string s)
    {
        return Parse(s, null);
    }

    public static EmailAddress Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static EmailAddress Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, null);
    }

    public static EmailAddress Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : ThrowHelper.ThrowFormatException<EmailAddress>($"Failed to parse {nameof(EmailAddress)}: '{s}'");
    }

    public static EmailAddress ParseInvariant(ReadOnlySpan<char> s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        out EmailAddress result)
    {
        return TryParse(s, null, out result);
    }

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
    {
        return TryParse(s, null, out result);
    }

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
            result = new(s, splitIndex);
            return true;
        }

        result = default;
        return false;
    }

    public override string ToString()
    {
        return ToString(null, null);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return _value ?? string.Empty;
    }

    public bool TryFormat(Span<char> destination, out int charsWritten)
    {
        return TryFormat(destination, out charsWritten, default, default);
    }

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

#pragma warning disable CA2225 // Operator overloads have named alternates - explicit conversion operators
    public static explicit operator EmailAddress(string value)
    {
        return new(value);
    }

    public static explicit operator string(EmailAddress value)
    {
        return value.ToString();
    }

    public static explicit operator ReadOnlySpan<char>(EmailAddress value)
    {
        return value._value;
    }

    public static explicit operator ReadOnlyMemory<char>(EmailAddress value)
    {
        return value._value.AsMemory();
    }
#pragma warning restore CA2225 // Operator overloads have named alternates

    public static bool operator <(EmailAddress left, EmailAddress right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(EmailAddress left, EmailAddress right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(EmailAddress left, EmailAddress right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(EmailAddress left, EmailAddress right)
    {
        return left.CompareTo(right) >= 0;
    }
}
