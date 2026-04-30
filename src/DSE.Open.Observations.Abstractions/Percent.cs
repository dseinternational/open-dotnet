// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;
using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

/// <summary>
/// A value that expresses a ratio as a signed percentage in the inclusive range -100 to 100.
/// </summary>
[DivisibleValue]
[JsonConverter(typeof(JsonDecimalValueConverter<Percent>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct Percent : IDivisibleValue<Percent, decimal>, IUtf8SpanSerializable<Percent>
{
    /// <summary>
    /// Gets the maximum length, in characters, of the serialized representation of a <see cref="Percent"/>.
    /// </summary>
    public static int MaxSerializedCharLength => 32;

    /// <summary>
    /// Gets the maximum length, in bytes, of the serialized representation of a <see cref="Percent"/>.
    /// </summary>
    public static int MaxSerializedByteLength => 32;

    /// <summary>
    /// Gets a <see cref="Percent"/> representing zero.
    /// </summary>
    public static Percent Zero { get; } = new(0);

    /// <summary>
    /// Initializes a new <see cref="Percent"/> from the specified value.
    /// </summary>
    /// <param name="value">The underlying value, in the inclusive range -100 to 100.</param>
    public Percent(decimal value) : this(value, false) { }

    /// <summary>
    /// Determines whether the specified value is a valid <see cref="Percent"/> value.
    /// </summary>
    public static bool IsValidValue(decimal value)
    {
        return value is >= -100m and <= 100m;
    }

    /// <summary>
    /// Returns this percentage expressed as a <see cref="Ratio"/>.
    /// </summary>
    public Ratio ToRatio()
    {
        return (Ratio)(_value / 100m);
    }

    /// <summary>
    /// Converts a <see cref="Ratio"/> to a <see cref="Percent"/>.
    /// </summary>
    public static explicit operator Percent(Ratio value)
    {
        return FromRatio(value);
    }

    /// <summary>
    /// Returns the specified <see cref="Ratio"/> expressed as a <see cref="Percent"/>.
    /// </summary>
    public static Percent FromRatio(Ratio value)
    {
        return new((decimal)value * 100m);
    }
}
