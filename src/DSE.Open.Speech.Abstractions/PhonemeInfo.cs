// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;

namespace DSE.Open.Speech;

public static class PhonemeInfo
{
    /// <summary>
    /// Determines if the specified sound is classified as a consonant.
    /// </summary>
    /// <param name="sound">The sound, transcribed in the International Phonetic Alphabet.</param>
    /// <returns><see langword="true"/> if the specified sound is classified as a consonant, otherwise
    /// <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sound"/> is <see langword="null"/></exception>
    /// <exception cref="ArgumentException"><paramref name="sound"/> contains only whitespace</exception>
    public static bool IsConsonant(string sound)
    {
        Guard.IsNotNullOrWhiteSpace(sound);
        return Consonants.Contains(sound);
    }

    /// <summary>
    /// Determines if the specified sound is classified as a vowel.
    /// </summary>
    /// <param name="sound">The sound, transcribed in the International Phonetic Alphabet.</param>
    /// <returns><see langword="true"/> if the specified sound is classified as a vowel, otherwise
    /// <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sound"/> is <see langword="null"/></exception>
    /// <exception cref="ArgumentException"><paramref name="sound"/> contains only whitespace</exception>
    public static bool IsVowel(string sound)
    {
        Guard.IsNotNullOrWhiteSpace(sound);
        return Vowels.Contains(sound);
    }

    /// <summary>
    /// Determines if the specified sound is classified as a specified <see cref="SpeechSoundType"/>.
    /// </summary>
    /// <param name="sound">The sound, transcribed in the International Phonetic Alphabet.</param>
    /// <param name="speechSoundType"></param>
    /// <returns><see langword="true"/> if the specified sound is classified as the specified <see cref="SpeechSoundType"/>, otherwise
    /// <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="sound"/> is <see langword="null"/></exception>
    /// <exception cref="ArgumentException"><paramref name="sound"/> contains only whitespace</exception>
    public static bool IsSpeechSoundType(string sound, SpeechSoundType speechSoundType)
    {
        Guard.IsNotNullOrWhiteSpace(sound);

        switch (speechSoundType)
        {
            case SpeechSoundType.Consonant:
                return Consonants.Contains(sound);
            case SpeechSoundType.Vowel:
                return Vowels.Contains(sound);
            default:
                ThrowHelper.ThrowArgumentOutOfRangeException();
                return false; // unreachable
        }
    }

    public static readonly FrozenSet<string> CloseVowels = FrozenSet.ToFrozenSet(
    [
        "i", // close front unrounded vowel
        "y", // close front rounded vowel
        "ɨ", // close central unrounded vowel
        "ʉ", // close central rounded vowel
        "ɯ", // close back unrounded vowel
        "u", // close back rounded vowel
    ]);

    public static readonly FrozenSet<string> NearCloseVowels = FrozenSet.ToFrozenSet(
    [
        "ɪ", // near-close near-front unrounded vowel
        "ʏ", // near-close near-front rounded vowel
        "ʊ", // near-close near-back rounded vowel
    ]);

    public static readonly FrozenSet<string> CloseMidVowels = FrozenSet.ToFrozenSet(
    [
        "e", // close-mid front unrounded vowel
        "ø", // close-mid front rounded vowel
        "ɘ", // close-mid central unrounded vowel
        "ɵ", // close-mid central rounded vowel
        "ɤ", // close-mid back unrounded vowel
        "o", // close-mid back rounded vowel
    ]);

    public static readonly FrozenSet<string> MidVowels = FrozenSet.ToFrozenSet(
    [
        "ə", // schwa, mid central vowel
    ]);

    public static readonly FrozenSet<string> OpenMidVowels = FrozenSet.ToFrozenSet(
    [
        "ɛ", // open-mid front unrounded vowel
        "œ", // open-mid front rounded vowel
        "ɜ", // open-mid central unrounded vowel
        "ɞ", // open-mid central rounded vowel
        "ʌ", // open-mid back unrounded vowel
        "ɔ", // open-mid back rounded vowel
    ]);

