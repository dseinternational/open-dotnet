// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Language.Annotations;

/// <summary>
/// <see href="https://universaldependencies.org/u/pos/">Universal POS tags</see> mark the core
/// part-of-speech categories.
/// </summary>
[EquatableValue]
[StructLayout(LayoutKind.Auto)]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<UniversalPosTag, AsciiString>))]
public readonly partial struct UniversalPosTag : IEquatableValue<UniversalPosTag, AsciiString>, IUtf8SpanSerializable<UniversalPosTag>
{
    public static int MaxSerializedCharLength => 5;

    public static int MaxSerializedByteLength => 5;

    public UniversalPosTag(AsciiString value) : this(value, false)
    {
    }

    public UniversalPosTag(string value) : this((AsciiString)value)
    {
    }

    private UniversalPosTag(string value, bool skipValidation) : this((AsciiString)value, skipValidation)
    {
    }

    public int Length => _value.Length;

    public AsciiString Value => _value;

    public static bool IsValidValue(AsciiString value)
    {
        return !value.IsEmpty
            && value.Length <= MaxSerializedCharLength
            && s_validValues.Contains(value);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates - Parse
    public static explicit operator UniversalPosTag(string value)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return new UniversalPosTag(value);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates
    public static implicit operator PosTag(UniversalPosTag value)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return new PosTag(value._value);
    }

    public static readonly UniversalPosTag Adjective = new("ADJ", true);

    public static readonly UniversalPosTag Adposition = new("ADP", true);

    public static readonly UniversalPosTag Adverb = new("ADV", true);

    public static readonly UniversalPosTag Auxiliary = new("AUX", true);

    public static readonly UniversalPosTag CoordinatingConjunction = new("CCONJ", true);

    public static readonly UniversalPosTag Determiner = new("DET", true);

    public static readonly UniversalPosTag Interjection = new("INTJ", true);

    public static readonly UniversalPosTag Noun = new("NOUN", true);

    public static readonly UniversalPosTag Numeral = new("NUM", true);

    public static readonly UniversalPosTag Particle = new("PART", true);

    public static readonly UniversalPosTag Pronoun = new("PRON", true);

    public static readonly UniversalPosTag ProperNoun = new("PROPN", true);

    public static readonly UniversalPosTag Punctuation = new("PUNCT", true);

    public static readonly UniversalPosTag SubordinatingConjunction = new("SCONJ", true);

    public static readonly UniversalPosTag Symbol = new("SYM", true);

    public static readonly UniversalPosTag Verb = new("VERB", true);

    public static readonly UniversalPosTag Other = new("X", true);

    public static readonly FrozenSet<UniversalPosTag> All = FrozenSet.ToFrozenSet(
    [
        Adjective,
        Adposition,
        Adverb,
        Auxiliary,
        CoordinatingConjunction,
        Determiner,
        Interjection,
        Noun,
        Numeral,
        Particle,
        Pronoun,
        ProperNoun,
        Punctuation,
        SubordinatingConjunction,
        Symbol,
        Verb,
        Other
    ]);

    private static readonly FrozenSet<AsciiString> s_validValues = FrozenSet.ToFrozenSet(All.Select(x => x._value));
}
