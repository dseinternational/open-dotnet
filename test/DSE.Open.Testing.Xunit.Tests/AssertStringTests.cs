// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Testing.Xunit;

public class AssertStringTests
{
    [Fact]
    public void IsNotNullOrEmpty()
    {
        AssertString.IsNotNullOrEmpty("Hello");
    }

    [Fact]
    public void IsNotNullOrEmpty_throws_if_not()
    {
        _ = Assert.Throws<StringException>(() => AssertString.IsNotNullOrEmpty(string.Empty));
        _ = Assert.Throws<StringException>(() => AssertString.IsNotNullOrEmpty(null!));
    }

    [Fact]
    public void IsNotNullOrWhiteSpace()
    {
        AssertString.IsNotNullOrWhiteSpace("Hello");
    }

    [Fact]
    public void IsNotNullOrWhiteSpace_throws_if_not()
    {
        _ = Assert.Throws<StringException>(() => AssertString.IsNotNullOrWhiteSpace(string.Empty));
        _ = Assert.Throws<StringException>(() => AssertString.IsNotNullOrWhiteSpace(null!));
        _ = Assert.Throws<StringException>(() => AssertString.IsNotNullOrWhiteSpace("    "));
    }

    [Fact]
    public void IsAscii()
    {
        AssertString.IsAscii("Hello");
    }

    [Fact]
    public void IsAscii_throws_if_not()
    {
        _ = Assert.Throws<StringException>(() => AssertString.IsAscii("Ελληνικά"));
        _ = Assert.Throws<StringException>(() => AssertString.IsAscii("ɲ ɳ ŋ"));
    }
}
