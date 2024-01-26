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
    public const char OpenFrontUnrounded = (char)0x0061;

    /// <summary>
    /// The Unicode 'latin small letter ae' <c>æ</c> character, used to represent raised open front unrounded.
    /// </summary>
    public const char RaisedOpenFrontUnrounded = (char)0x00E6;

    /// <summary>
    /// The Unicode 'latin small letter turned a' <c>ɐ</c> character, used to represent lowered schwa.
    /// </summary>
    public const char LoweredSchwa = (char)0x0250;

    public const char OpenBackUnrounded = (char)0x0251;  // ɑ,latin small letter alpha,open back unrounded

    public const char OpenBackRounded = (char)0x0252;  // ɒ,latin small letter turned alpha,open back rounded

    public const char VoicedBilabialPlosive = (char)0x0062;  // b,latin small letter b,voiced bilabial plosive

    public const char VoicedBilabialTrill = (char)0x0299;  // ʙ,latin letter small capital b,voiced bilabial trill

    public const char VoicedBilabialImplosive = (char)0x0253;  // ɓ,latin small letter b with hook,voiced bilabial implosive

    public const char VoicelessPalatalPlosive = (char)0x0063;  // c,latin small letter c,voiceless palatal plosive

    public const char VoicelessPalatalFricative = (char)0x00E7;  // ç,latin small letter c with cedilla,voiceless palatal fricative

    public const char VoicelessAlveoloPalatalFricative = (char)0x0255;  // ɕ,latin small letter c with curl,voiceless alveolo-palatal fricative

    public const char VoicedAlveolarPlosive = (char)0x0064;  // d,latin small letter d,voiced alveolar plosive

    public const char VoicedDentalFricative = (char)0x00F0;  // ð,latin small letter eth,voiced dental fricative

    public const char VoicedRetroflexPlosive = (char)0x0256;  // ɖ,latin small letter d with tail,voiced retroflex plosive

    public const char VoicedDentalAlveolarImplosive = (char)0x0257;  // ɗ,latin small letter d with hook,voiced dental/alveolar implosive

    public const char CloseMidFrontUnrounded = (char)0x0065;  // e,latin small letter e,close-mid front unrounded

    public const char MidCentralSchwa = (char)0x0259;  // ə,latin small letter schwa,mid-central schwa

    public const char OpenMidFrontUnrounded = (char)0x025B;  // ɛ,latin small letter open e,open-mid front unrounded

    public const char CloseMidCentralUnrounded = (char)0x0258;  // ɘ,latin small letter reversed e,close-mid central unrounded

    public const char OpenMidCentralUnrounded = (char)0x025C;  // ɜ,latin small letter reversed open e,open-mid central unrounded

    public const char OpenMidCentralRounded = (char)0x025E;  // ɞ,latin small letter closed reversed open e,open-mid central rounded

    public const char VoicelessLabiodentalFricative = (char)0x0066;  // f,latin small letter f,voiceless labiodental fricative

    public const char VoicedVelarPlosive = (char)0x0261;  // ɡ,latin small letter script g,voiced velar plosive

    public const char VoicedUvularPlosive = (char)0x0262;  // ɢ,latin letter small capital g,voiced uvular plosive

    public const char VoicedVelarImplosive = (char)0x0260;  // ɠ,latin small letter g with hook,voiced velar implosive

    public const char VoicedUvularImplosive = (char)0x029B;  // ʛ,latin letter small capital g with hook,voiced uvular implosive

    public const char CloseMidBackUnrounded = (char)0x0264;  // ɤ,latin small letter rams horn,close-mid back unrounded

    public const char VoicedVelarFricative = (char)0x0263;  // ɣ,latin small letter gamma,voiced velar fricative

    public const char VoicelessGlottalFricative = (char)0x0068;  // h,latin small letter h,voiceless glottal fricative

    public const char VoicelessPharyngealFricative = (char)0x0127;  // ħ,latin small letter h with stroke,voiceless pharyngeal fricative

    public const char VoicelessEpiglottalFricative = (char)0x029C;  // ʜ,latin letter small capital h,voiceless epiglottal fricative

    public const char VoicedGlottalFricative = (char)0x0266;  // ɦ,latin small letter h with hook,voiced glottal fricative

    public const char SimultaneousVoicelessPostalveolarVelarFricative = (char)0x0267;  // ɧ,latin small letter heng with hook,simultaneous voiceless postalveolar+velar fricative

    public const char VoicedLabialPalatalApproximant = (char)0x0265;  // ɥ,latin small letter turned h,voiced labial-palatal approximant

    public const char CloseFrontUnrounded = (char)0x0069;  // i,latin small letter i,close front unrounded

    public const char LaxCloseFrontUnrounded = (char)0x026A;  // ɪ,latin letter small capital i,lax close front unrounded

    public const char CloseCentralUnrounded = (char)0x0268;  // ɨ,latin small letter i with stroke,close central unrounded

    public const char VoicedPalatalApproximant = (char)0x006A;  // j,latin small letter j,voiced palatal approximant

    public const char VoicedPalatalFricative = (char)0x029D;  // ʝ,latin small letter j with crossed tail,voiced palatal fricative

    public const char VoicedPalatalPlosive = (char)0x025F;  // ɟ,latin small letter dotless j with stroke,voiced palatal plosive

    public const char VoicedPalatalImplosive = (char)0x0284;  // ʄ,latin small letter dotless j with stroke and hook,voiced palatal implosive

    public const char VoicelessVelarPlosive = (char)0x006B;  // k,latin small letter k,voiceless velar plosive

    public const char VoicedAlveolarLateralApproximant = (char)0x006C;  // l,latin small letter l,voiced alveolar lateral approximant

    public const char VoicedVelarLateralApproximant = (char)0x029F;  // ʟ,latin letter small capital l,voiced velar lateral approximant

    public const char VoicelessAlveolarLateralFricative = (char)0x026C;  // ɬ,latin small letter l with belt,voiceless alveolar lateral fricative

    public const char VoicedRetroflexLateralApproximant = (char)0x026D;  // ɭ,latin small letter l with retroflex hook,voiced retroflex lateral approximant

    public const char VoicedAlveolarLateralFricative = (char)0x026E;  // ɮ,latin small letter lezh,voiced alveolar lateral fricative

    public const char VoicedPalatalLateralApproximant = (char)0x028E;  // ʎ,latin small letter turned y,voiced palatal lateral approximant

    public const char VoicedBilabialNasal = (char)0x006D;  // m,latin small letter m,voiced bilabial nasal

    public const char VoicedLabiodentalNasal = (char)0x0271;  // ɱ,latin small letter m with hook,voiced labiodental nasal

    public const char VoicedAlveolarNasal = (char)0x006E;  // n,latin small letter n,voiced alveolar nasal

    public const char VoicedUvularNasal = (char)0x0274;  // ɴ,latin letter small capital n,voiced uvular nasal

    public const char VoicedPalatalNasal = (char)0x0272;  // ɲ,latin small letter n with left hook,voiced palatal nasal

    public const char VoicedRetroflexNasal = (char)0x0273;  // ɳ,latin small letter n with retroflex hook,voiced retroflex nasal

    public const char VoicedVelarNasal = (char)0x014B;  // ŋ,latin small letter eng,voiced velar nasal

    public const char CloseMidBackRounded = (char)0x006F;  // o,latin small letter o,close-mid back rounded

    public const char CloseMidFrontRounded = (char)0x00F8;  // ø,latin small letter o with stroke,close-mid front rounded

    public const char OpenMidFrontRounded = (char)0x0153;  // œ,latin small ligature oe,open-mid front rounded

    public const char OpenFrontRounded = (char)0x0276;  // ɶ,latin letter small capital oe,open front rounded

    public const char OpenMidBackRounded = (char)0x0254;  // ɔ,latin small letter open o,open-mid back rounded

    public const char CloseMidCentralRounded = (char)0x0275;  // ɵ,latin small letter barred o,close-mid central rounded

    public const char VoicelessBilabialPlosive = (char)0x0070;  // p,latin small letter p,voiceless bilabial plosive

    public const char VoicelessBilabialFricative = (char)0x0278;  // ɸ,latin small letter phi,voiceless bilabial fricative

    public const char VoicelessUvularPlosive = (char)0x0071;  // q,latin small letter q,voiceless uvular plosive

    public const char VoicedAlveolarTrill = (char)0x0072;  // r,latin small letter r,voiced alveolar trill

    public const char VoicedUvularTrill = (char)0x0280;  // ʀ,latin letter small capital r,voiced uvular trill

    public const char VoicedAlveolarApproximant = (char)0x0279;  // ɹ,latin small letter turned r,voiced alveolar approximant

    public const char VoicedAlveolarLateralFlap = (char)0x027A;  // ɺ,latin small letter turned r with long leg,voiced alveolar lateral flap

    public const char VoicedRetroflexApproximant = (char)0x027B;  // ɻ,latin small letter turned r with hook,voiced retroflex approximant

    public const char VoicedRetroflexTap = (char)0x027D;  // ɽ,latin small letter r with tail,voiced retroflex tap

    public const char VoicedAlveolarTap = (char)0x027E;  // ɾ,latin small letter r with fishhook,voiced alveolar tap

    public const char VoicedUvularFricative = (char)0x0281;  // ʁ,latin letter small capital inverted r,voiced uvular fricative

    public const char VoicelessAlveolarFricative = (char)0x0073;  // s,latin small letter s,voiceless alveolar fricative

    public const char VoicelessRetroflexFricative = (char)0x0282;  // ʂ,latin small letter s with hook,voiceless retroflex fricative

    public const char VoicelessPostalveolarFricative = (char)0x0283;  // ʃ,latin small letter esh,voiceless postalveolar fricative

    public const char VoicelessAlveolarPlosive = (char)0x0074;  // t,latin small letter t,voiceless alveolar plosive

    public const char VoicelessRetroflexPlosive = (char)0x0288;  // ʈ,latin small letter t with retroflex hook,voiceless retroflex plosive

    public const char CloseBackRounded = (char)0x0075;  // u,latin small letter u,close back rounded

    public const char CloseCentralRounded = (char)0x0289;  // ʉ,latin small letter u bar,close central rounded

    public const char CloseBackUnrounded = (char)0x026F;  // ɯ,latin small letter turned m,close back unrounded

    public const char VoicedVelarApproximant = (char)0x0270;  // ɰ,latin small letter turned m with long leg,voiced velar approximant

    public const char LaxCloseBackRounded = (char)0x028A;  // ʊ,latin small letter upsilon,lax close back rounded

    public const char VoicedLabiodentalFricative = (char)0x0076;  // v,latin small letter v,voiced labiodental fricative

    public const char VoicedLabiodentalApproximant = (char)0x028B;  // ʋ,latin small letter v with hook,voiced labiodental approximant

    public const char VoicedLabiodentalTap = (char)0x2C71;  // ⱱ,latin small letter v with right hook,voiced labiodental tap

    public const char OpenMidBackUnrounded = (char)0x028C;  // ʌ,latin small letter turned v,open-mid back unrounded

    public const char VoicedLabialVelarApproximant = (char)0x0077;  // w,latin small letter w,voiced labial-velar approximant

    public const char VoicelessLabialVelarFricative = (char)0x028D;  // ʍ,latin small letter turned w,voiceless labial-velar fricative

    public const char VoicelessVelarFricative = (char)0x0078;  // x,latin small letter x,voiceless velar fricative

    public const char CloseFrontRounded = (char)0x0079;  // y,latin small letter y,close front rounded

    public const char LaxCloseFrontRounded = (char)0x028F;  // ʏ,latin letter small capital y,lax close front rounded

    public const char VoicedAlveolarFricative = (char)0x007A;  // z,latin small letter z,voiced alveolar fricative

    public const char VoicedRetroflexFricative = (char)0x0290;  // ʐ,latin small letter z with retroflex hook,voiced retroflex fricative

    public const char VoicedAlveoloPalatalFricative = (char)0x0291;  // ʑ,latin small letter z with curl,voiced alveolo-palatal fricative

    public const char VoicedPostalveolarFricative = (char)0x0292;  // ʒ,latin small letter ezh,voiced postalveolar fricative

    public const char VoicelessGlottalPlosive = (char)0x0294;  // ʔ,latin letter glottal stop,voiceless glottal plosive

    public const char VoicedPharyngealFricative = (char)0x0295;  // ʕ,latin letter pharyngeal voiced fricative,voiced pharyngeal fricative

    public const char EpiglottalPlosive = (char)0x02A1;  // ʡ,latin letter glottal stop with stroke,epiglottal plosive

    public const char VoicedEpiglottalFricative = (char)0x02A2;  // ʢ,latin letter reversed glottal stop with stroke,voiced epiglottal fricative

    public const char VoicelessDentalClick = (char)0x01C0;  // ǀ,latin letter dental click,voiceless dental click

    public const char VoicelessAlveolarLateralClick = (char)0x01C1;  // ǁ,latin letter lateral click,voiceless alveolar lateral click

    public const char VoicelessPalatoalveolarClick = (char)0x01C2;  // ǂ,latin letter alveolar click,voiceless palatoalveolar click

    public const char VoicelessPostalveolarClick = (char)0x01C3;  // ǃ,latin letter retroflex click,voiceless (post)alveolar click

    public const char VoicelessBilabialClick = (char)0x0298;  // ʘ,latin letter bilabial click,voiceless bilabial click

    public const char VoicedBilabialFricative = (char)0x03B2;  // β,greek small letter beta,voiced bilabial fricative

    public const char VoicelessDentalFricative = (char)0x03B8;  // θ,greek small letter theta,voiceless dental fricative

    public const char VoicelessUvularFricative = (char)0x03C7;  // χ,greek small letter chi,voiceless uvular fricative

    public const char VelarizedOrPharyngealized = (char)0x0334;  // ◌̴,combining tilde overlay,velarized or pharyngealized

    public const char Linguolabial = (char)0x033C;  // ◌̼,combining seagull below,linguolabial

    public const char Dental = (char)0x032A;  // ◌̪,combining bridge below,dental

    public const char Laminal = (char)0x033B;  // ◌̻,combining square below,laminal

    public const char Apical = (char)0x033A;  // ◌̺,combining inverted bridge below,apical

    public const char Advanced = (char)0x031F;  // ◌̟,combining plus sign below,advanced

    public const char Retracted = (char)0x0320;  // ◌̠,combining minus sign below,retracted

    public const char Raised = (char)0x031D;  // ◌̝,combining up tack below,raised

    public const char Lowered = (char)0x031E;  // ◌̞,combining down tack below,lowered

    public const char AdvancedTongueRoot = (char)0x0318;  // ◌̘,combining left tack below,advanced tongue root

    public const char RetractedTongueRoot = (char)0x0319;  // ◌̙,combining right tack below,retracted tongue root

    public const char LessRounded = (char)0x031C;  // ◌̜,combining left half ring below,less rounded

    public const char MoreRounded = (char)0x0339;  // ◌̹,combining right half ring below,more rounded

    public const char Voiced = (char)0x032C;  // ◌̬,combining caron below,voiced

    public const char Voiceless = (char)0x0325;  // ◌̥,combining ring below,voiceless

    public const char CreakyVoiced = (char)0x0330;  // ◌̰,combining tilde below,creaky voiced

    public const char BreathyVoiced = (char)0x0324;  // ◌̤,combining diaeresis below,breathy voiced

    public const char Syllabic = (char)0x0329;  // ◌̩,combining vertical line below,syllabic

    public const char NonSyllabic = (char)0x032F;  // ◌̯,combining inverted breve below,non-syllabic

    public const char Nasalized = (char)0x0303;  // ◌̃,combining tilde,nasalized

    public const char Centralized = (char)0x0308;  // ◌̈,combining diaeresis,centralized

    public const char MidCentralized = (char)0x033D;  // ◌̽,combining x above,mid-centralized

    public const char ExtraShort = (char)0x0306;  // ◌̆,combining breve,extra-short

    public const char NoAudibleRelease = (char)0x031A;  // ◌̚,combining left angle above,no audible release

    public const char Rhotacized = (char)0x02DE;  // ◌˞,modifier letter rhotic hook,rhotacized

    public const char LateralRelease = (char)0x02E1;  // ˡ,modifier letter small l,lateral release

    public const char NasalRelease = (char)0x207F;  // ⁿ,superscript latin small letter n,nasal release

    public const char Labialized = (char)0x02B7;  // ʷ,modifier letter small w,labialized

    public const char Palatalized = (char)0x02B2;  // ʲ,modifier letter small j,palatalized

    public const char Velarized = (char)0x02E0;  // ˠ,modifier letter small gamma,velarized

    public const char Pharyngealized = (char)0x02E4;  // ˤ,modifier letter small reversed glottal stop,pharyngealized

    public const char Aspirated = (char)0x02B0;  // ʰ,modifier letter small h,aspirated

    public const char Ejective = (char)0x02BC;  // ʼ,modifier letter apostrophe,ejective

    public const char Long = (char)0x02D0;  // ː,modifier letter triangular colon,long

    public const char HalfLong = (char)0x02D1;  // ˑ,modifier letter half triangular colon,half-long

    public const char TieBar = (char)0x0361;  // ͡,combining double inverted breve,tie bar

    public const char PrimaryStress = (char)0x02C8;  // ˈ,modifier letter vertical line,primary stress

    public const char SecondaryStress = (char)0x02CC;  // ˌ,modifier letter low vertical line,secondary stress

    public const char ExtraHighTone = (char)0x02E5;  // ˥,modifier letter extra-high tone bar,extra high tone

    public const char HighTone = (char)0x02E6;  // ˦,modifier letter high tone bar,high tone

    public const char MidTone = (char)0x02E7;  // ˧,modifier letter mid tone bar,mid tone

    public const char LowTone = (char)0x02E8;  // ˨,modifier letter low tone bar,low tone

    public const char ExtraLowTone = (char)0x02E9;  // ˩,modifier letter extra-low tone bar,extra low tone

    public const char Upstep = (char)0xA71B;  // ꜛ,modifier letter raised up arrow,upstep

    public const char Downstep = (char)0xA71C;  // ꜜ,modifier letter raised down arrow,downstep

    public const char GlobalRiseUp = (char)0x2191;  // ↑,upwards arrow,global rise

    public const char GlobalFallDown = (char)0x2193;  // ↓,downwards arrow,global fall

    public const char GlobalRiseUpRight = (char)0x2197;  // ↗,north east arrow,global rise

    public const char GlobalFallDownRight = (char)0x2198;  // ↘,south east arrow,global fall

    public const char WordBreak = (char)0x0020;  // ,space,word break

    public const char SyllableBreak = (char)0x002E;  // ,full stop,syllable break

    public const char MinorGroupBreakFoot = (char)0x007C;  // |,vertical line,minor group break (foot)

    public const char MajorGroupBreakIntonation = (char)0x2016;  // ‖,double vertical line,major group break (intonation)

    public const char LinkingAbsenceOfABreak = (char)0x203F;  // ‿,undertie,linking (absence of a break)

    // --------------------------------------------------------------------------------------------
    // Additional characters for valid-IPA with Unicode encodings

    /// <summary>
    /// The Unicode 'combining ring above' <c>◌̊</c> character, used to represent voiceless (above). 
    /// </summary>
    public const char VoicelessAlt = (char)0x030A;  // ◌̊,combining ring above,voiceless (above)

    /// <summary>
    /// The Unicode 'latin small letter g' <c>g</c> character, used to represent voiced velar plosive (alt).
    /// </summary>
    public const char VoicedVelarPlosiveAlt = (char)0x0067;  // g,latin small letter g,voiced velar plosive

    /// <summary>
    /// The Unicode 'combining double acute accent' <c>◌̋</c> character, used to represent extra high tone (alt).
    /// </summary>
    public const char ExtraHighToneAlt = (char)0x030B;  // ◌̋,combining double acute accent,extra high tone

    public const char HighToneAlt = (char)0x0301;  // ◌́,combining acute accent,high tone

    public const char MidToneAlt = (char)0x0304;  // ◌̄,combining macron,mid tone

    public const char LowToneAlt = (char)0x0300;  // ◌̀,combining grave accent,low tone

    public const char ExtraLowToneAlt = (char)0x030F;  // ◌̏,combining double grave accent,extra low tone

    public const char Falling = (char)0x0302;  // ◌̂,combining circumflex accent,falling

    public const char Rising = (char)0x030C;  // ◌̌,combining caron,rising

    public const char HighRising = (char)0x1DC4;  // ◌᷄,combining macron-acute,high rising

    public const char LowRising = (char)0x1DC5;  // ◌᷅,combining grave-macron,low rising

    public const char LowFalling = (char)0x1DC6;  // ◌᷆,combining macron-grave,low falling

    public const char HighFalling = (char)0x1DC7;  // ◌᷇,combining acute-macron,high falling

    public const char RisingFalling = (char)0x1DC8;  // ◌᷈,combining grave-acute-grave,rising-falling

    public const char FallingRising = (char)0x1DC9;  // ◌᷉,combining acute-grave-acute,falling-rising

    public const char TieBarBelow = (char)0x035C;  // ◌͜,combining double breve below,tie bar (below)

    /*
    // --------------------------------------------------------------------------------------------
    // Additions to widened-IPA with Unicode encodings

    public const char RetroflexClick = (char)0x203C;  // ‼,double exclamation mark,retroflex click

    public const char VoicedRetroflexImplosive = (char)0x1D91;  // ᶑ,latin small letter d with hook and tail,voiced retroflex implosive

    public const char Fortis = (char)0x0348;  // ◌͈,combining double vertical line below,fortis

    public const char Lenis = (char)0x0349;  // ◌͉,combining left angle below,lenis

    public const char Frictionalized = (char)0x0353;  // ◌͓,combining x below,frictionalized

    public const char Derhoticized = (char)0x032E;  // ◌̮,combining breve below,derhoticized

    public const char NonSibilant = (char)0x0347;  // ◌͇,combining equals sign below,non-sibilant

    public const char Glottalized = (char)0x02C0;  // ◌ˀ,modifier letter glottal stop,glottalized

    public const char VoicedPreAspirated = (char)0x02B1;  // ʱ◌,modifier letter small h with hook,voiced pre-aspirated

    public const char EpilaryngealPhonation = (char)0x1D31;  // ◌ᴱ,modifier letter capital e,epilaryngeal phonation
    
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
