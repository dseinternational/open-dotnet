// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;

namespace DSE.Open.Values;

/// <summary>
/// A value that expresses a ratio as a signed value between 0 and 1 (values between -1 and 1).
/// </summary>
[DivisibleValue]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct Ratio : IDivisibleValue<Ratio, double>
{
    public static int MaxSerializedCharLength { get; } = 128; // TODO

    public static Ratio Zero { get; } = new(0);

    public static bool IsValidValue(double value) => value is >= -1 and <= 1;

    public Percent ToPercent() => (Percent)(_value * 100);

    public static explicit operator Ratio(Percent value) => FromPercent(value);

    public static Ratio FromPercent(Percent value) => new((double)value / 100);
}
