// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public class AsciiChar2ComparerTests
{
    [Fact]
    public void CaseSensitive_TreatsCasesAsDistinct()
    {
        var comparer = AsciiChar2Comparer.CaseSensitive;
        var upper = new AsciiChar2('A', 'B');
        var lower = new AsciiChar2('a', 'b');

        Assert.False(comparer.Equals(upper, lower));
        Assert.True(comparer.Equals(upper, upper));
        Assert.NotEqual(0, comparer.Compare(upper, lower));
        Assert.Equal(upper.GetHashCode(), comparer.GetHashCode(upper));
    }

    [Fact]
    public void IgnoreCase_TreatsCasesAsEqual()
    {
        var comparer = AsciiChar2Comparer.IgnoreCase;
        var upper = new AsciiChar2('A', 'B');
        var lower = new AsciiChar2('a', 'b');

        Assert.True(comparer.Equals(upper, lower));
        Assert.Equal(0, comparer.Compare(upper, lower));
        Assert.Equal(comparer.GetHashCode(upper), comparer.GetHashCode(lower));
    }

    [Fact]
    public void IgnoreCase_HashSetTreatsCasesAsSame()
    {
        var set = new HashSet<AsciiChar2>(AsciiChar2Comparer.IgnoreCase)
        {
            new('A', 'B'),
        };

        Assert.Contains(new AsciiChar2('a', 'b'), set);
    }
}

public class AsciiChar3ComparerTests
{
    [Fact]
    public void CaseSensitive_TreatsCasesAsDistinct()
    {
        var comparer = AsciiChar3Comparer.CaseSensitive;
        var upper = new AsciiChar3('A', 'B', 'C');
        var lower = new AsciiChar3('a', 'b', 'c');

        Assert.False(comparer.Equals(upper, lower));
        Assert.True(comparer.Equals(upper, upper));
        Assert.NotEqual(0, comparer.Compare(upper, lower));
    }

    [Fact]
    public void IgnoreCase_TreatsCasesAsEqual()
    {
        var comparer = AsciiChar3Comparer.IgnoreCase;
        var upper = new AsciiChar3('A', 'B', 'C');
        var lower = new AsciiChar3('a', 'b', 'c');

        Assert.True(comparer.Equals(upper, lower));
        Assert.Equal(0, comparer.Compare(upper, lower));
        Assert.Equal(comparer.GetHashCode(upper), comparer.GetHashCode(lower));
    }
}

public class AsciiStringComparerTests
{
    [Fact]
    public void CaseSensitive_TreatsCasesAsDistinct()
    {
        var comparer = AsciiStringComparer.CaseSensitive;
        var upper = AsciiString.Parse("HELLO", null);
        var lower = AsciiString.Parse("hello", null);

        Assert.False(comparer.Equals(upper, lower));
        Assert.True(comparer.Equals(upper, upper));
        Assert.NotEqual(0, comparer.Compare(upper, lower));
    }

    [Fact]
    public void IgnoreCase_TreatsCasesAsEqual()
    {
        var comparer = AsciiStringComparer.IgnoreCase;
        var upper = AsciiString.Parse("HELLO", null);
        var lower = AsciiString.Parse("hello", null);

        Assert.True(comparer.Equals(upper, lower));
        Assert.Equal(0, comparer.Compare(upper, lower));
        Assert.Equal(comparer.GetHashCode(upper), comparer.GetHashCode(lower));
    }
}
