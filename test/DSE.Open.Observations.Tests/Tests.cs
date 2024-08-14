// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System;
using DSE.Open.Collections.Generic;
using DSE.Open.Language;
using DSE.Open.Values;
using MessagePack;
using MessagePack.Resolvers;
using Xunit;

namespace DSE.Open.Observations;

public sealed class Tests
{
    private static readonly Uri s_uri = new("https://schema-test.dseapi.app/testing/measure");

    private static readonly BinaryMeasure s_binaryMeasure =
        new(MeasureId.GetRandomId(), s_uri, "Test measure", "[subject] does something");

    private static readonly ReadOnlyValueCollection<BinaryWordSnapshot> s_collection =
    [
        BinaryWordSnapshot.ForUtcNow(BinaryWordObservation.Create(s_binaryMeasure, WordId.GetRandomId(), true))
    ];

    private static readonly BinaryWordSnapshotSet s_set = BinaryWordSnapshotSet.Create(Identifier.New(), s_collection);

    [Fact]
    public void MethodName_WithWhat_ShouldDoWhat()
    {
        var resolver = CompositeResolver.Create(
            StandardResolver.Instance
        );

        var options = MessagePackSerializerOptions.Standard.WithResolver(resolver);

        var bytes = MessagePackSerializer.Serialize(s_set, options);
        var d = MessagePackSerializer.Deserialize<BinaryWordSnapshotSet>(bytes, options);

        Assert.NotNull(d);
        Assert.IsType<BinaryWordSnapshotSet>(d);
        Assert.Equivalent(s_set, d);
    }
}
