// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Globalization.Text.Json;

namespace DSE.Open.Globalization;

/// <summary>
/// Represents an international telephone number, comprising an
/// <see href="https://www.itu.int/rec/T-REC-E.164-201011-I/en">E.164</see>
/// country calling code and a national subscriber number.
/// </summary>
[JsonConverter(typeof(JsonStringTelephoneNumberConverter))]
[StructLayout(LayoutKind.Sequential)]
public readonly record struct TelephoneNumber
    : ISpanParsable<TelephoneNumber>,
      ISpanFormattable
{
    /// <summary>
    /// An empty <see cref="TelephoneNumber"/> with default country code and national number.
    /// </summary>
    public static readonly TelephoneNumber Empty;

    /// <summary>
    /// The maximum total number of digits permitted in an E.164 telephone number.
    /// </summary>
    // https://www.itu.int/rec/T-REC-E.164-201011-I/en
    public const int MaxDigitCount = 15;

    /// <summary>
    /// The maximum number of digits permitted in an E.164 country calling code.
    /// </summary>
    public const int MaxCountryCodeDigitCount = 3;

    /// <summary>
    /// The maximum length of the formatted telephone number string (including the
    /// <see cref="CountryCodePrefix"/> and <see cref="NationalNumberDelimiter"/>).
    /// </summary>
    public const int MaxFormattedLength = 20; // +1 123 456 789 01234

    /// <summary>
    /// The minimum length of the formatted telephone number string.
    /// </summary>
    public const int MinFormattedLength = 8;  // +1 12345

    /// <summary>
    /// The character used to prefix the country calling code in formatted output.
    /// </summary>
    public const char CountryCodePrefix = '+';

    /// <summary>
    /// The character used to separate the country calling code from the national number in formatted output.
    /// </summary>
    public const char NationalNumberDelimiter = ' ';

    /// <summary>
    /// Initializes a new <see cref="TelephoneNumber"/> with the specified country calling code and national number.
    /// </summary>
    /// <param name="countryCode">The E.164 country calling code.</param>
    /// <param name="nationalNumber">The national subscriber number.</param>
    public TelephoneNumber(uint countryCode, ulong nationalNumber)
        : this(countryCode, nationalNumber, false)
    {
    }

    private TelephoneNumber(uint countryCode, ulong nationalNumber, bool skipChecks)
    {
        if (!skipChecks)
        {
            EnsureIsValidCountryCallingCode(countryCode);
            EnsureIsValidNationalNumber(nationalNumber);
        }

        CountryCode = countryCode;
        NationalNumber = nationalNumber;
    }

    /// <summary>
    /// Gets the E.164 country calling code component of the telephone number.
    /// </summary>
    public uint CountryCode { get; }

    /// <summary>
    /// Gets the national subscriber number component of the telephone number.
    /// </summary>
    public ulong NationalNumber { get; }

    /// <summary>
    /// Gets the <see cref="CountryCallingCodeInfo"/> entries that match the
    /// current <see cref="CountryCode"/>.
    /// </summary>
    public IEnumerable<CountryCallingCodeInfo> GetCountryCallingCodeInfo()
    {
        return CountryCallingCodeInfo.GetInfo(CountryCode);
    }

    /// <summary>
    /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="code"/>
    /// is not a valid country calling code.
    /// </summary>
    /// <param name="code">The country calling code to validate.</param>
    /// <param name="name">The name of the argument used in the exception message.</param>
    public static void EnsureIsValidCountryCallingCode(
        uint code,
        [CallerArgumentExpression(nameof(code))] string? name = null)
    {
        if (!IsValidCountryCallingCode(code))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(name,
                $"Invalid national country calling code: '{code}'");
        }
    }

    /// <summary>
    /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="number"/>
    /// is not a valid national telephone number.
    /// </summary>
    /// <param name="number">The national number to validate.</param>
    /// <param name="name">The name of the argument used in the exception message.</param>
    public static void EnsureIsValidNationalNumber(
        ulong number,
        [CallerArgumentExpression(nameof(number))] string? name = null)
    {
        if (!IsValidNationalNumber(number))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(name,
                $"Invalid national telephone number: '{number}'");
        }
    }

    /// <summary>
    /// Returns a value indicating whether the specified number is a valid national telephone number.
    /// </summary>
    /// <param name="number">The national number to check.</param>
    public static bool IsValidNationalNumber(ulong number)
    {
        return number is >= 1ul and <= 999999999999999ul;
    }

    /// <summary>
    /// Returns a value indicating whether the specified code is an assigned country calling code.
    /// </summary>
    /// <param name="code">The country calling code to check.</param>
    public static bool IsValidCountryCallingCode(uint code)
    {
        return CountryCallingCodeInfo.IsAssignedCode(code);
    }

    /// <summary>
    /// Parses a span of characters into a <see cref="TelephoneNumber"/>.
    /// </summary>
    /// <param name="s">The character span to parse.</param>
    public static TelephoneNumber Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, null);
    }

    /// <inheritdoc/>
    public static TelephoneNumber Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : ThrowHelper.ThrowFormatException<TelephoneNumber>(
                $"'{s.ToString()}' is not a valid {nameof(TelephoneNumber)}.");
    }

    /// <summary>
    /// Parses a string into a <see cref="TelephoneNumber"/>.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    public static TelephoneNumber Parse(string s)
    {
        return Parse(s, null);
    }

    /// <inheritdoc/>
    public static TelephoneNumber Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    /// <summary>
    /// Tries to parse a span of characters into a <see cref="TelephoneNumber"/>.
    /// </summary>
    /// <param name="s">The character span to parse.</param>
    /// <param name="result">When this method returns, contains the parsed value, or
    /// <see cref="Empty"/> if parsing failed.</param>
    /// <returns><see langword="true"/> if parsing succeeded; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, out TelephoneNumber result)
    {
        return TryParse(s, null, out result);
    }

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out TelephoneNumber result)
    {
        if (s.IsEmpty)
        {
            result = Empty;
            return true;
        }

        if (s.Length is < MinFormattedLength or > MaxFormattedLength)
        {
            return Failed(out result);
        }

        var span = s[0] == CountryCodePrefix ? s[1..] : s;
        var delimiterIndex = span.IndexOf(NationalNumberDelimiter);

        Span<char> digitBuffer = stackalloc char[MaxDigitCount];

        if (delimiterIndex > -1)
        {
            if (uint.TryParse(span[..delimiterIndex], NumberStyles.None, CultureInfo.InvariantCulture, out var countryCode)
                && span[(delimiterIndex + 1)..].TryCopyWhereNotPunctuationOrWhitespace(digitBuffer, out var digitsWritten)
                && ulong.TryParse(digitBuffer[..digitsWritten], NumberStyles.None, CultureInfo.InvariantCulture, out var nationalNumber))
            {
                result = new(countryCode, nationalNumber, true);
                return true;
            }

            return Failed(out result);
        }

        // no delimiter - try to infer country code
        if (!span.TryCopyWhereNotPunctuationOrWhitespace(digitBuffer, out var digitsWritten2) || digitsWritten2 <= 3)
        {
            return Failed(out result);
        }

        for (var i = 3; i > 0; i--)
        {
            if (!uint.TryParse(digitBuffer[..i], NumberStyles.None, CultureInfo.InvariantCulture, out var possibleCode))
            {
                continue;
            }

            if (!IsValidCountryCallingCode(possibleCode))
            {
                continue;
            }

            if (!ulong.TryParse(digitBuffer[i..], NumberStyles.None, CultureInfo.InvariantCulture, out var nationalNumber))
            {
                continue;
            }

            result = new(possibleCode, nationalNumber, true);
            return true;
        }

        return Failed(out result);

        static bool Failed(out TelephoneNumber result)
        {
            result = Empty;
            return false;
        }
    }

    /// <inheritdoc/>
    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out TelephoneNumber result)
    {
        if (s is null)
        {
            result = Empty;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return ToString(null, null);
    }

    /// <inheritdoc/>
    [SkipLocalsInit]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        Span<char> buffer = stackalloc char[MaxFormattedLength];

        return TryFormat(buffer, out var charsWritten, format, formatProvider)
            ? buffer[..charsWritten].ToString()
            : string.Empty;
    }

    /// <inheritdoc/>
    // +44 3300430021
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (destination.Length > 6)
        {
            destination[0] = CountryCodePrefix;

            if (CountryCode.TryFormat(destination[1..], out charsWritten, "D", CultureInfo.InvariantCulture))
            {
                charsWritten++; // initial '+'

                if (destination.Length > charsWritten)
                {
                    destination[charsWritten] = NationalNumberDelimiter;
                    charsWritten++;
                }

                if (NationalNumber.TryFormat(destination[charsWritten..],
                    out var nationalNumberCharsWritten, "D", CultureInfo.InvariantCulture))
                {
                    charsWritten += nationalNumberCharsWritten;
                    return true;
                }
            }
        }
        else
        {
            charsWritten = 0;
        }

        return false;
    }
}
