// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Language.Annotations;

// https://www.ling.upenn.edu/courses/Fall_2003/ling001/penn_treebank_pos.html

/// <summary>
/// A part-of-speech tag from the Penn Treebank tag set
/// (<see href="https://www.ling.upenn.edu/courses/Fall_2003/ling001/penn_treebank_pos.html"/>).
/// </summary>
[EquatableValue]
[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<TreebankPosTag, AsciiString>))]
public readonly partial struct TreebankPosTag
    : IEquatableValue<TreebankPosTag, AsciiString>,
      IUtf8SpanSerializable<TreebankPosTag>,
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
    /// Initializes a new <see cref="TreebankPosTag"/> from the specified value.
    /// </summary>
    public TreebankPosTag(AsciiString value) : this(value, false)
    {
    }

    /// <summary>
    /// Initializes a new <see cref="TreebankPosTag"/> from the specified value.
    /// </summary>
    public TreebankPosTag(string value) : this((AsciiString)value)
    {
    }

    private TreebankPosTag(string value, bool skipValidation) : this((AsciiString)value, skipValidation)
    {
    }

    /// <summary>
    /// Gets the underlying tag value.
    /// </summary>
    public AsciiString Value => _value;

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> is a valid Penn Treebank POS tag.
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
    /// Explicitly converts a string value to a <see cref="TreebankPosTag"/>.
    /// </summary>
    public static explicit operator TreebankPosTag(string value)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return new(value);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates
    /// <summary>
    /// Implicitly converts a <see cref="TreebankPosTag"/> to a <see cref="PosTag"/>.
    /// </summary>
    public static implicit operator PosTag(TreebankPosTag value)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return new(value._value);
    }

    /// <summary><c>""</c>: closing quotation mark.</summary>
    public static readonly TreebankPosTag ClosingQuotationMark = new("\"\"", true);

    /// <summary><c>#</c>: symbol, number sign.</summary>
    public static readonly TreebankPosTag SymbolNumberSign = new("#", true);

    /// <summary><c>$</c>: symbol, currency.</summary>
    public static readonly TreebankPosTag SymbolCurrency = new("$", true);

    /// <summary><c>''</c>: closing quotation mark.</summary>
    public static readonly TreebankPosTag ClosingQuotationMark2 = new("''", true);

    /// <summary><c>,</c>: punctuation mark, comma.</summary>
    public static readonly TreebankPosTag PunctuationMarkComma = new(",", true);

    /// <summary><c>-LRB-</c>: left round bracket.</summary>
    public static readonly TreebankPosTag LeftRoundBracket = new("-LRB-", true);

    /// <summary><c>-RRB-</c>: right round bracket.</summary>
    public static readonly TreebankPosTag RightRoundBracket = new("-RRB-", true);

    /// <summary><c>.</c>: punctuation mark, sentence closer.</summary>
    public static readonly TreebankPosTag PunctuationMarkSentenceCloser = new(".", true);

    /// <summary><c>:</c>: punctuation mark, colon or ellipsis.</summary>
    public static readonly TreebankPosTag PunctuationMarkColonOrEllipsis = new(":", true);

    /// <summary><c>ADD</c>: email address.</summary>
    public static readonly TreebankPosTag Email = new("ADD", true);

    /// <summary><c>AFX</c>: affix.</summary>
    public static readonly TreebankPosTag Affix = new("AFX", true);

    /// <summary><c>BES</c>: auxiliary "be".</summary>
    public static readonly TreebankPosTag AuxiliaryBe = new("BES", true);

    /// <summary><c>CC</c>: conjunction, coordinating.</summary>
    public static readonly TreebankPosTag ConjunctionCoordinating = new("CC", true);

    /// <summary><c>CD</c>: cardinal number.</summary>
    public static readonly TreebankPosTag CardinalNumber = new("CD", true);

    /// <summary><c>DT</c>: determiner.</summary>
    public static readonly TreebankPosTag Determiner = new("DT", true);

    /// <summary><c>EX</c>: existential "there".</summary>
    public static readonly TreebankPosTag ExistentialThere = new("EX", true);

    /// <summary><c>FW</c>: foreign word.</summary>
    public static readonly TreebankPosTag ForeignWord = new("FW", true);

    /// <summary><c>GW</c>: additional word in multi-word expression.</summary>
    public static readonly TreebankPosTag AdditionalWordInMultiwordExpression = new("GW", true);

    /// <summary><c>HVS</c>: forms of "have".</summary>
    public static readonly TreebankPosTag FormsOfHave = new("HVS", true);

    /// <summary><c>HYPH</c>: punctuation mark, hyphen.</summary>
    public static readonly TreebankPosTag PunctuationMarkHyphen = new("HYPH", true);

    /// <summary><c>IN</c>: conjunction, subordinating or preposition.</summary>
    public static readonly TreebankPosTag ConjunctionSubordinatingOrPreposition = new("IN", true);

    /// <summary><c>JJ</c>: adjective.</summary>
    public static readonly TreebankPosTag Adjective = new("JJ", true);

    /// <summary><c>JJR</c>: adjective, comparative.</summary>
    public static readonly TreebankPosTag AdjectiveComparative = new("JJR", true);

    /// <summary><c>JJS</c>: adjective, superlative.</summary>
    public static readonly TreebankPosTag AdjectiveSuperlative = new("JJS", true);

    /// <summary><c>LS</c>: list item marker.</summary>
    public static readonly TreebankPosTag ListItemMarker = new("LS", true);

    /// <summary><c>MD</c>: verb, modal auxiliary.</summary>
    public static readonly TreebankPosTag VerbModalAuxiliary = new("MD", true);

    /// <summary><c>NFP</c>: superfluous punctuation.</summary>
    public static readonly TreebankPosTag SuperfluousPunctuation = new("NFP", true);

    /// <summary><c>NIL</c>: missing tag.</summary>
    public static readonly TreebankPosTag MissingTag = new("NIL", true);

    /// <summary><c>NN</c>: noun, singular or mass.</summary>
    public static readonly TreebankPosTag NounSingularOrMass = new("NN", true);

    /// <summary><c>NNP</c>: noun, proper singular.</summary>
    public static readonly TreebankPosTag NounProperSingular = new("NNP", true);

    /// <summary><c>NNPS</c>: noun, proper plural.</summary>
    public static readonly TreebankPosTag NounProperPlural = new("NNPS", true);

    /// <summary><c>NNS</c>: noun, plural.</summary>
    public static readonly TreebankPosTag NounPlural = new("NNS", true);

    /// <summary><c>PDT</c>: predeterminer.</summary>
    public static readonly TreebankPosTag Predeterminer = new("PDT", true);

    /// <summary><c>POS</c>: possessive ending.</summary>
    public static readonly TreebankPosTag PossessiveEnding = new("POS", true);

    /// <summary><c>PRP</c>: pronoun, personal.</summary>
    public static readonly TreebankPosTag PronounPersonal = new("PRP", true);

    /// <summary><c>PRP$</c>: pronoun, possessive.</summary>
    public static readonly TreebankPosTag PronounPossessive = new("PRP$", true);

    /// <summary><c>RB</c>: adverb.</summary>
    public static readonly TreebankPosTag Adverb = new("RB", true);

    /// <summary><c>RBR</c>: adverb, comparative.</summary>
    public static readonly TreebankPosTag AdverbComparative = new("RBR", true);

    /// <summary><c>RBS</c>: adverb, superlative.</summary>
    public static readonly TreebankPosTag AdverbSuperlative = new("RBS", true);

    /// <summary><c>RP</c>: adverb, particle.</summary>
    public static readonly TreebankPosTag AdverbParticle = new("RP", true);

    /// <summary><c>SP</c>: space.</summary>
    public static readonly TreebankPosTag Space = new("SP", true);

    /// <summary><c>SYM</c>: symbol.</summary>
    public static readonly TreebankPosTag Symbol = new("SYM", true);

    /// <summary><c>TO</c>: infinitival "to".</summary>
    public static readonly TreebankPosTag InfinitivalTo = new("TO", true);

    /// <summary><c>UH</c>: interjection.</summary>
    public static readonly TreebankPosTag Interjection = new("UH", true);

    /// <summary><c>VB</c>: verb, base form.</summary>
    public static readonly TreebankPosTag VerbBaseForm = new("VB", true);

    /// <summary><c>VBD</c>: verb, past tense.</summary>
    public static readonly TreebankPosTag VerbPastTense = new("VBD", true);

    /// <summary><c>VBG</c>: verb, gerund or present participle.</summary>
    public static readonly TreebankPosTag VerbGerundOrPresentParticiple = new("VBG", true);

    /// <summary><c>VBN</c>: verb, past participle.</summary>
    public static readonly TreebankPosTag VerbPastParticiple = new("VBN", true);

    /// <summary><c>VBP</c>: verb, non-3rd person singular present.</summary>
    public static readonly TreebankPosTag VerbNon3rdPersonSingularPresent = new("VBP", true);

    /// <summary><c>VBZ</c>: verb, 3rd person singular present.</summary>
    public static readonly TreebankPosTag Verb3rdPersonSingularPresent = new("VBZ", true);

    /// <summary><c>WDT</c>: wh-determiner.</summary>
    public static readonly TreebankPosTag WhDeterminer = new("WDT", true);

    /// <summary><c>WP</c>: wh-pronoun, personal.</summary>
    public static readonly TreebankPosTag WhPronounPersonal = new("WP", true);

    /// <summary><c>WP$</c>: wh-pronoun, possessive.</summary>
    public static readonly TreebankPosTag WhPronounPossessive = new("WP$", true);

    /// <summary><c>WRB</c>: wh-adverb.</summary>
    public static readonly TreebankPosTag WhAdverb = new("WRB", true);

    /// <summary><c>XX</c>: unknown.</summary>
    public static readonly TreebankPosTag Unknown = new("XX", true);

    /// <summary><c>``</c>: opening quotation mark.</summary>
    public static readonly TreebankPosTag OpeningQuotationMark = new("``", true);

    /// <summary>
    /// The set of all defined Penn Treebank POS tags.
    /// </summary>
    public static readonly FrozenSet<TreebankPosTag> All = FrozenSet.ToFrozenSet(
    [
        AdditionalWordInMultiwordExpression,
        Adjective,
        AdjectiveComparative,
        AdjectiveSuperlative,
        Adverb,
        AdverbComparative,
        AdverbParticle,
        AdverbSuperlative,
        Affix,
        AuxiliaryBe,
        CardinalNumber,
        ClosingQuotationMark,
        ClosingQuotationMark2,
        ConjunctionCoordinating,
        ConjunctionSubordinatingOrPreposition,
        Determiner,
        Email,
        ExistentialThere,
        ForeignWord,
        FormsOfHave,
        InfinitivalTo,
        Interjection,
        LeftRoundBracket,
        ListItemMarker,
        MissingTag,
        NounPlural,
        NounProperPlural,
        NounProperSingular,
        NounSingularOrMass,
        OpeningQuotationMark,
        PossessiveEnding,
        Predeterminer,
        PronounPersonal,
        PronounPossessive,
        PunctuationMarkColonOrEllipsis,
        PunctuationMarkComma,
        PunctuationMarkHyphen,
        PunctuationMarkSentenceCloser,
        RightRoundBracket,
        Space,
        SuperfluousPunctuation,
        Symbol,
        SymbolCurrency,
        SymbolNumberSign,
        Unknown,
        Verb3rdPersonSingularPresent,
        VerbBaseForm,
        VerbGerundOrPresentParticiple,
        VerbModalAuxiliary,
        VerbNon3rdPersonSingularPresent,
        VerbPastParticiple,
        VerbPastTense,
        WhAdverb,
        WhDeterminer,
        WhPronounPersonal,
        WhPronounPossessive,
    ]);

    private static readonly FrozenSet<AsciiString> s_validValues = FrozenSet.ToFrozenSet(All.Select(x => x._value));
}
