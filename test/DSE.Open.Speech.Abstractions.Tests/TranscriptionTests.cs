// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Speech.Tests;

public class TranscriptionTests
{
    [Theory]
    [InlineData("/kloʊz/")]
    [InlineData("/kloʊs/")]
    [InlineData("/ˈɛərəpleɪn/")]
    [InlineData("/prəˈvaɪd/")]
    [InlineData("/ˈzaɪləfoʊn/")]
    [InlineData("/ˈzɛbrə/")]
    [InlineData("/ʃɛər/")]
    [InlineData("/tʃɛər/")]
    public void CanCreateAndFormat(string example)
    {
        var original = new Transcription(example);
        var formatted = original.ToString();
        Assert.Equal(example, formatted);
    }
}
