// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Globalization.Text.Json;

namespace DSE.Open.Globalization;

[JsonConverter(typeof(JsonStringTelephoneNumberConverter))]
[StructLayout(LayoutKind.Auto)]
public readonly record struct TelephoneNumber
    : ISpanParsable<TelephoneNumber>,
      ISpanFormattable
{
    public static readonly TelephoneNumber Empty;

    // https://www.itu.int/rec/T-REC-E.164-201011-I/en
    public const int MaxDigitCount = 15;
    public const int MaxCountryCodeDigitCount = 3;

    public const int MaxFormattedLength = 20; // +1 123 456 789 01234
    public const int MinFormattedLength = 8;  // +1 12345

    public const char CountryCodePrefix = '+';
    public const char NationalNumberDelimiter = ' ';

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

    public uint CountryCode { get; }

    public ulong NationalNumber { get; }

    public IEnumerable<CountryCallingCodeInfo> GetCountryCallingCodeInfo()
        => CountryCallingCodeInfo.GetInfo(CountryCode);

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

    public static bool IsValidNationalNumber(ulong number) => number is >= 1ul and <= 999999999999999ul;

    public static bool IsValidCountryCallingCode(uint code) => CountryCallingCodeInfo.IsAssignedCode(code);

    public static TelephoneNumber Parse(ReadOnlySpan<char> s) => Parse(s, null);
    public static TelephoneNumber Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : ThrowHelper.ThrowFormatException<TelephoneNumber>(
                $"'{s.ToString()}' is not a valid {nameof(TelephoneNumber)}.");
    }

    public static TelephoneNumber Parse(string s) => Parse(s, null);

    public static TelephoneNumber Parse(string s, IFormatProvider? provider)
    {
        Guard.IsNotNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(ReadOnlySpan<char> s, out TelephoneNumber result)
        => TryParse(s, null, out result);

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
                result = new TelephoneNumber(countryCode, nationalNumber, true);
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

            result = new TelephoneNumber(possibleCode, nationalNumber, true);
            return true;
        }

        return Failed(out result);

        static bool Failed(out TelephoneNumber result)
        {
            result = Empty;
            return false;
        }
    }

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

    public override string ToString() => ToString(null, null);

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        Span<char> buffer = stackalloc char[MaxFormattedLength];

        return TryFormat(buffer, out var charsWritten, format, formatProvider)
            ? buffer[..charsWritten].ToString()
            : string.Empty;
    }

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
