// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using BenchmarkDotNet.Attributes;

namespace DSE.Open.Benchmarks.Values;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser(displayGenColumns: false)]
[ExecutionValidator(true)]
public class BinaryValueFromEncodedCharsBenchmarks
{
    private readonly char[] _upperHexEncoded;
    private readonly char[] _lowerHexEncoded;
    private readonly char[] _base64Encoded;
    private readonly char[] _base62Encoded;

    public BinaryValueFromEncodedCharsBenchmarks()
    {
        var value = BinaryValue.FromEncodedString("Hello, World!");

        Span<char> buffer = stackalloc char[128];

        value.TryFormat(buffer, out var base64Written, "B", provider: default);
        _base64Encoded = buffer[..base64Written].ToArray();

        Console.WriteLine($"Base64: {new string(_base64Encoded)}");

        value.TryFormat(buffer, out var base62Written, "b", provider: default);
        _base62Encoded = buffer[..base62Written].ToArray();

        Console.WriteLine($"Base62: {new string(_base62Encoded)}");

        value.TryFormat(buffer, out var hexUpperWritten, "X", provider: default);
        _upperHexEncoded = buffer[..hexUpperWritten].ToArray();

        Console.WriteLine($"HexUpper: {new string(_upperHexEncoded)}");

        value.TryFormat(buffer, out var hexLowerWritten, "x", provider: default);
        _lowerHexEncoded = buffer[..hexLowerWritten].ToArray();

        Console.WriteLine($"HexLower: {new string(_lowerHexEncoded)}");
    }

    [Benchmark]
    public bool FromBase62EncodedChars() => BinaryValue.TryFromEncodedChars(_base62Encoded, BinaryStringEncoding.Base62, out _);

    [Benchmark]
    public bool FromBase64EncodedChars() => BinaryValue.TryFromEncodedChars(_base64Encoded, BinaryStringEncoding.Base64, out _);

    [Benchmark]
    public bool FromHexUpperChars() => BinaryValue.TryFromEncodedChars(_upperHexEncoded, BinaryStringEncoding.HexUpper, out _);

    [Benchmark]
    public bool FromHexLowerChars() => BinaryValue.TryFromEncodedChars(_lowerHexEncoded, BinaryStringEncoding.HexLower, out _);
}
