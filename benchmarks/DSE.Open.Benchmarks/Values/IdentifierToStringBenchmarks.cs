// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using BenchmarkDotNet.Attributes;
using DSE.Open.Values;

namespace DSE.Open.Benchmarks.Values;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser(displayGenColumns: false)]
public class IdentifierToStringBenchmarks
{
    private static readonly Identifier s_identifier = Identifier.Parse("dse_sub_VjHlsZTVmKRGglRRjSkPeQL71M17c7sQ"u8, null);

    [Benchmark]
    public string ToString_Default()
    {
        return s_identifier.ToString();
    }
}
