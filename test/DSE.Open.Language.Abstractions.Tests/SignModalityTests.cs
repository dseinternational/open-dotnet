// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;

namespace DSE.Open.Language;

public class SignModalityTests
{
    public static TheoryData<SignModality, string> KnownModalities { get; } = new()
    {
        { SignModality.Pictured, "pictured" },
        { SignModality.Spoken, "spoken" },
        { SignModality.Gestured, "gestured" },
        { SignModality.Written, "written" },
    };

    [Theory]
    [MemberData(nameof(KnownModalities))]
    public void ToString_ReturnsCanonicalValue(SignModality modality, string expected)
    {
        Assert.Equal(expected, modality.ToString());
    }

    [Theory]
    [MemberData(nameof(KnownModalities))]
    public void Parse_ReturnsKnownModality(SignModality modality, string value)
    {
        Assert.Equal(modality, SignModality.Parse(value, CultureInfo.InvariantCulture));
    }

    [Theory]
    [MemberData(nameof(KnownModalities))]
    public void SerializeDeserialize(SignModality modality, string _)
    {
        AssertJson.Roundtrip(modality);
    }

    [Theory]
    [InlineData("Pictured")]
    [InlineData("PICTURED")]
    [InlineData("unknown")]
    [InlineData("")]
    public void Parse_RejectsInvalidValues(string value)
    {
        _ = Assert.Throws<FormatException>(() => SignModality.Parse(value, CultureInfo.InvariantCulture));
    }

    [Fact]
    public void All_ContainsAllKnownModalities()
    {
        Assert.Contains(SignModality.Pictured, SignModality.All);
        Assert.Contains(SignModality.Spoken, SignModality.All);
        Assert.Contains(SignModality.Gestured, SignModality.All);
        Assert.Contains(SignModality.Written, SignModality.All);
        Assert.Equal(4, SignModality.All.Count);
    }
}
