// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Language;

/// <summary>
/// Names of universal features (<see href="https://universaldependencies.org/u/feat/index.html"/>).
/// </summary>
[EquatableValue]
[StructLayout(LayoutKind.Auto)]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<UniversalFeatureName, AsciiString>))]
public readonly partial struct UniversalFeatureName : IEquatableValue<UniversalFeatureName, AsciiString>, IUtf8SpanSerializable<UniversalFeatureName>
{
    public static int MaxSerializedCharLength => 9;

    public static int MaxSerializedByteLength => 9;

    public UniversalFeatureName(string value) : this((AsciiString)value)
    {
    }

    private UniversalFeatureName(string value, bool skipValidation) : this((AsciiString)value, skipValidation)
    {
    }

    public AsciiString Value => _value;

    public static bool IsValidValue(AsciiString value)
    {
        return !value.IsEmpty
            && value.Length <= MaxSerializedCharLength
            && s_validValues.Contains(value);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates - Parse
    public static explicit operator UniversalFeatureName(string value)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return new UniversalFeatureName(value);
    }

    public static readonly UniversalFeatureName Abbr = new("Abbr", true);

    public static readonly UniversalFeatureName Animacy = new("Animacy", true);

    public static readonly UniversalFeatureName Aspect = new("Aspect", true);

    public static readonly UniversalFeatureName Case = new("Case", true);

    public static readonly UniversalFeatureName Clusivity = new("Clusivity", true);

    public static readonly UniversalFeatureName Definite = new("Definite", true);

    public static readonly UniversalFeatureName Degree = new("Degree", true);

    public static readonly UniversalFeatureName Deixis = new("Deixis", true);

    public static readonly UniversalFeatureName DeixisRef = new("DeixisRef", true);

    public static readonly UniversalFeatureName Evident = new("Evident", true);

    public static readonly UniversalFeatureName Foreign = new("Foreign", true);

    public static readonly UniversalFeatureName Gender = new("Gender", true);

    public static readonly UniversalFeatureName Mood = new("Mood", true);

    public static readonly UniversalFeatureName NounClass = new("NounClass", true);

    public static readonly UniversalFeatureName Number = new("Number", true);

    public static readonly UniversalFeatureName NumType = new("NumType", true);

    public static readonly UniversalFeatureName Person = new("Person", true);

    public static readonly UniversalFeatureName Polarity = new("Polarity", true);

    public static readonly UniversalFeatureName Polite = new("Polite", true);

    public static readonly UniversalFeatureName Poss = new("Poss", true);

    public static readonly UniversalFeatureName PronType = new("PronType", true);

    public static readonly UniversalFeatureName Reflex = new("Reflex", true);

    public static readonly UniversalFeatureName Tense = new("Tense", true);

    public static readonly UniversalFeatureName Typo = new("Typo", true);

    public static readonly UniversalFeatureName VerbForm = new("VerbForm", true);

    public static readonly UniversalFeatureName Voice = new("Voice", true);

    public static readonly FrozenSet<UniversalFeatureName> All = FrozenSet.ToFrozenSet(
    [
        Abbr,
        Animacy,
        Aspect,
        Case,
        Clusivity,
        Definite,
        Degree,
        Deixis,
        DeixisRef,
        Evident,
        Foreign,
        Gender,
        Mood,
        NounClass,
        Number,
        NumType,
        Person,
        Polarity,
        Polite,
        Poss,
        PronType,
        Reflex,
        Tense,
        Typo,
        VerbForm,
        Voice,
    ]);

    private static readonly FrozenSet<AsciiString> s_validValues =
        FrozenSet.ToFrozenSet(All.Select(x => x._value));
}
