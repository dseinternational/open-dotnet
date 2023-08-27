// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Speech.Abstractions;

/// <summary>
/// Identifies a phoneme, the smallest unit of sound in a language that distinguishes one
/// word from another. Phonemes are represented by the International Phonetic Alphabet (IPA)
/// and stored as the corresponding value from the IPA Extensions Unicode block (0x0250–0x02AF).
/// </summary>
[EquatableValue]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<Phoneme, CharSequence>))]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct Phoneme : IEquatableValue<Phoneme, CharSequence>
{
    public static int MaxSerializedCharLength => 2;

    public Phoneme(string value) : this(new CharSequence(value)) { }

    public Phoneme(char value) : this(new CharSequence(new ReadOnlyMemory<char>(new[] { value }))) { }

    public Phoneme(CharSequence value) : this(value, false) { }

    public static bool IsValidValue(CharSequence value)
    {
        // TODO: a decent validation algorithm
        return value.Length > 0 && value.Length < 3;
    }

    public int Length => _value.Length;

#pragma warning disable CA2225 // Operator overloads have named alternates
    public static Phoneme operator +(Phoneme left, Phoneme right)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        Span<char> b = stackalloc char[left.Length + right.Length];
        left._value.Span.CopyTo(b);
        right._value.Span.CopyTo(b[left.Length..]);
        return new Phoneme(new CharSequence(b.ToArray()));
    }

    public static readonly Phoneme VoicelessBilabialPlosive = new("p");
    public static readonly Phoneme VoicedBilabialPlosive = new("b");
    public static readonly Phoneme VoicedBilabialNasal = new("m");
    public static readonly Phoneme VoicedBilabialTrill = new("ʙ");
    public static readonly Phoneme VoicelessBilabialFricative = new("ɸ");
    public static readonly Phoneme VoicedBilabialFricative = new("β");
    public static readonly Phoneme VoicedLabiodentalNasal = new("ɱ");
    public static readonly Phoneme VoicedLabiodentalFlap = new("ⱱ");
    public static readonly Phoneme VoicelessLabiodentalFricative = new("f");
    public static readonly Phoneme VoicedLabiodentalFricative = new("v");
    public static readonly Phoneme VoicedLabiodentalApproximant = new("ʋ");
    public static readonly Phoneme VoicelessAlveolarPlosive = new("t");
    public static readonly Phoneme VoicedAlveolarPlosive = new("d");
    public static readonly Phoneme VoicedAlveolarNasal = new("n");
    public static readonly Phoneme VoicedAlveolarTrill = new("r");
    public static readonly Phoneme VoicedAlveolarTap = new("ɾ");
    public static readonly Phoneme VoicelessDentalFricative = new("θ");
    public static readonly Phoneme VoicedDentalFricative = new("ð");
    public static readonly Phoneme VoicelessAlveolarFricative = new("s");
    public static readonly Phoneme VoicedAlveolarFricative = new("z");
    public static readonly Phoneme VoicelessPostalveolarFricative = new("ʃ");
    public static readonly Phoneme VoicedPostalveolarFricative = new("ʒ");
    public static readonly Phoneme VoicelessLateralAlveolarFricative = new("ɬ");
    public static readonly Phoneme VoicedLateralAlveolarFricative = new("ɮ");
    public static readonly Phoneme VoicedAlveolarApproximant = new("ɹ");
    public static readonly Phoneme VoicedLateralAlveolarApproximant = new("l");
    public static readonly Phoneme VoicelessRetroflexPlosive = new("ʈ");
    public static readonly Phoneme VoicedRetroflexPlosive = new("ɖ");
    public static readonly Phoneme VoicedRetroflexNasal = new("ɳ");
    public static readonly Phoneme VoicedRetroflexFlap = new("ɽ");
    public static readonly Phoneme VoicelessRetroflexFricative = new("ʂ");
    public static readonly Phoneme VoicedRetroflexFricative = new("ʐ");
    public static readonly Phoneme VoicedRetroflexApproximant = new("ɻ");
    public static readonly Phoneme VoicedRetroflexLateralApproximant = new("ɭ");
    public static readonly Phoneme VoicelessPalatalPlosive = new("c");
    public static readonly Phoneme VoicedPalatalPlosive = new("ɟ");
    public static readonly Phoneme VoicedPalatalNasal = new("ɲ");
    public static readonly Phoneme VoicelessPalatalFricative = new("ç");
    public static readonly Phoneme VoicedPalatalFricative = new("ʝ");
    public static readonly Phoneme VoicedPalatalApproximant = new("j");
    public static readonly Phoneme VoicedPalatalLateralApproximant = new("ʎ");
    public static readonly Phoneme VoicelessVelarPlosive = new("k");
    public static readonly Phoneme VoicedVelarPlosive = new("ɡ");
    public static readonly Phoneme VoicedVelarNasal = new("ŋ");
    public static readonly Phoneme VoicelessVelarFricative = new("x");
    public static readonly Phoneme VoicedVelarFricative = new("ɣ");
    public static readonly Phoneme VoicedVelarApproximant = new("ɰ");
    public static readonly Phoneme VoicedVelarLateralApproximant = new("ʟ");
    public static readonly Phoneme VoicelessUvularPlosive = new("q");
    public static readonly Phoneme VoicedUvularPlosive = new("ɢ");
    public static readonly Phoneme VoicedUvularNasal = new("ɴ");
    public static readonly Phoneme VoicedUvularTrill = new("ʀ");
    public static readonly Phoneme VoicelessUvularFricative = new("χ");
    public static readonly Phoneme VoicedUvularFricative = new("ʁ");
    public static readonly Phoneme VoicelessPharyngealFricative = new("ħ");
    public static readonly Phoneme VoicedPharyngealFricative = new("ʕ");
    public static readonly Phoneme VoicelessGlottalPlosive = new("ʔ");
    public static readonly Phoneme VoicelessGlottalFricative = new("h");
    public static readonly Phoneme VoicedGlottalFricative = new("ɦ");
    public static readonly Phoneme CloseFrontUnroundedVowel = new("i");
    public static readonly Phoneme CloseFrontRoundedVowel = new("y");
    public static readonly Phoneme LoweredCloseFrontUnroundedVowel = new("ɪ");
    public static readonly Phoneme LoweredCloseFrontRoundedVowel = new("ʏ");
    public static readonly Phoneme CloseMidFrontUnroundedVowel = new("e");
    public static readonly Phoneme CloseMidFrontRoundedVowel = new("ø");
    public static readonly Phoneme OpenMidFrontUnroundedVowel = new("ɛ");
    public static readonly Phoneme OpenMidFrontRoundedVowel = new("œ");
    public static readonly Phoneme RaisedOpenFrontUnroundedVowel = new("æ");
    public static readonly Phoneme OpenFrontUnroundedVowel = new("a");
    public static readonly Phoneme OpenFrontRoundedVowel = new("ɶ");
    public static readonly Phoneme CloseCentralUnroundedVowel = new("ɨ");
    public static readonly Phoneme CloseCentralRoundedVowel = new("ʉ");
    public static readonly Phoneme CloseMidCentralUnroundedVowel = new("ɘ");
    public static readonly Phoneme CloseMidCentralRoundedVowel = new("ɵ");
    public static readonly Phoneme MidCentralUnroundedVowel = new("ə");
    public static readonly Phoneme OpenMidCentralUnroundedVowel = new("ɜ");
    public static readonly Phoneme OpenMidCentralRoundedVowel = new("ɞ");
    public static readonly Phoneme RaisedOpenCentralUnroundedVowel = new("ɐ");
    public static readonly Phoneme CloseBackUnroundedVowel = new("ɯ");
    public static readonly Phoneme CloseBackRoundedVowel = new("u");
    public static readonly Phoneme LoweredCloseBackRoundedVowel = new("ʊ");
    public static readonly Phoneme CloseMidBackUnroundedVowel = new("ɤ");
    public static readonly Phoneme CloseMidBackRoundedVowel = new("o");
    public static readonly Phoneme OpenMidBackUnroundedVowel = new("ʌ");
    public static readonly Phoneme OpenMidBackRoundedVowel = new("ɔ");
    public static readonly Phoneme OpenBackUnroundedVowel = new("ɑ");
    public static readonly Phoneme OpenBackRoundedVowel = new("ɒ");
}
