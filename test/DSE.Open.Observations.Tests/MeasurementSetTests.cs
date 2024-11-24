// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Speech;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public class MeasurementSetTests
{
    [Fact]
    public void CreateMeasurementSet()
    {
        var observations = new MeasurementSet
        {
            Observation.Create(TestMeasures.BinaryMeasure, true),
            Observation.Create(TestMeasures.CountMeasure, (Count)42),
            Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ay.Abstraction, true),
            Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ch.Abstraction, false),
        };

        Assert.Equal(4, observations.Count);
    }

    [Fact]
    public void CannotAddSameMeasureTwice()
    {
        MeasurementSet observations = [];
        var obs1 = Observation.Create(TestMeasures.CountMeasure, (Count)42);
        var obs2 = Observation.Create(TestMeasures.CountMeasure, (Count)420);
        _ = observations.Add(obs1);
        _ = observations.Add(obs2);
        _ = Assert.Single(observations);
    }

    [Fact]
    public void CannotAddSameMeasureWithParamTwice()
    {
        MeasurementSet observations = [];
        var obs1 = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ay.Abstraction, true);
        var obs2 = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ay.Abstraction, false);
        _ = observations.Add(obs1);
        _ = observations.Add(obs2);
        _ = Assert.Single(observations);
    }
}
