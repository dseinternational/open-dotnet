// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Collections.Generic;
using DSE.Open.Text.Json;

namespace DSE.Open.Speech.Abstractions.Tests;

public class SpeechSoundTests
{
    [Theory]
    [MemberData(nameof(SpeechSounds))]
    public void Equality(SpeechSound p)
    {
        var p2 = SpeechSound.ParseInvariant(p.ToString());
        Assert.Equal(p, p2);
    }

    [Theory]
    [MemberData(nameof(SpeechSounds))]
    public void Serialize(SpeechSound p)
    {
        var json = JsonSerializer.Serialize(p);
        var expected = JsonSerializer.Serialize(p.ToString());

        Assert.Equal(expected, json);
    }

    [Theory]
    [MemberData(nameof(SpeechSounds))]
    public void SerializeWithRelaxedJsonEscaping(SpeechSound p)
    {
        var json = JsonSerializer.Serialize(p, JsonSharedOptions.RelaxedJsonEscaping);
        Assert.Equal($"\"{p}\"", json);
    }

    [Theory]
    [MemberData(nameof(SpeechSounds))]
    public void SerializeDeserialize(SpeechSound p)
    {
        var json = JsonSerializer.Serialize(p);
        var deserialized = JsonSerializer.Deserialize<SpeechSound>(json);
        Assert.Equal(p, deserialized);
    }

    [Theory]
    [MemberData(nameof(SpeechSounds))]
    public void SerializeDeserializeWithRelaxedJsonEscaping(SpeechSound p)
    {
        var json = JsonSerializer.Serialize(p, JsonSharedOptions.RelaxedJsonEscaping);
        var deserialized = JsonSerializer.Deserialize<SpeechSound>(json);
        Assert.Equal(p, deserialized);
    }

    public static TheoryData<SpeechSound> SpeechSounds
    {
        get
        {
            var data = new TheoryData<SpeechSound>()
            {
                SpeechSound.ParseInvariant("b"),
                SpeechSound.ParseInvariant("d"),
                SpeechSound.ParseInvariant("ð"),
                SpeechSound.ParseInvariant("dʒ"),
                SpeechSound.ParseInvariant("f"),
                SpeechSound.ParseInvariant("ɡ"), // U+0261
                SpeechSound.ParseInvariant("h"),
                SpeechSound.ParseInvariant("j"),
                SpeechSound.ParseInvariant("k"),
                SpeechSound.ParseInvariant("l"),
                SpeechSound.ParseInvariant("m"),
                SpeechSound.ParseInvariant("n"),
                SpeechSound.ParseInvariant("ŋ"),
                SpeechSound.ParseInvariant("p"),
                SpeechSound.ParseInvariant("ɹ"),
                SpeechSound.ParseInvariant("s"),
                SpeechSound.ParseInvariant("ʃ"),
                SpeechSound.ParseInvariant("t"),
                SpeechSound.ParseInvariant("tʃ"),
                SpeechSound.ParseInvariant("v"),
                SpeechSound.ParseInvariant("w"),
                SpeechSound.ParseInvariant("z"),
                SpeechSound.ParseInvariant("ʒ"),
                SpeechSound.ParseInvariant("θ"),
                SpeechSound.ParseInvariant("ɒ"),
                SpeechSound.ParseInvariant("ɑː"),
                SpeechSound.ParseInvariant("æ"),
                SpeechSound.ParseInvariant("aɪ"),
                SpeechSound.ParseInvariant("aʊ"),
                SpeechSound.ParseInvariant("ɔː"),
                SpeechSound.ParseInvariant("ɔɪ"),
                SpeechSound.ParseInvariant("ə"),
                SpeechSound.ParseInvariant("eə"),
                SpeechSound.ParseInvariant("eɪ"),
                SpeechSound.ParseInvariant("əʊ"),
                SpeechSound.ParseInvariant("e"),
                SpeechSound.ParseInvariant("ɜː"),
                SpeechSound.ParseInvariant("ɪ"),
                SpeechSound.ParseInvariant("iː"),
                SpeechSound.ParseInvariant("ɪə"),
                SpeechSound.ParseInvariant("ʊ"),
                SpeechSound.ParseInvariant("uː"),
                SpeechSound.ParseInvariant("ʊə"),
                SpeechSound.ParseInvariant("ʌ"),
            };
            return data;
        }
    }

    [Theory]
    [MemberData(nameof(Consonants))]
    public void IsConsonantReturnsTrueForConsonants(string sound)
    {
        Assert.True(SpeechSound.IsConsonant(sound));
    }

    [Theory]
    [MemberData(nameof(Vowels))]
    public void IsConsonantReturnsFalseForVowels(string sound)
    {
        Assert.False(SpeechSound.IsConsonant(sound));
    }

    [Theory]
    [MemberData(nameof(Vowels))]
    public void IsVowelReturnsTrueForVowels(string sound)
    {
        Assert.True(SpeechSound.IsVowel(sound));
    }

    [Theory]
    [MemberData(nameof(Consonants))]
    public void IsVowelReturnsFalseForConsonants(string sound)
    {
        Assert.False(SpeechSound.IsVowel(sound));
    }

