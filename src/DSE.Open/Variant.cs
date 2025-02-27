// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open;

/// <summary>
/// A value that is either an integer, a floating-point number, a string, a boolean, or null.
/// </summary>
[JsonConverter(typeof(JsonVariantConverter))]
public readonly record struct Variant : ISpanFormattable
{
    public static readonly Variant Null;

    public Variant(long number)
    {
        Integer = number;
    }

    public Variant(double number)
    {
        FloatingPoint = number;
    }

    public Variant(string text)
    {
        ArgumentNullException.ThrowIfNull(text);
        Text = text;
    }

    public Variant(bool boolean)
    {
        Boolean = boolean;
    }

    public long? Integer { get; }

    public double? FloatingPoint { get; }

    public string? Text { get; }

    public bool? Boolean { get; }

    [MemberNotNullWhen(true, nameof(Integer))]
    public bool IsInteger => Integer is not null;

    [MemberNotNullWhen(true, nameof(FloatingPoint))]
    public bool IsFloatingPoint => FloatingPoint is not null;

    [MemberNotNullWhen(true, nameof(Text))]
    public bool IsText => Text is not null;

    [MemberNotNullWhen(true, nameof(Boolean))]
    public bool IsBoolean => Boolean is not null;

    public bool IsNull => Integer is null && FloatingPoint is null && Text is null && Boolean is null;

    /// <summary>
    /// Gets the value as an object.
    /// </summary>
    /// <returns></returns>
#pragma warning disable CA1024 // Use properties where appropriate
    public object? GetValue()
#pragma warning restore CA1024 // Use properties where appropriate
    {
        return Integer is not null ? Integer
            : FloatingPoint is not null ? FloatingPoint
            : Text ?? (object?)Boolean;
    }

    public override string? ToString()
    {
        return ToString(null, null);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (Text is not null)
        {
            return Text;
        }

        Span<char> span = stackalloc char[64];

        _ = TryFormat(span, out var charsWritten, format.AsSpan(), formatProvider);

        return new string(span[..charsWritten]);
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (Integer is not null)
        {
            return Integer.Value.TryFormat(destination, out charsWritten, format, provider);
        }
        else if (FloatingPoint is not null)
        {
            return FloatingPoint.Value.TryFormat(destination, out charsWritten, format, provider);
        }
        else
        {
            var text = Text is not null ? (ReadOnlySpan<char>)Text
                : (ReadOnlySpan<char>)(Boolean is not null ? Boolean.Value ? TrueString : FalseString : NullString);

            if (text.TryCopyTo(destination))
            {
                charsWritten = text.Length;
                return true;
            }

            charsWritten = 0;
            return false;
        }
    }

    public static implicit operator Variant(long number)
    {
        return FromInt64(number);
    }

    public static Variant FromInt64(long number)
    {
        return new(number);
    }

    public static implicit operator Variant(double number)
    {
        return FromDouble(number);
    }

    public static Variant FromDouble(double number)
    {
        return new(number);
    }

    public static implicit operator Variant(string text)
    {
        return FromString(text);
    }

    public static Variant FromString(string text)
    {
        return new(text);
    }

    public static implicit operator Variant(bool boolean)
    {
        return FromBoolean(boolean);
    }

    public static Variant FromBoolean(bool boolean)
    {
        return new(boolean);
    }

    public static explicit operator Variant(JsonElement element)
    {
        return FromJsonElement(element);
    }

    public static Variant FromJsonElement(JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.Null)
        {
            return Null;
        }
        else if (element.ValueKind == JsonValueKind.String)
        {
            return new Variant(element.GetString()!);
        }
        else if (element.ValueKind == JsonValueKind.Number)
        {
            return element.TryGetInt64(out var number) ? new Variant(number) : new Variant(element.GetDouble());
        }
        else if (element.ValueKind == JsonValueKind.True)
        {
            return new Variant(true);
        }
        else if (element.ValueKind == JsonValueKind.False)
        {
            return new Variant(false);
        }
        else
        {
            throw new NotSupportedException();
        }
    }

    private const string NullString = "null";
    private const string TrueString = "true";
    private const string FalseString = "false";
}

