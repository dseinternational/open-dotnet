// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;

namespace DSE.Open.Observations;

public sealed class BinaryObservationTests
{
    [Fact]
    public void CanSerializeAndDeserialize()
    {
        var obs = BinaryObservation.Create(TestMeasures.BinaryMeasure, true);
        AssertJson.Roundtrip(obs);
    }
}
