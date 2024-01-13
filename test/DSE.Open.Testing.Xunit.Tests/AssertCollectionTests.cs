// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Testing.Xunit;

public partial class AssertCollectionTests
{
    [Fact]
    public void Empty()
    {
        AssertSequence.Empty(Enumerable.Empty<int>().ToList());
    }

    [Fact]
    public void NotEmpty()
    {
        AssertSequence.NotEmpty(Enumerable.Range(1, 10).ToList());
    }
}
