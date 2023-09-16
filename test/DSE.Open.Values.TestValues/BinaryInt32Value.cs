// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values.TestValues;

[EquatableValue]
[JsonConverter(typeof(JsonInt32ValueConverter<BinaryInt32Value>))]
public readonly partial struct BinaryInt32Value : IEquatableValue<BinaryInt32Value, int>
{
    public static int MaxSerializedCharLength => 128; // TODO

    public static int MaxSerializedByteLength => 128; // TODO

    public static readonly BinaryInt32Value False = new(0);

    public static readonly BinaryInt32Value True = new(1);

    public static IEnumerable<BinaryInt32Value> ValidValues { get; } = new[] { False, True };

    public static bool IsValidValue(int value) => value is 0 or 1;

}
