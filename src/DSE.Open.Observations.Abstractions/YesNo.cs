// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Observations;

/// <summary>
/// Represents the selection of a choice between "Yes" and "No".
/// </summary>
[EquatableValue]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<YesNo, AsciiString>))]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct YesNo : IEquatableValue<YesNo, AsciiString>
{
    public static int MaxSerializedCharLength => 3;

    public static bool IsValidValue(AsciiString value)
    {
        return value == Yes._value || value == No._value;
    }   

    public static readonly YesNo Yes = new((AsciiString)"yes", true);

    public static readonly YesNo No = new((AsciiString)"no", true);
}
