// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using CommunityToolkit.HighPerformance.Buffers;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

/// <summary>
/// A label. Must contain at least 2 non-whitespace characters and be no longer than 120 characters long
/// in total, and may not start or end with whitespace characters.
/// </summary>
[StructLayout(LayoutKind.Auto)]
[JsonConverter(typeof(JsonStringLabelConverter))]
public readonly record struct Label
    : IComparable<Label>,
      ISpanFormattable,
      ISpanParsable<Label>, // ISpanProvider<char>,
      IEquatable<Label>
{
    public const int MinLength = 2;
    public const int MaxLength = 120;

    public static readonly Label Empty;

    /// <remarks>
    /// <c>null</c> if <cref cref="Empty"/>.
    /// </remarks>
    private readonly string? _label;

    public Label(string label) : this(label, false)
    {
    }

    public Label(ReadOnlySpan<char> label) : this(label, false)
    {
    }

    private Label(string label, bool skipValidation)
    {
        Guard.IsNotNullOrWhiteSpace(label);

        if (!skipValidation)
        {
            EnsureValidLabel(label.AsSpan());
        }

        _label = string.IsInterned(label) ?? LabelStringPool.Shared.GetOrAdd(label);
    }

    private Label(ReadOnlySpan<char> label, bool skipValidation)
    {
        if (!skipValidation)
        {
            EnsureValidLabel(label);
        }

        _label = LabelStringPool.Shared.GetOrAdd(label);
    }

    public bool HasPrefix
    {
        get
        {
            var span = _label.AsSpan();
            return span.Length > MinLength && span[2..].Contains(':');
        }
    }

    public ReadOnlySpan<char> AsSpan() => _label.AsSpan();

    public ReadOnlyMemory<char> AsMemory() => _label.AsMemory();

    public string GetPrefix()
    {
        var span = _label.AsSpan();

        if (span.Length <= MinLength)
        {
            return string.Empty;
        }

        var firstColonIndex = span.IndexOf(':');

        return firstColonIndex > -1 ? span[..firstColonIndex].ToString() : string.Empty;
    }

    public static void EnsureValidLabel(ReadOnlySpan<char> label)
    {
        if (!IsValidLabel(label))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(label), $"Invalid {nameof(Label)}: {label}");
        }
    }

    public bool Equals(string? other) => Equals(other, StringComparison.Ordinal);

    public bool Equals(string? other, StringComparison comparison) => other is not null
        && ((_label is null && other.Length == 0) || string.Equals(_label, other, comparison));

    public bool Equals(ReadOnlyMemory<char> other) => Equals(other.Span);

    public bool Equals(ReadOnlySpan<char> other) => _label.AsSpan().SequenceEqual(other);

    public bool Equals(Label other) => Equals(other._label.AsSpan());

    public override int GetHashCode() => string.GetHashCode(_label.AsSpan(), StringComparison.Ordinal);

    public static bool IsValidLabel(ReadOnlySpan<char> label)
    {
        if (label.Length is < MinLength or > MaxLength)
        {
            return false;
        }

        if (char.IsWhiteSpace(label[0]) || char.IsWhiteSpace(label[^1]))
        {
            return false;
        }

        var nonWhitespaceCount = 0;

        foreach (var c in label)
        {
            if (!char.IsWhiteSpace(c))
            {
                nonWhitespaceCount++;
                if (nonWhitespaceCount == 2)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public int CompareTo(Label other) => CompareTo(other, StringComparison.CurrentCulture);

    public int CompareTo(Label other, StringComparison comparisonType)
        => string.Compare(_label, other._label, comparisonType);

    public bool TryFormat(Span<char> destination, out int charsWritten)
        => TryFormat(destination, out charsWritten, default, null);

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        var span = _label.AsSpan();

        if (destination.Length < span.Length)
        {
            charsWritten = 0;
            return false;
        }

        span.CopyTo(destination);
        charsWritten = span.Length;

        return true;
    }

    public override string ToString() => ToString(null, null);

    public string ToString(string? format, IFormatProvider? formatProvider) => _label ?? string.Empty;

    public static Label Parse(string s) => Parse(s, null);

    public static Label Parse(string s, IFormatProvider? provider)
    {
        Guard.IsNotNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static Label Parse(ReadOnlySpan<char> s) => Parse(s, null);

    public static Label Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var label)
            ? label
            : ThrowHelper.ThrowFormatException<Label>($"Could not parse {nameof(Label)} with value: {s}");
    }

    public static bool TryParse(string? s, out Label result) => TryParse(s, null, out result);

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Label result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    public static bool TryParse(ReadOnlySpan<char> s, out Label result) => TryParse(s, null, out result);

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Label result)
    {
        if (s.IsEmpty)
        {
            result = default;
            return true;
        }

        if (!IsValidLabel(s))
        {
            result = default;
            return false;
        }

        result = new Label(s, skipValidation: true);
        return true;
    }

    public static explicit operator Label(string label) => FromString(label);

    public static Label FromString(string label) => new(label);

    public static explicit operator string(Label label) => label.ToString();

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator ReadOnlyMemory<char>(Label label) => label._label.AsMemory();

    public static explicit operator ReadOnlySpan<char>(Label label) => label._label;

#pragma warning restore CA2225 // Operator overloads have named alternates

    private static class LabelStringPool
    {
        public static readonly StringPool Shared = new(512);
    }

    public static bool operator <(Label left, Label right) => left.CompareTo(right, StringComparison.CurrentCulture) < 0;

    public static bool operator <=(Label left, Label right) => left.CompareTo(right, StringComparison.CurrentCulture) <= 0;

    public static bool operator >(Label left, Label right) => left.CompareTo(right, StringComparison.CurrentCulture) > 0;

    public static bool operator >=(Label left, Label right) => left.CompareTo(right, StringComparison.CurrentCulture) >= 0;
}
