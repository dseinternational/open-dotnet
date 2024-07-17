// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using DSE.Open.Values.Text.Json.Serialization;
using System.Text.Json.Serialization;

namespace DSE.Open.Values;

/// <summary>
/// A (non-negative) count.
/// </summary>
[DivisibleValue]
[JsonConverter(typeof(JsonUInt32ValueConverter<Count>))]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct Count : IDivisibleValue<Count, uint>, IUtf8SpanSerializable<Count>
{
    public static int MaxSerializedCharLength => 10;

    public static int MaxSerializedByteLength => 10;

    public static Count Zero { get; } = new(0);

    public Count(uint value) : this(value, true) { }

    public static bool IsValidValue(uint value)
    {
        return value >= Zero._value;
    }
}
