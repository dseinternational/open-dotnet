// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Speech.Serialization;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Speech;

/// <summary>
/// Identifies a speech sound (phone).
/// </summary>
/// <remarks>See <see href="https://en.wikipedia.org/wiki/International_Phonetic_Alphabet"/></remarks>
[JsonConverter(typeof(JsonStringSpeechSoundConverter))]
[StructLayout(LayoutKind.Auto)]
public readonly struct SpeechSound : IEquatable<SpeechSound>, ISpanFormattable, ISpanParsable<SpeechSound>
{
    private readonly string _value;
    private readonly bool _initialized;

    public static int MaxLength => 4;

    public static readonly SpeechSound Empty;

    public SpeechSound(char value) : this(value.ToString())
    {
    }

    public SpeechSound(string value) : this(value, false)
    {
    }

    public SpeechSound(ReadOnlySpan<char> value) : this(value, false)
    {
    }

    private SpeechSound(ReadOnlySpan<char> value, bool skipValidation)
    {
        if (!skipValidation)
        {
            EnsureValidValue(value);
        }

        _value = SpeechSoundStringPool.Shared.GetOrAdd(value);
        _initialized = true;
    }

    private SpeechSound(string value, bool skipValidation)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        if (!skipValidation)
        {
            EnsureValidValue(value);
        }

        _value = string.IsInterned(value) ?? SpeechSoundStringPool.Shared.GetOrAdd(value);
        _initialized = true;
    }

    public static bool IsValidValue(ReadOnlySpan<char> value)
    {
        return value.Length > 0
            && value.Length < MaxLength
            && value.All(SpeechSymbol.IsStrictIpaSymbol);
    }

    private static void EnsureValidValue(ReadOnlySpan<char> value)
    {
        if (!IsValidValue(value))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
            return; // Unreachable
        }
    }

    public int Length => _value.Length;

    public SpeechSound Long()
    {
        return new(_value + "ː", true);
    }

    public SpeechSound HalfLong()
    {
        return new(_value + "ˑ", true);
    }

    public override bool Equals(object? obj)
    {
        return obj is SpeechSound ph && Equals(ph);
    }

    public bool Equals(SpeechSound other)
    {
        return string.Equals(_value, other._value, StringComparison.Ordinal);
    }

    public override int GetHashCode()
    {
        return string.GetHashCode(_value, StringComparison.Ordinal);
    }

    public override string ToString()
    {
        return _value;
    }

    public string ToStringInvariant()
    {
        return _value;
    }

    /// <summary>
    /// Gets a (phonetic) <see cref="Transcription"/> representation of the speech sound.
    /// </summary>
    /// <returns></returns>
    public Transcription ToTranscription()
    {
        return Transcription.Phonetic(_value);
    }

    // TODO: formatting options: escaped Unicode? binary format?

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        var span = _value.AsSpan();

        if (span.TryCopyTo(destination))
        {
            charsWritten = span.Length;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return _value is null
            ? string.Empty
            : _value;
    }

    public static SpeechSound Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : ThrowHelper.ThrowFormatException<SpeechSound>($"Could not parse {nameof(SpeechSound)}");
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out SpeechSound result)
    {
        s = s.Trim();

        if (s.IsEmpty)
        {
            result = default;
            return true;
        }

        if (s.Length > MaxLength)
        {
            result = default;
            return false;
        }

        result = new SpeechSound(s, true);
        return true;
    }

    public static SpeechSound ParseInvariant(string s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    public static SpeechSound Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out SpeechSound result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    public static bool operator ==(SpeechSound left, SpeechSound right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(SpeechSound left, SpeechSound right)
    {
        return !(left == right);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates
    public static SpeechSound operator +(SpeechSound left, SpeechSound right)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return new SpeechSound(left._value + right._value);
    }

    /// <summary>
    /// Converts a <see cref="SpeechSound"/> to a (phonetic) <see cref="Transcription"/>.
    /// </summary>
    /// <param name="value"></param>
    public static implicit operator Transcription(SpeechSound value)
    {
        return value.ToTranscription();
    }

    /// <summary>
    /// Determines if the specified sound is classified as a consonant.
    /// </summary>
    /// <param name="sound"></param>
    /// <returns><see langword="true"/> if the specified sound is classified as a consonant, otherwise
    /// <see langword="false"/>.</returns>
    public static bool IsConsonant(SpeechSound sound)
    {
        return IsConsonant(sound._value);
    }

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
        ArgumentException.ThrowIfNullOrWhiteSpace(sound);
        return Consonants.Contains(sound) || Consonants.Contains(sound[0].ToString());
    }

    /// <summary>
    /// Determines if the specified sound is classified as a vowel.
    /// </summary>
    /// <param name="sound"></param>
    /// <returns><see langword="true"/> if the specified sound is classified as a vowel, otherwise
    /// <see langword="false"/>.</returns>
    public static bool IsVowel(SpeechSound sound)
    {
        return IsVowel(sound._value);
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
        ArgumentException.ThrowIfNullOrWhiteSpace(sound);

        return Vowels.Contains(sound)
            || Vowels.Contains(sound[0].ToString())
            || sound.StartsWith("ju", StringComparison.Ordinal);
    }

    /// <summary>
    /// The voiceless bilabial plosive, a type of consonantal sound
    /// represented in the IPA by the symbol <c>⟨p⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiceless_bilabial_plosive"/></remarks>
    public static readonly SpeechSound VoicelessBilabialPlosive = new([SpeechSymbol.VoicelessBilabialPlosive], true);

    /// <summary>
    /// The voiced bilabial plosive or stop, a type of consonantal sound
    /// represented in the IPA by the symbol <c>⟨b⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_bilabial_plosive"/></remarks>
    public static readonly SpeechSound VoicedBilabialPlosive = new("b", true);

    /// <summary>
    /// The voiced bilabial nasal, a type of consonantal sound
    /// represented in the IPA by the symbol <c>⟨m⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_bilabial_nasal"/></remarks>
    public static readonly SpeechSound VoicedBilabialNasal = new("m", true);

    /// <summary>
    /// The voiced bilabial trill, a type of consonantal sound
    /// represented in the IPA by the symbol <c>⟨ʙ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_bilabial_trill"/></remarks>
    public static readonly SpeechSound VoicedBilabialTrill = new("ʙ", true);

    /// <summary>
    /// The voiceless bilabial fricative, a type of consonantal sound
    /// represented in the IPA by the symbol <c>⟨ɸ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiceless_bilabial_fricative"/></remarks>
    public static readonly SpeechSound VoicelessBilabialFricative = new("ɸ", true);

    /// <summary>
    /// The voiced bilabial fricative, a type of consonantal sound
    /// represented in the IPA by the symbol <c>⟨β⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_bilabial_fricative"/></remarks>
    public static readonly SpeechSound VoicedBilabialFricative = new("β", true);

    /// <summary>
    /// The voiced labiodental nasal, a type of consonantal sound
    /// represented in the IPA by the symbol <c>⟨ɱ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_labiodental_nasal"/></remarks>
    public static readonly SpeechSound VoicedLabiodentalNasal = new("ɱ", true);

    /// <summary>
    /// The voiced labiodental flap, a type of consonantal sound
    /// represented in the IPA by the symbol <c>⟨ⱱ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_labiodental_flap"/></remarks>
    public static readonly SpeechSound VoicedLabiodentalFlap = new("ⱱ", true);

    /// <summary>
    /// The voiceless labiodental fricative, a type of consonantal sound
    /// represented in the IPA by the symbol <c>⟨f⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiceless_labiodental_fricative"/></remarks>
    public static readonly SpeechSound VoicelessLabiodentalFricative = new("f", true);

    /// <summary>
    /// The voiced labiodental fricative is a type of consonantal sound
    /// represented in the IPA by the symbol <c>⟨v⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_labiodental_fricative"/></remarks>
    public static readonly SpeechSound VoicedLabiodentalFricative = new("v", true);

    /// <summary>
    /// The voiced labiodental approximant is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨ʋ⟩</c>.
    /// </summary>
    public static readonly SpeechSound VoicedLabiodentalApproximant = new("ʋ", true);

    /// <summary>
    /// The voiceless alveolar plosive is a type of consonantal sound
    /// represented in the IPA by the symbols <c>⟨t⟩</c>.
    /// </summary>
    public static readonly SpeechSound VoicelessAlveolarPlosive = new("t", true);

    /// <summary>
    /// The voiced alveolar plosive is a type of consonantal sound
    /// represented in the IPA by the symbol <c>⟨d⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_dental_and_alveolar_plosives"/></remarks>
    public static readonly SpeechSound VoicedAlveolarPlosive = new("d", true);

    /// <summary>
    /// The voiced alveolar nasal is a type of consonantal sound
    /// represented in the IPA by the symbol <c>⟨n⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_dental,_alveolar_and_postalveolar_nasals"/></remarks>
    public static readonly SpeechSound VoicedAlveolarNasal = new("n", true);

    public static readonly SpeechSound VoicedAlveolarTrill = new("r", true);

    /// <summary>
    /// The voiced alveolar tap or flap is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨ɾ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_dental_and_alveolar_taps_and_flaps"/></remarks>
    public static readonly SpeechSound VoicedAlveolarTap = new("ɾ", true);

    /// <summary>
    /// The voiceless dental non-sibilant fricative is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨θ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiceless_dental_fricative"/></remarks>
    public static readonly SpeechSound VoicelessDentalFricative = new("θ", true);

    /// <summary>
    /// The voiced dental fricative is a consonant sound,
    /// represented in the IPA by the symbol <c>⟨ð⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_dental_fricative"/></remarks>
    public static readonly SpeechSound VoicedDentalFricative = new("ð", true);

    /// <summary>
    /// The voiceless alveolar fricative is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨s⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiceless_alveolar_fricative"/></remarks>
    public static readonly SpeechSound VoicelessAlveolarFricative = new("s", true);

    /// <summary>
    /// The voiced alveolar fricative is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨z⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_alveolar_fricative"/></remarks>
    public static readonly SpeechSound VoicedAlveolarFricative = new("z", true);

    /// <summary>
    /// A voiceless postalveolar fricative is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨ʃ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiceless_postalveolar_fricative"/></remarks>
    public static readonly SpeechSound VoicelessPostalveolarFricative = new("ʃ", true);

    /// <summary>
    /// The voiceless palato-alveolar sibilant affricate or voiceless domed postalveolar sibilant
    /// affricate is a type of consonantal sound, represented in the IPA by the symbol <c>tʃ</c>.
    /// </summary>
    public static readonly SpeechSound VoicelessPostalveolarAffricate = new("tʃ", true);

    /// <summary>
    /// A voiced postalveolar fricative is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨ʒ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_postalveolar_fricative"/></remarks>
    public static readonly SpeechSound VoicedPostalveolarFricative = new("ʒ", true);

    /// <summary>
    /// The voiced alveolar approximant is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨ɹ̠⟩</c>. (See also <see cref="VoicedAlveolarApproximant"/>)
    /// </summary>
    /// <remarks>
    /// The most common sound represented by the letter r in English is the voiced
    /// postalveolar approximant, pronounced a little more back and transcribed more precisely
    /// in IPA as ⟨ɹ̠⟩, but ⟨ɹ⟩ is often used for convenience in its place.
    /// </remarks>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_alveolar_and_postalveolar_approximants"/></remarks>
    public static readonly SpeechSound VoicedPostalveolarApproximant = new("ɹ̠", true);

    public static readonly SpeechSound VoicedPostalveolarAffricate = new("dʒ", true);

    public static readonly SpeechSound VoicelessLateralAlveolarFricative = new("ɬ", true);

    public static readonly SpeechSound VoicedLateralAlveolarFricative = new("ɮ", true);

    /// <summary>
    /// The voiced alveolar approximant is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨ɹ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_alveolar_and_postalveolar_approximants"/></remarks>
    public static readonly SpeechSound VoicedAlveolarApproximant = new("ɹ", true);

    /// <summary>
    /// The voiced alveolar lateral approximant is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨l⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_dental,_alveolar_and_postalveolar_lateral_approximants"/></remarks>
    public static readonly SpeechSound VoicedAlveolarLateralApproximant = new("l", true);

    public static readonly SpeechSound VoicelessRetroflexPlosive = new("ʈ", true);

    public static readonly SpeechSound VoicedRetroflexPlosive = new("ɖ", true);

    public static readonly SpeechSound VoicedRetroflexNasal = new("ɳ", true);

    public static readonly SpeechSound VoicedRetroflexFlap = new("ɽ", true);

    public static readonly SpeechSound VoicelessRetroflexFricative = new("ʂ", true);

    public static readonly SpeechSound VoicedRetroflexFricative = new("ʐ", true);

    /// <summary>
    /// The voiced retroflex approximant is a type of consonant,
    /// represented in the IPA by the symbol <c>⟨ɻ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_retroflex_approximant"/></remarks>
    public static readonly SpeechSound VoicedRetroflexApproximant = new("ɻ", true);

    public static readonly SpeechSound VoicedRetroflexLateralApproximant = new("ɭ", true);

    public static readonly SpeechSound VoicelessPalatalPlosive = new("c", true);

    public static readonly SpeechSound VoicedPalatalPlosive = new("ɟ", true);

    public static readonly SpeechSound VoicedPalatalNasal = new("ɲ", true);

    public static readonly SpeechSound VoicelessPalatalFricative = new("ç", true);

    public static readonly SpeechSound VoicedPalatalFricative = new("ʝ", true);

    /// <summary>
    /// The voiced palatal approximant, or yod, is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨j⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_palatal_approximant"/></remarks>
    public static readonly SpeechSound VoicedPalatalApproximant = new("j", true);

    public static readonly SpeechSound VoicedPalatalLateralApproximant = new("ʎ", true);

    /// <summary>
    /// The voiceless velar plosive or stop is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨k⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiceless_velar_plosive"/></remarks>
    public static readonly SpeechSound VoicelessVelarPlosive = new("k", true);

    public static readonly SpeechSound VoicedVelarPlosive = new("ɡ", true);

    /// <summary>
    /// The voiced velar nasal, also known as agma, is a type of consonantal sound
    /// represented in the IPA by the symbol <c>⟨ŋ⟩</c>.
    /// </summary>
    public static readonly SpeechSound VoicedVelarNasal = new("ŋ", true);

    /// <summary>
    /// The voiceless velar fricative is a type of consonantal sound used,
    /// represented in the IPA by the symbol <c>⟨x⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiceless_velar_fricative"/></remarks>
    public static readonly SpeechSound VoicelessVelarFricative = new("x", true);

    public static readonly SpeechSound VoicedVelarFricative = new("ɣ", true);

    /// <summary>
    /// The voiced labial–velar approximant is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨w⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_labial%E2%80%93velar_approximant"/></remarks>
    public static readonly SpeechSound VoicedLabialVelarApproximant = new("w", true);

    /// <summary>
    /// The voiceless labial–velar fricative is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨ʍ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiceless_labial%E2%80%93velar_fricative"/></remarks>
    public static readonly SpeechSound VoicelessLabialVelarFricative = new("ʍ", true);

    public static readonly SpeechSound VoicedVelarApproximant = new("ɰ", true);

    public static readonly SpeechSound VoicedVelarLateralApproximant = new("ʟ", true);

    public static readonly SpeechSound VoicelessUvularPlosive = new("q", true);

    public static readonly SpeechSound VoicedUvularPlosive = new("ɢ", true);

    public static readonly SpeechSound VoicedUvularNasal = new("ɴ", true);

    public static readonly SpeechSound VoicedUvularTrill = new("ʀ", true);

    public static readonly SpeechSound VoicelessUvularFricative = new("χ", true);

    /// <summary>
    /// The voiced uvular fricative is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨ʁ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_uvular_fricative"/></remarks>
    public static readonly SpeechSound VoicedUvularFricative = new("ʁ", true);

    public static readonly SpeechSound VoicelessPharyngealFricative = new("ħ", true);

    public static readonly SpeechSound VoicedPharyngealFricative = new("ʕ", true);

    public static readonly SpeechSound VoicelessGlottalPlosive = new("ʔ", true);

    /// <summary>
    /// The voiceless glottal fricative, sometimes called voiceless glottal transition or the aspirate,
    /// is a type of sound used in some spoken languages that patterns like a fricative or approximant
    /// consonant phonologically, but often lacks the usual phonetic characteristics of a consonant.
    /// The symbol in the International Phonetic Alphabet that represents this sound is ⟨h⟩.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiceless_glottal_fricative"/></remarks>
    public static readonly SpeechSound VoicelessGlottalFricative = new("h", true);

    public static readonly SpeechSound VoicedGlottalFricative = new("ɦ", true);

    /// <summary>
    /// The close front unrounded vowel, or high front unrounded vowel, is a type of vowel sound,
    /// represented in the IPA by the symbol <c>⟨i⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Close_front_unrounded_vowel"/></remarks>
    public static readonly SpeechSound CloseFrontUnroundedVowel = new("i", true);

    public static readonly SpeechSound CloseFrontRoundedVowel = new("y", true);

    /// <summary>
    /// The near-close near-front unrounded vowel, or near-high near-front unrounded vowel, is a type of vowel sound,
    /// represented in the IPA by the symbol <c>⟨ɪ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Near-close_near-front_unrounded_vowel"/></remarks>
    public static readonly SpeechSound NearCloseNearFrontUnroundedVowel = new("ɪ", true);

    public static readonly SpeechSound LoweredCloseFrontRoundedVowel = new("ʏ", true);

    public static readonly SpeechSound CloseMidFrontUnroundedVowel = new("e", true);

    public static readonly SpeechSound CloseMidFrontRoundedVowel = new("ø", true);

    /// <summary>
    /// The open-mid front unrounded vowel, or low-mid front unrounded vowel, is a type of vowel sound,
    /// represented in the IPA by the symbol <c>⟨ɛ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Open-mid_front_unrounded_vowel"/></remarks>
    public static readonly SpeechSound OpenMidFrontUnroundedVowel = new("ɛ", true);

    public static readonly SpeechSound OpenMidFrontRoundedVowel = new("œ", true);

    /// <summary>
    /// The near-open front unrounded vowel, or near-low front unrounded vowel, is a type of vowel sound,
    /// represented in the IPA by the symbol <c>⟨æ⟩</c>.
    /// </summary>
    public static readonly SpeechSound NearOpenFrontUnroundedVowel = new("æ", true);

    /// <summary>
    /// The open front unrounded vowel, or low front unrounded vowel, is a type of vowel sound,
    /// represented in the IPA by the symbol <c>⟨a⟩</c>.
    /// </summary>
    public static readonly SpeechSound OpenFrontUnroundedVowel = new("a", true);

    public static readonly SpeechSound OpenFrontRoundedVowel = new("ɶ", true);

    public static readonly SpeechSound CloseCentralUnroundedVowel = new("ɨ", true);

    public static readonly SpeechSound CloseCentralRoundedVowel = new("ʉ", true);

    public static readonly SpeechSound CloseMidCentralUnroundedVowel = new("ɘ", true);

    public static readonly SpeechSound CloseMidCentralRoundedVowel = new("ɵ", true);

    /// <summary>
    /// The mid central vowel (also known as schwa) is a type of vowel sound,
    /// represented in the IPA by the symbol <c>⟨ə⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Mid_central_vowel"/></remarks>
    public static readonly SpeechSound MidCentralUnroundedVowel = new("ə", true);

    /// <summary>
    /// The open-mid central unrounded vowel, or low-mid central unrounded vowel, is a type of vowel sound,
    /// represented in the IPA by the symbol <c>⟨ɜ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Open-mid_central_unrounded_vowel"/></remarks>
    public static readonly SpeechSound OpenMidCentralUnroundedVowel = new("ɜ", true);

    public static readonly SpeechSound OpenMidCentralRoundedVowel = new("ɞ", true);

    /// <summary>
    /// The near-open central vowel, or near-low central vowel,
    /// represented in the IPA by the symbol <c>⟨ɐ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Near-open_central_vowel"/></remarks>
    public static readonly SpeechSound NearOpenCentralUnroundedVowel = new("ɐ", true);

    /// <summary>
    /// The close back unrounded vowel, or high back unrounded vowel,
    /// represented in the IPA by the symbol <c>⟨ɯ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Close_back_unrounded_vowel"/></remarks>
    public static readonly SpeechSound CloseBackUnroundedVowel = new("ɯ", true);

    /// <summary>
    /// The close back rounded vowel, or high back rounded vowel,
    /// represented in the IPA by the symbol <c>⟨u⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Close_back_rounded_vowel"/></remarks>
    public static readonly SpeechSound CloseBackRoundedVowel = new("u", true);

    /// <summary>
    /// The near-close near-back rounded vowel, or near-high near-back rounded vowel,
    /// represented in the IPA by the symbol <c>⟨ʊ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Near-close_near-back_rounded_vowel"/></remarks>
    public static readonly SpeechSound NearCloseNearBackRoundedVowel = new("ʊ", true);

    /// <summary>
    /// The close-mid back unrounded vowel, or high-mid back unrounded vowel,
    /// represented in the IPA by the symbol <c>⟨ɤ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Close-mid_back_unrounded_vowel"/></remarks>
    public static readonly SpeechSound CloseMidBackUnroundedVowel = new("ɤ", true);

    /// <summary>
    /// The close-mid back rounded vowel, or high-mid back rounded vowel,
    /// represented in the IPA by the symbol <c>⟨o⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Close-mid_back_rounded_vowel"/></remarks>
    public static readonly SpeechSound CloseMidBackRoundedVowel = new("o", true);

    /// <summary>
    /// The open-mid back unrounded vowel or low-mid back unrounded vowel,
    /// representented in the IPA by the symbol <c>⟨ʌ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Open-mid_back_unrounded_vowel"/></remarks>
    public static readonly SpeechSound OpenMidBackUnroundedVowel = new("ʌ", true);

    /// <summary>
    /// The open-mid back rounded vowel, or low-mid back rounded vowel,
    /// represented in the IPA by the symbol <c>⟨ɔ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Open-mid_back_rounded_vowel"/></remarks>
    public static readonly SpeechSound OpenMidBackRoundedVowel = new("ɔ", true);

    /// <summary>
    /// The open back unrounded vowel, or low back unrounded vowel,
    /// represented in the IPA by the symbol <c>⟨ɑ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Open_back_unrounded_vowel"/></remarks>
    public static readonly SpeechSound OpenBackUnroundedVowel = new("ɑ", true);

    /// <summary>
    /// The open back rounded vowel, or low back rounded vowel,
    /// represented in the IPA by the symbol <c>⟨ɒ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Open_back_rounded_vowel"/></remarks>
    public static readonly SpeechSound OpenBackRoundedVowel = new("ɒ", true);


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

    public static readonly FrozenSet<string> Diphthongs = FrozenSet.ToFrozenSet(
    [
        "aɪ",
        "aʊ",
        "ɔɪ",
        "eɪ",
        "əʊ",
        "ɪə",
        "eə",
        "ʊə",
    ]);

    public static readonly FrozenSet<string> Monophthongs = FrozenSet.ToFrozenSet(
    [
        "iː",
        "ɜː",
        "uː",
        "ɔː",
        "ɑː",
        "ɑːr",
        "æ",
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
        .. Diphthongs,
        .. Monophthongs,
    ]);

    // https://en.wikipedia.org/wiki/Consonant

    public static readonly FrozenSet<string> Bilabials = FrozenSet.ToFrozenSet(
    [
        VoicelessBilabialPlosive._value,        // [p]
        VoicedBilabialPlosive._value,           // [b]
        VoicedBilabialNasal._value,             // [m]
        VoicedBilabialTrill._value,             // [ʙ]
        VoicelessBilabialFricative._value,      // [ɸ]
        VoicedBilabialFricative._value,         // [β]
        // TODO: ʙ̥ https://en.wikipedia.org/wiki/Voiceless_bilabial_trill
        // TODO: m̥ https://en.wikipedia.org/wiki/Voiceless_bilabial_nasal
    ]);

    public static readonly FrozenSet<string> Labiodentals = FrozenSet.ToFrozenSet(
    [
        VoicelessLabiodentalFricative._value,   // [f]
        VoicedLabiodentalFricative._value,      // [v]
        VoicedLabiodentalNasal._value,          // [ɱ]
        VoicedLabiodentalFlap._value,           // [ⱱ]
        VoicedLabiodentalApproximant._value,    // [ʋ]
    ]);

    public static readonly FrozenSet<string> Dentals = FrozenSet.ToFrozenSet(
    [
        VoicelessDentalFricative._value,        // [θ]
        VoicedDentalFricative._value,           // [ð]
    ]);

    public static readonly FrozenSet<string> Alveolars = FrozenSet.ToFrozenSet(
    [
        VoicelessAlveolarPlosive._value,            // [t]
        VoicedAlveolarPlosive._value,               // [d]
        VoicedAlveolarNasal._value,                 // [n]
        VoicedAlveolarTrill._value,                 // [r]
        VoicelessAlveolarFricative._value,          // [s]
        VoicedAlveolarFricative._value,             // [z]
        VoicedAlveolarApproximant._value,           // [ɹ]
        VoicedAlveolarTap._value,                   // [ɾ]
        VoicelessLateralAlveolarFricative._value,   // [ɬ]
        VoicedLateralAlveolarFricative._value,      // [ɮ]
        VoicedAlveolarLateralApproximant._value,    // [l]
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
        "ɡ", // voiced velar plosive (U+0261)
        // "g", // ... U+0067 is not supported
        "ŋ", // velar nasal
        "x", // voiceless velar fricative
        "ɣ", // voiced velar fricative
        "w",
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

    public static readonly FrozenSet<string> Coarticulated = FrozenSet.ToFrozenSet(
    [
        VoicelessLabialVelarFricative._value,       // [ʍ]
        VoicedLabialVelarApproximant._value,        // [w]
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
        .. Coarticulated,
    ]);
}
