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
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<SentenceFunction, AsciiString>))]
public readonly partial struct SentenceFunction
    : IEquatableValue<SentenceFunction, AsciiString>,
      IUtf8SpanSerializable<SentenceFunction>
{
    public static int MaxSerializedCharLength => 32;

    public static int MaxSerializedByteLength => 32;

    public SentenceFunction(string value) : this((AsciiString)value)
    {
    }

    private SentenceFunction(string value, bool skipValidation) : this((AsciiString)value, skipValidation)
    {
    }

    public static bool IsValidValue(AsciiString value)
    {
        return !value.IsEmpty
            && value.Length <= MaxSerializedCharLength
            && s_validValues.Contains(value);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates - Parse
    public static explicit operator SentenceFunction(string value)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return new(value);
    }

    public static readonly SentenceFunction Declarative = new("declarative", true);

    public static readonly SentenceFunction Interrogative = new("interrogative", true);

    public static readonly SentenceFunction Imperative = new("imperative", true);

    public static readonly SentenceFunction Exclamatory = new("exclamatory", true);

    public static readonly FrozenSet<SentenceFunction> All = FrozenSet.ToFrozenSet(
    [
        Declarative,
        Interrogative,
        Imperative,
        Exclamatory,
    ]);

    private static readonly FrozenSet<AsciiString> s_validValues =
        FrozenSet.ToFrozenSet(All.Select(x => x._value));
}
