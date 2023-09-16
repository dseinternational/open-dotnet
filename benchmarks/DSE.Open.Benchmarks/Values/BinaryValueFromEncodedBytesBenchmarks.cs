// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using BenchmarkDotNet.Attributes;

namespace DSE.Open.Benchmarks.Values;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser(displayGenColumns: false)]
[ExecutionValidator(true)]
public class BinaryValueFromEncodedBytesBenchmarks
{
    private readonly byte[] _upperHexEncoded;
    private readonly byte[] _lowerHexEncoded;
    private readonly byte[] _base64Encoded;
    private readonly byte[] _base62Encoded;

    public BinaryValueFromEncodedBytesBenchmarks()
    {
        var value = BinaryValue.FromEncodedString("Hello, World!");

        Span<byte> buffer = stackalloc byte[128];

        value.TryFormat(buffer, out var base64Written, "B", provider: default);
        _base64Encoded = buffer[..base64Written].ToArray();

        value.TryFormat(buffer, out var base62Written, "b", provider: default);
        _base62Encoded = buffer[..base62Written].ToArray();

        value.TryFormat(buffer, out var hexUpperWritten, "X", provider: default);
        _upperHexEncoded = buffer[..hexUpperWritten].ToArray();

        value.TryFormat(buffer, out var hexLowerWritten, "x", provider: default);
        _lowerHexEncoded = buffer[..hexLowerWritten].ToArray();
    }

    [Benchmark]
    public bool FromBase62EncodedBytes() => BinaryValue.TryFromEncodedBytes(_base62Encoded, BinaryStringEncoding.Base62, out _);

    [Benchmark]
    public bool FromBase64EncodedBytes() => BinaryValue.TryFromEncodedBytes(_base64Encoded, BinaryStringEncoding.Base64, out _);

    [Benchmark]
    public bool FromHexUpperBytes() => BinaryValue.TryFromEncodedBytes(_upperHexEncoded, BinaryStringEncoding.HexUpper, out _);

    [Benchmark]
    public bool FromHexLowerBytes() => BinaryValue.TryFromEncodedBytes(_lowerHexEncoded, BinaryStringEncoding.HexLower, out _);
}
