// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;

namespace DSE.Open.Language.Annotations;

public static class UniversalFeatureValues
{
    /// <summary>
    /// <c>Prs</c>: personal or possessive personal pronoun or determiner.
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/PronType.html"><c>PronType</c>: pronominal type</see>
    /// </remarks>
    public static readonly AlphaNumericCode PronominalPersonal = new("Prs");

    /// <summary>
    /// <c>Rcp</c>: reciprocal pronoun
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/PronType.html"><c>PronType</c>: pronominal type</see>
    /// </remarks>
    public static readonly AlphaNumericCode PronominalRecipricol = new("Rcp");

    /// <summary>
    /// Art: article
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/PronType.html"><c>PronType</c>: pronominal type</see>
    /// </remarks>
    public static readonly AlphaNumericCode PronominalArticle = new("Art");

    /// <summary>
    /// <c>Int</c>: interrogative pronoun, determiner, numeral or adverb
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/PronType.html"><c>PronType</c>: pronominal type</see>
    /// </remarks>
    public static readonly AlphaNumericCode PronominalInterrogative = new("Int");

    /// <summary>
    /// <c>Rel</c>: relative pronoun, determiner, numeral or adverb
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/PronType.html"><c>PronType</c>: pronominal type</see>
    /// </remarks>
    public static readonly AlphaNumericCode PronominalRelative = new("Rel");

    /// <summary>
    /// <c>Exc</c>: exclamative determiner
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/PronType.html"><c>PronType</c>: pronominal type</see>
    /// </remarks>
    public static readonly AlphaNumericCode PronominalExclamative = new("Exc");

    /// <summary>
    /// <c>Dem</c>: demonstrative pronoun, determiner, numeral or adverb
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/PronType.html"><c>PronType</c>: pronominal type</see>
    /// </remarks>
    public static readonly AlphaNumericCode PronominalDemonstrative = new("Dem");

    /// <summary>
    /// <c>Emp</c>: emphatic determiner
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/PronType.html"><c>PronType</c>: pronominal type</see>
    /// </remarks>
    public static readonly AlphaNumericCode PronominalEmphatic = new("Emp");

    /// <summary>
    /// <c>Tot</c>: total (collective) pronoun, determiner or adverb
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/PronType.html"><c>PronType</c>: pronominal type</see>
    /// </remarks>
    public static readonly AlphaNumericCode PronominalTotal = new("Tot");

    /// <summary>
    /// <c>Neg</c>: negative pronoun, determiner or adverb
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/PronType.html"><c>PronType</c>: pronominal type</see>
    /// </remarks>
    public static readonly AlphaNumericCode PronominalNegative = new("Neg");

    /// <summary>
    /// <c>Ind</c>: indefinite pronoun, determiner, numeral or adverb
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/PronType.html"><c>PronType</c>: pronominal type</see>
    /// </remarks>
    public static readonly AlphaNumericCode PronominalIndefinite = new("Ind");

    /// <summary>
    /// <c>Finite</c>
    /// </summary>
    public static readonly AlphaNumericCode VerbFormFinite = new("Finite");

    /// <summary>
    /// <c>Infinitive</c>
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/VerbForm.html">
    /// <c>VerbForm</c>: form of verb or deverbative</see>
    /// </remarks>
    public static readonly AlphaNumericCode VerbFormInfinitive = new("Infinitive");

    /// <summary>
    /// <c>Part</c>
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/VerbForm.html">
    /// <c>VerbForm</c>: form of verb or deverbative</see>
    /// </remarks>
    public static readonly AlphaNumericCode VerbFormParticiple = new("Part");

    /// <summary>
    /// <c>Conv</c>
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/VerbForm.html">
    /// <c>VerbForm</c>: form of verb or deverbative</see>
    /// </remarks>
    public static readonly AlphaNumericCode VerbFormConverb = new("Conv");

    /// <summary>
    /// <c>Past</c>: past tense / preterite / aorist
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/Tense.html"><c>Tense</c>: tense</see>
    /// </remarks>
    public static readonly AlphaNumericCode TensePast = new("Past");

    /// <summary>
    /// <c>Pres</c>: present / non-past tense / aorist
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/Tense.html"><c>Tense</c>: tense</see>
    /// </remarks>
    public static readonly AlphaNumericCode TensePresent = new("Pres");

    /// <summary>
    /// <c>Fut</c>: future tense
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/Tense.html"><c>Tense</c>: tense</see>
    /// </remarks>
    public static readonly AlphaNumericCode TenseFuture = new("Pres");

    /// <summary>
    /// <c>Imp</c>: imperfect
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/Tense.html"><c>Tense</c>: tense</see>
    /// </remarks>
    public static readonly AlphaNumericCode TenseImperfect = new("Imp");

    /// <summary>
    /// <c>Pqp</c>: pluperfect
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/Tense.html"><c>Tense</c>: tense</see>
    /// </remarks>
    public static readonly AlphaNumericCode TensePluperfect = new("Pqp");

    /// <summary>
    /// <c>Sing</c>: singular number
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/Number.html"><c>Number</c>: number</see>
    /// </remarks>
    public static readonly AlphaNumericCode NumberSingular = new("Sing");

    /// <summary>
    /// <c>Plur</c>: plural number
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/Number.html"><c>Number</c>: number</see>
    /// </remarks>
    public static readonly AlphaNumericCode NumberPlural = new("Plur");

