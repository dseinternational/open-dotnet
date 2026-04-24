// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json;

namespace DSE.Open;

public class Date64Tests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(1619827200000)]
    [InlineData(-1)]
    [InlineData(-62135596800000)]
    [InlineData(253402300799999)]
    public void Create_Milliseconds(long milliseconds)
    {
        var date = new DateTime64(milliseconds);
        Assert.Equal(milliseconds, date.TotalMilliseconds);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(milliseconds), date);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(1619827200000)]
    [InlineData(-1)]
    [InlineData(-62135596800000)]
    [InlineData(253402300799999)]
    public void Create_DateTimeOffset(long milliseconds)
    {
        var dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(milliseconds);
        var date = new DateTime64(milliseconds);
        Assert.Equal(milliseconds, date.TotalMilliseconds);
        Assert.Equal(dateTimeOffset, date);
    }

    [Fact]
    public void Serialize()
    {
        var date = new DateTime64(1619827200000);
        var json = JsonSerializer.Serialize(date);
        Assert.Equal("1619827200000", json);
    }

    [Fact]
    public void Deserialize()
    {
        var date = JsonSerializer.Deserialize<DateTime64>("1619827200000");
        Assert.Equal(1619827200000, date.TotalMilliseconds);
    }

    [Theory]
    [InlineData(1L)]
    [InlineData(1000L)]
    [InlineData(1619827200000L)]
    public void UnaryNegation_NegatesValue(long milliseconds)
    {
        var value = new DateTime64(milliseconds);

        var negated = Negate(value);

        Assert.Equal(-milliseconds, negated.TotalMilliseconds);

        static T Negate<T>(T v) where T : IUnaryNegationOperators<T, T>
        {
            return -v;
        }
    }

    [Fact]
    public void BinaryInteger_BigEndianRoundTrip()
    {
        var value = new DateTime64(1619827200000L);
        Span<byte> buffer = stackalloc byte[sizeof(long)];

        var writeSuccess = TryWriteBigEndian(value, buffer, out var bytesWritten);
        var readSuccess = TryReadBigEndian(buffer[..bytesWritten], out DateTime64 result);

        Assert.True(writeSuccess);
        Assert.True(readSuccess);
        Assert.Equal(value, result);

        static bool TryWriteBigEndian<T>(T value, Span<byte> destination, out int bytesWritten)
            where T : IBinaryInteger<T>
        {
            return value.TryWriteBigEndian(destination, out bytesWritten);
        }

        static bool TryReadBigEndian<T>(ReadOnlySpan<byte> source, out T value)
            where T : IBinaryInteger<T>
        {
            return T.TryReadBigEndian(source, isUnsigned: false, out value);
        }
    }

    [Fact]
    public void BinaryInteger_BitwiseAnd_UsesMilliseconds()
    {
        var result = And(new DateTime64(3), new DateTime64(1));

        Assert.Equal(1, result.TotalMilliseconds);

        static T And<T>(T left, T right)
            where T : IBitwiseOperators<T, T, T>
        {
            return left & right;
        }
    }
}
