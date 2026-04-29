// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
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
[StructLayout(LayoutKind.Sequential)]
public readonly record struct EmailAddress
    : IComparable<EmailAddress>,
      ISpanParsable<EmailAddress>,
      ISpanFormattable,
      IEquatable<string>,
      IEquatable<ReadOnlyMemory<char>>,
      IRepeatableHash64
{
    // https://datatracker.ietf.org/doc/html/rfc5322#section-3.2.3
    private const string ATextSymbolChars = "!#$%&'*+-/=?^_`{|}~";

    /// <summary>
    /// The maximum length, in characters, of the local part of an email address.
    /// </summary>
    public const int MaxLocalPartLength = 64;

    /// <summary>
    /// The maximum length, in characters, of the domain part of an email address.
    /// </summary>
    public const int MaxDomainPartLength = MaxLength - MaxLocalPartLength - 1;

    /// <summary>
    /// The character that separates the local part from the domain part of an email address.
    /// </summary>
    public const char AtChar = '@';

    // https://stackoverflow.com/questions/386294/what-is-the-maximum-length-of-a-valid-email-address
    /// <summary>
    /// The maximum length, in characters, of an email address.
    /// </summary>
    public const int MaxLength = 254;

    /// <summary>
    /// An empty <see cref="EmailAddress"/>.
    /// </summary>
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

    /// <summary>
    /// Initialises a new <see cref="EmailAddress"/> by parsing the supplied character span.
    /// </summary>
    /// <exception cref="FormatException">The span does not contain a valid email address.</exception>
    public EmailAddress(ReadOnlySpan<char> email) : this(email.ToString())
    {
    }

    /// <summary>
    /// Initialises a new <see cref="EmailAddress"/> by parsing the supplied string after trimming whitespace.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="email"/> is <see langword="null"/>, empty, or whitespace.</exception>
    /// <exception cref="FormatException"><paramref name="email"/> is not a valid email address.</exception>
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

    /// <summary>
    /// Gets the length of the email address, in characters.
    /// </summary>
    public int Length => _value?.Length ?? 0;

    /// <summary>
    /// Returns a read-only span over the local part (the portion preceding <see cref="AtChar"/>) of this email address.
    /// </summary>
    public ReadOnlySpan<char> LocalPart()
    {
        return _value.AsSpan(0, _splitIndex);
    }

    /// <summary>
    /// Returns a read-only span over the domain part (the portion following <see cref="AtChar"/>) of this email address.
    /// </summary>
    public ReadOnlySpan<char> DomainPart()
    {
        return _value.AsSpan(_splitIndex + 1);
    }

    /// <summary>
    /// Returns <see langword="true"/> if the supplied character span is a valid email address according to the rules documented on <see cref="EmailAddress"/>.
    /// </summary>
    public static bool IsValid(ReadOnlySpan<char> email)
    {
        return IsValid(email, out _);
    }

    /// <summary>
    /// Returns <see langword="true"/> if the supplied character span is a valid email address according to the rules documented on <see cref="EmailAddress"/>,
    /// also outputting the index of the <see cref="AtChar"/> separator.
    /// </summary>
    /// <param name="email">The candidate email address.</param>
    /// <param name="splitIndex">When the email is valid, the index of the <see cref="AtChar"/> separator; otherwise unspecified.</param>
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

        var anyAsciiLetters = false;
        var labelCount = 0;
        var remaining = domain;

        while (true)
        {
            var dotIndex = remaining.IndexOf('.');
            var label = dotIndex < 0 ? remaining : remaining[..dotIndex];

            if (!IsValidDomainLabel(label, ref anyAsciiLetters))
            {
                return false;
            }

            labelCount++;

            if (dotIndex < 0)
            {
                break;
            }

            remaining = remaining[(dotIndex + 1)..];
        }

        // We don't support domains with less than two labels (e.g. `fred@example`).
        return labelCount >= 2 && anyAsciiLetters;
    }

    private static bool IsValidDomainLabel(ReadOnlySpan<char> label, ref bool anyAsciiLetters)
    {
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

    /// <summary>
    /// Returns a read-only span over the email address's characters.
    /// </summary>
    public ReadOnlySpan<char> AsSpan()
    {
        return _value.AsSpan();
    }

    /// <inheritdoc/>
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
        return Contains(value.AsSpan(), StringComparison.InvariantCultureIgnoreCase);
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

    /// <inheritdoc/>
    public bool Equals(EmailAddress other)
    {
        return Equals(other._value.AsSpan());
    }

    /// <inheritdoc/>
    public bool Equals(string? other)
    {
        return Equals(other.AsSpan());
    }

    /// <inheritdoc/>
    public bool Equals(ReadOnlyMemory<char> other)
    {
        return Equals(other.Span);
    }

    /// <summary>
    /// Determines whether this email address is equal to the supplied character span using ordinal comparison.
    /// </summary>
    public bool Equals(ReadOnlySpan<char> other)
    {
        return _value.AsSpan().SequenceEqual(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return string.GetHashCode(_value, StringComparison.Ordinal);
    }

    /// <summary>
    /// Parses the supplied string as an <see cref="EmailAddress"/>.
    /// </summary>
    public static EmailAddress Parse(string s)
    {
        return Parse(s, null);
    }

    /// <inheritdoc/>
    public static EmailAddress Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    /// <summary>
    /// Parses the supplied character span as an <see cref="EmailAddress"/>.
    /// </summary>
    public static EmailAddress Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, null);
    }

    /// <inheritdoc/>
    public static EmailAddress Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : ThrowHelper.ThrowFormatException<EmailAddress>($"Failed to parse {nameof(EmailAddress)}: '{s}'");
    }

    /// <summary>
    /// Parses the supplied character span as an <see cref="EmailAddress"/> using the invariant culture.
    /// </summary>
    public static EmailAddress ParseInvariant(ReadOnlySpan<char> s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Attempts to parse the supplied string as an <see cref="EmailAddress"/>.
    /// </summary>
    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        out EmailAddress result)
    {
        return TryParse(s, null, out result);
    }

    /// <inheritdoc/>
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

    /// <summary>
    /// Attempts to parse the supplied character span as an <see cref="EmailAddress"/>.
    /// </summary>
    public static bool TryParse(ReadOnlySpan<char> s, out EmailAddress result)
    {
        return TryParse(s, null, out result);
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public override string ToString()
    {
        return ToString(null, null);
    }

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return _value ?? string.Empty;
    }

    /// <summary>
    /// Attempts to copy the email address into the destination span.
    /// </summary>
    public bool TryFormat(Span<char> destination, out int charsWritten)
    {
        return TryFormat(destination, out charsWritten, default, default);
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value.AsSpan());
    }

