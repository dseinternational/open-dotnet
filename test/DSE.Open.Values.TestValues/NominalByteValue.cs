// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.TestValues;

[EquatableValue]
public readonly partial struct NominalByteValue : IEquatableValue<NominalByteValue, byte>
{
    public static int MaxSerializedCharLength => 128; // TODO

    public static int MaxSerializedByteLength => 128; // TODO

    public static readonly NominalByteValue Value1;
    public static readonly NominalByteValue Value2 = new(1);

    public static IEnumerable<NominalByteValue> ValidValues { get; } = [Value1, Value2];

    public static bool IsValidValue(byte value)
    {
        return value <= 1;
    }
}
