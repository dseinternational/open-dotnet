// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Language;

[EquatableValue]
[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<SentenceStructure, AsciiString>))]
public readonly partial struct SentenceStructure
    : IEquatableValue<SentenceStructure, AsciiString>,
      IUtf8SpanSerializable<SentenceStructure>
{
    public static int MaxSerializedCharLength => 32;

    public static int MaxSerializedByteLength => 32;

    public SentenceStructure(string value) : this((AsciiString)value)
    {
    }

    private SentenceStructure(string value, bool skipValidation) : this((AsciiString)value, skipValidation)
    {
    }

    public static bool IsValidValue(AsciiString value)
    {
        return !value.IsEmpty
            && value.Length <= MaxSerializedCharLength
            && s_validValues.Contains(value);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates - Parse
    public static explicit operator SentenceStructure(string value)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return new(value);
    }

    public static readonly SentenceStructure Simple = new("simple", true);

    public static readonly SentenceStructure Compound = new("compound", true);

    public static readonly SentenceStructure Complex = new("complex", true);

    public static readonly SentenceStructure CompoundComplex = new("compound-complex", true);

    public static readonly FrozenSet<SentenceStructure> All = FrozenSet.ToFrozenSet(
    [
        Simple,
        Compound,
        Complex,
        CompoundComplex,
    ]);

    private static readonly FrozenSet<AsciiString> s_validValues =
        FrozenSet.ToFrozenSet(All.Select(x => x._value));
}
