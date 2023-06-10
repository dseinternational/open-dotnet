// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Security.Cryptography;

namespace DSE.Open.Tests;

public class Base62ConverterTests
{
    public Base62ConverterTests(ITestOutputHelper output)
    {
        Output = output;
    }

    public ITestOutputHelper Output { get; }

    [Fact]
    public void ToBase62String_FromBase62_roundtrip()
    {
        for (var i = 0; i < 100; i++)
        {
            var data = RandomNumberGenerator.GetBytes(8);
            var encoded = Base62Converter.ToBase62String(data);

            Output.WriteLine($"{Convert.ToHexString(data)} {encoded}");

            var decoded = Base62Converter.FromBase62(encoded);

            Assert.True(data.SequenceEqual(decoded));
        }
    }

    [Theory]
    [InlineData("5h7Ao8e5qPCZNKJEoACLAvJ2bT6srtfrllMyoqdDrmBRhyxAQjyPBz5yz3NEJMAnSmTgFATqRUulAchYsMGm5q")]
    public void TryParse(string encoded)
    {
        var succeeded = Base62Converter.TryFromBase62(encoded, out _);
        Assert.True(succeeded);
    }
}
