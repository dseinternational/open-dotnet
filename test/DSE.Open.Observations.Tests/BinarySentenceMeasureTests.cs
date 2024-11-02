// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;

namespace DSE.Open.Observations;

public sealed class BinarySentenceMeasureTests
{
    [Fact]
    public void JsonRoundtrip()
    {
        var measure = new BinarySentenceMeasure(
            MeasureId.GetRandomId(),
            new Uri("https://schema-test.dseapi.app/testing/binary-sentence-measure"),
            "Test measure",
            "[subject] does something");

        AssertJson.Roundtrip(measure, JsonContext.Default);
    }
}
