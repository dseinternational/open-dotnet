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
    /// <summary>
    /// A <see cref="Variant"/> representing a null value.
    /// </summary>
    public static readonly Variant Null;

    /// <summary>Initialises a new <see cref="Variant"/> representing an integer value.</summary>
    public Variant(long number)
    {
        Integer = number;
    }

    /// <summary>Initialises a new <see cref="Variant"/> representing a floating-point value.</summary>
    public Variant(double number)
    {
        FloatingPoint = number;
    }

    /// <summary>Initialises a new <see cref="Variant"/> representing a string value.</summary>
    public Variant(string text)
    {
        ArgumentNullException.ThrowIfNull(text);
        Text = text;
    }

    /// <summary>Initialises a new <see cref="Variant"/> representing a boolean value.</summary>
    public Variant(bool boolean)
    {
        Boolean = boolean;
    }

    /// <summary>Gets the integer value, if this <see cref="Variant"/> is an integer.</summary>
    public long? Integer { get; }

    /// <summary>Gets the floating-point value, if this <see cref="Variant"/> is a floating-point number.</summary>
    public double? FloatingPoint { get; }

    /// <summary>Gets the string value, if this <see cref="Variant"/> is a string.</summary>
    public string? Text { get; }

    /// <summary>Gets the boolean value, if this <see cref="Variant"/> is a boolean.</summary>
    public bool? Boolean { get; }

    /// <summary>Gets a value indicating whether this <see cref="Variant"/> represents an integer.</summary>
    [MemberNotNullWhen(true, nameof(Integer))]
    public bool IsInteger => Integer is not null;

    /// <summary>Gets a value indicating whether this <see cref="Variant"/> represents a floating-point number.</summary>
    [MemberNotNullWhen(true, nameof(FloatingPoint))]
    public bool IsFloatingPoint => FloatingPoint is not null;

    /// <summary>Gets a value indicating whether this <see cref="Variant"/> represents a string.</summary>
    [MemberNotNullWhen(true, nameof(Text))]
    public bool IsText => Text is not null;

    /// <summary>Gets a value indicating whether this <see cref="Variant"/> represents a boolean.</summary>
    [MemberNotNullWhen(true, nameof(Boolean))]
    public bool IsBoolean => Boolean is not null;

    /// <summary>Gets a value indicating whether this <see cref="Variant"/> represents null (no value).</summary>
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

    /// <inheritdoc/>
    public override string? ToString()
    {
        return ToString(null, null);
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
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
            var text = Text is not null
                ? Text
                : (ReadOnlySpan<char>)(Boolean is not null
                    ? Boolean.Value ? TrueString
                    : FalseString : NullString);

            if (text.TryCopyTo(destination))
            {
                charsWritten = text.Length;
                return true;
            }

            charsWritten = 0;
            return false;
        }
    }

    /// <summary>Creates a <see cref="Variant"/> from an <see cref="long"/> value.</summary>
    public static implicit operator Variant(long number)
    {
        return FromInt64(number);
    }

    /// <summary>Creates a <see cref="Variant"/> from an <see cref="long"/> value.</summary>
    public static Variant FromInt64(long number)
    {
        return new(number);
    }

    /// <summary>Creates a <see cref="Variant"/> from a <see cref="double"/> value.</summary>
    public static implicit operator Variant(double number)
    {
        return FromDouble(number);
    }

    /// <summary>Creates a <see cref="Variant"/> from a <see cref="double"/> value.</summary>
    public static Variant FromDouble(double number)
    {
        return new(number);
    }

    /// <summary>Creates a <see cref="Variant"/> from a <see cref="string"/> value.</summary>
    public static implicit operator Variant(string text)
    {
        return FromString(text);
    }

    /// <summary>Creates a <see cref="Variant"/> from a <see cref="string"/> value.</summary>
    public static Variant FromString(string text)
    {
        return new(text);
    }

    /// <summary>Creates a <see cref="Variant"/> from a <see cref="bool"/> value.</summary>
    public static implicit operator Variant(bool boolean)
    {
        return FromBoolean(boolean);
    }

    /// <summary>Creates a <see cref="Variant"/> from a <see cref="bool"/> value.</summary>
    public static Variant FromBoolean(bool boolean)
    {
        return new(boolean);
    }

    /// <summary>
    /// Creates a <see cref="Variant"/> from a JSON value. Supported kinds are <see cref="JsonValueKind.Null"/>,
    /// <see cref="JsonValueKind.String"/>, <see cref="JsonValueKind.Number"/>, <see cref="JsonValueKind.True"/>
    /// and <see cref="JsonValueKind.False"/>.
    /// </summary>
    public static explicit operator Variant(JsonElement element)
    {
        return FromJsonElement(element);
    }

    /// <summary>
    /// Creates a <see cref="Variant"/> from a JSON value. Supported kinds are <see cref="JsonValueKind.Null"/>,
    /// <see cref="JsonValueKind.String"/>, <see cref="JsonValueKind.Number"/>, <see cref="JsonValueKind.True"/>
    /// and <see cref="JsonValueKind.False"/>.
    /// </summary>
    /// <exception cref="NotSupportedException">The JSON value kind is not supported.</exception>
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

