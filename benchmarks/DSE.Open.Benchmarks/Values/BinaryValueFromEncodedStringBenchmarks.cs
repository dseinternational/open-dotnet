// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using BenchmarkDotNet.Attributes;

namespace DSE.Open.Benchmarks.Values;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser(displayGenColumns: false)]
public class BinaryValueFromEncodedStringBenchmarks
{
    private static readonly string s_base62Encoded = BinaryValue.FromEncodedString("Hello, World!").ToString(BinaryStringEncoding.Base62);

    private static readonly string s_base64Encoded = BinaryValue.FromEncodedString("Hello, World!").ToString(BinaryStringEncoding.Base64);

    private static readonly string s_hexLowerEncoded = BinaryValue.FromEncodedString("Hello, World!").ToString(BinaryStringEncoding.HexLower);

    private static readonly string s_hexUpperEncoded = BinaryValue.FromEncodedString("Hello, World!").ToString(BinaryStringEncoding.HexUpper);

    [Benchmark]
    public BinaryValue FromBase62EncodedString()
    {
        return BinaryValue.FromEncodedString(s_base62Encoded, BinaryStringEncoding.Base62);
    }

    [Benchmark]
    public BinaryValue FromBase64EncodedString()
    {
        return BinaryValue.FromEncodedString(s_base64Encoded, BinaryStringEncoding.Base64);
    }

    [Benchmark]
    public BinaryValue FromHexLowerString()
    {
        return BinaryValue.FromEncodedString(s_hexLowerEncoded, BinaryStringEncoding.HexLower);
    }

    [Benchmark]
    public BinaryValue FromHexUpperString()
    {
        return BinaryValue.FromEncodedString(s_hexUpperEncoded, BinaryStringEncoding.HexUpper);
    }
}
