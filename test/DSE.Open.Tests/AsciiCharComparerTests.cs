// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public class AsciiCharComparerTests
{
    [Theory]
    [InlineData('A', 'a')]
    [InlineData('z', 'Z')]
    [InlineData('m', 'M')]
    public void IgnoreCase_EqualAsciiCharsHaveEqualHashCodes(char upper, char lower)
    {
        var comparer = AsciiCharComparer.IgnoreCase;

        var u = (AsciiChar)upper;
        var l = (AsciiChar)lower;

        Assert.True(comparer.Equals(u, l));
        Assert.Equal(comparer.GetHashCode(u), comparer.GetHashCode(l));
    }

    [Fact]
    public void IgnoreCase_HashSetTreatsCaseDifferencesAsEqual()
    {
        var set = new HashSet<AsciiChar>(AsciiCharComparer.IgnoreCase) { (AsciiChar)'A' };

        Assert.Contains((AsciiChar)'a', set);
    }
}
