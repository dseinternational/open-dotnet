// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;

namespace DSE.Open.Language;

public class SignTests
{
    [Theory]
    [InlineData("spoken", "ball", "spoken:ball")]
    [InlineData("written", "ball", "written:ball")]
    [InlineData("gestured", "ball", "gestured:ball")]
    [InlineData("pictured", "ball", "pictured:ball")]
    public void ToStringReturnsExpected(string modalityValue, string wordValue, string expected)
    {
        var modality = SignModality.Parse(modalityValue, CultureInfo.InvariantCulture);
        var word = WordText.Parse(wordValue, CultureInfo.InvariantCulture);
        var sign = new Sign(modality, word);
        Assert.Equal(expected, sign.ToString());
    }

    [Theory]
    [InlineData("spoken:ball")]
    [InlineData("written:ball")]
    [InlineData("gestured:ball")]
    [InlineData("pictured:ball")]
    public void SerializeDeserialize(string signValue)
    {
        var sign = Sign.Parse(signValue, CultureInfo.InvariantCulture);
        AssertJson.Roundtrip(sign);
    }
}
