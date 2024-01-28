// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Speech.Abstractions.Tests;

public class TranscriptionTests
{
    public TranscriptionTests(ITestOutputHelper output)
    {
        Output = output;
    }

    public ITestOutputHelper Output { get; }

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
        var original = SpeechTranscription.Parse(example, CultureInfo.InvariantCulture);
        var formatted = original.ToString();
        Assert.Equal(example, formatted);
    }

    [Theory]
    [InlineData("kloʊz/")]
    [InlineData("/kloʊs")]
    [InlineData("/x-ray/")]
    [InlineData("[prəˈvaɪd")]
    public void InvalidTranscriptionsThrowOnParse(string example)
    {
        _ = Assert.Throws<FormatException>(() => SpeechTranscription.Parse(example, CultureInfo.InvariantCulture));
    }

    [Theory]
    [MemberData(nameof(WordTranscriptions))]
    public void CanCreateWordTranscriptions(string example)
    {
        var original = SpeechTranscription.Parse(example, CultureInfo.InvariantCulture);
        var formatted = original.ToString();
        Assert.Equal(example, formatted);
    }

    public static TheoryData<string> WordTranscriptions =>
        new(TranscriptionData.Transcriptions.Select(t => $"[{t}]").ToArray());
}
