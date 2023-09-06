// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Language;

// https://www.ling.upenn.edu/courses/Fall_2003/ling001/penn_treebank_pos.html

[EquatableValue]
[StructLayout(LayoutKind.Auto)]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<TreebankPosTag, AsciiString>))]
public readonly partial struct TreebankPosTag : IEquatableValue<TreebankPosTag, AsciiString>
{
    public static int MaxSerializedCharLength => 5;

    public TreebankPosTag(string value) : this((AsciiString)value)
    {
    }

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
        return new TreebankPosTag(value);
    }

    public static readonly TreebankPosTag ClosingQuotationMark = (TreebankPosTag)"\"\"";

    public static readonly TreebankPosTag SymbolNumberSign = (TreebankPosTag)"#";

    public static readonly TreebankPosTag SymbolCurrency = (TreebankPosTag)"$";

    public static readonly TreebankPosTag ClosingQuotationMark2 = (TreebankPosTag)"''";

    public static readonly TreebankPosTag PunctuationMarkComma = (TreebankPosTag)",";

    public static readonly TreebankPosTag LeftRoundBracket = (TreebankPosTag)"-LRB-";

    public static readonly TreebankPosTag RightRoundBracket = (TreebankPosTag)"-RRB-";

    public static readonly TreebankPosTag PunctuationMarkSentenceCloser = (TreebankPosTag)".";

    public static readonly TreebankPosTag PunctuationMarkColonOrEllipsis = (TreebankPosTag)":";

    public static readonly TreebankPosTag Email = (TreebankPosTag)"ADD";

    public static readonly TreebankPosTag Affix = (TreebankPosTag)"AFX";

    public static readonly TreebankPosTag AuxiliaryBe = (TreebankPosTag)"BES";

    public static readonly TreebankPosTag ConjunctionCoordinating = (TreebankPosTag)"CC";

    public static readonly TreebankPosTag CardinalNumber = (TreebankPosTag)"CD";

    public static readonly TreebankPosTag Determiner = (TreebankPosTag)"DT";

    public static readonly TreebankPosTag ExistentialThere = (TreebankPosTag)"EX";

    public static readonly TreebankPosTag ForeignWord = (TreebankPosTag)"FW";

    public static readonly TreebankPosTag AdditionalWordInMultiwordExpression = (TreebankPosTag)"GW";

    public static readonly TreebankPosTag FormsOfHave = (TreebankPosTag)"HVS";

    public static readonly TreebankPosTag PunctuationMarkHyphen = (TreebankPosTag)"HYPH";

    public static readonly TreebankPosTag ConjunctionSubordinatingOrPreposition = (TreebankPosTag)"IN";

    public static readonly TreebankPosTag Adjective = (TreebankPosTag)"JJ";

    public static readonly TreebankPosTag AdjectiveComparative = (TreebankPosTag)"JJR";

    public static readonly TreebankPosTag AdjectiveSuperlative = (TreebankPosTag)"JJS";

    public static readonly TreebankPosTag ListItemMarker = (TreebankPosTag)"LS";

    public static readonly TreebankPosTag VerbModalAuxiliary = (TreebankPosTag)"MD";

    public static readonly TreebankPosTag SuperfluousPunctuation = (TreebankPosTag)"NFP";

    public static readonly TreebankPosTag MissingTag = (TreebankPosTag)"NIL";

    public static readonly TreebankPosTag NounSingularOrMass = (TreebankPosTag)"NN";

    public static readonly TreebankPosTag NounProperSingular = (TreebankPosTag)"NNP";

    public static readonly TreebankPosTag NounProperPlural = (TreebankPosTag)"NNPS";

    public static readonly TreebankPosTag NounPlural = (TreebankPosTag)"NNS";

    public static readonly TreebankPosTag Predeterminer = (TreebankPosTag)"PDT";

    public static readonly TreebankPosTag PossessiveEnding = (TreebankPosTag)"POS";

    public static readonly TreebankPosTag PronounPersonal = (TreebankPosTag)"PRP";

    public static readonly TreebankPosTag PronounPossessive = (TreebankPosTag)"PRP$";

    public static readonly TreebankPosTag Adverb = (TreebankPosTag)"RB";

    public static readonly TreebankPosTag AdverbComparative = (TreebankPosTag)"RBR";

    public static readonly TreebankPosTag AdverbSuperlative = (TreebankPosTag)"RBS";

    public static readonly TreebankPosTag AdverbParticle = (TreebankPosTag)"RP";

    public static readonly TreebankPosTag Space = (TreebankPosTag)"SP";

    public static readonly TreebankPosTag InfinitivalTo = (TreebankPosTag)"TO";

    public static readonly TreebankPosTag Interjection = (TreebankPosTag)"UH";

    public static readonly TreebankPosTag VerbBaseForm = (TreebankPosTag)"VB";

    public static readonly TreebankPosTag VerbPastTense = (TreebankPosTag)"VBD";

    public static readonly TreebankPosTag VerbGerundOrPresentParticiple = (TreebankPosTag)"VBG";

    public static readonly TreebankPosTag VerbPastParticiple = (TreebankPosTag)"VBN";

    public static readonly TreebankPosTag VerbNon3rdPersonSingularPresent = (TreebankPosTag)"VBP";

    public static readonly TreebankPosTag Verb3rdPersonSingularPresent = (TreebankPosTag)"VBZ";

    public static readonly TreebankPosTag WhDeterminer = (TreebankPosTag)"WDT";

    public static readonly TreebankPosTag WhPronounPersonal = (TreebankPosTag)"WP";

    public static readonly TreebankPosTag WhPronounPossessive = (TreebankPosTag)"WP$";

    public static readonly TreebankPosTag WhAdverb = (TreebankPosTag)"WRB";

    public static readonly TreebankPosTag Unknown = (TreebankPosTag)"XX";

    public static readonly TreebankPosTag OpeningQuotationMark = (TreebankPosTag)"``";

    public static readonly IReadOnlySet<TreebankPosTag> All = new HashSet<TreebankPosTag>(new TreebankPosTag[]
    {
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
    });

    private static readonly IReadOnlySet<AsciiString> s_validValues = FrozenSet.ToFrozenSet(All.Select(x => x._value));
}
