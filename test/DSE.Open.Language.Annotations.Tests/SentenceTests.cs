// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Annotations;

public class SentenceTests
{
    public SentenceTests(ITestOutputHelper output)
    {
        Output = output;
    }

    public ITestOutputHelper Output { get; }

    [Fact]
    public void FormatSentence()
    {
        var tokens = TheCatIsDrinkingMilk
            .Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(Token.ParseInvariant);

        var sentence = new Sentence
        {
            Text = "He ate a dog.",
            Tokens = [.. tokens],
            Comments = []
        };

        var formatted = sentence.ToString();

        Output.WriteLine(formatted);
    }

    [Fact]
    public void GetRepeatableHashCode_ReturnsExpectedValue()
    {
        var tokens = TheCatIsDrinkingMilk
            .Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(Token.ParseInvariant);

        var sentence = new Sentence
        {
            Text = "He ate a dog.",
            Tokens = [.. tokens],
            Comments = []
        };

        Assert.Equal(9629756338239080133u, sentence.GetRepeatableHashCode());
    }

    private const string TheCatIsDrinkingMilk =
        "1\tThe\tthe\tDET\tDT\tDefinite=Def|PronType=Art\t2\tdet\t_\t_" + "\n" +
        "2\tcat\tcat\tNOUN\tNN\tNumber=Sing\t4\tnsubj\t_\t_" + "\n" +
        "3\tis\tbe\tAUX\tVBZ\tMood=Ind|Number=Sing|Person=3|Tense=Pres|VerbForm=Fin\t4\taux\t_\t_" + "\n" +
        "4\tdrinking\tdrink\tVERB\tVBG\tTense=Pres|VerbForm=Part\t0\troot\t_\t_" + "\n" +
        "5\tmilk\tmilk\tNOUN\tNN\tNumber=Sing\t4\tobj\t_\t_" + "\n" +
        "6\t.\t.\tPUNCT\t.\t_\t4\tpunct\t_\t_" + "\n";

    [Fact]
    public void ReadConllu_HeCanSwimAndMustPractiseEveryDay()
    {
        var sentence = Sentence.FromConllu(ExampleSentenceDefinitions.HeCanSwimAndMustPractiseEveryDay);
        Assert.Equal("He can swim and must practise every day, but he might rest tomorrow.", sentence.Text);
        Assert.Equal(15, sentence.Tokens.Count);
    }

    [Fact]
    public void ReadConllu_TheDogIsRunningAndTheHorseIsWalking()
    {
        var sentence = Sentence.FromConllu(ExampleSentenceDefinitions.TheDogIsRunningAndTheHorseIsWalking);
        Assert.Equal("The dog is running and the horse is walking, but the snail is crawling and the duck is swimming and the bird is flying.", sentence.Text);
        Assert.Equal(26, sentence.Tokens.Count);
    }
}
