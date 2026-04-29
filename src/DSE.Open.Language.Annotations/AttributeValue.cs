// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Hashing;
using DSE.Open.Language.Annotations.Serialization;
using DSE.Open.Runtime.Helpers;
using DSE.Open.Values;

namespace DSE.Open.Language.Annotations;

/// <summary>
/// A name/value attribute pair used for the CoNLL-U <c>MISC</c> column. The value
/// part may consist of one or more comma-separated character sequences.
/// </summary>
[JsonConverter(typeof(JsonStringAttributeValueConverter))]
public sealed record AttributeValue
    : ISpanFormattable,
      ISpanParsable<AttributeValue>,
      ISpanSerializable<AttributeValue>,
      IRepeatableHash64
{
    /// <summary>
    /// The maximum number of characters used to serialize an <see cref="AttributeValue"/>.
    /// </summary>
    public static int MaxSerializedCharLength { get; } = 512;

    private readonly ReadOnlyValueCollection<CharSequence> _values;

    /// <summary>
    /// Initializes a new <see cref="AttributeValue"/> with the specified name and values.
    /// </summary>
    public AttributeValue(AlphaNumericCode name, IEnumerable<CharSequence> values)
        : this(name, [.. values])
    {
    }

    /// <summary>
    /// Initializes a new <see cref="AttributeValue"/> with the specified name and values.
    /// </summary>
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

    /// <inheritdoc/>
    public override string ToString()
    {
        return ToString(null, CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    [SkipLocalsInit]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var charCount = GetCharCount();
        char[]? rented = null;

        Span<char> buffer = MemoryThresholds.CanStackalloc<char>(charCount)
            ? stackalloc char[charCount]
            : (rented = ArrayPool<char>.Shared.Rent(charCount));

        try
        {
            if (TryFormat(buffer, out var charsWritten, format, formatProvider))
            {
                return buffer[..charsWritten].ToString();
            }

            return ThrowHelper.ThrowFormatException<string>(
                $"Could not format {nameof(AttributeValue)} to string.");
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<char>.Shared.Return(rented);
            }
        }
    }

    /// <inheritdoc/>
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

    /// <summary>
    /// Parses an <see cref="AttributeValue"/> from the specified character span using the invariant culture.
    /// </summary>
    public static AttributeValue Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    public static AttributeValue Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var value))
        {
            return value;
        }

        return ThrowHelper.ThrowFormatException<AttributeValue>($"Cannot parse '{s}' as {nameof(AttributeValue)}.");
    }

    /// <summary>
    /// Tries to parse an <see cref="AttributeValue"/> from the specified character span using the invariant culture.
    /// </summary>
    public static bool TryParse(
        ReadOnlySpan<char> s,
        [MaybeNullWhen(false)] out AttributeValue result)
    {
        return TryParse(s, CultureInfo.InvariantCulture, out result);
    }

    /// <inheritdoc/>
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

                var values = new List<CharSequence>();
                var remaining = valuesSpan;

                while (true)
                {
                    var separatorIndex = remaining.IndexOf(',');
                    var v = (separatorIndex < 0 ? remaining : remaining[..separatorIndex]).Trim();

                    if (!v.IsEmpty)
                    {
                        if (CharSequence.TryParse(v, provider, out var value))
                        {
                            values.Add(value);
                        }
                        else
                        {
                            result = default;
                            return false;
                        }
                    }

                    if (separatorIndex < 0)
                    {
                        break;
                    }

                    remaining = remaining[(separatorIndex + 1)..];
                }

                if (values.Count == 0)
                {
                    result = default;
                    return false;
                }

                result = new(name, values);
                return true;
            }
        }

        result = default;
        return false;
    }

    /// <summary>
    /// Parses an <see cref="AttributeValue"/> from the specified string using the invariant culture.
    /// </summary>
    public static AttributeValue Parse(string s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Parses an <see cref="AttributeValue"/> from the specified string using the invariant culture.
    /// </summary>
    public static AttributeValue ParseInvariant(string s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    public static AttributeValue Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    /// <inheritdoc/>
    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out AttributeValue result)
    {
        return TryParse(s.AsSpan(), provider, out result);
    }

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        var hash = Name.GetRepeatableHashCode();

        for (var i = 0; i < _values.Count; i++)
        {
            hash = RepeatableHash64Provider.Default.CombineHashCodes(hash, _values[i].GetRepeatableHashCode());
        }

        return hash;
    }
}
