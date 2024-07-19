// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public class ObservationEqualityComparerTests
{
    [Fact]
    public void DefaultNotEqualIfValuesDiffer()
    {
        var obs1 = Observation.Create(68436815, true);
        var obs2 = Observation.Create(68436815, false);
        Assert.False(ObservationEqualityComparer<Observation<bool>>.Default.Equals(obs1, obs2));
    }

    [Fact]
    public void DiscriminatorEqualIfDiscriminatorEqual()
    {
        var obs1 = Observation.Create(68436815, true);
        var obs2 = Observation.Create(68436815, false);
        Assert.True(ObservationEqualityComparer<Observation<bool>>.Discriminator.Equals(obs1, obs2));
    }
}
