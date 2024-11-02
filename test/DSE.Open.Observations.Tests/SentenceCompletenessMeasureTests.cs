// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;

namespace DSE.Open.Observations;

public sealed class SentenceCompletenessMeasureTests
{
    [Fact]
    public void JsonRoundtrip()
    {
        var measure = new SentenceCompletenessMeasure(
            MeasureId.GetRandomId(),
            new Uri("https://schema-test.dseapi.app/testing/sentence-completeness-measure"),
            "Test measure",
            "[subject] does something");

        AssertJson.Roundtrip(measure, JsonContext.Default);
    }
}
