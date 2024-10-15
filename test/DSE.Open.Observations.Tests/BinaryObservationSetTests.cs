// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed class BinaryObservationSetTests
{
    [Fact]
    public void CanSerializeAndDeserialize()
    {
        var obs = CreateBinaryObservationSet();
        AssertJson.Roundtrip(obs);
    }

    [Fact]
    public void JsonRoundTrip_WithContext()
    {
        var obs = CreateBinaryObservationSet();
        AssertJson.Roundtrip(obs, ObservationsJsonSerializerContext.RelaxedJsonEscaping);
    }

    private static BinaryObservationSet CreateBinaryObservationSet()
    {
        return BinaryObservationSet.Create(
            Identifier.New("trk"),
            Identifier.New("obr"),
            new Uri("https://test.dsegroup.net"),
            [
                BinaryObservation.Create(TestMeasures.BinaryMeasure, true),
                BinaryObservation.Create(TestMeasures.BinaryMeasure2, false),
            ]
        );
    }
}
