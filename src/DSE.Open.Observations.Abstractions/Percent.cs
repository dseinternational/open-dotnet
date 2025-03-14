// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;
using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

/// <summary>
/// A value that expresses a ratio as a signed value between 0 and 100 (values between -100 and 100).
/// </summary>
[DivisibleValue]
[JsonConverter(typeof(JsonDecimalValueConverter<Percent>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct Percent : IDivisibleValue<Percent, decimal>, IUtf8SpanSerializable<Percent>
{
    public static int MaxSerializedCharLength => 128; // TODO

    public static int MaxSerializedByteLength => 128; // TODO

    public static Percent Zero { get; } = new(0);

    public Percent(decimal value) : this(value, false) { }

    public static bool IsValidValue(decimal value)
    {
        return value is >= -100m and <= 100m;
    }

    public Ratio ToRatio()
    {
        return (Ratio)(_value * 100);
    }

    public static explicit operator Percent(Ratio value)
    {
        return FromRatio(value);
    }

    public static Percent FromRatio(Ratio value)
    {
        return new((decimal)value * 100m);
    }
}