#pragma warning disable CA2225 // Operator overloads have named alternates - explicit conversion operators
    /// <summary>
    /// Explicitly converts a string to an <see cref="EmailAddress"/>.
    /// </summary>
    public static explicit operator EmailAddress(string value)
    {
        return new(value);
    }

    /// <summary>
    /// Explicitly converts an <see cref="EmailAddress"/> to its string representation.
    /// </summary>
    public static explicit operator string(EmailAddress value)
    {
        return value.ToString();
    }

    /// <summary>
    /// Explicitly converts an <see cref="EmailAddress"/> to a <see cref="ReadOnlySpan{T}"/> over its characters.
    /// </summary>
    public static explicit operator ReadOnlySpan<char>(EmailAddress value)
    {
        return value._value;
    }

    /// <summary>
    /// Explicitly converts an <see cref="EmailAddress"/> to a <see cref="ReadOnlyMemory{T}"/> over its characters.
    /// </summary>
    public static explicit operator ReadOnlyMemory<char>(EmailAddress value)
    {
        return value._value.AsMemory();
    }
#pragma warning restore CA2225 // Operator overloads have named alternates

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="left"/> sorts before <paramref name="right"/> using ordinal comparison.
    /// </summary>
    public static bool operator <(EmailAddress left, EmailAddress right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="left"/> sorts before or equals <paramref name="right"/> using ordinal comparison.
    /// </summary>
    public static bool operator <=(EmailAddress left, EmailAddress right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="left"/> sorts after <paramref name="right"/> using ordinal comparison.
    /// </summary>
    public static bool operator >(EmailAddress left, EmailAddress right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="left"/> sorts after or equals <paramref name="right"/> using ordinal comparison.
    /// </summary>
    public static bool operator >=(EmailAddress left, EmailAddress right)
    {
        return left.CompareTo(right) >= 0;
    }
}
