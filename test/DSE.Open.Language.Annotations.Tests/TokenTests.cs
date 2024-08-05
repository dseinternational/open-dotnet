// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;

namespace DSE.Open.Language.Annotations;

public class TokenTests
{
    [Theory]
    [InlineData("1\tHe\the\tPRON\tPRP\tCase=Nom|Gender=Masc|Number=Sing|Person=3|PronType=Prs\t2\tnsubj\t_\t_")]
    [InlineData("3\tbrushing\tbrush\tVERB\tVBG\tAspect=Prog|Tense=Pres|VerbForm=Part\t0\tROOT\t_\t_")]
    [InlineData("4\ta\ta\tDET\tDT\tDefinite=Ind|PronType=Art\t5\tdet\t_\t_")]
    [InlineData("5\tdog\tdog\tNOUN\tNN\tNumber=Sing\t3\tdobj\t_\tSpaceAfter=No")]
    [InlineData("6\t.\t.\tPUNCT\t.\tPunctType=Peri\t3\tpunct\t_\tSpaceAfter=No")]
    [InlineData("2\tate\teat\tVERB\tVBD\tMood=Ind|Number=Sing|Person=3|Tense=Past|VerbForm=Fin\t0\troot\t_\t_")]
    [InlineData("1\tThe\tthe\tDET\tDT\tDefinite=Def|PronType=Art\t2\tdet\t_\t_")]
    [InlineData("2\tcat\tcat\tNOUN\tNN\tNumber=Sing\t4\tnsubj\t_\t_")]
    [InlineData("3\tis\tbe\tAUX\tVBZ\tMood=Ind|Number=Sing|Person=3|Tense=Pres|VerbForm=Fin\t4\taux\t_\t_")]
    [InlineData("4\tdrinking\tdrink\tVERB\tVBG\tTense=Pres|VerbForm=Part\t0\troot\t_\t_")]
    [InlineData("5\tmilk\tmilk\tNOUN\tNN\tNumber=Sing\t4\tobj\t_\t_")]
    [InlineData("6\t.\t.\tPUNCT\t.\t_\t4\tpunct\t_\t_")]
    public void Parse(string value)
    {
        var token = Word.Parse(value, CultureInfo.InvariantCulture);
        Assert.NotNull(token);
    }

    [Fact]
    public void FormatToStringAndParse()
    {
        var token = new Token
        {
            Text = (TokenText)"He",
            Words =
            [
                new()
                {
                    Index = 1,
                    Form = (WordText)"He",
                    Lemma = (WordText)"he",
                    Pos = UniversalPosTag.Pronoun,
                    AltPos = TreebankPosTag.PronounPersonal,
                    Features = ReadOnlyWordFeatureValueCollection.ParseInvariant("Case=Nom|Gender=Masc|Number=Sing|Person=3|PronType=Prs"),
                    HeadIndex = 2,
                    Relation = UniversalRelationTag.NominalSubject,
                }
            ]
        };

        var str = token.ToString();

        const string expected = "1\tHe\the\tPRON\tPRP\tCase=Nom|Gender=Masc|Number=Sing|Person=3|PronType=Prs\t2\tnsubj\t_\t_";

        Assert.Equal(expected, str);

        var parsed = Token.Parse(str, CultureInfo.InvariantCulture);

        Assert.Equal(token, parsed);
    }

    [Fact]
    public void SerializeDeserialize()
    {
        var token = new Token
        {
            Text = (TokenText)"He",
            Words =
            [
                new()
                {
                    Index = 1,
                    Form = (WordText)"He",
                    Lemma = (WordText)"he",
                    Pos = UniversalPosTag.Pronoun,
                    AltPos = TreebankPosTag.PronounPersonal,
                    Features = ReadOnlyWordFeatureValueCollection.ParseInvariant("Case=Nom|Gender=Masc|Number=Sing|Person=3|PronType=Prs"),
                    HeadIndex = 2,
                    Relation = UniversalRelationTag.NominalSubject,
                }
            ]
        };

        AssertJson.Roundtrip(token);
    }

    [Fact]
    public void FormatMultiwordToStringAndParse()
    {
        var token = new Token
        {
            Text = (TokenText)"cat's",
            Words =
            [
                new()
                {
                    Index = 1,
                    Form = (TokenText)"cat",
                    Lemma = (TokenText)"cat",
                    Pos = UniversalPosTag.Noun,
                    AltPos = TreebankPosTag.NounSingularOrMass,
                    Features = [],
                    HeadIndex = 0,
                    Relation = UniversalRelationTag.PossessiveNominalModifier,
                },
                new()
                {
                    Index = 2,
                    Form = (TokenText)"'s",
                    Lemma = (TokenText)"'s",
                    Pos = UniversalPosTag.Particle,
                    AltPos = TreebankPosTag.PossessiveEnding,
                    Features = [],
                    HeadIndex = 0,
                    Relation = UniversalRelationTag.CaseMarking,
                }
            ]
        };

        var str = token.ToString();

        const string expected =
            "1-2\tcat's\t_\t_\t_\t_\t_\t_\t_\t_\n" +
            "1\tcat\tcat\tNOUN\tNN\t_\t0\tnmod:poss\t_\t_\n" +
            "2\t's\t's\tPART\tPOS\t_\t0\tcase\t_\t_";

        Assert.Equal(expected, str);

        var parsed = Token.Parse(str, CultureInfo.InvariantCulture);

        Assert.Equal(token, parsed);
    }
}
