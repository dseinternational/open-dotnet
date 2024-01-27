// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

// Documentation and definitions for the 'strict'/'valid' character sets are from:
//
// Steven Moran & Michael Cysouw. 2018. The Unicode Cookboook for Linguists: Managing
// writing systems using orthography profiles (Translation and Multilingual Natural
// Language Processing 10). Berlin: Language Science Press.
// 
// Available at: http://langsci-press.org/catalog/book/176
// Also at: https://github.com/unicode-cookbook/cookbook
//
// Published under the Creative Commons Attribution 4.0 Licence: http://creativecommons.org/licenses/by/4.0/

using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Runtime.InteropServices;
using DSE.Open.Values;

namespace DSE.Open.Speech;

/// <summary>
/// A symbol used to represent the sounds of language.
/// </summary>
[ComparableValue]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct SpeechSymbol
    : IComparableValue<SpeechSymbol, char>,
      IUtf8SpanSerializable<SpeechSymbol>
{
    public static int MaxSerializedCharLength => 1;

    public static int MaxSerializedByteLength => 2;

    public SpeechSymbol(char value) : this(value, false)
    {
    }

    public static bool TryCreate(char value, out SpeechSymbol symbol)
    {
        if (IsValidValue(value))
        {
            symbol = new SpeechSymbol(value, true);
            return true;
        }

        symbol = default;
        return false;
    }

    public static bool IsValidValue(char value)
    {
        return IsStrictIpaSymbol(value);
    }

    /// <summary>
    /// Determines if the specified character is a 'strict' IPA character. Strict-IPA
    /// represents a strongly constrained subset of IPA geared towards uniqueness of encoding.
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    /// <remarks>
    /// Strict-IPA encoding is supposed to be used when interoperability of phonetic
    /// resources is intended. It is a strongly constrained subset of IPA geared towards
    /// uniqueness of encoding. See <see href="https://github.com/unicode-cookbook/cookbook">The
    /// Unicode Cookbook for Linguists</see> for further detail.
    /// <para>Strict-IPA includes 159 different Unicode code points that form the basis
    /// (107 letters, 36 diacritics and 16 remaining symbols).</para>
    /// </remarks>
    public static bool IsStrictIpaSymbol(char c)
    {
        return StrictIpaSet.Contains(c);
    }

    /// <summary>
    /// Determines if the specified character is a 'valid' IPA character. Valid-IPA allows
    /// alternative symbols with the same phonetic meaning, in accordance with the official
    /// IPA specifications.
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    /// <remarks>
    /// Valid-IPA does not enforce a specific ordering of diacritics, because the IPA does
    /// not propose any such ordering. This means that in valid-IPA the same phonetic intention
    /// can be encoded in multiple ways. This is sufficient for phonetically trained human eyes,
    /// but it is not sufficient for automatic interoperability.
    /// </remarks>
    public static bool IsValidIpaSymbol(char c)
    {
        return IsStrictIpaSymbol(c) || ValidIpaSet.Contains(c);
    }

    /// <summary>
    /// Determines if all of characters in the specified sequence are 'strict' IPA characters.
    /// Returns <see langword="true"/> if the sequence is empty. Strict-IPA represents a strongly
    /// constrained subset of IPA geared towards uniqueness of encoding.
    /// </summary>
    /// <param name="chars">The sequence of characters to evaluate.</param>
    /// <returns><see langword="true"/> if all of characters in the specified sequence are 'strict'
    /// IPA characters or the sequence is empty; otherwise <see langword="false"/>.</returns>
    public static bool AllStrictIpaChars(ReadOnlySpan<char> chars)
    {
        foreach (var c in chars)
        {
            if (!IsStrictIpaSymbol(c))
            {
                return false;
            }
        }

        return true;
    }

    public static bool AllValidIpaChars(ReadOnlySpan<char> chars)
    {
        foreach (var c in chars)
        {
            if (!IsValidIpaSymbol(c))
            {
                return false;
            }
        }

        return true;
    }

    // --------------------------------------------------------------------------------------------
    // 'Strict-IPA' (see https://github.com/unicode-cookbook/cookbook, Chapter 5)

    /// <summary>
    /// The Unicode 'latin small letter a' <c>a</c> character, used to represent open front unrounded.
    /// </summary>
    public static readonly SpeechSymbol OpenFrontUnrounded = new((char)0x0061, true);

    /// <summary>
    /// The Unicode 'latin small letter ae' <c>æ</c> character, used to represent raised open front unrounded.
    /// </summary>
    public static readonly SpeechSymbol RaisedOpenFrontUnrounded = new((char)0x00E6, true);

    /// <summary>
    /// The Unicode 'latin small letter turned a' <c>ɐ</c> character, used to represent lowered schwa.
    /// </summary>
    public static readonly SpeechSymbol LoweredSchwa = new((char)0x0250, true);

    /// <summary>
    /// The Unicode 'latin small letter alpha' <c>ɑ</c> character, used to represent open back unrounded.
    /// </summary>
    public static readonly SpeechSymbol OpenBackUnrounded = new((char)0x0251, true);

    /// <summary>
    /// The Unicode 'latin small letter turned alpha' <c>ɒ</c> character, used to represent open back rounded.
    /// </summary>
    public static readonly SpeechSymbol OpenBackRounded = new((char)0x0252, true);

    /// <summary>
    /// The Unicode 'latin small letter b' <c>b</c> character, used to represent voiced bilabial plosive.
    /// </summary>
    public static readonly SpeechSymbol VoicedBilabialPlosive = new((char)0x0062, true);

    /// <summary>
    /// The Unicode 'latin letter small capital b' <c>ʙ</c> character, used to represent voiced bilabial trill.
    /// </summary>
    public static readonly SpeechSymbol VoicedBilabialTrill = new((char)0x0299, true);

    /// <summary>
    /// The Unicode 'latin small letter b with hook' <c>ɓ</c> character, used to represent voiced bilabial implosive.
    /// </summary>
    public static readonly SpeechSymbol VoicedBilabialImplosive = new((char)0x0253, true);

    /// <summary>
    /// The Unicode 'latin small letter c' <c>c</c> character, used to represent voiceless palatal plosive.
    /// </summary>
    public static readonly SpeechSymbol VoicelessPalatalPlosive = new((char)0x0063, true);

    /// <summary>
    /// The Unicode 'latin small letter c with cedilla' <c>ç</c> character, used to represent voiceless palatal fricative.
    /// </summary>
    public static readonly SpeechSymbol VoicelessPalatalFricative = new((char)0x00E7, true);

    /// <summary>
    /// The Unicode 'latin small letter c with curl' <c>ɕ</c> character, used to represent voiceless alveolo-palatal fricative.
    /// </summary>
    public static readonly SpeechSymbol VoicelessAlveoloPalatalFricative = new((char)0x0255, true);

    /// <summary>
    /// The Unicode 'latin small letter d' <c>d</c> character, used to represent voiced alveolar plosive.
    /// </summary>
    public static readonly SpeechSymbol VoicedAlveolarPlosive = new((char)0x0064, true);

    /// <summary>
    /// The Unicode 'lating small letter eth' <c>ð</c> character, used to represent voiced dental fricative.
    /// </summary>
    public static readonly SpeechSymbol VoicedDentalFricative = new((char)0x00F0, true);

    /// <summary>
    /// The Unicode 'lating small letter d with tail' <c>ɖ</c> character, used to represent voiced retroflex plosive.
    /// </summary>
    public static readonly SpeechSymbol VoicedRetroflexPlosive = new((char)0x0256, true);

    public static readonly SpeechSymbol VoicedDentalAlveolarImplosive = new((char)0x0257, true);  // ɗ,latin small letter d with hook,voiced dental/alveolar implosive

    public static readonly SpeechSymbol CloseMidFrontUnrounded = new((char)0x0065, true);  // e,latin small letter e,close-mid front unrounded

    public static readonly SpeechSymbol MidCentralSchwa = new((char)0x0259, true);  // ə,latin small letter schwa,mid-central schwa

    public static readonly SpeechSymbol OpenMidFrontUnrounded = new((char)0x025B, true);  // ɛ,latin small letter open e,open-mid front unrounded

    public static readonly SpeechSymbol CloseMidCentralUnrounded = new((char)0x0258, true);  // ɘ,latin small letter reversed e,close-mid central unrounded

    public static readonly SpeechSymbol OpenMidCentralUnrounded = new((char)0x025C, true);  // ɜ,latin small letter reversed open e,open-mid central unrounded

    public static readonly SpeechSymbol OpenMidCentralRounded = new((char)0x025E, true);  // ɞ,latin small letter closed reversed open e,open-mid central rounded

    public static readonly SpeechSymbol VoicelessLabiodentalFricative = new((char)0x0066, true);  // f,latin small letter f,voiceless labiodental fricative

    public static readonly SpeechSymbol VoicedVelarPlosive = new((char)0x0261, true);  // ɡ,latin small letter script g,voiced velar plosive

    public static readonly SpeechSymbol VoicedUvularPlosive = new((char)0x0262, true);  // ɢ,latin letter small capital g,voiced uvular plosive

    public static readonly SpeechSymbol VoicedVelarImplosive = new((char)0x0260, true);  // ɠ,latin small letter g with hook,voiced velar implosive

    public static readonly SpeechSymbol VoicedUvularImplosive = new((char)0x029B, true);  // ʛ,latin letter small capital g with hook,voiced uvular implosive

    public static readonly SpeechSymbol CloseMidBackUnrounded = new((char)0x0264, true);  // ɤ,latin small letter rams horn,close-mid back unrounded

    public static readonly SpeechSymbol VoicedVelarFricative = new((char)0x0263, true);  // ɣ,latin small letter gamma,voiced velar fricative

    public static readonly SpeechSymbol VoicelessGlottalFricative = new((char)0x0068, true);  // h,latin small letter h,voiceless glottal fricative

    public static readonly SpeechSymbol VoicelessPharyngealFricative = new((char)0x0127, true);  // ħ,latin small letter h with stroke,voiceless pharyngeal fricative

    public static readonly SpeechSymbol VoicelessEpiglottalFricative = new((char)0x029C, true);  // ʜ,latin letter small capital h,voiceless epiglottal fricative

    public static readonly SpeechSymbol VoicedGlottalFricative = new((char)0x0266, true);  // ɦ,latin small letter h with hook,voiced glottal fricative

    public static readonly SpeechSymbol SimultaneousVoicelessPostalveolarVelarFricative = new((char)0x0267, true);  // ɧ,latin small letter heng with hook,simultaneous voiceless postalveolar+velar fricative

    public static readonly SpeechSymbol VoicedLabialPalatalApproximant = new((char)0x0265, true);  // ɥ,latin small letter turned h,voiced labial-palatal approximant

    public static readonly SpeechSymbol CloseFrontUnrounded = new((char)0x0069, true);  // i,latin small letter i,close front unrounded

    public static readonly SpeechSymbol LaxCloseFrontUnrounded = new((char)0x026A, true);  // ɪ,latin letter small capital i,lax close front unrounded

    public static readonly SpeechSymbol CloseCentralUnrounded = new((char)0x0268, true);  // ɨ,latin small letter i with stroke,close central unrounded

    public static readonly SpeechSymbol VoicedPalatalApproximant = new((char)0x006A, true);  // j,latin small letter j,voiced palatal approximant

    public static readonly SpeechSymbol VoicedPalatalFricative = new((char)0x029D, true);  // ʝ,latin small letter j with crossed tail,voiced palatal fricative

    public static readonly SpeechSymbol VoicedPalatalPlosive = new((char)0x025F, true);  // ɟ,latin small letter dotless j with stroke,voiced palatal plosive

    public static readonly SpeechSymbol VoicedPalatalImplosive = new((char)0x0284, true);  // ʄ,latin small letter dotless j with stroke and hook,voiced palatal implosive

    public static readonly SpeechSymbol VoicelessVelarPlosive = new((char)0x006B, true);  // k,latin small letter k,voiceless velar plosive

    public static readonly SpeechSymbol VoicedAlveolarLateralApproximant = new((char)0x006C, true);  // l,latin small letter l,voiced alveolar lateral approximant

    public static readonly SpeechSymbol VoicedVelarLateralApproximant = new((char)0x029F, true);  // ʟ,latin letter small capital l,voiced velar lateral approximant

    public static readonly SpeechSymbol VoicelessAlveolarLateralFricative = new((char)0x026C, true);  // ɬ,latin small letter l with belt,voiceless alveolar lateral fricative

    public static readonly SpeechSymbol VoicedRetroflexLateralApproximant = new((char)0x026D, true);  // ɭ,latin small letter l with retroflex hook,voiced retroflex lateral approximant

    public static readonly SpeechSymbol VoicedAlveolarLateralFricative = new((char)0x026E, true);  // ɮ,latin small letter lezh,voiced alveolar lateral fricative

    public static readonly SpeechSymbol VoicedPalatalLateralApproximant = new((char)0x028E, true);  // ʎ,latin small letter turned y,voiced palatal lateral approximant

    public static readonly SpeechSymbol VoicedBilabialNasal = new((char)0x006D, true);  // m,latin small letter m,voiced bilabial nasal

    public static readonly SpeechSymbol VoicedLabiodentalNasal = new((char)0x0271, true);  // ɱ,latin small letter m with hook,voiced labiodental nasal

    public static readonly SpeechSymbol VoicedAlveolarNasal = new((char)0x006E, true);  // n,latin small letter n,voiced alveolar nasal

    public static readonly SpeechSymbol VoicedUvularNasal = new((char)0x0274, true);  // ɴ,latin letter small capital n,voiced uvular nasal

    public static readonly SpeechSymbol VoicedPalatalNasal = new((char)0x0272, true);  // ɲ,latin small letter n with left hook,voiced palatal nasal

    public static readonly SpeechSymbol VoicedRetroflexNasal = new((char)0x0273, true);  // ɳ,latin small letter n with retroflex hook,voiced retroflex nasal

    public static readonly SpeechSymbol VoicedVelarNasal = new((char)0x014B, true);  // ŋ,latin small letter eng,voiced velar nasal

    public static readonly SpeechSymbol CloseMidBackRounded = new((char)0x006F, true);  // o,latin small letter o,close-mid back rounded

    public static readonly SpeechSymbol CloseMidFrontRounded = new((char)0x00F8, true);  // ø,latin small letter o with stroke,close-mid front rounded

    public static readonly SpeechSymbol OpenMidFrontRounded = new((char)0x0153, true);  // œ,latin small ligature oe,open-mid front rounded

    public static readonly SpeechSymbol OpenFrontRounded = new((char)0x0276, true);  // ɶ,latin letter small capital oe,open front rounded

    public static readonly SpeechSymbol OpenMidBackRounded = new((char)0x0254, true);  // ɔ,latin small letter open o,open-mid back rounded

    public static readonly SpeechSymbol CloseMidCentralRounded = new((char)0x0275, true);  // ɵ,latin small letter barred o,close-mid central rounded

    public static readonly SpeechSymbol VoicelessBilabialPlosive = new((char)0x0070, true);  // p,latin small letter p,voiceless bilabial plosive

    public static readonly SpeechSymbol VoicelessBilabialFricative = new((char)0x0278, true);  // ɸ,latin small letter phi,voiceless bilabial fricative

    public static readonly SpeechSymbol VoicelessUvularPlosive = new((char)0x0071, true);  // q,latin small letter q,voiceless uvular plosive

    public static readonly SpeechSymbol VoicedAlveolarTrill = new((char)0x0072, true);  // r,latin small letter r,voiced alveolar trill

    public static readonly SpeechSymbol VoicedUvularTrill = new((char)0x0280, true);  // ʀ,latin letter small capital r,voiced uvular trill

    public static readonly SpeechSymbol VoicedAlveolarApproximant = new((char)0x0279, true);  // ɹ,latin small letter turned r,voiced alveolar approximant

    public static readonly SpeechSymbol VoicedAlveolarLateralFlap = new((char)0x027A, true);  // ɺ,latin small letter turned r with long leg,voiced alveolar lateral flap

    public static readonly SpeechSymbol VoicedRetroflexApproximant = new((char)0x027B, true);  // ɻ,latin small letter turned r with hook,voiced retroflex approximant

    public static readonly SpeechSymbol VoicedRetroflexTap = new((char)0x027D, true);  // ɽ,latin small letter r with tail,voiced retroflex tap

    public static readonly SpeechSymbol VoicedAlveolarTap = new((char)0x027E, true);  // ɾ,latin small letter r with fishhook,voiced alveolar tap

    public static readonly SpeechSymbol VoicedUvularFricative = new((char)0x0281, true);  // ʁ,latin letter small capital inverted r,voiced uvular fricative

    public static readonly SpeechSymbol VoicelessAlveolarFricative = new((char)0x0073, true);  // s,latin small letter s,voiceless alveolar fricative

    public static readonly SpeechSymbol VoicelessRetroflexFricative = new((char)0x0282, true);  // ʂ,latin small letter s with hook,voiceless retroflex fricative

    public static readonly SpeechSymbol VoicelessPostalveolarFricative = new((char)0x0283, true);  // ʃ,latin small letter esh,voiceless postalveolar fricative

    public static readonly SpeechSymbol VoicelessAlveolarPlosive = new((char)0x0074, true);  // t,latin small letter t,voiceless alveolar plosive

    public static readonly SpeechSymbol VoicelessRetroflexPlosive = new((char)0x0288, true);  // ʈ,latin small letter t with retroflex hook,voiceless retroflex plosive

    public static readonly SpeechSymbol CloseBackRounded = new((char)0x0075, true);  // u,latin small letter u,close back rounded

    public static readonly SpeechSymbol CloseCentralRounded = new((char)0x0289, true);  // ʉ,latin small letter u bar,close central rounded

    public static readonly SpeechSymbol CloseBackUnrounded = new((char)0x026F, true);  // ɯ,latin small letter turned m,close back unrounded

    public static readonly SpeechSymbol VoicedVelarApproximant = new((char)0x0270, true);  // ɰ,latin small letter turned m with long leg,voiced velar approximant

    public static readonly SpeechSymbol LaxCloseBackRounded = new((char)0x028A, true);  // ʊ,latin small letter upsilon,lax close back rounded

    public static readonly SpeechSymbol VoicedLabiodentalFricative = new((char)0x0076, true);  // v,latin small letter v,voiced labiodental fricative

    public static readonly SpeechSymbol VoicedLabiodentalApproximant = new((char)0x028B, true);  // ʋ,latin small letter v with hook,voiced labiodental approximant

    public static readonly SpeechSymbol VoicedLabiodentalTap = new((char)0x2C71, true);  // ⱱ,latin small letter v with right hook,voiced labiodental tap

    public static readonly SpeechSymbol OpenMidBackUnrounded = new((char)0x028C, true);  // ʌ,latin small letter turned v,open-mid back unrounded

    public static readonly SpeechSymbol VoicedLabialVelarApproximant = new((char)0x0077, true);  // w,latin small letter w,voiced labial-velar approximant

    public static readonly SpeechSymbol VoicelessLabialVelarFricative = new((char)0x028D, true);  // ʍ,latin small letter turned w,voiceless labial-velar fricative

    public static readonly SpeechSymbol VoicelessVelarFricative = new((char)0x0078, true);  // x,latin small letter x,voiceless velar fricative

    public static readonly SpeechSymbol CloseFrontRounded = new((char)0x0079, true);  // y,latin small letter y,close front rounded

    public static readonly SpeechSymbol LaxCloseFrontRounded = new((char)0x028F, true);  // ʏ,latin letter small capital y,lax close front rounded

    public static readonly SpeechSymbol VoicedAlveolarFricative = new((char)0x007A, true);  // z,latin small letter z,voiced alveolar fricative

    public static readonly SpeechSymbol VoicedRetroflexFricative = new((char)0x0290, true);  // ʐ,latin small letter z with retroflex hook,voiced retroflex fricative

    public static readonly SpeechSymbol VoicedAlveoloPalatalFricative = new((char)0x0291, true);  // ʑ,latin small letter z with curl,voiced alveolo-palatal fricative

    public static readonly SpeechSymbol VoicedPostalveolarFricative = new((char)0x0292, true);  // ʒ,latin small letter ezh,voiced postalveolar fricative

    public static readonly SpeechSymbol VoicelessGlottalPlosive = new((char)0x0294, true);  // ʔ,latin letter glottal stop,voiceless glottal plosive

    public static readonly SpeechSymbol VoicedPharyngealFricative = new((char)0x0295, true);  // ʕ,latin letter pharyngeal voiced fricative,voiced pharyngeal fricative

    public static readonly SpeechSymbol EpiglottalPlosive = new((char)0x02A1, true);  // ʡ,latin letter glottal stop with stroke,epiglottal plosive

    public static readonly SpeechSymbol VoicedEpiglottalFricative = new((char)0x02A2, true);  // ʢ,latin letter reversed glottal stop with stroke,voiced epiglottal fricative

    public static readonly SpeechSymbol VoicelessDentalClick = new((char)0x01C0, true);  // ǀ,latin letter dental click,voiceless dental click

    public static readonly SpeechSymbol VoicelessAlveolarLateralClick = new((char)0x01C1, true);  // ǁ,latin letter lateral click,voiceless alveolar lateral click

    public static readonly SpeechSymbol VoicelessPalatoalveolarClick = new((char)0x01C2, true);  // ǂ,latin letter alveolar click,voiceless palatoalveolar click

    public static readonly SpeechSymbol VoicelessPostalveolarClick = new((char)0x01C3, true);  // ǃ,latin letter retroflex click,voiceless (post)alveolar click

    public static readonly SpeechSymbol VoicelessBilabialClick = new((char)0x0298, true);  // ʘ,latin letter bilabial click,voiceless bilabial click

    public static readonly SpeechSymbol VoicedBilabialFricative = new((char)0x03B2, true);  // β,greek small letter beta,voiced bilabial fricative

    public static readonly SpeechSymbol VoicelessDentalFricative = new((char)0x03B8, true);  // θ,greek small letter theta,voiceless dental fricative

    public static readonly SpeechSymbol VoicelessUvularFricative = new((char)0x03C7, true);  // χ,greek small letter chi,voiceless uvular fricative

    public static readonly SpeechSymbol VelarizedOrPharyngealized = new((char)0x0334, true);  // ◌̴,combining tilde overlay,velarized or pharyngealized

    public static readonly SpeechSymbol Linguolabial = new((char)0x033C, true);  // ◌̼,combining seagull below,linguolabial

    public static readonly SpeechSymbol Dental = new((char)0x032A, true);  // ◌̪,combining bridge below,dental

    public static readonly SpeechSymbol Laminal = new((char)0x033B, true);  // ◌̻,combining square below,laminal

    public static readonly SpeechSymbol Apical = new((char)0x033A, true);  // ◌̺,combining inverted bridge below,apical

    public static readonly SpeechSymbol Advanced = new((char)0x031F, true);  // ◌̟,combining plus sign below,advanced

    public static readonly SpeechSymbol Retracted = new((char)0x0320, true);  // ◌̠,combining minus sign below,retracted

    public static readonly SpeechSymbol Raised = new((char)0x031D, true);  // ◌̝,combining up tack below,raised

    public static readonly SpeechSymbol Lowered = new((char)0x031E, true);  // ◌̞,combining down tack below,lowered

    public static readonly SpeechSymbol AdvancedTongueRoot = new((char)0x0318, true);  // ◌̘,combining left tack below,advanced tongue root

    public static readonly SpeechSymbol RetractedTongueRoot = new((char)0x0319, true);  // ◌̙,combining right tack below,retracted tongue root

    public static readonly SpeechSymbol LessRounded = new((char)0x031C, true);  // ◌̜,combining left half ring below,less rounded

    public static readonly SpeechSymbol MoreRounded = new((char)0x0339, true);  // ◌̹,combining right half ring below,more rounded

    public static readonly SpeechSymbol Voiced = new((char)0x032C, true);  // ◌̬,combining caron below,voiced

    public static readonly SpeechSymbol Voiceless = new((char)0x0325, true);  // ◌̥,combining ring below,voiceless

    public static readonly SpeechSymbol CreakyVoiced = new((char)0x0330, true);  // ◌̰,combining tilde below,creaky voiced

    public static readonly SpeechSymbol BreathyVoiced = new((char)0x0324, true);  // ◌̤,combining diaeresis below,breathy voiced

    public static readonly SpeechSymbol Syllabic = new((char)0x0329, true);  // ◌̩,combining vertical line below,syllabic

    public static readonly SpeechSymbol NonSyllabic = new((char)0x032F, true);  // ◌̯,combining inverted breve below,non-syllabic

    public static readonly SpeechSymbol Nasalized = new((char)0x0303, true);  // ◌̃,combining tilde,nasalized

    public static readonly SpeechSymbol Centralized = new((char)0x0308, true);  // ◌̈,combining diaeresis,centralized

    public static readonly SpeechSymbol MidCentralized = new((char)0x033D, true);  // ◌̽,combining x above,mid-centralized

    public static readonly SpeechSymbol ExtraShort = new((char)0x0306, true);  // ◌̆,combining breve,extra-short

    public static readonly SpeechSymbol NoAudibleRelease = new((char)0x031A, true);  // ◌̚,combining left angle above,no audible release

    public static readonly SpeechSymbol Rhotacized = new((char)0x02DE, true);  // ◌˞,modifier letter rhotic hook,rhotacized

    public static readonly SpeechSymbol LateralRelease = new((char)0x02E1, true);  // ˡ,modifier letter small l,lateral release

    public static readonly SpeechSymbol NasalRelease = new((char)0x207F, true);  // ⁿ,superscript latin small letter n,nasal release

    public static readonly SpeechSymbol Labialized = new((char)0x02B7, true);  // ʷ,modifier letter small w,labialized

    public static readonly SpeechSymbol Palatalized = new((char)0x02B2, true);  // ʲ,modifier letter small j,palatalized

    public static readonly SpeechSymbol Velarized = new((char)0x02E0, true);  // ˠ,modifier letter small gamma,velarized

    public static readonly SpeechSymbol Pharyngealized = new((char)0x02E4, true);  // ˤ,modifier letter small reversed glottal stop,pharyngealized

    public static readonly SpeechSymbol Aspirated = new((char)0x02B0, true);  // ʰ,modifier letter small h,aspirated

    public static readonly SpeechSymbol Ejective = new((char)0x02BC, true);  // ʼ,modifier letter apostrophe,ejective

    public static readonly SpeechSymbol Long = new((char)0x02D0, true);  // ː,modifier letter triangular colon,long

    public static readonly SpeechSymbol HalfLong = new((char)0x02D1, true);  // ˑ,modifier letter half triangular colon,half-long

    public static readonly SpeechSymbol TieBar = new((char)0x0361, true);  // ͡,combining double inverted breve,tie bar

    public static readonly SpeechSymbol PrimaryStress = new((char)0x02C8, true);  // ˈ,modifier letter vertical line,primary stress

    public static readonly SpeechSymbol SecondaryStress = new((char)0x02CC, true);  // ˌ,modifier letter low vertical line,secondary stress

    public static readonly SpeechSymbol ExtraHighTone = new((char)0x02E5, true);  // ˥,modifier letter extra-high tone bar,extra high tone

    public static readonly SpeechSymbol HighTone = new((char)0x02E6, true);  // ˦,modifier letter high tone bar,high tone

    public static readonly SpeechSymbol MidTone = new((char)0x02E7, true);  // ˧,modifier letter mid tone bar,mid tone

    public static readonly SpeechSymbol LowTone = new((char)0x02E8, true);  // ˨,modifier letter low tone bar,low tone

    public static readonly SpeechSymbol ExtraLowTone = new((char)0x02E9, true);  // ˩,modifier letter extra-low tone bar,extra low tone

    public static readonly SpeechSymbol Upstep = new((char)0xA71B, true);  // ꜛ,modifier letter raised up arrow,upstep

    public static readonly SpeechSymbol Downstep = new((char)0xA71C, true);  // ꜜ,modifier letter raised down arrow,downstep

    public static readonly SpeechSymbol GlobalRiseUp = new((char)0x2191, true);  // ↑,upwards arrow,global rise

    public static readonly SpeechSymbol GlobalFallDown = new((char)0x2193, true);  // ↓,downwards arrow,global fall

    public static readonly SpeechSymbol GlobalRiseUpRight = new((char)0x2197, true);  // ↗,north east arrow,global rise

    public static readonly SpeechSymbol GlobalFallDownRight = new((char)0x2198, true);  // ↘,south east arrow,global fall

    public static readonly SpeechSymbol WordBreak = new((char)0x0020, true);  // ,space,word break

    public static readonly SpeechSymbol SyllableBreak = new((char)0x002E, true);  // ,full stop,syllable break

    public static readonly SpeechSymbol MinorGroupBreakFoot = new((char)0x007C, true);  // |,vertical line,minor group break (foot)

    public static readonly SpeechSymbol MajorGroupBreakIntonation = new((char)0x2016, true);  // ‖,double vertical line,major group break (intonation)

    public static readonly SpeechSymbol LinkingAbsenceOfABreak = new((char)0x203F, true);  // ‿,undertie,linking (absence of a break)

    // --------------------------------------------------------------------------------------------
    // Additional characters for valid-IPA with Unicode encodings

    /// <summary>
    /// The Unicode 'combining ring above' <c>◌̊</c> character, used to represent voiceless (above). 
    /// </summary>
    public static readonly SpeechSymbol VoicelessAlt = new((char)0x030A, true);  // ◌̊,combining ring above,voiceless (above)

    /// <summary>
    /// The Unicode 'latin small letter g' <c>g</c> character, used to represent voiced velar plosive (alt).
    /// </summary>
    public static readonly SpeechSymbol VoicedVelarPlosiveAlt = new((char)0x0067, true);  // g,latin small letter g,voiced velar plosive

    /// <summary>
    /// The Unicode 'combining double acute accent' <c>◌̋</c> character, used to represent extra high tone (alt).
    /// </summary>
    public static readonly SpeechSymbol ExtraHighToneAlt = new((char)0x030B, true);  // ◌̋,combining double acute accent,extra high tone

    public static readonly SpeechSymbol HighToneAlt = new((char)0x0301, true);  // ◌́,combining acute accent,high tone

    public static readonly SpeechSymbol MidToneAlt = new((char)0x0304, true);  // ◌̄,combining macron,mid tone

    public static readonly SpeechSymbol LowToneAlt = new((char)0x0300, true);  // ◌̀,combining grave accent,low tone

    public static readonly SpeechSymbol ExtraLowToneAlt = new((char)0x030F, true);  // ◌̏,combining double grave accent,extra low tone

    public static readonly SpeechSymbol Falling = new((char)0x0302, true);  // ◌̂,combining circumflex accent,falling

    public static readonly SpeechSymbol Rising = new((char)0x030C, true);  // ◌̌,combining caron,rising

    public static readonly SpeechSymbol HighRising = new((char)0x1DC4, true);  // ◌᷄,combining macron-acute,high rising

    public static readonly SpeechSymbol LowRising = new((char)0x1DC5, true);  // ◌᷅,combining grave-macron,low rising

    public static readonly SpeechSymbol LowFalling = new((char)0x1DC6, true);  // ◌᷆,combining macron-grave,low falling

    public static readonly SpeechSymbol HighFalling = new((char)0x1DC7, true);  // ◌᷇,combining acute-macron,high falling

    public static readonly SpeechSymbol RisingFalling = new((char)0x1DC8, true);  // ◌᷈,combining grave-acute-grave,rising-falling

    public static readonly SpeechSymbol FallingRising = new((char)0x1DC9, true);  // ◌᷉,combining acute-grave-acute,falling-rising

    public static readonly SpeechSymbol TieBarBelow = new((char)0x035C, true);  // ◌͜,combining double breve below,tie bar (below)

    /*
    // --------------------------------------------------------------------------------------------
    // Additions to widened-IPA with Unicode encodings

    public static readonly SpeechSymbol RetroflexClick = new((char)0x203C, true);  // ‼,double exclamation mark,retroflex click

    public static readonly SpeechSymbol VoicedRetroflexImplosive = new((char)0x1D91, true);  // ᶑ,latin small letter d with hook and tail,voiced retroflex implosive

    public static readonly SpeechSymbol Fortis = new((char)0x0348, true);  // ◌͈,combining double vertical line below,fortis

    public static readonly SpeechSymbol Lenis = new((char)0x0349, true);  // ◌͉,combining left angle below,lenis

    public static readonly SpeechSymbol Frictionalized = new((char)0x0353, true);  // ◌͓,combining x below,frictionalized

    public static readonly SpeechSymbol Derhoticized = new((char)0x032E, true);  // ◌̮,combining breve below,derhoticized

    public static readonly SpeechSymbol NonSibilant = new((char)0x0347, true);  // ◌͇,combining equals sign below,non-sibilant

    public static readonly SpeechSymbol Glottalized = new((char)0x02C0, true);  // ◌ˀ,modifier letter glottal stop,glottalized

    public static readonly SpeechSymbol VoicedPreAspirated = new((char)0x02B1, true);  // ʱ◌,modifier letter small h with hook,voiced pre-aspirated

    public static readonly SpeechSymbol EpilaryngealPhonation = new((char)0x1D31, true);  // ◌ᴱ,modifier letter capital e,epilaryngeal phonation
    
    */

    /// <summary>
    /// Characters allowed for strict IPA.
    /// </summary>
    private static readonly ImmutableArray<int> s_strictIpa =
    [
        0x0020,  // space (word break)
        0x002E,  // full stop (syllable break)
        0x0061,  // latin small letter a (open front unrounded)
        0x0062,  // latin small letter b (voiced bilabial plosive)
        0x0063,  // latin small letter c (voiceless palatal plosive)
        0x0064,  // latin small letter d (voiced alveolar plosive)
        0x0065,  // latin small letter e (close-mid front unrounded)
        0x0066,  // latin small letter f (voiceless labiodental fricative)
        0x0068,  // latin small letter h (voiceless glottal fricative)
        0x0069,  // latin small letter i (close front unrounded)
        0x006A,  // latin small letter j (voiced palatal approximant)
        0x006B,  // latin small letter k (voiceless velar plosive)
        0x006C,  // latin small letter l (voiced alveolar lateral approximant)
        0x006D,  // latin small letter m (voiced bilabial nasal)
        0x006E,  // latin small letter n (voiced alveolar nasal)
        0x006F,  // latin small letter o (close-mid back rounded)
        0x0070,  // latin small letter p (voiceless bilabial plosive)
        0x0071,  // latin small letter q (voiceless uvular plosive)
        0x0072,  // latin small letter r (voiced alveolar trill)
        0x0073,  // latin small letter s (voiceless alveolar fricative)
        0x0074,  // latin small letter t (voiceless alveolar plosive)
        0x0075,  // latin small letter u (close back rounded)
        0x0076,  // latin small letter v (voiced labiodental fricative)
        0x0077,  // latin small letter w (voiced labial-velar approximant)
        0x0078,  // latin small letter x (voiceless velar fricative)
        0x0079,  // latin small letter y (close front rounded)
        0x007A,  // latin small letter z (voiced alveolar fricative)
        0x007C,  // vertical line (minor group break (foot))
        0x00E6,  // latin small letter ae (raised open front unrounded)
        0x00E7,  // latin small letter c with cedilla (voiceless palatal fricative)
        0x00F0,  // latin small letter eth (voiced dental fricative)
        0x00F8,  // latin small letter o with stroke (close-mid front rounded)
        0x0127,  // latin small letter h with stroke (voiceless pharyngeal fricative)
        0x014B,  // latin small letter eng (voiced velar nasal)
        0x0153,  // latin small ligature oe (open-mid front rounded)
        0x01C0,  // latin letter dental click (voiceless dental click)
        0x01C1,  // latin letter lateral click (voiceless alveolar lateral click)
        0x01C2,  // latin letter alveolar click (voiceless palatoalveolar click)
        0x01C3,  // latin letter retroflex click (voiceless (post)alveolar click)
        0x0250,  // latin small letter turned a (lowered schwa)
        0x0251,  // latin small letter alpha (open back unrounded)
        0x0252,  // latin small letter turned alpha (open back rounded)
        0x0253,  // latin small letter b with hook (voiced bilabial implosive)
        0x0254,  // latin small letter open o (open-mid back rounded)
        0x0255,  // latin small letter c with curl (voiceless alveolo-palatal fricative)
        0x0256,  // latin small letter d with tail (voiced retroflex plosive)
        0x0257,  // latin small letter d with hook (voiced dental/alveolar implosive)
        0x0258,  // latin small letter reversed e (close-mid central unrounded)
        0x0259,  // latin small letter schwa (mid-central schwa)
        0x025B,  // latin small letter open e (open-mid front unrounded)
        0x025C,  // latin small letter reversed open e (open-mid central unrounded)
        0x025E,  // latin small letter closed reversed open e (open-mid central rounded)
        0x025F,  // latin small letter dotless j with stroke (voiced palatal plosive)
        0x0260,  // latin small letter g with hook (voiced velar implosive)
        0x0261,  // latin small letter script g (voiced velar plosive)
        0x0262,  // latin letter small capital g (voiced uvular plosive)
        0x0263,  // latin small letter gamma (voiced velar fricative)
        0x0264,  // latin small letter rams horn (close-mid back unrounded)
        0x0265,  // latin small letter turned h (voiced labial-palatal approximant)
        0x0266,  // latin small letter h with hook (voiced glottal fricative)
        0x0267,  // latin small letter heng with hook (simultaneous voiceless postalveolar+velar fricative)
        0x0268,  // latin small letter i with stroke (close central unrounded)
        0x026A,  // latin letter small capital i (lax close front unrounded)
        0x026C,  // latin small letter l with belt (voiceless alveolar lateral fricative)
        0x026D,  // latin small letter l with retroflex hook (voiced retroflex lateral approximant)
        0x026E,  // latin small letter lezh (voiced alveolar lateral fricative)
        0x026F,  // latin small letter turned m (close back unrounded)
        0x0270,  // latin small letter turned m with long leg (voiced velar approximant)
        0x0271,  // latin small letter m with hook (voiced labiodental nasal)
        0x0272,  // latin small letter n with left hook (voiced palatal nasal)
        0x0273,  // latin small letter n with retroflex hook (voiced retroflex nasal)
        0x0274,  // latin letter small capital n (voiced uvular nasal)
        0x0275,  // latin small letter barred o (close-mid central rounded)
        0x0276,  // latin letter small capital oe (open front rounded)
        0x0278,  // latin small letter phi (voiceless bilabial fricative)
        0x0279,  // latin small letter turned r (voiced alveolar approximant)
        0x027A,  // latin small letter turned r with long leg (voiced alveolar lateral flap)
        0x027B,  // latin small letter turned r with hook (voiced retroflex approximant)
        0x027D,  // latin small letter r with tail (voiced retroflex tap)
        0x027E,  // latin small letter r with fishhook (voiced alveolar tap)
        0x0280,  // latin letter small capital r (voiced uvular trill)
        0x0281,  // latin letter small capital inverted r (voiced uvular fricative)
        0x0282,  // latin small letter s with hook (voiceless retroflex fricative)
        0x0283,  // latin small letter esh (voiceless postalveolar fricative)
        0x0284,  // latin small letter dotless j with stroke and hook (voiced palatal implosive)
        0x0288,  // latin small letter t with retroflex hook (voiceless retroflex plosive)
        0x0289,  // latin small letter u bar (close central rounded)
        0x028A,  // latin small letter upsilon (lax close back rounded)
        0x028B,  // latin small letter v with hook (voiced labiodental approximant)
        0x028C,  // latin small letter turned v (open-mid back unrounded)
        0x028D,  // latin small letter turned w (voiceless labial-velar fricative)
        0x028E,  // latin small letter turned y (voiced palatal lateral approximant)
        0x028F,  // latin letter small capital y (lax close front rounded)
        0x0290,  // latin small letter z with retroflex hook (voiced retroflex fricative)
        0x0291,  // latin small letter z with curl (voiced alveolo-palatal fricative)
        0x0292,  // latin small letter ezh (voiced postalveolar fricative)
        0x0294,  // latin letter glottal stop (voiceless glottal plosive)
        0x0295,  // latin letter pharyngeal voiced fricative (voiced pharyngeal fricative)
        0x0298,  // latin letter bilabial click (voiceless bilabial click)
        0x0299,  // latin letter small capital b (voiced bilabial trill)
        0x029B,  // latin letter small capital g with hook (voiced uvular implosive)
        0x029C,  // latin letter small capital h (voiceless epiglottal fricative)
        0x029D,  // latin small letter j with crossed tail (voiced palatal fricative)
        0x029F,  // latin letter small capital l (voiced velar lateral approximant)
        0x02A1,  // latin letter glottal stop with stroke (epiglottal plosive)
        0x02A2,  // latin letter reversed glottal stop with stroke (voiced epiglottal fricative)
        0x02B0,  // modifier letter small h (aspirated)
        0x02B2,  // modifier letter small j (palatalized)
        0x02B7,  // modifier letter small w (labialized)
        0x02BC,  // modifier letter apostrophe (ejective)
        0x02C8,  // modifier letter vertical line (primary stress)
        0x02CC,  // modifier letter low vertical line (secondary stress)
        0x02D0,  // modifier letter triangular colon (long)
        0x02D1,  // modifier letter half triangular colon (half-long)
        0x02DE,  // modifier letter rhotic hook (rhotacized)
        0x02E0,  // modifier letter small gamma (velarized)
        0x02E1,  // modifier letter small l (lateral release)
        0x02E4,  // modifier letter small reversed glottal stop (pharyngealized)
        0x02E5,  // modifier letter extra-high tone bar (extra high tone)
        0x02E6,  // modifier letter high tone bar (high tone)
        0x02E7,  // modifier letter mid tone bar (mid tone)
        0x02E8,  // modifier letter low tone bar (low tone)
        0x02E9,  // modifier letter extra-low tone bar (extra low tone)
        0x0303,  // combining tilde (nasalized)
        0x0306,  // combining breve (extra-short)
        0x0308,  // combining diaeresis (centralized)
        0x0318,  // combining left tack below (advanced tongue root)
        0x0319,  // combining right tack below (retracted tongue root)
        0x031A,  // combining left angle above (no audible release)
        0x031C,  // combining left half ring below (less rounded)
        0x031D,  // combining up tack below (raised)
        0x031E,  // combining down tack below (lowered)
        0x031F,  // combining plus sign below (advanced)
        0x0320,  // combining minus sign below (retracted)
        0x0324,  // combining diaeresis below (breathy voiced)
        0x0325,  // combining ring below (voiceless)
        0x0329,  // combining vertical line below (syllabic)
        0x032A,  // combining bridge below (dental)
        0x032C,  // combining caron below (voiced)
        0x032F,  // combining inverted breve below (non-syllabic)
        0x0330,  // combining tilde below (creaky voiced)
        0x0334,  // combining tilde overlay (velarized or pharyngealized)
        0x0339,  // combining right half ring below (more rounded)
        0x033A,  // combining inverted bridge below (apical)
        0x033B,  // combining square below (laminal)
        0x033C,  // combining seagull below (linguolabial)
        0x033D,  // combining x above (mid-centralized)
        0x0361,  // combining double inverted breve (tie bar)
        0x03B2,  // greek small letter beta (voiced bilabial fricative)
        0x03B8,  // greek small letter theta (voiceless dental fricative)
        0x03C7,  // greek small letter chi (voiceless uvular fricative)
        0x2016,  // double vertical line (major group break (intonation))
        0x203F,  // undertie (linking (absence of a break))
        0x207F,  // superscript latin small letter n (nasal release)
        0x2191,  // upwards arrow (global rise)
        0x2193,  // downwards arrow (global fall)
        0x2197,  // north east arrow (global rise)
        0x2198,  // south east arrow (global fall)
        0x2C71,  // latin small letter v with right hook (voiced labiodental tap)
        0xA71B,  // modifier letter raised up arrow (upstep)
        0xA71C,  // modifier letter raised down arrow (downstep)
    ];

    /// <summary>
    /// Characters (in addition to strict) allowed for valid IPA.
    /// </summary>
    private static readonly ImmutableArray<int> s_validIpa =
    [
        0x0067,  // latin small letter g (voiced velar plosive)
        0x0300,  // combining grave accent (low tone)
        0x0301,  // combining acute accent (high tone)
        0x0302,  // combining circumflex accent (falling)
        0x0304,  // combining macron (mid tone)
        0x030A,  // combining ring above (voiceless (above))
        0x030B,  // combining double acute accent (extra high tone)
        0x030C,  // combining caron (rising)
        0x030F,  // combining double grave accent (extra low tone)
        0x035C,  // combining double breve below (tie bar (below))
        0x1DC4,  // combining macron-acute (high rising)
        0x1DC5,  // combining grave-macron (low rising)
        0x1DC6,  // combining macron-grave (low falling)
        0x1DC7,  // combining acute-macron (high falling)
        0x1DC8,  // combining grave-acute-grave (rising-falling)
        0x1DC9,  // combining acute-grave-acute (falling-rising)
    ];

    private static readonly Lazy<FrozenSet<int>> s_strictIpaSet = new(() => s_strictIpa.ToFrozenSet());

    private static readonly Lazy<FrozenSet<int>> s_validIpaSet = new(() => s_validIpa.ToFrozenSet());

    internal static FrozenSet<int> StrictIpaSet { get; } = s_strictIpaSet.Value;

    internal static FrozenSet<int> ValidIpaSet { get; } = s_validIpaSet.Value;
}