    public static readonly FrozenSet<string> NearOpenVowels = FrozenSet.ToFrozenSet(
    [
        "æ", // near-open front unrounded vowel
    ]);

    public static readonly FrozenSet<string> OpenVowels = FrozenSet.ToFrozenSet(
    [
        "a", // open front unrounded vowel
        "ɶ", // open front rounded vowel
        "ä", // open central unrounded vowel
        "ɑ", // open back unrounded vowel
        "ɒ"  // open back rounded vowel
    ]);

    public static readonly FrozenSet<string> Vowels = FrozenSet.ToFrozenSet(
    [
        .. CloseVowels,
        .. NearCloseVowels,
        .. CloseMidVowels,
        .. MidVowels,
        .. OpenMidVowels,
        .. NearOpenVowels,
        .. OpenVowels,
    ]);

    public static readonly FrozenSet<string> Bilabials = FrozenSet.ToFrozenSet(
    [
        "p", // voiceless bilabial plosive
        "b", // voiced bilabial plosive
        "m", // bilabial nasal
        "ʙ", // bilabial trill
        "ɸ", // voiceless bilabial fricative
        "β", // voiced bilabial fricative
    ]);

    public static readonly FrozenSet<string> Labiodentals = FrozenSet.ToFrozenSet(
    [
        "f", // voiceless labiodental fricative
        "v", // voiced labiodental fricative
        "ʋ", // labiodental approximant
        "ⱱ", // labiodental flap
    ]);

    public static readonly FrozenSet<string> Dentals = FrozenSet.ToFrozenSet(
    [
        "θ", // voiceless dental fricative
        "ð", // voiced dental fricative
    ]);

    public static readonly FrozenSet<string> Alveolars = FrozenSet.ToFrozenSet(
    [
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
    ]);

    public static readonly FrozenSet<string> PostAlveolars = FrozenSet.ToFrozenSet(
    [
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
    ]);

    public static readonly FrozenSet<string> Palatals = FrozenSet.ToFrozenSet(
    [
        "c", // voiceless palatal plosive
        "ɟ", // voiced palatal plosive
        "ɲ", // palatal nasal
        "ç", // voiceless palatal fricative
        "ʝ", // voiced palatal fricative
        "j", // palatal approximant
        "ʎ", // palatal lateral approximant
    ]);

    public static readonly FrozenSet<string> Velars = FrozenSet.ToFrozenSet(
    [
        "k", // voiceless velar plosive
        "g", // voiced velar plosive
        "ŋ", // velar nasal
        "x", // voiceless velar fricative
        "ɣ", // voiced velar fricative
        "w", // velar approximant
        "ɰ", // velar non-sibilant fricative
    ]);

    public static readonly FrozenSet<string> Uvulars = FrozenSet.ToFrozenSet(
    [
        "q", // voiceless uvular plosive
        "ɢ", // voiced uvular plosive
        "ɴ", // uvular nasal
        "χ", // voiceless uvular fricative
        "ʁ", // voiced uvular fricative
        "ʀ", // uvular trill
    ]);

    public static readonly FrozenSet<string> Pharyngeals = FrozenSet.ToFrozenSet(
    [
        "ħ", // voiceless pharyngeal fricative
        "ʕ", // voiced pharyngeal fricative
    ]);

    public static readonly FrozenSet<string> Glottals = FrozenSet.ToFrozenSet(
    [
        "ʔ", // glottal plosive
        "h", // voiceless glottal fricative
        "ɦ"  // voiced glottal fricative
    ]);

    public static readonly FrozenSet<string> Consonants = FrozenSet.ToFrozenSet(
    [
        .. Bilabials,
        .. Labiodentals,
        .. Dentals,
        .. Alveolars,
        .. PostAlveolars,
        .. Palatals,
        .. Velars,
        .. Uvulars,
        .. Pharyngeals,
        .. Glottals,
    ]);
}
