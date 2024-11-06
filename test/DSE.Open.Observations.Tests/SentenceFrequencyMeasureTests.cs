// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;

namespace DSE.Open.Observations;

public sealed class SentenceFrequencyMeasureTests
{
    [Fact]
    public void JsonRoundtrip()
    {
        var measure = new SentenceFrequencyMeasure(
            MeasureId.GetRandomId(),
            new Uri("https://schema-test.dseapi.app/testing/sentence-frequency-measure"),
            "Test measure",
            "[subject] does something");

        AssertJson.Roundtrip(measure, JsonContext.Default);
    }
}