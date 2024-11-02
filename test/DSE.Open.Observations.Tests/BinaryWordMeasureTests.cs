// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;

namespace DSE.Open.Observations;

public sealed class BinaryWordMeasureTests
{
    [Fact]
    public void JsonRoundtrip()
    {
        var measure = new BinaryWordMeasure(
            MeasureId.GetRandomId(),
            new Uri("https://schema-test.dseapi.app/testing/binary-word-measure"),
            "Test measure",
            "Test measure description");

        AssertJson.Roundtrip(measure, JsonContext.Default);
    }
}
