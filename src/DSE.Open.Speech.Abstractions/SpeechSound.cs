// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Speech.Serialization;

namespace DSE.Open.Speech;

/// <summary>
/// Identifies a speech sound (phone).
/// </summary>
/// <remarks>See <see href="https://en.wikipedia.org/wiki/International_Phonetic_Alphabet"/></remarks>
[JsonConverter(typeof(JsonStringSpeechSoundConverter))]
[StructLayout(LayoutKind.Auto)]
public readonly struct SpeechSound
    : IEquatable<SpeechSound>,
      ISpanFormattable,
      ISpanParsable<SpeechSound>
{
    private readonly SpeechSymbolSequence _value;

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

        _value = SpeechSymbolSequence.Parse(value, CultureInfo.InvariantCulture); // TODO: more efficient construction given validation
    }

    private SpeechSound(string value, bool skipValidation)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        if (!skipValidation)
        {
            EnsureValidValue(value);
        }

        _value = SpeechSymbolSequence.Parse(value, CultureInfo.InvariantCulture); // TODO: more efficient construction given validation
    }

    private SpeechSound(SpeechSymbolSequence symbols, bool skipValidation)
    {
        if (!skipValidation)
        {
            EnsureValidValue(symbols);
        }

        _value = symbols;
    }

    public static bool IsValidValue(SpeechSymbolSequence value)
    {
        return value.Length > 0
            && value.Length < MaxLength;
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

    private static void EnsureValidValue(SpeechSymbolSequence value)
    {
        if (!IsValidValue(value))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
            return; // Unreachable
        }
    }

    public SpeechSymbol this[int index] => _value[index];

    public SpeechSymbolSequence.Enumerator GetEnumerator()
    {
        return _value.GetEnumerator();
    }

    public bool IsEmpty => _value.IsEmpty;

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
        return _value.Equals(other._value);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (_value.TryFormat(destination, out charsWritten, format, provider))
        {
            return true;
        }

        return false;
    }

    public override string ToString()
    {
        return ToString(null, null);
    }

    public string ToStringInvariant()
    {
        return ToString(null, null);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return _value.IsEmpty
            ? string.Empty
            : _value.ToString(format, formatProvider);
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
        return !sound.IsEmpty
            && (Consonants.Contains(sound)
                || (sound.Length > 1 && Consonants.Contains(sound[0])));
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

        return TryParse(sound, CultureInfo.InvariantCulture, out var result)
            && IsConsonant(result);
    }

    /// <summary>
    /// Determines if the specified sound is classified as a vowel.
    /// </summary>
    /// <param name="sound"></param>
    /// <returns><see langword="true"/> if the specified sound is classified as a vowel, otherwise
    /// <see langword="false"/>.</returns>
    public static bool IsVowel(SpeechSound sound)
    {
        return !sound.IsEmpty
            && (Vowels.Contains(sound)
                || (sound.Length > 1 && Vowels.Contains(sound[0]))
                // special case [ju] /yoo/ as in <view>
                || (sound.Length > 1
                    && sound._value[0] == VoicedPalatalApproximant
                    && sound._value[1] == CloseBackRoundedVowel));
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

        return TryParse(sound, CultureInfo.InvariantCulture, out var result)
            && IsVowel(result);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates
    public static implicit operator SpeechSound(SpeechSymbol value)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return new SpeechSound(new SpeechSymbolSequence([value]), true);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates
    public static implicit operator SpeechSymbolSequence(SpeechSound value)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return value._value;
    }

    /// <summary>
    /// The voiceless bilabial plosive, a type of consonantal sound
    /// represented in the IPA by the symbol <c>⟨p⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiceless_bilabial_plosive"/></remarks>
    public static readonly SpeechSound VoicelessBilabialPlosive = SpeechSymbol.VoicelessBilabialPlosive;

    /// <summary>
    /// The voiced bilabial plosive or stop, a type of consonantal sound
    /// represented in the IPA by the symbol <c>⟨b⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_bilabial_plosive"/></remarks>
    public static readonly SpeechSound VoicedBilabialPlosive = SpeechSymbol.VoicedBilabialPlosive;

    /// <summary>
    /// The voiced bilabial nasal, a type of consonantal sound
    /// represented in the IPA by the symbol <c>⟨m⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_bilabial_nasal"/></remarks>
    public static readonly SpeechSound VoicedBilabialNasal = SpeechSymbol.VoicedBilabialNasal;

    /// <summary>
    /// The voiced bilabial trill, a type of consonantal sound
    /// represented in the IPA by the symbol <c>⟨ʙ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_bilabial_trill"/></remarks>
    public static readonly SpeechSound VoicedBilabialTrill = SpeechSymbol.VoicedBilabialTrill;

    /// <summary>
    /// The voiceless bilabial fricative, a type of consonantal sound
    /// represented in the IPA by the symbol <c>⟨ɸ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiceless_bilabial_fricative"/></remarks>
    public static readonly SpeechSound VoicelessBilabialFricative = SpeechSymbol.VoicelessBilabialFricative;

    /// <summary>
    /// The voiced bilabial fricative, a type of consonantal sound
    /// represented in the IPA by the symbol <c>⟨β⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_bilabial_fricative"/></remarks>
    public static readonly SpeechSound VoicedBilabialFricative = SpeechSymbol.VoicedBilabialFricative;

    /// <summary>
    /// The voiced labiodental nasal, a type of consonantal sound
    /// represented in the IPA by the symbol <c>⟨ɱ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_labiodental_nasal"/></remarks>
    public static readonly SpeechSound VoicedLabiodentalNasal = SpeechSymbol.VoicedLabiodentalNasal;

    /// <summary>
    /// The voiced labiodental flap, a type of consonantal sound
    /// represented in the IPA by the symbol <c>⟨ⱱ⟩</c>.
    /// </summary>
    /// Added to the IPA in 2005. Added to Unicode in version 5.1 (2008).
    /// <para>See <see href="https://en.wikipedia.org/wiki/Voiced_labiodental_flap" /></para>
    public static readonly SpeechSound VoicedLabiodentalFlap = SpeechSymbol.VoicedLabiodentalFlap;

    /// <summary>
    /// The voiceless labiodental fricative, a type of consonantal sound
    /// represented in the IPA by the symbol <c>⟨f⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiceless_labiodental_fricative"/></remarks>
    public static readonly SpeechSound VoicelessLabiodentalFricative = SpeechSymbol.VoicelessLabiodentalFricative;

    /// <summary>
    /// The voiced labiodental fricative is a type of consonantal sound
    /// represented in the IPA by the symbol <c>⟨v⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_labiodental_fricative"/></remarks>
    public static readonly SpeechSound VoicedLabiodentalFricative = SpeechSymbol.VoicedLabiodentalFricative;

    /// <summary>
    /// The voiced labiodental approximant is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨ʋ⟩</c>.
    /// </summary>
    public static readonly SpeechSound VoicedLabiodentalApproximant = SpeechSymbol.VoicedLabiodentalApproximant;

    /// <summary>
    /// The voiceless alveolar plosive is a type of consonantal sound
    /// represented in the IPA by the symbols <c>⟨t⟩</c>.
    /// </summary>
    public static readonly SpeechSound VoicelessAlveolarPlosive =
        SpeechSymbol.VoicelessAlveolarPlosive;

    /// <summary>
    /// The voiced alveolar plosive is a type of consonantal sound
    /// represented in the IPA by the symbol <c>⟨d⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_dental_and_alveolar_plosives"/></remarks>
    public static readonly SpeechSound VoicedAlveolarPlosive =
        SpeechSymbol.VoicedAlveolarPlosive;

    /// <summary>
    /// The voiced alveolar nasal is a type of consonantal sound
    /// represented in the IPA by the symbol <c>⟨n⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_dental,_alveolar_and_postalveolar_nasals"/></remarks>
    public static readonly SpeechSound VoicedAlveolarNasal =
        SpeechSymbol.VoicedAlveolarNasal;

    public static readonly SpeechSound VoicedAlveolarTrill =
        SpeechSymbol.VoicedAlveolarTrill;

    /// <summary>
    /// The voiced alveolar tap or flap is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨ɾ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_dental_and_alveolar_taps_and_flaps"/></remarks>
    public static readonly SpeechSound VoicedAlveolarTap =
        SpeechSymbol.VoicedAlveolarTap;

    /// <summary>
    /// The voiceless dental non-sibilant fricative is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨θ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiceless_dental_fricative"/></remarks>
    public static readonly SpeechSound VoicelessDentalFricative =
        SpeechSymbol.VoicelessDentalFricative;

    /// <summary>
    /// The voiced dental fricative is a consonant sound,
    /// represented in the IPA by the symbol <c>⟨ð⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_dental_fricative"/></remarks>
    public static readonly SpeechSound VoicedDentalFricative =
        SpeechSymbol.VoicedDentalFricative;

    /// <summary>
    /// The voiceless alveolar fricative is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨s⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiceless_alveolar_fricative"/></remarks>
    public static readonly SpeechSound VoicelessAlveolarFricative =
        SpeechSymbol.VoicelessAlveolarFricative;

    /// <summary>
    /// The voiced alveolar fricative is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨z⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_alveolar_fricative"/></remarks>
    public static readonly SpeechSound VoicedAlveolarFricative =
        SpeechSymbol.VoicedAlveolarFricative;

    /// <summary>
    /// A voiceless postalveolar fricative is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨ʃ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiceless_postalveolar_fricative"/></remarks>
    public static readonly SpeechSound VoicelessPostalveolarFricative =
        SpeechSymbol.VoicelessPostalveolarFricative;

    /// <summary>
    /// The voiceless palato-alveolar sibilant affricate or voiceless domed postalveolar sibilant
    /// affricate is a type of consonantal sound, represented in the IPA by the symbol <c>tʃ</c>.
    /// </summary>
    public static readonly SpeechSound VoicelessPostalveolarAffricate =
        new(
        [
            SpeechSymbol.VoicelessAlveolarPlosive,
            SpeechSymbol.TieBar,
            SpeechSymbol.VoicelessPostalveolarFricative
        ],
        true);

    /// <summary>
    /// A voiced postalveolar fricative is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨ʒ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_postalveolar_fricative"/></remarks>
    public static readonly SpeechSound VoicedPostalveolarFricative =
        SpeechSymbol.VoicedPostalveolarFricative;

    /// <summary>
    /// The voiced alveolar approximant is a type of consonantal sound,
    /// represented in the IPA by the transcription <c>⟨ɹ̠⟩</c>. (See also <see cref="VoicedAlveolarApproximant"/>)
    /// </summary>
    /// <remarks>
    /// The most common sound represented by the letter r in English is the voiced
    /// postalveolar approximant, pronounced a little more back and transcribed more precisely
    /// in IPA as ⟨ɹ̠⟩, but ⟨ɹ⟩ is often used for convenience in its place.
    /// </remarks>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_alveolar_and_postalveolar_approximants"/></remarks>
    public static readonly SpeechSound VoicedPostalveolarApproximant =
        new(
        [
            SpeechSymbol.VoicedAlveolarApproximant,
            SpeechSymbol.Retracted,
        ],
        true);

    public static readonly SpeechSound VoicedPostalveolarAffricate = new(
        [
            SpeechSymbol.VoicedAlveolarPlosive,
            SpeechSymbol.Retracted,
            SpeechSymbol.VoicedPostalveolarFricative,
        ],
        true);

    public static readonly SpeechSound VoicelessAlveolarLateralFricative =
        SpeechSymbol.VoicelessAlveolarLateralFricative;

    public static readonly SpeechSound VoicedAlveolarLateralFricative =
        SpeechSymbol.VoicedAlveolarLateralFricative;

    /// <summary>
    /// The voiced alveolar approximant is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨ɹ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_alveolar_and_postalveolar_approximants"/></remarks>
    public static readonly SpeechSound VoicedAlveolarApproximant =
        SpeechSymbol.VoicedAlveolarApproximant;

    /// <summary>
    /// The voiced alveolar lateral approximant is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨l⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_dental,_alveolar_and_postalveolar_lateral_approximants"/></remarks>
    public static readonly SpeechSound VoicedAlveolarLateralApproximant =
        SpeechSymbol.VoicedAlveolarLateralApproximant;

    /// <summary>
    /// The voiceless retroflex plosive is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨ʈ⟩</c>.
    /// </summary>
    public static readonly SpeechSound VoicelessRetroflexPlosive =
        SpeechSymbol.VoicelessRetroflexPlosive;

    /// <summary>
    /// The voiced retroflex plosive is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨ɖ⟩</c>.
    /// </summary>
    public static readonly SpeechSound VoicedRetroflexPlosive =
        SpeechSymbol.VoicedRetroflexPlosive;

    public static readonly SpeechSound VoicedRetroflexNasal =
        SpeechSymbol.VoicedRetroflexNasal;

    /// <summary>
    /// The voiced retroflex tap is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨ɽ⟩</c>.
    /// </summary>
    public static readonly SpeechSound VoicedRetroflexTap =
        SpeechSymbol.VoicedRetroflexTap;

    public static readonly SpeechSound VoicelessRetroflexFricative =
        SpeechSymbol.VoicelessRetroflexFricative;

    public static readonly SpeechSound VoicedRetroflexFricative =
        SpeechSymbol.VoicedRetroflexFricative;

    /// <summary>
    /// The voiced retroflex approximant is a type of consonant,
    /// represented in the IPA by the symbol <c>⟨ɻ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_retroflex_approximant"/></remarks>
    public static readonly SpeechSound VoicedRetroflexApproximant =
        SpeechSymbol.VoicedRetroflexApproximant;

    public static readonly SpeechSound VoicedRetroflexLateralApproximant =
        SpeechSymbol.VoicedRetroflexLateralApproximant;

    public static readonly SpeechSound VoicelessPalatalPlosive =
        SpeechSymbol.VoicelessPalatalPlosive;

    public static readonly SpeechSound VoicedPalatalPlosive =
        SpeechSymbol.VoicedPalatalPlosive;

    public static readonly SpeechSound VoicedPalatalNasal =
        SpeechSymbol.VoicedPalatalNasal;

    public static readonly SpeechSound VoicelessPalatalFricative =
        SpeechSymbol.VoicelessPalatalFricative;

    public static readonly SpeechSound VoicedPalatalFricative =
        SpeechSymbol.VoicedPalatalFricative;

    /// <summary>
    /// The voiced palatal approximant, or yod, is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨j⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_palatal_approximant"/></remarks>
    public static readonly SpeechSound VoicedPalatalApproximant =
        SpeechSymbol.VoicedPalatalApproximant;

    public static readonly SpeechSound VoicedPalatalLateralApproximant =
        SpeechSymbol.VoicedPalatalLateralApproximant;

    /// <summary>
    /// The voiceless velar plosive or stop is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨k⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiceless_velar_plosive"/></remarks>
    public static readonly SpeechSound VoicelessVelarPlosive =
        SpeechSymbol.VoicelessVelarPlosive;

    public static readonly SpeechSound VoicedVelarPlosive =
        SpeechSymbol.VoicedVelarPlosive;

    /// <summary>
    /// The voiced velar nasal, also known as agma, is a type of consonantal sound
    /// represented in the IPA by the symbol <c>⟨ŋ⟩</c>.
    /// </summary>
    public static readonly SpeechSound VoicedVelarNasal =
        SpeechSymbol.VoicedVelarNasal;

    /// <summary>
    /// The voiceless velar fricative is a type of consonantal sound used,
    /// represented in the IPA by the symbol <c>⟨x⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiceless_velar_fricative"/></remarks>
    public static readonly SpeechSound VoicelessVelarFricative =
        SpeechSymbol.VoicelessVelarFricative;

    public static readonly SpeechSound VoicedVelarFricative =
        SpeechSymbol.VoicedVelarFricative;

    /// <summary>
    /// The voiced labial–velar approximant is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨w⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_labial%E2%80%93velar_approximant"/></remarks>
    public static readonly SpeechSound VoicedLabialVelarApproximant =
        SpeechSymbol.VoicedLabialVelarApproximant;

    /// <summary>
    /// The voiceless labial–velar fricative is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨ʍ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiceless_labial%E2%80%93velar_fricative"/></remarks>
    public static readonly SpeechSound VoicelessLabialVelarFricative =
        SpeechSymbol.VoicelessLabialVelarFricative;

    public static readonly SpeechSound VoicedVelarApproximant =
        SpeechSymbol.VoicedVelarApproximant;

    public static readonly SpeechSound VoicedVelarLateralApproximant =
        SpeechSymbol.VoicedVelarLateralApproximant;

    public static readonly SpeechSound VoicelessUvularPlosive =
        SpeechSymbol.VoicelessUvularPlosive;

    public static readonly SpeechSound VoicedUvularPlosive =
        SpeechSymbol.VoicedUvularPlosive;

    public static readonly SpeechSound VoicedUvularNasal =
        SpeechSymbol.VoicedUvularNasal;

    public static readonly SpeechSound VoicedUvularTrill =
        SpeechSymbol.VoicedUvularTrill;

    public static readonly SpeechSound VoicelessUvularFricative =
        SpeechSymbol.VoicelessUvularFricative;

    /// <summary>
    /// The voiced uvular fricative is a type of consonantal sound,
    /// represented in the IPA by the symbol <c>⟨ʁ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiced_uvular_fricative"/></remarks>
    public static readonly SpeechSound VoicedUvularFricative =
        SpeechSymbol.VoicedUvularFricative;

    public static readonly SpeechSound VoicelessPharyngealFricative =
        SpeechSymbol.VoicelessPharyngealFricative;

    public static readonly SpeechSound VoicedPharyngealFricative =
        SpeechSymbol.VoicedPharyngealFricative;

    public static readonly SpeechSound VoicelessGlottalPlosive =
        SpeechSymbol.VoicelessGlottalPlosive;

    /// <summary>
    /// The voiceless glottal fricative, sometimes called voiceless glottal transition or the aspirate,
    /// is a type of sound used in some spoken languages that patterns like a fricative or approximant
    /// consonant phonologically, but often lacks the usual phonetic characteristics of a consonant.
    /// The symbol in the International Phonetic Alphabet that represents this sound is ⟨h⟩.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Voiceless_glottal_fricative"/></remarks>
    public static readonly SpeechSound VoicelessGlottalFricative =
        SpeechSymbol.VoicelessGlottalFricative;

    public static readonly SpeechSound VoicedGlottalFricative =
        SpeechSymbol.VoicedGlottalFricative;

    /// <summary>
    /// The close front unrounded vowel, or high front unrounded vowel, is a type of vowel sound,
    /// represented in the IPA by the symbol <c>⟨i⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Close_front_unrounded_vowel"/></remarks>
    public static readonly SpeechSound CloseFrontUnroundedVowel =
        SpeechSymbol.CloseFrontUnrounded;

    public static readonly SpeechSound CloseFrontRoundedVowel =
        SpeechSymbol.CloseFrontRounded;

    /// <summary>
    /// The near-close near-front unrounded vowel, or near-high near-front unrounded vowel, is a type of vowel sound,
    /// represented in the IPA by the symbol <c>⟨ɪ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Near-close_near-front_unrounded_vowel"/></remarks>
    public static readonly SpeechSound NearCloseNearFrontUnrounded =
        SpeechSymbol.LaxCloseFrontUnrounded;

    public static readonly SpeechSound NearCloseNearFrontRounded =
        SpeechSymbol.LaxCloseFrontRounded;

    public static readonly SpeechSound CloseMidFrontUnroundedVowel =
        SpeechSymbol.CloseMidFrontUnrounded;

    public static readonly SpeechSound CloseMidFrontRoundedVowel =
        SpeechSymbol.CloseMidFrontRounded;

    /// <summary>
    /// The open-mid front unrounded vowel, or low-mid front unrounded vowel, is a type of vowel sound,
    /// represented in the IPA by the symbol <c>⟨ɛ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Open-mid_front_unrounded_vowel"/></remarks>
    public static readonly SpeechSound OpenMidFrontUnroundedVowel =
        SpeechSymbol.OpenMidFrontUnrounded;

    public static readonly SpeechSound OpenMidFrontRoundedVowel =
        SpeechSymbol.OpenMidFrontRounded;

    /// <summary>
    /// The near-open front unrounded vowel, or near-low front unrounded vowel, is a type of vowel sound,
    /// represented in the IPA by the symbol <c>⟨æ⟩</c>.
    /// </summary>
    public static readonly SpeechSound NearOpenFrontUnroundedVowel =
        SpeechSymbol.RaisedOpenFrontUnrounded;

    /// <summary>
    /// The open front unrounded vowel, or low front unrounded vowel, is a type of vowel sound,
    /// represented in the IPA by the symbol <c>⟨a⟩</c>.
    /// </summary>
    public static readonly SpeechSound OpenFrontUnroundedVowel =
        SpeechSymbol.OpenFrontUnrounded;

    public static readonly SpeechSound OpenFrontRoundedVowel =
        SpeechSymbol.OpenFrontRounded;

    public static readonly SpeechSound CloseCentralUnroundedVowel =
        SpeechSymbol.CloseCentralUnrounded;

    public static readonly SpeechSound CloseCentralRoundedVowel =
        SpeechSymbol.CloseCentralRounded;

    public static readonly SpeechSound CloseMidCentralUnroundedVowel =
        SpeechSymbol.CloseMidCentralUnrounded;

    public static readonly SpeechSound CloseMidCentralRoundedVowel =
        SpeechSymbol.CloseMidCentralRounded;

    /// <summary>
    /// The mid central vowel (also known as schwa) is a type of vowel sound,
    /// represented in the IPA by the symbol <c>⟨ə⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Mid_central_vowel"/></remarks>
    public static readonly SpeechSound MidCentralVowel =
        SpeechSymbol.MidCentralSchwa;

    /// <summary>
    /// The open-mid central unrounded vowel, or low-mid central unrounded vowel, is a type of vowel sound,
    /// represented in the IPA by the symbol <c>⟨ɜ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Open-mid_central_unrounded_vowel"/></remarks>
    public static readonly SpeechSound OpenMidCentralUnroundedVowel =
        SpeechSymbol.OpenMidCentralUnrounded;

    public static readonly SpeechSound OpenMidCentralRoundedVowel =
        SpeechSymbol.OpenMidCentralRounded;

    /// <summary>
    /// The near-open central vowel, or near-low central vowel,
    /// represented in the IPA by the symbol <c>⟨ɐ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Near-open_central_vowel"/></remarks>
    public static readonly SpeechSound NearOpenCentralUnroundedVowel =
        SpeechSymbol.LoweredSchwa;

    /// <summary>
    /// The open central unrounded vowel, or low central unrounded vowel,
    /// represented in the IPA by the transcription <c>⟨ä⟩</c>.
    /// </summary>
    public static readonly SpeechSound OpenCentralUnroundedVowel = new(
        [
            SpeechSymbol.OpenFrontUnrounded,
            SpeechSymbol.Centralized,
        ],
        true);

    /// <summary>
    /// The close back unrounded vowel, or high back unrounded vowel,
    /// represented in the IPA by the symbol <c>⟨ɯ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Close_back_unrounded_vowel"/></remarks>
    public static readonly SpeechSound CloseBackUnroundedVowel =
        SpeechSymbol.CloseBackUnrounded;

    /// <summary>
    /// The close back rounded vowel, or high back rounded vowel,
    /// represented in the IPA by the symbol <c>⟨u⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Close_back_rounded_vowel"/></remarks>
    public static readonly SpeechSound CloseBackRoundedVowel =
        SpeechSymbol.CloseBackRounded;

    /// <summary>
    /// The near-close near-back rounded vowel, or near-high near-back rounded vowel,
    /// represented in the IPA by the symbol <c>⟨ʊ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Near-close_near-back_rounded_vowel"/></remarks>
    public static readonly SpeechSound NearCloseNearBackRoundedVowel =
        SpeechSymbol.LaxCloseBackRounded;

    /// <summary>
    /// The close-mid back unrounded vowel, or high-mid back unrounded vowel,
    /// represented in the IPA by the symbol <c>⟨ɤ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Close-mid_back_unrounded_vowel"/></remarks>
    public static readonly SpeechSound CloseMidBackUnroundedVowel =
        SpeechSymbol.CloseMidBackUnrounded;

    /// <summary>
    /// The close-mid back rounded vowel, or high-mid back rounded vowel,
    /// represented in the IPA by the symbol <c>⟨o⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Close-mid_back_rounded_vowel"/></remarks>
    public static readonly SpeechSound CloseMidBackRoundedVowel =
        SpeechSymbol.CloseMidBackRounded;

    /// <summary>
    /// The open-mid back unrounded vowel or low-mid back unrounded vowel,
    /// representented in the IPA by the symbol <c>⟨ʌ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Open-mid_back_unrounded_vowel"/></remarks>
    public static readonly SpeechSound OpenMidBackUnroundedVowel =
        SpeechSymbol.OpenMidBackUnrounded;

    /// <summary>
    /// The open-mid back rounded vowel, or low-mid back rounded vowel,
    /// represented in the IPA by the symbol <c>⟨ɔ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Open-mid_back_rounded_vowel"/></remarks>
    public static readonly SpeechSound OpenMidBackRoundedVowel =
        SpeechSymbol.OpenMidBackRounded;

    /// <summary>
    /// The open back unrounded vowel, or low back unrounded vowel,
    /// represented in the IPA by the symbol <c>⟨ɑ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Open_back_unrounded_vowel"/></remarks>
    public static readonly SpeechSound OpenBackUnroundedVowel =
        SpeechSymbol.OpenBackUnrounded;

    /// <summary>
    /// The open back rounded vowel, or low back rounded vowel,
    /// represented in the IPA by the symbol <c>⟨ɒ⟩</c>.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Open_back_rounded_vowel"/></remarks>
    public static readonly SpeechSound OpenBackRoundedVowel =
        SpeechSymbol.OpenBackRounded;

    public static readonly FrozenSet<SpeechSound> CloseVowels = FrozenSet.ToFrozenSet(
    [
        CloseFrontUnroundedVowel,
        CloseFrontRoundedVowel,
        CloseCentralUnroundedVowel,
        CloseCentralRoundedVowel,
        CloseBackUnroundedVowel,
        CloseBackRoundedVowel,
    ]);

    public static readonly FrozenSet<SpeechSound> NearCloseVowels = FrozenSet.ToFrozenSet(
    [
        NearCloseNearFrontUnrounded,
        NearCloseNearFrontRounded,
        NearCloseNearBackRoundedVowel,
    ]);

    public static readonly FrozenSet<SpeechSound> CloseMidVowels = FrozenSet.ToFrozenSet(
    [
        CloseMidFrontUnroundedVowel,
        CloseMidFrontRoundedVowel,
        CloseMidCentralUnroundedVowel,
        CloseMidCentralRoundedVowel,
        CloseMidBackUnroundedVowel,
        CloseMidBackRoundedVowel,
    ]);

    public static readonly FrozenSet<SpeechSound> MidVowels = FrozenSet.ToFrozenSet(
    [
        MidCentralVowel,
    ]);

    public static readonly FrozenSet<SpeechSound> OpenMidVowels = FrozenSet.ToFrozenSet(
    [
        OpenMidFrontUnroundedVowel,
        OpenMidFrontRoundedVowel,
        OpenMidCentralUnroundedVowel,
        OpenMidCentralRoundedVowel,
        OpenMidBackUnroundedVowel,
        OpenMidBackRoundedVowel,
    ]);

    public static readonly FrozenSet<SpeechSound> NearOpenVowels = FrozenSet.ToFrozenSet(
    [
        NearOpenFrontUnroundedVowel,
        NearOpenCentralUnroundedVowel,
    ]);

    public static readonly FrozenSet<SpeechSound> OpenVowels = FrozenSet.ToFrozenSet(
    [
        OpenFrontUnroundedVowel,
        OpenFrontRoundedVowel,
        OpenCentralUnroundedVowel,
        OpenBackUnroundedVowel,
        OpenBackRoundedVowel,
    ]);

    public static readonly FrozenSet<SpeechSound> Diphthongs = FrozenSet.ToFrozenSet(
    [
        ParseInvariant("aɪ"),
        ParseInvariant("aʊ"),
        ParseInvariant("ɔɪ"),
        ParseInvariant("eɪ"),
        ParseInvariant("əʊ"),
        ParseInvariant("ɪə"),
        ParseInvariant("eə"),
        ParseInvariant("ʊə"),
    ]);

    public static readonly FrozenSet<SpeechSound> Vowels = FrozenSet.ToFrozenSet(
    [
        .. CloseVowels,
        .. NearCloseVowels,
        .. CloseMidVowels,
        .. MidVowels,
        .. OpenMidVowels,
        .. NearOpenVowels,
        .. OpenVowels,
        .. Diphthongs,
    ]);

    // https://en.wikipedia.org/wiki/Consonant

    public static readonly FrozenSet<SpeechSound> Bilabials = FrozenSet.ToFrozenSet(
    [
        VoicelessBilabialPlosive,        // [p]
        VoicedBilabialPlosive,           // [b]
        VoicedBilabialNasal,             // [m]
        VoicedBilabialTrill,             // [ʙ]
        VoicelessBilabialFricative,      // [ɸ]
        VoicedBilabialFricative,         // [β]
    ]);

    public static readonly FrozenSet<SpeechSound> Labiodentals = FrozenSet.ToFrozenSet(
    [
        VoicelessLabiodentalFricative,   // [f]
        VoicedLabiodentalFricative,      // [v]
        VoicedLabiodentalNasal,          // [ɱ]
        VoicedLabiodentalFlap,           // [ⱱ]
        VoicedLabiodentalApproximant,    // [ʋ]
    ]);

    public static readonly FrozenSet<SpeechSound> Dentals = FrozenSet.ToFrozenSet(
    [
        VoicelessDentalFricative,        // [θ]
        VoicedDentalFricative,           // [ð]
    ]);

    public static readonly FrozenSet<SpeechSound> Alveolars = FrozenSet.ToFrozenSet(
    [
        VoicelessAlveolarPlosive,            // [t]
        VoicedAlveolarPlosive,               // [d]
        VoicedAlveolarNasal,                 // [n]
        VoicedAlveolarTrill,                 // [r]
        VoicelessAlveolarFricative,          // [s]
        VoicedAlveolarFricative,             // [z]
        VoicedAlveolarApproximant,           // [ɹ]
        VoicedAlveolarTap,                   // [ɾ]
        VoicelessAlveolarLateralFricative,   // [ɬ]
        VoicedAlveolarLateralFricative,      // [ɮ]
        VoicedAlveolarLateralApproximant,    // [l]
    ]);

    public static readonly FrozenSet<SpeechSound> PostAlveolars = FrozenSet.ToFrozenSet(
    [
        VoicelessPostalveolarFricative,     // [ʃ]
        VoicelessPostalveolarAffricate,     // [tʃ]
        VoicedPostalveolarFricative,        // [ʒ]
        VoicedPostalveolarAffricate,        // [dʒ]
        VoicelessRetroflexPlosive,          // [ʈ]
        VoicedRetroflexPlosive,             // [ɖ]
        VoicedRetroflexNasal,               // [ɳ]
        VoicedRetroflexApproximant,         // [ɻ]
        VoicedRetroflexTap,                 // [ɽ]
        VoicelessRetroflexFricative,        // [ʂ]
        VoicedRetroflexFricative,           // [ʐ]
        VoicedRetroflexLateralApproximant,  // [ɭ]

        VoicelessPostalveolarAffricate,     // [tʃ]
        VoicedPostalveolarAffricate,        // [dʒ]
    ]);

    public static readonly FrozenSet<SpeechSound> Palatals = FrozenSet.ToFrozenSet(
    [
        VoicelessPalatalPlosive,        // [c]
        VoicedPalatalPlosive,           // [ɟ]
        VoicedPalatalNasal,             // [ɲ]
        VoicelessPalatalFricative,      // [ç]
        VoicedPalatalFricative,         // [ʝ]
        VoicedPalatalApproximant,       // [j]
        VoicedPalatalLateralApproximant // [ʎ]
    ]);

    public static readonly FrozenSet<SpeechSound> Velars = FrozenSet.ToFrozenSet(
    [
        VoicelessVelarPlosive,      // [k]
        VoicedVelarPlosive,         // [ɡ]
        VoicedVelarNasal,           // [ŋ]
        VoicelessVelarFricative,    // [x]
        VoicedVelarFricative,       // [ɣ]
        VoicedVelarApproximant,     // [ɰ]
    ]);

    public static readonly FrozenSet<SpeechSound> Uvulars = FrozenSet.ToFrozenSet(
    [
        VoicelessUvularPlosive,     // [q]
        VoicedUvularPlosive,        // [ɢ]
        VoicedUvularNasal,          // [ɴ]
        VoicelessUvularFricative,   // [χ]
        VoicedUvularFricative,      // [ʁ]
        VoicedUvularTrill,          // [ʀ]
    ]);

    public static readonly FrozenSet<SpeechSound> Pharyngeals = FrozenSet.ToFrozenSet(
    [
        VoicelessPharyngealFricative, // [ħ]
        VoicedPharyngealFricative,    // [ʕ]
    ]);

    public static readonly FrozenSet<SpeechSound> Glottals = FrozenSet.ToFrozenSet(
    [
        VoicelessGlottalPlosive, // [ʔ]
        VoicelessGlottalFricative, // [h]
        VoicedGlottalFricative, // [ɦ]
    ]);

    public static readonly FrozenSet<SpeechSound> Coarticulated = FrozenSet.ToFrozenSet(
    [
        VoicelessLabialVelarFricative,       // [ʍ]
        VoicedLabialVelarApproximant,        // [w]
    ]);

    public static readonly FrozenSet<SpeechSound> Consonants = FrozenSet.ToFrozenSet(
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

    public Transcription ToTranscription()
    {
        throw new NotImplementedException();
    }
}