    public static TheoryData<string> Consonants
    {
        get
        {
            var data = new TheoryData<string>();
            s_consonants.ForEach(data.Add);
            return data;
        }
    }

    public static TheoryData<string> Vowels
    {
        get
        {
            var data = new TheoryData<string>();
            s_vowels.ForEach(data.Add);
            return data;
        }
    }

    private static readonly string[] s_vowels =
    [
        // Close (High) Vowels
        "i", // close front unrounded vowel
        "y", // close front rounded vowel
        "ɨ", // close central unrounded vowel
        "ʉ", // close central rounded vowel
        "ɯ", // close back unrounded vowel
        "u", // close back rounded vowel

        // Near-Close (Near-High) Vowels
        "ɪ", // near-close near-front unrounded vowel
        "ʏ", // near-close near-front rounded vowel
        "ʊ", // near-close near-back rounded vowel

        // Close-Mid (High-Mid) Vowels
        "e", // close-mid front unrounded vowel
        "ø", // close-mid front rounded vowel
        "ɘ", // close-mid central unrounded vowel
        "ɵ", // close-mid central rounded vowel
        "ɤ", // close-mid back unrounded vowel
        "o", // close-mid back rounded vowel

        // Mid Vowel
        "ə", // schwa, mid central vowel

        // Open-Mid (Low-Mid) Vowels
        "ɛ", // open-mid front unrounded vowel
        "œ", // open-mid front rounded vowel
        "ɜ", // open-mid central unrounded vowel
        "ɞ", // open-mid central rounded vowel
        "ʌ", // open-mid back unrounded vowel
        "ɔ", // open-mid back rounded vowel

        // Near-Open Vowel
        "æ", // near-open front unrounded vowel

        // Open (Low) Vowels
        "a", // open front unrounded vowel
        "ɶ", // open front rounded vowel
        "ä", // open central unrounded vowel
        "ɑ", // open back unrounded vowel
        "ɒ", // open back rounded vowel

        // Vowels - Diphthongs
        "aɪ",
        "aʊ",
        "ɔɪ",
        "eɪ",
        "əʊ",
        "ɪə",
        "eə",
        "ʊə",

        // Vowels - Monophthongs (not above)
        "iː",
        "ɜː",
        "uː",
        "ɔː",
        "ɑː",
        "ɑːr",
        "æ",
    ];

    private static readonly string[] s_consonants =
    [
        // Bilabials
        "p", // voiceless bilabial plosive
        "b", // voiced bilabial plosive
        "m", // bilabial nasal
        "ʙ", // bilabial trill
        "ɸ", // voiceless bilabial fricative
        "β", // voiced bilabial fricative

        // Labiodentals
        "f", // voiceless labiodental fricative
        "v", // voiced labiodental fricative
        "ʋ", // labiodental approximant
        "ⱱ", // labiodental flap

        // Dentals
        "θ", // voiceless dental fricative
        "ð", // voiced dental fricative

        // Alveolars
        "t", // voiceless alveolar plosive
        "d", // voiced alveolar plosive
        "n", // alveolar nasal
        "r", // alveolar trill
        "s", // voiceless alveolar fricative
        "z", // voiced alveolar fricative
        "ɹ", // alveolar approximant
        "ɾ", // alveolar flap
        "ɬ", // voiceless alveolar lateral fricative
        "ɮ", // voiced alveolar lateral fricative
        "l", // alveolar lateral approximant

        // Post-Alveolars
        "ʃ", // voiceless postalveolar fricative
        "ʒ", // voiced postalveolar fricative
        "ʈ", // voiceless retroflex plosive
        "ɖ", // voiced retroflex plosive
        "ɳ", // retroflex nasal
        "ɻ", // retroflex approximant
        "ɽ", // retroflex flap
        "ʂ", // voiceless retroflex fricative
        "ʐ", // voiced retroflex fricative
        "ɭ", // retroflex lateral approximant

        // Palatals
        "c", // voiceless palatal plosive
        "ɟ", // voiced palatal plosive
        "ɲ", // palatal nasal
        "ç", // voiceless palatal fricative
        "ʝ", // voiced palatal fricative
        "j", // palatal approximant
        "ʎ", // palatal lateral approximant

        // Velars
        "k", // voiceless velar plosive
        "ɡ", // voiced velar plosive
        "ŋ", // velar nasal
        "x", // voiceless velar fricative
        "ɣ", // voiced velar fricative
        "w", // velar approximant
        "ɰ", // velar non-sibilant fricative

        // Uvulars
        "q", // voiceless uvular plosive
        "ɢ", // voiced uvular plosive
        "ɴ", // uvular nasal
        "χ", // voiceless uvular fricative
        "ʁ", // voiced uvular fricative
        "ʀ", // uvular trill

        // Pharyngeals
        "ħ", // voiceless pharyngeal fricative
        "ʕ", // voiced pharyngeal fricative

        // Glottals
        "ʔ", // glottal plosive
        "h", // voiceless glottal fricative
        "ɦ",  // voiced glottal fricative


        "tʃ",
        "dʒ",
    ];
}
