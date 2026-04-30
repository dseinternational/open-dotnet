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
/// Names of universal features (<see href="https://universaldependencies.org/u/feat/index.html"/>).
/// </summary>
/// <remarks>
/// See also <see cref="UniversalFeatureValues"/>.
/// </remarks>
[EquatableValue]
[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<UniversalFeatureName, AsciiString>))]
public readonly partial struct UniversalFeatureName
    : IEquatableValue<UniversalFeatureName, AsciiString>,
      IUtf8SpanSerializable<UniversalFeatureName>,
      IRepeatableHash64
{
    /// <summary>
    /// The maximum number of characters required to format a value as a string.
    /// </summary>
    public static int MaxSerializedCharLength => 9;

    /// <summary>
    /// The maximum number of bytes required to format a value as UTF-8.
    /// </summary>
    public static int MaxSerializedByteLength => 9;

    /// <summary>
    /// Initializes a new <see cref="UniversalFeatureName"/> from the specified value.
    /// </summary>
    public UniversalFeatureName(string value) : this((AsciiString)value)
    {
    }

    private UniversalFeatureName(string value, bool skipValidation) : this((AsciiString)value, skipValidation)
    {
    }

    /// <summary>
    /// Gets the underlying feature name value.
    /// </summary>
    public AsciiString Value => _value;

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> is a valid universal feature name.
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
    /// Explicitly converts a string value to a <see cref="UniversalFeatureName"/>.
    /// </summary>
    public static explicit operator UniversalFeatureName(string value)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return new(value);
    }

    /// <summary>
    /// Creates a <see cref="UniversalFeatureName"/> from an <see cref="AlphaNumericCode"/>.
    /// </summary>
    public static UniversalFeatureName FromAlphaNumericCode(AlphaNumericCode value)
    {
        return new(value);
    }

    /// <summary>
    /// Explicitly converts an <see cref="AlphaNumericCode"/> to a <see cref="UniversalFeatureName"/>.
    /// </summary>
    public static explicit operator UniversalFeatureName(AlphaNumericCode value)
    {
        return FromAlphaNumericCode(value);
    }

    /// <summary>
    /// Returns the value as an <see cref="AlphaNumericCode"/>.
    /// </summary>
    public AlphaNumericCode ToAlphaNumericCode()
    {
        return new AlphaNumericCode(this);
    }

    /// <summary>
    /// Implicitly converts a <see cref="UniversalFeatureName"/> to an <see cref="AlphaNumericCode"/>.
    /// </summary>
    public static implicit operator AlphaNumericCode(UniversalFeatureName value)
    {
        return value.ToAlphaNumericCode();
    }

    /// <summary><c>Abbr</c>: abbreviation.</summary>
    public static readonly UniversalFeatureName Abbr = new("Abbr", true);

    /// <summary><c>Animacy</c>: animacy.</summary>
    public static readonly UniversalFeatureName Animacy = new("Animacy", true);

    /// <summary><c>Aspect</c>: aspect.</summary>
    public static readonly UniversalFeatureName Aspect = new("Aspect", true);

    /// <summary><c>Case</c>: case.</summary>
    public static readonly UniversalFeatureName Case = new("Case", true);

    /// <summary><c>Clusivity</c>: clusivity.</summary>
    public static readonly UniversalFeatureName Clusivity = new("Clusivity", true);

    /// <summary><c>Definite</c>: definiteness or state.</summary>
    public static readonly UniversalFeatureName Definite = new("Definite", true);

    /// <summary><c>Degree</c>: degree of comparison.</summary>
    public static readonly UniversalFeatureName Degree = new("Degree", true);

    /// <summary><c>Deixis</c>: deixis.</summary>
    public static readonly UniversalFeatureName Deixis = new("Deixis", true);

    /// <summary><c>DeixisRef</c>: deictic reference (relative to a reference point).</summary>
    public static readonly UniversalFeatureName DeixisRef = new("DeixisRef", true);

    /// <summary><c>Evident</c>: evidentiality.</summary>
    public static readonly UniversalFeatureName Evident = new("Evident", true);

    /// <summary><c>Foreign</c>: marks a foreign word.</summary>
    public static readonly UniversalFeatureName Foreign = new("Foreign", true);

    /// <summary><c>Gender</c>: gender.</summary>
    public static readonly UniversalFeatureName Gender = new("Gender", true);

    /// <summary><c>Mood</c>: mood.</summary>
    public static readonly UniversalFeatureName Mood = new("Mood", true);

    /// <summary><c>NounClass</c>: noun class.</summary>
    public static readonly UniversalFeatureName NounClass = new("NounClass", true);

    /// <summary><c>Number</c>: number.</summary>
    public static readonly UniversalFeatureName Number = new("Number", true);

    /// <summary><c>NumType</c>: numeral type.</summary>
    public static readonly UniversalFeatureName NumType = new("NumType", true);

    /// <summary><c>Person</c>: person.</summary>
    public static readonly UniversalFeatureName Person = new("Person", true);

    /// <summary><c>Polarity</c>: polarity.</summary>
    public static readonly UniversalFeatureName Polarity = new("Polarity", true);

    /// <summary><c>Polite</c>: politeness.</summary>
    public static readonly UniversalFeatureName Polite = new("Polite", true);

    /// <summary><c>Poss</c>: possessive.</summary>
    public static readonly UniversalFeatureName Poss = new("Poss", true);

    /// <summary><c>PronType</c>: pronominal type.</summary>
    public static readonly UniversalFeatureName PronType = new("PronType", true);

    /// <summary><c>Reflex</c>: reflexive.</summary>
    public static readonly UniversalFeatureName Reflex = new("Reflex", true);

    /// <summary><c>Tense</c>: tense.</summary>
    public static readonly UniversalFeatureName Tense = new("Tense", true);

    /// <summary><c>Typo</c>: marks a typographical error.</summary>
    public static readonly UniversalFeatureName Typo = new("Typo", true);

    /// <summary><c>VerbForm</c>: form of verb or deverbative.</summary>
    public static readonly UniversalFeatureName VerbForm = new("VerbForm", true);

    /// <summary><c>Voice</c>: voice.</summary>
    public static readonly UniversalFeatureName Voice = new("Voice", true);

    /// <summary>
    /// The set of all defined universal feature names.
    /// </summary>
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
