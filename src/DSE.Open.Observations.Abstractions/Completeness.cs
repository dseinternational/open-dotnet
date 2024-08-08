// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Observations;

/// <summary>
/// A rating of completeness.
/// </summary>
[ComparableValue]
[JsonConverter(typeof(JsonUInt32ValueConverter<Completeness>))]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct Completeness
    : IComparableValue<Completeness, uint>,
      IUtf8SpanSerializable<Completeness>
{
    private const uint PartialValue = 10;
    private const uint DevelopingValue = 50;
    private const uint CompleteValue = 90;

    public static int MaxSerializedCharLength => 2;

    public static int MaxSerializedByteLength => 2;

    public static bool IsValidValue(uint value)
    {
        return value is PartialValue or DevelopingValue or CompleteValue;
    }

    public static Completeness Partial => new(PartialValue);

    public static Completeness Developing => new(DevelopingValue);

    public static Completeness Complete => new(CompleteValue);
}
