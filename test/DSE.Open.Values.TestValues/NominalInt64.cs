// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.TestValues;

[EquatableValue]
public readonly partial struct NominalInt64 : IEquatableValue<NominalInt64, long>
{
    public static int MaxSerializedCharLength => 128; // TODO

    public static int MaxSerializedByteLength => 128; // TODO

    public static IEnumerable<NominalInt64> ValueSet => new[] { (NominalInt64)default };

    public static bool IsValidValue(long value)
    {
        return value == 0;
    }
}
