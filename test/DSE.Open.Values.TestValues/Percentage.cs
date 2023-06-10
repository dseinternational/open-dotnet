// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.TestValues;

[RatioValue]
public readonly partial struct Percentage : IRatioValue<Percentage, float>
{
    static int ISpanSerializable<Percentage>.MaxSerializedCharLength { get; } = 128; // TODO

    public static Percentage Zero { get; } = new(0);

    public static bool IsValidValue(float value) => value is >= 0 and <= 100;
}
