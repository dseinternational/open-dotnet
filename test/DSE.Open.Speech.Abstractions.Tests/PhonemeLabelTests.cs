// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Speech.Abstractions.Tests;

public class PhonemeLabelTests
{
    [Fact]
    public void SeeAndLearnV2Labels_count_equals_41()
    {
        var labels = PhonemeLabel.Labels[PhonemeLabelScheme.SeeAndLearnV2];
        Assert.Equal(41, labels.Count); 
    }
}
