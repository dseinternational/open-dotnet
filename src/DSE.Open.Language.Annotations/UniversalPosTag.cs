// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Language.Annotations;

/// <summary>
/// <see href="https://universaldependencies.org/u/pos/">Universal POS tags</see> mark the core
/// part-of-speech categories.
/// </summary>
[EquatableValue]
[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<UniversalPosTag, AsciiString>))]
public readonly partial struct UniversalPosTag
    : IEquatableValue<UniversalPosTag, AsciiString>,
      IUtf8SpanSerializable<UniversalPosTag>,
      IRepeatableHash64
{
    /// <summary>
    /// The maximum number of characters required to format a value as a string.
    /// </summary>
    public static int MaxSerializedCharLength => 5;

    /// <summary>
    /// The maximum number of bytes required to format a value as UTF-8.
    /// </summary>
    public static int MaxSerializedByteLength => 5;

    /// <summary>
    /// Initializes a new <see cref="UniversalPosTag"/> from the specified value.
    /// </summary>
    public UniversalPosTag(AsciiString value) : this(value, false)
    {
    }

    /// <summary>
    /// Initializes a new <see cref="UniversalPosTag"/> from the specified value.
    /// </summary>
    public UniversalPosTag(string value) : this((AsciiString)value)
    {
    }

    private UniversalPosTag(string value, bool skipValidation) : this((AsciiString)value, skipValidation)
    {
    }

    /// <summary>
    /// Gets the length, in characters, of the tag value.
    /// </summary>
    public int Length => _value.Length;

    /// <summary>
    /// Gets the underlying tag value.
    /// </summary>
    public AsciiString Value => _value;

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> is a valid universal POS tag.
    /// </summary>
    public static bool IsValidValue(AsciiString value)
    {
        return !value.IsEmpty
            && value.Length <= MaxSerializedCharLength
            && s_validValues.Contains(value);
    }

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates - Parse
    /// <summary>
    /// Explicitly converts a string value to a <see cref="UniversalPosTag"/>.
    /// </summary>
    public static explicit operator UniversalPosTag(string value)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return new(value);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates
    /// <summary>
    /// Implicitly converts a <see cref="UniversalPosTag"/> to a <see cref="PosTag"/>.
    /// </summary>
    public static implicit operator PosTag(UniversalPosTag value)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return new(value._value);
    }

    /// <summary><c>ADJ</c>: adjective.</summary>
    public static readonly UniversalPosTag Adjective = new("ADJ", true);

    /// <summary><c>ADP</c>: adposition.</summary>
    public static readonly UniversalPosTag Adposition = new("ADP", true);

    /// <summary><c>ADV</c>: adverb.</summary>
    public static readonly UniversalPosTag Adverb = new("ADV", true);

    /// <summary><c>AUX</c>: auxiliary.</summary>
    public static readonly UniversalPosTag Auxiliary = new("AUX", true);

    /// <summary><c>CCONJ</c>: coordinating conjunction.</summary>
    public static readonly UniversalPosTag CoordinatingConjunction = new("CCONJ", true);

    /// <summary><c>DET</c>: determiner.</summary>
    public static readonly UniversalPosTag Determiner = new("DET", true);

    /// <summary><c>INTJ</c>: interjection.</summary>
    public static readonly UniversalPosTag Interjection = new("INTJ", true);

    /// <summary><c>NOUN</c>: noun.</summary>
    public static readonly UniversalPosTag Noun = new("NOUN", true);

    /// <summary><c>NUM</c>: numeral.</summary>
    public static readonly UniversalPosTag Numeral = new("NUM", true);

    /// <summary><c>PART</c>: particle.</summary>
    public static readonly UniversalPosTag Particle = new("PART", true);

    /// <summary><c>PRON</c>: pronoun.</summary>
    public static readonly UniversalPosTag Pronoun = new("PRON", true);

    /// <summary><c>PROPN</c>: proper noun.</summary>
    public static readonly UniversalPosTag ProperNoun = new("PROPN", true);

    /// <summary><c>PUNCT</c>: punctuation.</summary>
    public static readonly UniversalPosTag Punctuation = new("PUNCT", true);

    /// <summary><c>SCONJ</c>: subordinating conjunction.</summary>
    public static readonly UniversalPosTag SubordinatingConjunction = new("SCONJ", true);

    /// <summary><c>SYM</c>: symbol.</summary>
    public static readonly UniversalPosTag Symbol = new("SYM", true);

    /// <summary><c>VERB</c>: verb.</summary>
    public static readonly UniversalPosTag Verb = new("VERB", true);

    /// <summary><c>X</c>: other.</summary>
    public static readonly UniversalPosTag Other = new("X", true);

    /// <summary>
    /// The set of all defined universal POS tags.
    /// </summary>
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
