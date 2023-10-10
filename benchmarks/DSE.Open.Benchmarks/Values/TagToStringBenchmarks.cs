// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Globalization;
using BenchmarkDotNet.Attributes;
using DSE.Open.Values;

namespace DSE.Open.Benchmarks.Values;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser]
public class TagToStringBenchmarks
{
    private const string Str = "tag:something/else";
    private static readonly Tag s_tag = Tag.Parse(Str, CultureInfo.InvariantCulture);

    [GlobalSetup]
    public void Setup() => _ = Tag.GetString(Str); // Make sure it's in the pool

    [Benchmark]
    public string TagToString() => s_tag.ToString();
}
