// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Security.Cryptography;
using System.Text;

namespace DSE.Open.Tests;

public class Base62ConverterTests
{
    private readonly ITestOutputHelper _output;

    public Base62ConverterTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void ToBase62String_FromBase62_roundtrip()
    {
        for (var i = 0; i < 100; i++)
        {
            var data = RandomNumberGenerator.GetBytes(8);
            var encoded = Base62Converter.ToBase62String(data);

            _output.WriteLine($"{Convert.ToHexString(data)} {encoded}");

            var decoded = Base62Converter.FromBase62(encoded);

            Assert.True(data.SequenceEqual(decoded));
        }
    }

    [Fact]
    public void ToBase62_ShouldCorrectlyEncode()
    {
        foreach (var (text, base62) in s_validTestCases)
        {
            if (!Base62Converter.TryFromBase62(base62, out var data))
            {
                Assert.Fail("Failed to decode input");
            }

            var actual = Base62Converter.ToBase62String(data);

            Assert.Equal(base62, actual);
            Assert.Equal(text, Encoding.UTF8.GetString(data));
        }
    }

    [Fact]
    public void TryFromBase62_WithEmpty_ShouldReturnTrueAndEmpty()
    {
        // Arrange
        var encoded = string.Empty;

        // Act
        var succeeded = Base62Converter.TryFromBase62(encoded, out var data);

        // Assert
        Assert.True(succeeded);
        Assert.Empty(data);
    }

    [Fact]
    public void TryFromBase62Chars_WithEmpty_ShouldReturnTrueAndEmpty()
    {
        // Arrange
        ReadOnlySpan<char> encoded = string.Empty;

        // Act
        var succeeded = Base62Converter.TryFromBase62Chars(encoded, out var data);

        // Assert
        Assert.True(succeeded);
        Assert.Empty(data);
    }

    [Fact]
    public void TryFromBase62Chars_WithInvalidData_ShouldReturnFalse()
    {
        // Arrange
        const string invalid = "!@#$%^&*()";

        // Act
        var succeeded = Base62Converter.TryFromBase62Chars(invalid, out var data);

        // Assert
        Assert.False(succeeded);
        Assert.Empty(data);
    }

    [Theory]
    [InlineData("5h7Ao8e5qPCZNKJEoACLAvJ2bT6srtfrllMyoqdDrmBRhyxAQjyPBz5yz3NEJMAnSmTgFATqRUulAchYsMGm5q")]
    public void TryParse(string encoded)
    {
        var succeeded = Base62Converter.TryFromBase62(encoded, out _);
        Assert.True(succeeded);
    }

    private static readonly Dictionary<string, string> s_validTestCases = new()
    {
        { "a", "1Z" },
        { "Z", "1S" },
        { "Hello, world!", "1wJfrzvdbthTq5ANZB" },
        {
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Amet nisl suscipit adipiscing bibendum est ultricies integer quis. Sed risus pretium quam vulputate. Lorem ipsum dolor sit amet consectetur adipiscing elit pellentesque habitant. Ultricies mi quis hendrerit dolor. Proin sed libero enim sed faucibus turpis in eu. Sed vulputate odio ut enim. Quis hendrerit dolor magna eget est lorem ipsum. Vestibulum lorem sed risus ultricies tristique nulla aliquet. Elementum pulvinar etiam non quam lacus suspendisse faucibus interdum.",
            "7SV7pJHP2QYm18yr6naoggbJoylyTFryGTE5Q5kj58nU7A0mWg7J4A55C5qDmegD4Gp9P3AukZ3BjXfL9YnFlAcf73lxohbMKKERlljHdtmqW1jetkhaqMt6VEyBDFaR1Y3Pvd3ZsrIDV4164SZl5dkkFSGIQ1MXNTUUgzhExpo94XAz0OTobuqpTnZ8a8GLCT1y5p3KWjSH3k0l4FS26ZeJL5TahJqQ2fbKrBvZxn3vlekiuV5R5H9pMAfbcdce9jOJQBXjoMqysuw29PEavTOkK4yB3hMTjRDB0usdP5DMErbN468rxau2n3LasLmBoL1gJzKO8LHRhLYEdLrmACVyog5EWBcSHV2GCblTkr5OmO4RKQDQaZ4543FfH64tMfiRju4ValfqC346ULDdhqfWckE5DNNr4d08Do77HV4K5KnLND2JnpWkUPtBs8yjXGskPoEpgGeh0kLHqddLFdeSqqI84GiZzoinLIu8yawyAw4veClhexXVgX4Lyy3vabpd4witHLRMvDlU3hJIsY6f7FUcN0kwz4xMnMOjCoDjTJII3kmHm7G9I1JwVuNNJpqmriCqCoOmztA18HaKYhQIh4tduteaRvuD1rsAIlHQefoh52YNkeCLhYvIb7ZNqbgZkpc9J0boAf41rmZXwlKT5hkQLQPgZIngzqDLGgJxX1686KhAY3hUxGvUhZhFFKHQ65ud15ncqEYPFBeuteE3IH2qor7qiBVvp3jzrTyYgTHKrHC3o12COVVkEUdGvOtUauT7tpeiglFjsLpbPkGz8I2EiE8re"
        }
    };
}
