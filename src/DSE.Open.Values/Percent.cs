// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;

namespace DSE.Open.Values;

/// <summary>
/// A value that expresses a ratio as a signed value between 0 and 100 (values between -100 and 100).
/// </summary>
[RatioValue]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct Percent : IRatioValue<Percent, double>
{
    static int ISpanSerializable<Percent>.MaxSerializedCharLength { get; } = 128; // TODO

    public static Percent Zero { get; } = new(0);

    public static bool IsValidValue(double value) => value is >= -100 and <= 100;

    public Ratio ToRatio() => (Ratio)(_value * 100);

    public static explicit operator Percent(Ratio value) => FromRatio(value);

    public static Percent FromRatio(Ratio value) => new((double)value * 100);
}
