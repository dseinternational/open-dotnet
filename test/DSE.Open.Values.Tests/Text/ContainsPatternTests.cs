// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Values.Text;

namespace DSE.Open.Values.Tests.Text;

public class ContainsPatternTests
{
    [Theory]
    [MemberData(nameof(ValidValues))]
    public void ParseParsesValidValues(string value)
    {
        var pattern = ContainsPattern.Parse(value);
        Assert.Equal(value, pattern.ToString());
    }

    [Fact]
    public void Serializes_to_string_value()
    {
        var pattern = ContainsPattern.Parse("flower* AND petal*");
        var serialized = JsonSerializer.Serialize(pattern);
        Assert.Equal("\"flower* AND petal*\"", serialized);
    }

    [Fact]
    public void Serialized_and_deserialized_values_are_equal()
    {
        var pattern = ContainsPattern.Parse("flower* AND petal*");
        var serialized = JsonSerializer.Serialize(pattern);
        var deserialized = JsonSerializer.Deserialize<ContainsPattern>(serialized);
        Assert.Equal(pattern, deserialized);
    }

    public static TheoryData<string> ValidValues
    {
        get
        {
            var values = new TheoryData<string>()
            {
                "ball",
                "bat AND ball",
                "bat* AND ball",
                "bat & ball",
                "bat OR ball",
                "bat || ball",
                "bat AND NOT ball",
                "bat &! ball",
                "bat NEAR ball",
                "bat NEAR ball*",
                "      bat NEAR ball",
                "      bat NEAR ball          ",
            };
            return values;
        }
    }
}
