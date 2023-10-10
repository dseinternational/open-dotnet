// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Globalization;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using DSE.Open.Values;

namespace DSE.Open.Benchmarks.Text.Json;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser]
public class UriAsciiPathJsonBenchmarks
{
    private const string Path = "a/b/c/d/e/f/g/h";
    private static readonly UriAsciiPath s_pathValue = UriAsciiPath.Parse(Path, CultureInfo.InvariantCulture);

    [Benchmark]
    public UriAsciiPath RoundTrip()
    {
        var json = JsonSerializer.Serialize(s_pathValue);
        return JsonSerializer.Deserialize<UriAsciiPath>(json);
    }
}
