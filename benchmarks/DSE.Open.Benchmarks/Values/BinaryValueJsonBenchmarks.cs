// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using BenchmarkDotNet.Attributes;

#pragma warning disable CA1822 // Mark members as static

namespace DSE.Open.Benchmarks.Values;

[MemoryDiagnoser(false)]
public class BinaryValueJsonBenchmarks
{
    private static readonly BinaryValue s_value = BinaryValue.FromEncodedString("Hello, world!");

    private static readonly string s_json = JsonSerializer.Serialize(s_value);

    [Benchmark]
    public BinaryValue Deserialize() => JsonSerializer.Deserialize<BinaryValue>(s_json);

    [Benchmark]
    public string Serialize() => JsonSerializer.Serialize(s_value);
}
