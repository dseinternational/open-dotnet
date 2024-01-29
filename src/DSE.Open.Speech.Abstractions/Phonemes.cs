// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using DSE.Open.Globalization;

namespace DSE.Open.Speech;

/// <summary>
/// Defines commonly recognised phonemes.
/// </summary>
[SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Required")]
public static class Phonemes
{
    /// <summary>
    /// Defines commonly recognised phonemes in the English language.
    /// </summary>
    public static class English
    {
        // ---------------------------------------------------------------------
        // Labial consonants

        /// <summary>
        /// The English phoneme /m/ as in ⟨mouse⟩ and ⟨him⟩.
        /// </summary>
        public static readonly Phoneme m = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.VoicedBilabialNasal,
            Allophones =
            [
                // ⟨symphony⟩
                SpeechSound.VoicedLabiodentalNasal,
            ]
        };

        /// <summary>
        /// The English phoneme /p/ as in ⟨pin⟩ and ⟨pin⟩.
        /// </summary>
        public static readonly Phoneme p = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.VoicelessBilabialPlosive,
            Allophones =
            [
                // ⟨spin⟩
                SpeechSound.ParseInvariant("pʰ"),
            ]
        };

        /// <summary>
        /// The English phoneme /b/ as in ⟨bin⟩ and ⟨bit⟩.
        /// </summary>
        public static readonly Phoneme b = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.VoicedBilabialPlosive,
            Allophones =
            [
                // ⟨web⟩
                SpeechSound.ParseInvariant("b̥"),
            ]
        };

        /// <summary>
        /// The English phoneme /f/ as in ⟨find⟩ and ⟨fad⟩.
        /// </summary>
        public static readonly Phoneme f = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.VoicelessLabiodentalFricative,
            Allophones =
            [
                SpeechSound.VoicelessBilabialFricative,
            ]
        };

        /// <summary>
        /// The English phoneme /v/ as in ⟨van⟩ and ⟨vase⟩.
        /// </summary>
        public static readonly Phoneme v = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.VoicedLabiodentalFricative,
            Allophones =
            [
                SpeechSound.VoicedBilabialFricative,
                SpeechSound.ParseInvariant("v̥"),
            ]
        };

        // ---------------------------------------------------------------------
        // Dental consonants

        /// <summary>
        /// The English phoneme /th/ as in ⟨think⟩ and ⟨think⟩.
        /// </summary>
        /// <remarks>
        /// For constrasts with <see cref="dh"/> consider ⟨thigh⟩ and ⟨thy⟩.
        /// </remarks>
        public static readonly Phoneme th = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.VoicelessDentalFricative,
            Allophones =
            [
                SpeechSound.ParseInvariant("t̪ʰ"),
            ]
        };

        /// <summary>
        /// The English phoneme /dh/ (or /<u>th</u>/) as in ⟨feather⟩ and ⟨this⟩.
        /// </summary>
        /// <remarks>
        /// For constrasts with <see cref="th"/> consider ⟨thigh⟩ and ⟨thy⟩.
        /// </remarks>
        public static readonly Phoneme dh = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.VoicedDentalFricative,
            Allophones = []
        };

        // ---------------------------------------------------------------------
        // Alveolar consonants

        /// <summary>
        /// The English phoneme /n/ as in ⟨nod⟩.
        /// </summary>
        public static readonly Phoneme n = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.VoicedAlveolarNasal,
            Allophones = []
        };

        /// <summary>
        /// The English phoneme /t/ as in ⟨top⟩ or ⟨tin⟩.
        /// </summary>
        public static readonly Phoneme t = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.VoicelessAlveolarPlosive,
            Allophones = []
        };

        /// <summary>
        /// The English phoneme /d/ as in ⟨dog⟩ or ⟨din⟩.
        /// </summary>
        public static readonly Phoneme d = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.VoicedAlveolarPlosive,
            Allophones =
            [
                SpeechSound.VoicedAlveolarTap,
                SpeechSound.ParseInvariant("d̥"),
            ]
        };

        /// <summary>
        /// The English phoneme /s/ as in ⟨sing⟩.
        /// </summary>
        public static readonly Phoneme s = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.VoicelessAlveolarFricative,
            Allophones = []
        };

        /// <summary>
        /// The English phoneme /z/ as in ⟨zoo⟩.
        /// </summary>
        public static readonly Phoneme z = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.VoicedAlveolarFricative,
            Allophones = []
        };

        /// <summary>
        /// The English phoneme /l/ as in ⟨long⟩.
        /// </summary>
        public static readonly Phoneme l = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.VoicedAlveolarLateralApproximant,
            Allophones =
            [
                SpeechSound.VoicelessAlveolarLateralFricative,  // ɬ
            ]
        };

        // ---------------------------------------------------------------------
        // Post-alveolar consonants

        /// <summary>
        /// The English phoneme /ch/ as in ⟨church⟩.
        /// </summary>
        public static readonly Phoneme ch = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.VoicelessPostalveolarAffricate,
            Allophones =
            [
                new SpeechSound(
                [
                    SpeechSymbol.VoicelessAlveolarPlosive,
                    SpeechSymbol.VoicelessPostalveolarFricative
                ]),
            ]
        };

        /// <summary>
        /// The English phoneme /j/ as in ⟨judge⟩.
        /// </summary>
        public static readonly Phoneme j = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.VoicedPostalveolarAffricate,
            Allophones = []
        };

        /// <summary>
        /// The English phoneme /sh/ as in ⟨ship⟩.
        /// </summary>
        public static readonly Phoneme sh = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.VoicelessPostalveolarFricative,
            Allophones = []
        };

        /// <summary>
        /// The English phoneme /zh/ as in ⟨vision⟩.
        /// </summary>
        public static readonly Phoneme zh = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.VoicedPostalveolarFricative,
            Allophones = []
        };

        /// <summary>
        /// The English phoneme /r/ as in ⟨run⟩.
        /// </summary>
        /// <remarks>
        /// Our abstraction is the <see cref="SpeechSound.VoicedPostalveolarApproximant"/>,
        /// which is the most common allophone of /r/ in English.
        /// </remarks>
        public static readonly Phoneme r = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.VoicedPostalveolarApproximant,
            Allophones =
            [
                SpeechSound.VoicedAlveolarApproximant,

                // Some American, West Country English, and most Irish dialects.
                SpeechSound.VoicedRetroflexApproximant,

                // "Flapped" or "Tapped" R: alveolar flap [ɾ] (occurs in Scouse and conservative
                // Northern England English, most Scottish English, some South African, Welsh,
                // Indian and Irish English and early twentieth-century Received Pronunciation.
                SpeechSound.VoicedAlveolarTap,

                // "Trilled" or "Rolled" R: alveolar trill [r] (occurs in some conservative
                // Scottish English, South African English, some Welsh English, Indian English
                // and Jersey English)
                SpeechSound.VoicedAlveolarTrill,

                // Some Welsh and Northumbrian accents
                SpeechSound.VoicedUvularFricative,
            ]
        };

        // ---------------------------------------------------------------------
        // Palatal consonant

        /// <summary>
        /// The English phoneme /y/ as in ⟨yes⟩.
        /// </summary>
        public static readonly Phoneme y = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.VoicedPalatalApproximant,
            Allophones = []
        };

        // ---------------------------------------------------------------------
        // Velar consonants

        /// <summary>
        /// The English phoneme /ng/ as in ⟨sing⟩.
        /// </summary>
        public static readonly Phoneme ng = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.VoicedVelarNasal,
            Allophones = []
        };

        /// <summary>
        /// The English phoneme /k/ as in ⟨kick⟩.
        /// </summary>
        public static readonly Phoneme k = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.VoicelessVelarPlosive,
            Allophones = []
        };

        /// <summary>
        /// The English phoneme /g/ as in ⟨gate⟩.
        /// </summary>
        public static readonly Phoneme g = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.VoicedVelarPlosive,
            Allophones = []
        };

        /// <summary>
        /// The English phoneme /x/ as in ⟨loch⟩.
        /// </summary>
        public static readonly Phoneme k͟h = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.VoicelessVelarFricative,
            Allophones = []
        };

        /// <summary>
        /// The English phoneme /w/ as in ⟨wine⟩ and ⟨wine⟩.
        /// </summary>
        /// <remarks>
        /// Where the <see href="https://en.wikipedia.org/wiki/Wine%E2%80%93whine_merger">wine–whine merger</see>
        /// is present, there is no consonant difference in words like ⟨wine⟩ and ⟨whine⟩ (<see cref="hw"/>).
        /// </remarks>
        public static readonly Phoneme w = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.VoicedLabialVelarApproximant,
            Allophones =
            [
                SpeechSound.VoicelessLabialVelarFricative,
                SpeechSound.ParseInvariant("hw"),
            ]
        };

        /// <summary>
        /// The English phoneme /hw/ as in ⟨which⟩.
        /// </summary>
        /// <remarks>
        /// Where the <see href="https://en.wikipedia.org/wiki/Wine%E2%80%93whine_merger">wine–whine merger</see>
        /// is present, there is no consonant difference in words like ⟨wine⟩ and ⟨whine⟩ (<see cref="hw"/>).
        /// </remarks>
        public static readonly Phoneme hw = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.VoicelessLabialVelarFricative,
            Allophones =
            [
                SpeechSound.ParseInvariant("hw"),
            ]
        };

        // ---------------------------------------------------------------------
        // Glottal consonant

        /// <summary>
        /// The English phoneme /h/ as in ⟨hat⟩ and ⟨ham⟩.
        /// </summary>
        public static readonly Phoneme h = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.VoicelessGlottalFricative,
            Allophones = []
        };

        // ---------------------------------------------------------------------
        // Vowels - front

        /// <summary>
        /// The English phoneme /i/ as in ⟨bit⟩.
        /// </summary>
        public static readonly Phoneme i = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.NearCloseNearFrontUnrounded,
            Allophones = []
        };

        /// <summary>
        /// The English phoneme /e/ as in ⟨let⟩.
        /// </summary>
        public static readonly Phoneme e = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.OpenMidFrontUnroundedVowel,
            Allophones =
            [
                SpeechSound.ParseInvariant("ɛː")
            ]
        };

        /// <summary>
        /// The English phoneme /a/ as in ⟨cat⟩, ⟨trap⟩ and ⟨ham⟩.
        /// </summary>
        public static readonly Phoneme a = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.NearOpenFrontUnroundedVowel,
            Allophones =
            [
                SpeechSound.OpenFrontUnroundedVowel,
                // See also https://en.wikipedia.org/wiki/Open_central_unrounded_vowel
            ]
        };

        /// <summary>
        /// The English phoneme /ee/ as in ⟨see⟩.
        /// </summary>
        public static readonly Phoneme ee = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.ParseInvariant("iː"),
            Allophones =
            [
                SpeechSound.CloseFrontUnroundedVowel
            ]
        };

        // ---------------------------------------------------------------------
        // Vowels - central

        /// <summary>
        /// The English phoneme /uu/ as in ⟨took⟩.
        /// </summary>
        public static readonly Phoneme uu = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.NearCloseNearBackRoundedVowel,
            Allophones = []
        };

        /// <summary>
        /// The English phoneme /u/ as in ⟨cut⟩.
        /// </summary>
        public static readonly Phoneme u = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.OpenMidBackUnroundedVowel,
            Allophones = []
        };

        /// <summary>
        /// The English phoneme /uh/ as in ⟨about⟩, ⟨comma⟩ and ⟨letter⟩.
        /// </summary>
        public static readonly Phoneme uh = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.MidCentralVowel,
            Allophones =
            [
                SpeechSound.ParseInvariant("ər"),
                SpeechSound.ParseInvariant("ɐ"),   // [ɐbˈaʊt] ⟨about⟩
            ]
        };

        /// <summary>
        /// The English phoneme /oo/ as in ⟨soon⟩.
        /// </summary>
        public static readonly Phoneme oo = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.ParseInvariant("uː"),
            Allophones = []
        };



        // ---------------------------------------------------------------------
        // Vowels - back

        /// <summary>
        /// The English phoneme /aw/ as in ⟨caught⟩.
        /// </summary>
        /// <remarks>
        /// Where the <see href="https://en.wikipedia.org/wiki/Cot%E2%80%93caught_merger">cot–caught merger</see>
        /// and father–bother merger are present, there is no vowel difference in words like ⟨palm⟩ /ɑ/,
        /// ⟨pot⟩ /ɒ/, and ⟨thought⟩ /ɔ/.
        /// <para>Also see <see cref="or"/>.</para>
        /// </remarks>
        public static readonly Phoneme aw = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.ParseInvariant("ɔː"),
            Allophones =
            [
                SpeechSound.OpenMidBackRoundedVowel,    // cot-caught merger
                SpeechSound.OpenBackUnroundedVowel,
            ]
        };

        /// <summary>
        /// The English phoneme /o/ as in ⟨pot⟩.
        /// </summary>
        /// <remarks>
        /// Where the <see href="https://en.wikipedia.org/wiki/Cot%E2%80%93caught_merger">cot–caught merger</see>
        /// and father–bother merger are present, there is no vowel difference in words like ⟨palm⟩ /ɑ/,
        /// ⟨pot⟩ /ɒ/, and ⟨thought⟩ /ɔ/.
        /// </remarks>
        public static readonly Phoneme o = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.OpenBackRoundedVowel,
            Allophones = []
        };

        /// <summary>
        /// The English phoneme /ah/ as in ⟨father⟩. (See also <see cref="ar"/>.)
        /// </summary>
        /// <remarks>
        /// <para>
        /// The distinction between /ar/ ([ɑːr]) and /ah/ ([ɑː]) is subtle.
        /// </para>
        /// <para>
        /// Where the <see href="https://en.wikipedia.org/wiki/Cot%E2%80%93caught_merger">cot–caught merger</see>
        /// and father–bother merger are present, there is no vowel difference in words like ⟨palm⟩ /ɑ/,
        /// ⟨pot⟩ /ɒ/, and ⟨thought⟩ /ɔ/.
        /// </para>
        /// </remarks>
        public static readonly Phoneme ah = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.ParseInvariant("ɑː"),
            Allophones =
            [
                SpeechSound.NearOpenFrontUnroundedVowel, // bath like trap
            ]
        };

        // ---------------------------------------------------------------------
        // Vowels - diphthongs

        /// <summary>
        /// The English phoneme /ay/ as in ⟨day⟩.
        /// </summary>
        public static readonly Phoneme ay = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.ParseInvariant("eɪ"),
            Allophones = []
        };

        /// <summary>
        /// The English phoneme /igh/ as in ⟨by⟩ or ⟨high⟩.
        /// </summary>
        public static readonly Phoneme igh = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.ParseInvariant("aɪ"),
            Allophones =
            [
                SpeechSound.OpenFrontUnroundedVowel,
                SpeechSound.ParseInvariant("aɪ"),
                SpeechSound.ParseInvariant("ɐɪ"),
                SpeechSound.ParseInvariant("ɑɪ"),
            ]
        };

        /// <summary>
        /// The English phoneme /oh/ as in ⟨no⟩ and ⟨goat⟩.
        /// </summary>
        public static readonly Phoneme oh = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.ParseInvariant("oʊ"),
            Allophones =
            [
                SpeechSound.ParseInvariant("əʊ"),
            ]
        };

        /// <summary>
        /// The English phoneme /oy/ as in ⟨noise⟩.
        /// </summary>
        public static readonly Phoneme oy = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.ParseInvariant("ɔɪ"),
            Allophones = []
        };
        /// <summary>
        /// The English phoneme /ow/ as in ⟨out⟩.
        /// </summary>
        public static readonly Phoneme ow = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.ParseInvariant("aʊ"),
            Allophones = []
        };

        // ---------------------------------------------------------------------
        // Vowels - r-controlled

        /// <summary>
        /// The English phoneme /ur/ as in ⟨word⟩ and ⟨nurse⟩.
        /// </summary>
        public static readonly Phoneme ur = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.ParseInvariant("ɜː"),
            Allophones =
            [
                SpeechSound.ParseInvariant("ɜr"),
                SpeechSound.ParseInvariant("ɜːr"),
            ]
        };

        /// <summary>
        /// The English phoneme /ar/ as in ⟨arm⟩. (See also <see cref="ah"/>.)
        /// </summary>
        /// <remarks>
        /// The distinction between /ar/ ([ɑːr]) and /ah/ ([ɑː]) is subtle.
        /// </remarks>
        public static readonly Phoneme ar = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.ParseInvariant("ɑːr"), // distinguish from /ah/
            Allophones =
            [
                SpeechSound.ParseInvariant("ɑr"),
                SpeechSound.ParseInvariant("ɑː"),
                SpeechSound.ParseInvariant("ɜ˞"),
            ]
        };

        /// <summary>
        /// The English phoneme /or/ as in ⟨north⟩ and ⟨force⟩.
        /// </summary>
        public static readonly Phoneme or = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.ParseInvariant("ɔːr"), // distinguish from /aw/
            Allophones =
            [
                SpeechSound.ParseInvariant("oʊr"),
            ]
        };

        /// <summary>
        /// The English phoneme /air/ as in ⟨hair⟩ and ⟨square⟩.
        /// </summary>
        public static readonly Phoneme air = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.ParseInvariant("ɛː"),
            Allophones =
            [
                SpeechSound.ParseInvariant("ɛr"),
                SpeechSound.ParseInvariant("ɛːr"),
                SpeechSound.ParseInvariant("ɛə"),
                SpeechSound.ParseInvariant("ɛəɹ"),
                SpeechSound.ParseInvariant("eɹ"),
                SpeechSound.ParseInvariant("ɛɹ"),
                SpeechSound.ParseInvariant("eə"),
                SpeechSound.ParseInvariant("eəɹ"),
            ]
        };

        /// <summary>
        /// The English phoneme /eer/ (or /ear/) as in ⟨here⟩ or ⟨ear⟩.
        /// </summary>
        public static readonly Phoneme eer = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.ParseInvariant("ɪə"),
            Allophones =
            [
                SpeechSound.ParseInvariant("iə"),
                SpeechSound.ParseInvariant("ɪər"),
                SpeechSound.ParseInvariant("ɪər"),
                SpeechSound.ParseInvariant("ɪr"),
                SpeechSound.ParseInvariant("iɹ"),
            ]
        };

        /// <summary>
        /// The English phoneme /oor/ as in ⟨tour⟩.
        /// </summary>
        public static readonly Phoneme oor = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.ParseInvariant("ʊər"),
            Allophones = []
        };

        /// <summary>
        /// The English phoneme /er/ as in ⟨butter⟩.
        /// </summary>
        public static readonly Phoneme er = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.ParseInvariant("ər"),
            Allophones = []
        };

        // ---------------------------------------------------------------------
        // Vowels - other

        /// <summary>
        /// The English phoneme /yoo/ as in ⟨view⟩.
        /// </summary>
        public static readonly Phoneme yoo = new()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.ParseInvariant("juː"),
            Allophones = []
        };


        // ---------------------------------------------------------------------

        public static readonly FrozenSet<Phoneme> Consonants = FrozenSet.ToFrozenSet(
        [
            b,
            ch,
            d,
            dh,
            f,
            g,
            h,
            hw,
            j,
            k,
            k͟h,
            l,
            m,
            n,
            ng,
            p,
            r,
            s,
            sh,
            t,
            th,
            v,
            w,
            y,
            z,
            zh,
        ]);

        public static readonly FrozenSet<Phoneme> Vowels = FrozenSet.ToFrozenSet(
        [
            a,
            ah,
            air,
            ar,
            aw,
            ay,
            e,
            ee,
            eer,
            er,
            i,
            igh,
            o,
            oh,
            oo,
            oor,
            or,
            ow,
            oy,
            u,
            uh,
            ur,
            uu,
            yoo,            
        ]);

        public static readonly FrozenSet<Phoneme> All = FrozenSet.ToFrozenSet(Consonants.Union(Vowels));
    }
}
