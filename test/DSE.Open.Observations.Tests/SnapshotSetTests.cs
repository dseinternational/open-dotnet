// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Speech;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public class SnapshotSetTests
{
    [Fact]
    public void CreateSnapshotSet()
    {
        var observations = new SnapshotSet
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
        SnapshotSet observations = [];
        var obs1 = Observation.Create(TestMeasures.CountMeasure, (Count)42);
        var obs2 = Observation.Create(TestMeasures.CountMeasure, (Count)420);
        _ = observations.Add(obs1);
        _ = observations.Add(obs2);
        _ = Assert.Single(observations);
    }

    [Fact]
    public void DifferentMeasuresAdded()
    {
        SnapshotSet observations = [];
        var obs1 = Observation.Create(TestMeasures.CountMeasure, (Count)42);
        var obs2 = Observation.Create(TestMeasures.CountMeasure2, (Count)420);
        _ = observations.Add(obs1);
        _ = observations.Add(obs2);
        Assert.Equal(2, observations.Count);
    }

    [Fact]
    public void DifferentMeasuresWithParamsAdded()
    {
        SnapshotSet observations = [];
        var obs1 = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ay.Abstraction, true);
        var obs2 = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ch.Abstraction, false);
        _ = observations.Add(obs1);
        _ = observations.Add(obs2);
        Assert.Equal(2, observations.Count);
    }

    [Fact]
    public void CannotAddSameMeasureWithParamTwice()
    {
        SnapshotSet observations = [];
        var obs1 = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ay.Abstraction, true);
        var obs2 = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ay.Abstraction, false);
        _ = observations.Add(obs1);
        _ = observations.Add(obs2);
        _ = Assert.Single(observations);
    }

    [Fact]
    public async Task LatestSameMeasureAdded()
    {
        SnapshotSet observations = [];
        var obs1 = Observation.Create(TestMeasures.CountMeasure, (Count)42);
        await Task.Delay(1, TestContext.Current.CancellationToken);
        var obs2 = Observation.Create(TestMeasures.CountMeasure, (Count)420);
        _ = observations.Add(obs1);
        _ = observations.Add(obs2);
        _ = Assert.Single(observations);
        var kept = observations.Single() as Observation<Count>;
        Assert.NotNull(kept);
        Assert.Equal(obs2.Value, kept.Value);
    }

    [Fact]
    public async Task LatestSameMeasureWithParamAdded()
    {
        SnapshotSet observations = [];
        var obs1 = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ay.Abstraction, true);
        await Task.Delay(1, TestContext.Current.CancellationToken);
        var obs2 = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ay.Abstraction, false);
        _ = observations.Add(obs1);
        _ = observations.Add(obs2);
        _ = Assert.Single(observations);
        var kept = observations.Single() as Observation<bool, SpeechSound>;
        Assert.NotNull(kept);
        Assert.Equal(obs2.Value, kept.Value);
    }

    [Fact]
    public async Task UnionWithMeasuresWithParam()
    {
        var obs1 = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.d.Abstraction, true);
        await Task.Delay(1, TestContext.Current.CancellationToken);
        var obs2 = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.b.Abstraction, false);
        await Task.Delay(1, TestContext.Current.CancellationToken);
        var obs3 = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.d.Abstraction, true);
        await Task.Delay(1, TestContext.Current.CancellationToken);
        var obs4 = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.b.Abstraction, false);
        SnapshotSet observations1 = [obs1, obs2];
        SnapshotSet observations2 = [obs3, obs4];
        observations1.UnionWith(observations2);
        Assert.Equal(2, observations1.Count);
    }
}
