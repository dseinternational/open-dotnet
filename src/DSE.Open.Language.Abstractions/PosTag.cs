// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Language;

/// <summary>
/// Unconstrained POS tag (placeholder for others).
/// </summary>
[EquatableValue]
[StructLayout(LayoutKind.Auto)]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<PosTag, AsciiString>))]
public readonly partial struct PosTag
    : IEquatableValue<PosTag, AsciiString>,
      IUtf8SpanSerializable<PosTag>
{
    public static int MaxSerializedCharLength => 16;

    public static int MaxSerializedByteLength => 16;

    public PosTag(AsciiString value) : this(value, false)
    {
    }

    public PosTag(string value) : this((AsciiString)value)
    {
    }

    private PosTag(string value, bool skipValidation) : this((AsciiString)value, skipValidation)
    {
    }

    public AsciiString Value => _value;

    public bool IsValidUniversalPosTag => UniversalPosTag.IsValidValue(_value);

    public bool IsValidTreebankTag => TreebankPosTag.IsValidValue(_value);

    public static bool IsValidValue(AsciiString value)
    {
        return !value.IsEmpty
            && value.Length <= MaxSerializedCharLength;
    }

    public UniversalPosTag ToUniversalPosTag()
    {
        return new UniversalPosTag(_value);
    }

    public TreebankPosTag ToTreebankPosTag()
    {
        return new TreebankPosTag(_value);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates - Parse
    public static explicit operator PosTag(string value)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return new PosTag(value);
    }
}
