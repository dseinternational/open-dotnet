// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Text.Json;

namespace DSE.Open.Speech.Abstractions.Tests;

public class PhonemeTests
{

    [Theory]
    [MemberData(nameof(Phonemes))]
    public void Equality(Phoneme p)
    {
        var p2 = new Phoneme(p.ToString()[0]);
        Assert.Equal(p, p2);
    }

    [Theory]
    [MemberData(nameof(Phonemes))]
    public void Serialize(Phoneme p)
    {
        var json = JsonSerializer.Serialize(p);
        var expected = JsonSerializer.Serialize(p.ToString());

        Assert.Equal(expected, json);
    }

    [Theory]
    [MemberData(nameof(Phonemes))]
    public void SerializeWithRelaxedJsonEscaping(Phoneme p)
    {
        var json = JsonSerializer.Serialize(p, JsonSharedOptions.RelaxedJsonEscaping);
        Assert.Equal($"\"{p}\"", json);
    }

    public static TheoryData<Phoneme> Phonemes
    {
        get
        {
            var data = new TheoryData<Phoneme>
            {
                Phoneme.CloseBackUnroundedVowel,
                Phoneme.CloseCentralRoundedVowel,
                Phoneme.CloseCentralUnroundedVowel,
                Phoneme.CloseMidBackUnroundedVowel,
                Phoneme.CloseMidCentralRoundedVowel,
                Phoneme.CloseMidCentralUnroundedVowel,
                Phoneme.OpenBackRoundedVowel,
                Phoneme.OpenBackUnroundedVowel,
                Phoneme.OpenFrontRoundedVowel,
                Phoneme.OpenMidBackRoundedVowel,
                Phoneme.OpenMidBackUnroundedVowel,
                Phoneme.OpenMidCentralRoundedVowel,
                Phoneme.OpenMidCentralUnroundedVowel,
                Phoneme.OpenMidFrontUnroundedVowel,
                Phoneme.VoicedGlottalFricative,
                Phoneme.VoicedPalatalFricative,
                Phoneme.VoicedPalatalPlosive,
                Phoneme.VoicedPharyngealFricative,
                Phoneme.VoicedPostalveolarFricative,
                Phoneme.VoicedRetroflexFricative,
                Phoneme.VoicedRetroflexPlosive,
                Phoneme.VoicedUvularFricative,
                Phoneme.VoicedUvularPlosive,
                Phoneme.VoicedVelarFricative,
                Phoneme.VoicedVelarPlosive,
                Phoneme.VoicelessBilabialFricative,
                Phoneme.VoicelessPostalveolarFricative,
                Phoneme.VoicelessRetroflexFricative,
                Phoneme.VoicelessRetroflexPlosive,
            };
            return data;
        }
    }
}
