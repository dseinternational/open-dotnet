// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Speech;
using Microsoft.Extensions.Time.Testing;

namespace DSE.Open.Observations;

public class SnapshotTests
{
    [Fact]
    public void CreateSnapshot()
    {
        FakeTimeProvider timeProvider = new();
        timeProvider.SetUtcNow(DateTimeOffset.UtcNow);

        var o = Observation.Create(TestMeasures.BinaryMeasure, true);
        var s = new Snapshot<Observation<Binary>>(o, timeProvider);

        var roundedTime = DateTimeOffset.FromUnixTimeMilliseconds(timeProvider.GetUtcNow().ToUnixTimeMilliseconds());

        Assert.Equal(o, s.Observation);
        Assert.Equal(roundedTime, s.Time);
    }

    [Fact]
    public void SerializeDeserialize()
    {
        var o = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ch.Abstraction, false);
        var s = new Snapshot<Observation<Binary, SpeechSound>>(o);

        var json = JsonSerializer.Serialize(s);

        var s2 = JsonSerializer.Deserialize<Snapshot<Observation<Binary, SpeechSound>>>(json);

        Assert.Equal(s, s2);
    }
}
