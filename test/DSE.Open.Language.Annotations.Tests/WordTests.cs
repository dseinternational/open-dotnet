// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Annotations;

public class WordTests
{
    [Fact]
    public void ToString_WithLargeIndexes_DoesNotUnderestimateBufferLength()
    {
        var word = new Word
        {
            Index = int.MaxValue,
            Form = (TokenText)"word",
            Lemma = (TokenText)"word",
            Pos = UniversalPosTag.Noun,
            HeadIndex = int.MaxValue,
            Relation = UniversalRelationTag.NominalSubject
        };

        var formatted = word.ToString();

        Assert.Equal("2147483647\tword\tword\tNOUN\t_\t_\t2147483647\tnsubj\t_\t_", formatted);
    }
}
