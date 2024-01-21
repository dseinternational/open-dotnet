// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Speech.Tests;

public class PhonemesTests
{
    [Fact]
    public void CanLoadEnglishAll()
    {
        var all = Phonemes.English.All;
        Assert.NotNull(all);
    }
}
