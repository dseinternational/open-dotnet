// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values.TestValues;

[EquatableValue]
[JsonConverter(typeof(JsonUInt16ValueConverter<UInt16Value>))]
public readonly partial struct UInt16Value : IEquatableValue<UInt16Value, ushort>
{
    public static int MaxSerializedCharLength => 5;

    public static int MaxSerializedByteLength => 5;

    public static bool IsValidValue(ushort value)
    {
        return true;
    }
}
