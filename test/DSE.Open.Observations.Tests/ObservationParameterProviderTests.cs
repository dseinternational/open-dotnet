// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language;
using DSE.Open.Speech;

namespace DSE.Open.Observations;

public class ObservationParameterProviderTests
{
    [Fact]
    public void GetIntegerId_ReturnsValue_WordId()
    {
        var value = ObservationParameterProvider<WordId>.Default.GetIntegerId(WordId.GetRandomId());
        Assert.True(value <= IObservationParameter.MaxIntegerId);
    }

    [Fact]
    public void GetIntegerId_ReturnsValue_SentenceId()
    {
        var value = ObservationParameterProvider<SentenceId>.Default.GetIntegerId(SentenceId.GetRandomId());
        Assert.True(value <= IObservationParameter.MaxIntegerId);
    }

    [Fact]
    public void GetIntegerId_ReturnsValue_WordMeaningId()
    {
        var value = ObservationParameterProvider<WordMeaningId>.Default.GetIntegerId(WordMeaningId.GetRandomId());
        Assert.True(value <= IObservationParameter.MaxIntegerId);
    }

    [Fact]
    public void GetIntegerId_ReturnsValue_SentenceMeaningId()
    {
        var value = ObservationParameterProvider<SentenceMeaningId>.Default.GetIntegerId(SentenceMeaningId.GetRandomId());
        Assert.True(value <= IObservationParameter.MaxIntegerId);
    }

    [Fact]
    public void GetTextId_ReturnsValue_SpeechSound()
    {
        var value = ObservationParameterProvider<SpeechSound>.Default.GetTextId(SpeechSound.CloseMidFrontUnroundedVowel);
        Assert.True(value.Length >= IObservationParameter.MinTextLength);
        Assert.True(value.Length <= IObservationParameter.MaxTextLength);
    }
}
