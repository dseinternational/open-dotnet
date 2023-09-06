// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Language;

/// <summary>
/// <see href="https://universaldependencies.org/u/pos/">Universal POS tags</see> mark the core 
/// part-of-speech categories. 
/// </summary>
[EquatableValue]
[StructLayout(LayoutKind.Auto)]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<UniversalPosTag, AsciiString>))]
public readonly partial struct UniversalPosTag : IEquatableValue<UniversalPosTag, AsciiString>
{
    public static int MaxSerializedCharLength => 5;

    public UniversalPosTag(string value) : this((AsciiString)value)
    {
    }

    public static bool IsValidValue(AsciiString value)
    {
        return !value.IsEmpty || value.Length <= MaxSerializedCharLength;
    }

#pragma warning disable CA2225 // Operator overloads have named alternates - Parse
    public static explicit operator UniversalPosTag(string value)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return new UniversalPosTag(value);
    }


    public static readonly UniversalPosTag Adjective = (UniversalPosTag)"ADJ";

    public static readonly UniversalPosTag Adposition = (UniversalPosTag)"ADP";

    public static readonly UniversalPosTag Adverb = (UniversalPosTag)"ADV";

    public static readonly UniversalPosTag Auxiliary = (UniversalPosTag)"AUX";

    public static readonly UniversalPosTag CoordinatingConjunction = (UniversalPosTag)"CCONJ";

    public static readonly UniversalPosTag Determiner = (UniversalPosTag)"DET";

    public static readonly UniversalPosTag Interjection = (UniversalPosTag)"INTJ";

    public static readonly UniversalPosTag Noun = (UniversalPosTag)"NOUN";

    public static readonly UniversalPosTag Numeral = (UniversalPosTag)"NUM";

    public static readonly UniversalPosTag Particle = (UniversalPosTag)"PART";

    public static readonly UniversalPosTag Pronoun = (UniversalPosTag)"PRON";

    public static readonly UniversalPosTag ProperNoun = (UniversalPosTag)"PROPN";

    public static readonly UniversalPosTag Punctuation = (UniversalPosTag)"PUNCT";

    public static readonly UniversalPosTag SubordinatingConjunction = (UniversalPosTag)"SCONJ";

    public static readonly UniversalPosTag Symbol = (UniversalPosTag)"SYM";

    public static readonly UniversalPosTag Verb = (UniversalPosTag)"VERB";

    public static readonly UniversalPosTag Other = (UniversalPosTag)"X";

}

