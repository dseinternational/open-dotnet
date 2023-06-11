// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Tests;

public class BinaryValueTests
{
    [Fact]
    public void To_from_base62()
    {
        for (var i = 0; i < 100; i++)
        {
            var value = BinaryValue.GetRandomValue();
            var encoded = value.ToBase62EncodedString();
            var value2 = BinaryValue.FromBase62EncodedString(encoded);
            Assert.Equal(value, value2);
        }
    }

    [Fact]
    public void To_from_base64()
    {
        for (var i = 0; i < 100; i++)
        {
            var value = BinaryValue.GetRandomValue();
            var encoded = value.ToBase64EncodedString();
            var value2 = BinaryValue.FromBase64EncodedString(encoded);
            Assert.Equal(value, value2);
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("in/valid")]
    [InlineData("i~nvalid")]
    [InlineData("invalid*")]
    public void TryFromBase62EncodedString_fails_with_invalid_data(string encoded)
    {
        var succeeded = BinaryValue.TryFromBase62EncodedString(encoded, out _);
        Assert.False(succeeded);
    }

    [Theory]
    [InlineData("5h7Ao8e5qPCZNKJEoACLAvJ2bT6srtfrllMyoqdDrmBRhyxAQjyPBz5yz3NEJMAnSmTgFATqRUulAchYsMGm5q")]
    public void TryFromBase62EncodedString_succeeds_with_valid_data(string encoded)
    {
        var succeeded = BinaryValue.TryFromBase62EncodedString(encoded, out _);
        Assert.True(succeeded);
    }

    [Theory]
    [InlineData("5h7Ao8e5qPCZNKJEoACLAvJ2bT6srtfrllMyoqdDrmBRhyxAQjyPBz5yz3NEJMAnSmTgFATqRUulAchYsMGm5q")]
    public void TryFromEncodedString_succeeds_with_valid_data(string encoded)
    {
        var succeeded = BinaryValue.TryFromEncodedString(encoded, BinaryStringEncoding.Base62, out _);
        Assert.True(succeeded);
    }
}
