// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Speech;

namespace DSE.Open.Observations;

public sealed class SnapshotEqualityComparerTests
{
    [Fact]
    public void Equals_WithObservationsOfSameMeasure_ShouldReturnTrue()
    {
        // Arrange
        var observation1 = Observation.Create(TestMeasures.BinaryMeasure, true);
        var observation2 = Observation.Create(TestMeasures.BinaryMeasure, true);

        var snapshot1 = new Snapshot<Observation<Binary>>(observation1);
        var snapshot2 = new Snapshot<Observation<Binary>>(observation2);

        // Act
        var equal = SnapshotEqualityComparer.Measurement.Equals(snapshot1, snapshot2);

        // Assert
        Assert.True(equal);
    }

    [Fact]
    public void Equals_WithObservationsOfSameMeasureWithParams_ShouldReturnTrue()
    {
        // Arrange
        var observation1 =
            Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ay.Abstraction, true);

        var observation2 =
            Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ay.Abstraction, true);

        var snapshot1 = new Snapshot<Observation<Binary, SpeechSound>>(observation1);
        var snapshot2 = new Snapshot<Observation<Binary, SpeechSound>>(observation2);

        // Act
        var equal = SnapshotEqualityComparer.Measurement.Equals(snapshot1, snapshot2);

        // Assert
        Assert.True(equal);
    }

    [Fact]
    public void Equals_WithObservationsOfSameMeasureAndDifferentParam_ShouldReturnFalse()
    {
        // Arrange
        var observation1 =
            Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ay.Abstraction, true);

        var observation2 =
            Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ch.Abstraction, true);

        var snapshot1 = new Snapshot<Observation<Binary, SpeechSound>>(observation1);
        var snapshot2 = new Snapshot<Observation<Binary, SpeechSound>>(observation2);

        // Act
        var equal = SnapshotEqualityComparer.Measurement.Equals(snapshot1, snapshot2);

        // Assert
        Assert.False(equal);
    }

    [Fact]
    public void HashSet_WithNoParams()
    {
        // Arrange
        var set = new HashSet<ISnapshot>(SnapshotEqualityComparer.Measurement);
        var observation1 = Observation.Create(TestMeasures.BinaryMeasure, true);
        var observation2 = Observation.Create(TestMeasures.BinaryMeasure, false);

        var snapshot1 = new Snapshot<Observation<Binary>>(observation1);
        var snapshot2 = new Snapshot<Observation<Binary>>(observation2);

        // Act
        var result1 = set.Add(snapshot1);
        var result2 = set.Add(snapshot2);

        // Assert
        Assert.True(result1);
        Assert.False(result2);
        var ss = Assert.Single(set);
        Assert.Equal(observation1, ss.Observation);
    }

    [Fact]
    public void HashSet_WithParams()
    {
        // Arrange
        var set = new HashSet<ISnapshot>(SnapshotEqualityComparer.Measurement);
        var observation1 = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ay.Abstraction, true);
        var observation2 = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ch.Abstraction, true);

        var snapshot1 = new Snapshot<Observation<Binary, SpeechSound>>(observation1);
        var snapshot2 = new Snapshot<Observation<Binary, SpeechSound>>(observation2);

        // Act
        var result1 = set.Add(snapshot1);
        var result2 = set.Add(snapshot2);

        // Assert
        Assert.True(result1);
        Assert.True(result2);
        Assert.Equal(2, set.Count);
    }
}
