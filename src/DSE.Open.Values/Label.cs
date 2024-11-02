// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using CommunityToolkit.HighPerformance.Buffers;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

/// <summary>
/// A label. Must contain at least 2 non-whitespace characters and be no longer than 120 characters long
/// in total, and may not start or end with whitespace characters.
/// </summary>
[ComparableValue]
[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<Label, CharSequence>))]
public readonly partial struct Label : IComparableValue<Label, CharSequence>
{
    public const int MinLength = 2;
    public const int MaxLength = 120;

    public static int MaxSerializedCharLength => MaxLength;

    public static readonly Label Empty;

    public Label(string label) : this(label, skipValidation: false)
    {
    }

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

    public bool HasPrefix
    {
        get
        {
            var span = _value.AsSpan();
            return span.Length > MinLength && span[2..].Contains(':');
        }
    }

    public ReadOnlySpan<char> AsSpan()
    {
        return _value.AsSpan();
    }

    public ReadOnlyMemory<char> AsMemory()
    {
        return _value.AsMemory();
    }

    public ReadOnlySpan<char> GetPrefix()
    {
        var span = _value.AsSpan();

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

    public static bool IsValidLabel(ReadOnlySpan<char> label)
    {
        return CharSequence.TryParse(label, out var charSequence) && IsValidValue(charSequence);
    }

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

        foreach (var c in value.AsSpan())
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

    public static string GetString(ReadOnlySpan<char> value)
    {
        return LabelStringPool.Shared.GetOrAdd(value);
    }

    public static explicit operator Label(string label)
    {
        return FromString(label);
    }

    public static Label FromString(string label)
    {
        return new(label);
    }

    public static explicit operator string(Label label)
    {
        return label.ToString();
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator ReadOnlyMemory<char>(Label label)
    {
        return label._value.AsMemory();
    }

    public static explicit operator ReadOnlySpan<char>(Label label)
    {
        return label._value.AsSpan();
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

    private static class LabelStringPool
    {
        public static readonly StringPool Shared = new(512);
    }
}
