// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Language.Annotations;

public class TokenTests
{
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
