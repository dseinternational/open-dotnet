// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Language.Annotations.Serialization;
using DSE.Open.Values;

namespace DSE.Open.Language.Annotations;

[JsonConverter(typeof(JsonStringAttributeValueConverter))]
public sealed record AttributeValue
    : ISpanFormattable,
      ISpanParsable<AttributeValue>,
      ISpanSerializable<AttributeValue>
{
    public static int MaxSerializedCharLength { get; } = 512;

    private readonly ReadOnlyValueCollection<CharSequence> _values;

    public AttributeValue(AlphaNumericCode name, IEnumerable<CharSequence> values)
        : this(name, [.. values])
    {
    }

    public AttributeValue(AlphaNumericCode name, ReadOnlyValueCollection<CharSequence> values)
    {
        ArgumentNullException.ThrowIfNull(values);

        Name = name;

        _values = values;

        if (_values.Count == 0)
        {
            ThrowHelper.ThrowArgumentException("Must have at least one value", nameof(values));
        }
    }

    /// <summary>
    /// Gets the name of the feature.
    /// </summary>
    public AlphaNumericCode Name { get; }

    /// <summary>
    /// Gets the value specified for the feature. If more than one value is specified, this only
    /// returns the first value.
    /// </summary>
    public CharSequence Value => _values[0];

    /// <summary>
    /// Get the values specified for the feature.
    /// </summary>
    public ReadOnlyValueCollection<CharSequence> Values => _values;

    internal int GetCharCount()
    {
        return Name.Length + _values.Sum(v => v.Length + 1); // no -1 to accommodate = sign
    }

    public override string ToString()
    {
        return ToString(null, CultureInfo.InvariantCulture);
    }

    [SkipLocalsInit]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        Span<char> buffer = stackalloc char[MaxSerializedCharLength];
        _ = TryFormat(buffer, out var charsWritten, format, formatProvider);
        return buffer[..charsWritten].ToString();
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var charCount = GetCharCount();

        if (destination.Length >= charCount
            && Name.TryFormat(destination, out charsWritten, format, provider))
        {
            destination[charsWritten++] = '=';

            foreach (var value in _values)
            {
                if (!value.TryFormat(destination[charsWritten..], out var valueCharsWritten, format, provider))
                {
                    charsWritten = 0;
                    return false;
                }

                charsWritten += valueCharsWritten;

                if (charsWritten < charCount)
                {
                    destination[charsWritten++] = ',';
                }
            }

            return true;
        }

        charsWritten = 0;
        return false;
    }

    public static AttributeValue Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    public static AttributeValue Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var value))
        {
            return value;
        }

        return ThrowHelper.ThrowFormatException<AttributeValue>($"Cannot parse '{s}' as {typeof(AttributeValue).Name}.");
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        [MaybeNullWhen(false)] out AttributeValue result)
    {
        return TryParse(s, CultureInfo.InvariantCulture, out result);
    }

    [SkipLocalsInit]
    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out AttributeValue result)
    {
        if (s.Length >= 3)
        {
            var i = s.IndexOf('=');

            if (i > 0 && i <= s.Length - 2
                && AlphaNumericCode.TryParse(s[..i], provider, out var name))
            {
                var valuesSpan = s[(i + 1)..];

                var c = valuesSpan.IndexOf(',');

                if (c < 0)
                {
                    if (CharSequence.TryParse(valuesSpan, provider, out var value))
                    {
                        result = new(name, [value]);
                        return true;
                    }
                    else
                    {
                        result = default;
                        return false;
                    }
                }

                Span<Range> ranges = stackalloc Range[16];

                var l = valuesSpan.Split(ranges, ',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                var values = new CharSequence[l];

                for (var r = 0; r < l; r++)
                {
                    var v = valuesSpan[ranges[r].Start.Value..ranges[r].End.Value];

                    if (CharSequence.TryParse(v, provider, out var value))
                    {
                        values[r] = value;
                    }
                    else
                    {
                        result = default;
                        return false;
                    }
                }

                result = new(name, values);
                return true;
            }
        }

        result = default;
        return false;
    }

    public static AttributeValue Parse(string s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    public static AttributeValue ParseInvariant(string s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    public static AttributeValue Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out AttributeValue result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }
}
