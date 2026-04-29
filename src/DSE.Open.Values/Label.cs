// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using CommunityToolkit.HighPerformance.Buffers;
using DSE.Open.Hashing;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

/// <summary>
/// A label. Must contain at least 2 non-whitespace characters and be no longer than 120 characters long
/// in total, and may not start or end with whitespace characters.
/// </summary>
[ComparableValue]
[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<Label, CharSequence>))]
public readonly partial struct Label : IComparableValue<Label, CharSequence>, IRepeatableHash64
{
    /// <summary>
    /// The minimum number of non-whitespace characters required in a <see cref="Label"/>.
    /// </summary>
    public const int MinLength = 2;

    /// <summary>
    /// The maximum total length, in characters, of a <see cref="Label"/>.
    /// </summary>
    public const int MaxLength = 120;

    /// <summary>
    /// Gets the maximum number of characters required when serializing a <see cref="Label"/> as text.
    /// </summary>
    public static int MaxSerializedCharLength => MaxLength;

    /// <summary>
    /// An empty <see cref="Label"/>.
    /// </summary>
    public static readonly Label Empty;

    /// <summary>
    /// Initialises a new <see cref="Label"/> from the supplied string, validating its contents.
    /// </summary>
    public Label(string label) : this(label, skipValidation: false)
    {
    }

    /// <summary>
    /// Initialises a new <see cref="Label"/> from the supplied character span, validating its contents.
    /// </summary>
    public Label(ReadOnlySpan<char> label) : this(label, skipValidation: false)
    {
    }

    private Label(string label, bool skipValidation)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(label);

        if (!skipValidation)
        {
            EnsureIsValidValue(label);
        }

        _value = string.IsInterned(label) ?? LabelStringPool.Shared.GetOrAdd(label);
    }

    private Label(ReadOnlySpan<char> label, bool skipValidation)
    {
        if (!skipValidation)
        {
            EnsureIsValidValue(CharSequence.Parse(label, CultureInfo.InvariantCulture));
        }

        _value = LabelStringPool.Shared.GetOrAdd(label);
    }

    /// <summary>
    /// Gets a value indicating whether the label contains a prefix terminated by a colon.
    /// </summary>
    public bool HasPrefix
    {
        get
        {
            var span = _value.Span;
            return span.Length > MinLength && span[2..].Contains(':');
        }
    }

    /// <summary>
    /// Returns a read-only span over the label's characters.
    /// </summary>
    public ReadOnlySpan<char> AsSpan()
    {
        return _value.Span;
    }

    /// <summary>
    /// Returns a read-only memory region over the label's characters.
    /// </summary>
    public ReadOnlyMemory<char> AsMemory()
    {
        return _value.AsMemory();
    }

    /// <summary>
    /// Returns the prefix portion of the label (the characters preceding the first <c>:</c>),
    /// or an empty span if the label has no prefix.
    /// </summary>
    public ReadOnlySpan<char> GetPrefix()
    {
        var span = _value.Span;

        if (span.Length <= MinLength)
        {
            return [];
        }

        var firstColonIndex = span.IndexOf(':');

        return firstColonIndex switch
        {
            < 0 => [],
            _ => span[..firstColonIndex]
        };
    }

    /// <summary>
    /// Returns <see langword="true"/> if the supplied character span is a valid <see cref="Label"/>.
    /// </summary>
    public static bool IsValidLabel(ReadOnlySpan<char> label)
    {
        return CharSequence.TryParse(label, out var charSequence) && IsValidValue(charSequence);
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> is between <see cref="MinLength"/> and <see cref="MaxLength"/> characters,
    /// does not start or end with whitespace, and contains at least two non-whitespace characters.
    /// </summary>
    public static bool IsValidValue(CharSequence value)
    {
        if (value.Length is < MinLength or > MaxLength)
        {
            return false;
        }

        if (char.IsWhiteSpace(value[0]) || char.IsWhiteSpace(value[^1]))
        {
            return false;
        }

        var nonWhitespaceCount = 0;

        foreach (var c in value.Span)
        {
            if (char.IsWhiteSpace(c))
            {
                continue;
            }

            nonWhitespaceCount++;

            if (nonWhitespaceCount == 2)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns a pooled <see cref="string"/> instance equal to <paramref name="value"/>.
    /// </summary>
    public static string GetString(ReadOnlySpan<char> value)
    {
        return LabelStringPool.Shared.GetOrAdd(value);
    }

    /// <summary>
    /// Explicitly converts a string to a <see cref="Label"/>.
    /// </summary>
    public static explicit operator Label(string label)
    {
        return FromString(label);
    }

    /// <summary>
    /// Creates a <see cref="Label"/> from the supplied string.
    /// </summary>
    public static Label FromString(string label)
    {
        return new(label);
    }

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }

    /// <summary>
    /// Explicitly converts a <see cref="Label"/> to its string representation.
    /// </summary>
    public static explicit operator string(Label label)
    {
        return label.ToString();
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    /// <summary>
    /// Explicitly converts a <see cref="Label"/> to a <see cref="ReadOnlyMemory{T}"/> over its characters.
    /// </summary>
    public static explicit operator ReadOnlyMemory<char>(Label label)
    {
        return label._value.AsMemory();
    }

    /// <summary>
    /// Explicitly converts a <see cref="Label"/> to a <see cref="ReadOnlySpan{T}"/> over its characters.
    /// </summary>
    public static explicit operator ReadOnlySpan<char>(Label label)
    {
        return label._value.Span;
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

    private static class LabelStringPool
    {
        public static readonly StringPool Shared = new(512);
    }
}
