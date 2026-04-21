// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Speech;

namespace DSE.Open.Observations;

public sealed class ObservationEqualityComparerTests
{
    [Fact]
    public void Measurement_SameMeasurement_AreEqual()
    {
        var cmp = ObservationEqualityComparer.Measurement;

        var a = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ay.Abstraction, true);
        var b = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ay.Abstraction, false);

        Assert.True(cmp.Equals(a, b));
        Assert.Equal(cmp.GetHashCode(a), cmp.GetHashCode(b));
    }

    [Fact]
    public void Measurement_DifferentMeasureId_NotEqual()
    {
        var cmp = ObservationEqualityComparer.Measurement;

        var a = Observation.Create(TestMeasures.BinaryMeasure, true);
        var b = Observation.Create(TestMeasures.BinaryMeasure2, true);

        Assert.False(cmp.Equals(a, b));
    }

    [Fact]
    public void Measurement_SameMeasureIdDifferentParameter_NotEqual()
    {
        var cmp = ObservationEqualityComparer.Measurement;

        var a = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ay.Abstraction, true);
        var b = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ch.Abstraction, true);

        Assert.False(cmp.Equals(a, b));
    }

    [Fact]
    public void Measurement_HashCollisionWithDifferentIdentity_NotEqual()
    {
        // Regression for issue #326: previously Equals compared
        // GetMeasurementHashCode values directly, so two observations with
        // colliding measurement hashes but different MeasureId / Parameter
        // were reported as equal.
        var cmp = ObservationEqualityComparer<IObservation>.Measurement;

        var a = new HashCollidingObservation(new MeasureId(100000000001), parameter: "a");
        var b = new HashCollidingObservation(new MeasureId(100000000002), parameter: "b");

        Assert.Equal(a.GetMeasurementHashCode(), b.GetMeasurementHashCode());
        Assert.False(cmp.Equals(a, b));
    }

    private sealed class HashCollidingObservation : IObservation
    {
        public HashCollidingObservation(MeasureId measureId, object? parameter)
        {
            MeasureId = measureId;
            Parameter = parameter;
        }

        public ObservationId Id => default;
        public MeasureId MeasureId { get; }
        public DateTimeOffset Time => default;
        public object Value => false;
        public object? Parameter { get; }
        public object? Parameter2 => null;

        public int GetMeasurementHashCode()
        {
            return 0;
        }

        public double ConvertValueToDouble()
        {
            return 0;
        }

        public decimal ConvertValueToDecimal()
        {
            return 0;
        }
    }
}
