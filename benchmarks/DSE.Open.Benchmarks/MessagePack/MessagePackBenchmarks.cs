// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using BenchmarkDotNet.Attributes;
using DSE.Open.Collections.Generic;
using DSE.Open.Language;
using DSE.Open.Observations;
using DSE.Open.Values;
using MessagePack;

namespace DSE.Open.Benchmarks.MessagePack;

#pragma warning disable CA1822

[MemoryDiagnoser(false)]
public class MessagePackBenchmarks
{
    private static readonly Uri s_uri = new("https://schema-test.dseapi.app/testing/measure");

    private static readonly BinaryMeasure s_binaryMeasure =
        new(MeasureId.GetRandomId(), s_uri, "Test measure", "[subject] does something");

    private static readonly ReadOnlyValueCollection<BinaryWordSnapshot> s_collection =
    [
        BinaryWordSnapshot.ForUtcNow(BinaryWordObservation.Create(s_binaryMeasure, WordId.GetRandomId(), true))
    ];

    private static readonly BinaryWordSnapshotSet s_set = BinaryWordSnapshotSet.Create(Identifier.New(), s_collection);

    [Benchmark]
    public BinaryWordSnapshotSet MessagePack()
    {
        var bytes = MessagePackSerializer.Serialize(s_set);
        return MessagePackSerializer.Deserialize<BinaryWordSnapshotSet>(bytes);
    }

    [Benchmark(Baseline = true)]
    public BinaryWordSnapshotSet Json()
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(s_set);
        return JsonSerializer.Deserialize<BinaryWordSnapshotSet>(bytes)!;
    }
}
