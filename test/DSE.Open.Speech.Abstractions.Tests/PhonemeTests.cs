// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Globalization;

namespace DSE.Open.Speech.Abstractions.Tests;

public class PhonemeTests
{
    [Fact]
    public void Init()
    {
        var p = new Phoneme()
        {
            Language = LanguageCode2.English,
            Abstraction = SpeechSound.VoicelessBilabialPlosive,
            Allophones = []
        };

        Assert.Equal(LanguageCode2.English, p.Language);
        Assert.Equal(SpeechSound.VoicelessBilabialPlosive, p.Abstraction);
    }
}