    /// <summary>
    /// <c>Dual</c>: dual number
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/Number.html"><c>Number</c>: number</see>
    /// </remarks>
    public static readonly AlphaNumericCode NumberDual = new("Dual");

    /// <summary>
    /// <c>Tri</c>: trial number
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/Number.html"><c>Number</c>: number</see>
    /// </remarks>
    public static readonly AlphaNumericCode NumberTrial = new("Tri");

    /// <summary>
    /// <c>Card</c>: cardinal number or corresponding interrogative / relative / indefinite / demonstrative word
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/NumType.html"><c>NumType</c>: numeral type</see>
    /// </remarks>
    public static readonly AlphaNumericCode NumeralTypeCardinal = new("Card");

    /// <summary>
    /// <c>Ord</c>: ordinal number or corresponding interrogative / relative / indefinite / demonstrative word
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/NumType.html"><c>NumType</c>: numeral type</see>
    /// </remarks>
    public static readonly AlphaNumericCode NumeralTypeOrdinal = new("Ord");

    /// <summary>
    /// <c>Mult</c>: multiplicative numeral or corresponding interrogative / relative / indefinite / demonstrative word
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/NumType.html"><c>NumType</c>: numeral type</see>
    /// </remarks>
    public static readonly AlphaNumericCode NumeralTypeMultiplicative = new("Mult");

    /// <summary>
    /// <c>Frac</c>: fraction
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/NumType.html"><c>NumType</c>: numeral type</see>
    /// </remarks>
    public static readonly AlphaNumericCode NumeralTypeFraction = new("Frac");

    /// <summary>
    /// <c>Yes</c>: it is possessive
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/Poss.html"><c>Poss</c>: possessive</see>
    /// </remarks>
    public static readonly AlphaNumericCode PossessiveYes = new("Yes");

    /// <summary>
    /// <c>0</c>: zero person
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/Person.html"><c>Person</c>: person</see>
    /// </remarks>
    public static readonly AlphaNumericCode PersonZero = new("0");

    /// <summary>
    /// 1: first person
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/Person.html"><c>Person</c>: person</see>
    /// </remarks>
    public static readonly AlphaNumericCode PersonFirst = new("1");

    /// <summary>
    /// 2: second person
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/Person.html"><c>Person</c>: person</see>
    /// </remarks>
    public static readonly AlphaNumericCode PersonSecond = new("2");

    /// <summary>
    /// 3: third person
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/Person.html"><c>Person</c>: person</see>
    /// </remarks>
    public static readonly AlphaNumericCode PersonThird = new("3");

    /// <summary>
    /// 4: fourth person
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/Person.html"><c>Person</c>: person</see>
    /// </remarks>
    public static readonly AlphaNumericCode PersonFourth = new("4");

    /// <summary>
    /// <c>Pos</c>: positive, first degree
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/Degree.html"><c>Degree</c>: degree</see>
    /// </remarks>
    public static readonly AlphaNumericCode DegreePositive = new("Pos");

    /// <summary>
    /// <c>Equ</c>: equative
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/Degree.html"><c>Degree</c>: degree</see>
    /// </remarks>
    public static readonly AlphaNumericCode DegreeEquative = new("Equ");

    /// <summary>
    /// <c>Cmp</c>: comparative, second degree
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/Degree.html"><c>Degree</c>: degree</see>
    /// </remarks>
    public static readonly AlphaNumericCode DegreeComparative = new("Cmp");

    /// <summary>
    /// <c>Sup</c>: superlative, third degree
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/Degree.html"><c>Degree</c>: degree</see>
    /// </remarks>
    public static readonly AlphaNumericCode DegreeSuperlative = new("Sup");

    /// <summary>
    /// <c>Nom</c>: nominative / direct
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/Case.html"><c>Case</c>: case</see>
    /// </remarks>
    public static readonly AlphaNumericCode CaseNominative = new("Nom");

    /// <summary>
    /// <c>Acc</c>: accusative / oblique
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/Case.html"><c>Case</c>: case</see>
    /// </remarks>
    public static readonly AlphaNumericCode CaseAccusative = new("Acc");

    /// <summary>
    /// <c>Abs</c>: absolutive
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/Case.html"><c>Case</c>: case</see>
    /// </remarks>
    public static readonly AlphaNumericCode CaseAbsolutive = new("Abs");

    /// <summary>
    /// <c>Ind</c>: indicative or realis
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/Mood.html"><c>Mood</c>: mood</see>
    /// </remarks>
    public static readonly AlphaNumericCode MoodIndicative = new("Ind");

    /// <summary>
    /// <c>Imp</c>: imperative
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/Mood.html"><c>Mood</c>: mood</see>
    /// </remarks>
    public static readonly AlphaNumericCode MoodImperative = new("Ind");

    /// <summary>
    /// <c>Cnd</c>: conditional
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/Mood.html"><c>Mood</c>: mood</see>
    /// </remarks>
    public static readonly AlphaNumericCode MoodConditional = new("Cnd");

    /// <summary>
    /// Pot: potential
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/Mood.html"><c>Mood</c>: mood</see>
    /// </remarks>
    public static readonly AlphaNumericCode MoodPotential = new("Cnd");

    /// <summary>
    /// Sub: subjunctive / conjunctive
    /// </summary>
    /// <remarks>
    /// See <see href="https://universaldependencies.org/u/feat/Mood.html"><c>Mood</c>: mood</see>
    /// </remarks>
    public static readonly AlphaNumericCode MoodSubjective = new("Sub");

}
