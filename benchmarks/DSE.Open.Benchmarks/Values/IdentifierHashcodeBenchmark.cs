// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using BenchmarkDotNet.Attributes;
using DSE.Open.Values;

namespace DSE.Open.Benchmarks.Values;

[MemoryDiagnoser]
public class IdentifierHashcodeBenchmark
{
    private Identifier _identifier;
    private AsciiString _identifierStr;

    [GlobalSetup]
    public void Setup()
    {
        _identifier = Identifier.New();
        _identifierStr = (AsciiString)_identifier;
    }

    [Benchmark(Baseline = true)]
    public int BaseAsciiStringHash()
    {
        return _identifierStr.GetHashCode();
    }

    [Benchmark]
    public int IdentifierHash()
    {
        return _identifier.GetHashCode();
    }
}
