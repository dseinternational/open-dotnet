// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Speech;

namespace DSE.Open.Observations;

public sealed class ObservationComparerTests
{
    [Fact]
    public void Measurement_NullsSortFirst()
    {
        var cmp = ObservationComparer.Measurement;
        var obs = Observation.Create(TestMeasures.BinaryMeasure, true);

        Assert.Equal(0, cmp.Compare(null, null));
        Assert.Equal(-1, cmp.Compare(null, obs));
        Assert.Equal(1, cmp.Compare(obs, null));
    }

    [Fact]
    public void Measurement_OrdersByMeasureId()
    {
        var cmp = ObservationComparer.Measurement;

        // BinaryMeasure id = 10000, BinaryMeasure2 id = 120000 (see TestMeasures).
        var smaller = Observation.Create(TestMeasures.BinaryMeasure, true);
        var larger = Observation.Create(TestMeasures.BinaryMeasure2, true);

        Assert.True(cmp.Compare(smaller, larger) < 0);
        Assert.True(cmp.Compare(larger, smaller) > 0);
        Assert.Equal(0, cmp.Compare(smaller, smaller));
    }

    [Fact]
    public void Measurement_SameMeasureDifferentParameter_NotEqual()
    {
        var cmp = ObservationComparer.Measurement;

        var a = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ay.Abstraction, true);
        var b = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ch.Abstraction, true);

        // Regression for issue #332: two observations with the same MeasureId but
        // different parameters must not compare as equal (previously they could,
        // when their measurement hash codes collided).
        Assert.NotEqual(0, cmp.Compare(a, b));
        Assert.Equal(-cmp.Compare(a, b), cmp.Compare(b, a));
    }

    [Fact]
    public void Measurement_SameMeasureSameParameter_Equal()
    {
        var cmp = ObservationComparer.Measurement;

        var a = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ay.Abstraction, true);
        var b = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ay.Abstraction, false);

        Assert.Equal(0, cmp.Compare(a, b));
    }

    [Fact]
    public void Measurement_IsStableAcrossInstances()
    {
        var cmp = ObservationComparer.Measurement;

        var a1 = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ay.Abstraction, true);
        var a2 = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ay.Abstraction, true);
        var b1 = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ch.Abstraction, true);

        Assert.Equal(cmp.Compare(a1, b1), cmp.Compare(a2, b1));
    }
}
