// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.TestValues;

[NominalValue]
public readonly partial struct NominalByteValue : INominalValue<NominalByteValue, byte>
{
    static int ISpanSerializable<NominalByteValue>.MaxSerializedCharLength { get; } = 128; // TODO

    public static readonly NominalByteValue Value1;
    public static readonly NominalByteValue Value2 = new(1);

    public static IEnumerable<NominalByteValue> ValidValues { get; } = new[] { Value1, Value2 };

    public static bool IsValidValue(byte value) => value is >= 0 and <= 1;

}
