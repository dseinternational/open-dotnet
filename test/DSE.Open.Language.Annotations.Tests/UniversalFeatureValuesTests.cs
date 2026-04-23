// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;

namespace DSE.Open.Language.Annotations;

public sealed class UniversalFeatureValuesTests
{
    // Regression guards for copy-paste bugs where several feature values were
    // initialised with the wrong Universal Dependencies code.
    [Fact]
    public void TenseFuture_uses_Fut_code()
    {
        Assert.Equal((AlphaNumericCode)"Fut", UniversalFeatureValues.TenseFuture);
    }

    [Fact]
    public void MoodImperative_uses_Imp_code()
    {
        Assert.Equal((AlphaNumericCode)"Imp", UniversalFeatureValues.MoodImperative);
    }

    [Fact]
    public void MoodPotential_uses_Pot_code()
    {
        Assert.Equal((AlphaNumericCode)"Pot", UniversalFeatureValues.MoodPotential);
    }

    [Fact]
    public void Tense_values_are_distinct()
    {
        var values = new[]
        {
            UniversalFeatureValues.TensePresent,
            UniversalFeatureValues.TenseFuture,
            UniversalFeatureValues.TensePast,
            UniversalFeatureValues.TenseImperfect,
            UniversalFeatureValues.TensePluperfect,
        };

        Assert.Equal(values.Length, values.Distinct().Count());
    }

    [Fact]
    public void Mood_values_are_distinct()
    {
        var values = new[]
        {
            UniversalFeatureValues.MoodIndicative,
            UniversalFeatureValues.MoodImperative,
            UniversalFeatureValues.MoodConditional,
            UniversalFeatureValues.MoodPotential,
            UniversalFeatureValues.MoodSubjective,
        };

        Assert.Equal(values.Length, values.Distinct().Count());
    }
}
