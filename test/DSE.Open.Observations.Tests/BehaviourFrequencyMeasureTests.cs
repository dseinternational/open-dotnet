// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;

namespace DSE.Open.Observations;

public sealed class BehaviourFrequencyMeasureTests
{
    [Fact]
    public void JsonRoundtrip()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");

        var measure =
            new BehaviorFrequencyMeasure(MeasureId.GetRandomId(), uri, "Test measure", "[subject] does something");

        AssertJson.Roundtrip(measure, JsonContext.Default);
    }
}
