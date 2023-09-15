// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Records;

[EquatableValue]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<Gender, AsciiString>))]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct Gender : IEquatableValue<Gender, AsciiString>
{
    public static int MaxSerializedCharLength => 6;

    public static bool IsValidValue(AsciiString value)
        => value.Length > 1 && value.Length <= 6 && Lookup.ContainsKey(value);

    public static readonly Gender Female = new((AsciiString)"female", true);

    public static readonly Gender Male = new((AsciiString)"male", true);

    public static readonly Gender Other = new((AsciiString)"other", true);

    public static readonly IReadOnlyCollection<Gender> All = new[]
    {
        Female,
        Male,
        Other
    };

    public static readonly IReadOnlyDictionary<AsciiString, Gender> Lookup = All.ToDictionary(r => r._value);
}
