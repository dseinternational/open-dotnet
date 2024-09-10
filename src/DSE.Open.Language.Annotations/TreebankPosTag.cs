// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Language.Annotations;

// https://www.ling.upenn.edu/courses/Fall_2003/ling001/penn_treebank_pos.html

[EquatableValue]
[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<TreebankPosTag, AsciiString>))]
public readonly partial struct TreebankPosTag
    : IEquatableValue<TreebankPosTag, AsciiString>,
      IUtf8SpanSerializable<TreebankPosTag>
{
    public static int MaxSerializedCharLength => 5;

    public static int MaxSerializedByteLength => 5;

    public TreebankPosTag(AsciiString value) : this(value, false)
    {
    }

    public TreebankPosTag(string value) : this((AsciiString)value)
    {
    }

    private TreebankPosTag(string value, bool skipValidation) : this((AsciiString)value, skipValidation)
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
    public static explicit operator TreebankPosTag(string value)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return new(value);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates
    public static implicit operator PosTag(TreebankPosTag value)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return new(value._value);
    }

    public static readonly TreebankPosTag ClosingQuotationMark = new("\"\"", true);

    public static readonly TreebankPosTag SymbolNumberSign = new("#", true);

    public static readonly TreebankPosTag SymbolCurrency = new("$", true);

    public static readonly TreebankPosTag ClosingQuotationMark2 = new("''", true);

    public static readonly TreebankPosTag PunctuationMarkComma = new(",", true);

    public static readonly TreebankPosTag LeftRoundBracket = new("-LRB-", true);

    public static readonly TreebankPosTag RightRoundBracket = new("-RRB-", true);

    public static readonly TreebankPosTag PunctuationMarkSentenceCloser = new(".", true);

    public static readonly TreebankPosTag PunctuationMarkColonOrEllipsis = new(":", true);

    public static readonly TreebankPosTag Email = new("ADD", true);

    public static readonly TreebankPosTag Affix = new("AFX", true);

    public static readonly TreebankPosTag AuxiliaryBe = new("BES", true);

    public static readonly TreebankPosTag ConjunctionCoordinating = new("CC", true);

    public static readonly TreebankPosTag CardinalNumber = new("CD", true);

    public static readonly TreebankPosTag Determiner = new("DT", true);

    public static readonly TreebankPosTag ExistentialThere = new("EX", true);

    public static readonly TreebankPosTag ForeignWord = new("FW", true);

    public static readonly TreebankPosTag AdditionalWordInMultiwordExpression = new("GW", true);

    public static readonly TreebankPosTag FormsOfHave = new("HVS", true);

    public static readonly TreebankPosTag PunctuationMarkHyphen = new("HYPH", true);

    public static readonly TreebankPosTag ConjunctionSubordinatingOrPreposition = new("IN", true);

    public static readonly TreebankPosTag Adjective = new("JJ", true);

    public static readonly TreebankPosTag AdjectiveComparative = new("JJR", true);

    public static readonly TreebankPosTag AdjectiveSuperlative = new("JJS", true);

    public static readonly TreebankPosTag ListItemMarker = new("LS", true);

    public static readonly TreebankPosTag VerbModalAuxiliary = new("MD", true);

    public static readonly TreebankPosTag SuperfluousPunctuation = new("NFP", true);

    public static readonly TreebankPosTag MissingTag = new("NIL", true);

    public static readonly TreebankPosTag NounSingularOrMass = new("NN", true);

    public static readonly TreebankPosTag NounProperSingular = new("NNP", true);

    public static readonly TreebankPosTag NounProperPlural = new("NNPS", true);

    public static readonly TreebankPosTag NounPlural = new("NNS", true);

    public static readonly TreebankPosTag Predeterminer = new("PDT", true);

    public static readonly TreebankPosTag PossessiveEnding = new("POS", true);

    public static readonly TreebankPosTag PronounPersonal = new("PRP", true);

    public static readonly TreebankPosTag PronounPossessive = new("PRP$", true);

    public static readonly TreebankPosTag Adverb = new("RB", true);

    public static readonly TreebankPosTag AdverbComparative = new("RBR", true);

    public static readonly TreebankPosTag AdverbSuperlative = new("RBS", true);

    public static readonly TreebankPosTag AdverbParticle = new("RP", true);

    public static readonly TreebankPosTag Space = new("SP", true);

    public static readonly TreebankPosTag Symbol = new("SYM", true);

    public static readonly TreebankPosTag InfinitivalTo = new("TO", true);

    public static readonly TreebankPosTag Interjection = new("UH", true);

    public static readonly TreebankPosTag VerbBaseForm = new("VB", true);

    public static readonly TreebankPosTag VerbPastTense = new("VBD", true);

    public static readonly TreebankPosTag VerbGerundOrPresentParticiple = new("VBG", true);

    public static readonly TreebankPosTag VerbPastParticiple = new("VBN", true);

    public static readonly TreebankPosTag VerbNon3rdPersonSingularPresent = new("VBP", true);

    public static readonly TreebankPosTag Verb3rdPersonSingularPresent = new("VBZ", true);

    public static readonly TreebankPosTag WhDeterminer = new("WDT", true);

    public static readonly TreebankPosTag WhPronounPersonal = new("WP", true);

    public static readonly TreebankPosTag WhPronounPossessive = new("WP$", true);

    public static readonly TreebankPosTag WhAdverb = new("WRB", true);

    public static readonly TreebankPosTag Unknown = new("XX", true);

    public static readonly TreebankPosTag OpeningQuotationMark = new("``", true);

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
