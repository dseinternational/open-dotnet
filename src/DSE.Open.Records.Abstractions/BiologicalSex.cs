// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Records;

[EquatableValue]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<BiologicalSex, AsciiString>))]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct BiologicalSex : IEquatableValue<BiologicalSex, AsciiString>, IUtf8SpanSerializable<BiologicalSex>
{
    public static int MaxSerializedCharLength => 6;

    public static int MaxSerializedByteLength => 6;

    public static bool IsValidValue(AsciiString value)
    {
        return value.Length > 1 && value.Length <= 6 && Lookup.ContainsKey(value);
    }

    public static readonly BiologicalSex Female = new((AsciiString)"female", true);

    public static readonly BiologicalSex Male = new((AsciiString)"male", true);

    public static readonly IReadOnlyCollection<BiologicalSex> All = new[]
    {
        Female,
        Male,
    };

    public static readonly IReadOnlyDictionary<AsciiString, BiologicalSex> Lookup = All.ToDictionary(r => r._value);
}
