// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Language.Annotations;

public class TokenTests
{
    [Theory]
    [InlineData("1\tHe\the\tPRON\tPRP\tCase=Nom|Gender=Masc|Number=Sing|Person=3|PronType=Prs\t2\tnsubj\t_\t_")]
    [InlineData("3\tbrushing\tbrush\tVERB\tVBG\tAspect=Prog|Tense=Pres|VerbForm=Part\t0\tROOT\t_\t_")]
    [InlineData("4\ta\ta\tDET\tDT\tDefinite=Ind|PronType=Art\t5\tdet\t_\t_")]
    [InlineData("5\tdog\tdog\tNOUN\tNN\tNumber=Sing\t3\tdobj\t_\tSpaceAfter=No")]
    [InlineData("6\t.\t.\tPUNCT\t.\tPunctType=Peri\t3\tpunct\t_\tSpaceAfter=No")]
    public void Parse(string value)
    {
        var token = Token.Parse(value, CultureInfo.InvariantCulture);
        Assert.NotNull(token);
    }

    [Fact]
    public void FormatToStringAndParse()
    {
        var token = new Token
        {
            Id = new TokenIndex(1),
            Form = (Word)"He",
            Lemma = (Word)"he",
            Pos = UniversalPosTag.Pronoun,
            AltPos = TreebankPosTag.PronounPersonal,
            Features = ReadOnlyWordFeatureValueCollection.ParseInvariant("Case=Nom|Gender=Masc|Number=Sing|Person=3|PronType=Prs"),
            HeadIndex = 2,
            Relation = UniversalRelationTag.NominalSubject,
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
            Id = new TokenIndex(1),
            Form = (Word)"He",
            Lemma = (Word)"he",
            Pos = UniversalPosTag.Pronoun,
            AltPos = TreebankPosTag.PronounPersonal,
            Features = ReadOnlyWordFeatureValueCollection.ParseInvariant("Case=Nom|Gender=Masc|Number=Sing|Person=3|PronType=Prs"),
            HeadIndex = 2,
            Relation = UniversalRelationTag.NominalSubject,
        };

        var json = JsonSerializer.Serialize(token);

        var deserialized = JsonSerializer.Deserialize<Token>(json);

        Assert.Equal(token, deserialized);
    }
}
