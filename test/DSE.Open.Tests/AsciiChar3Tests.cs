// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public class AsciiChar3Tests
{
    [Fact]
    public void Constructor_FromChars_StoresOrdered()
    {
        var v = new AsciiChar3('a', 'b', 'c');

        Assert.Equal("abc", v.ToString());
    }

    [Fact]
    public void Deconstruct_YieldsOriginalChars()
    {
        var v = new AsciiChar3('f', 'o', 'o');

        var (c0, c1, c2) = v;

        Assert.Equal((AsciiChar)'f', c0);
        Assert.Equal((AsciiChar)'o', c1);
        Assert.Equal((AsciiChar)'o', c2);
    }

    [Fact]
    public void Equals_SameValue_ReturnsTrue()
    {
        var a = new AsciiChar3('c', 'a', 't');
        var b = new AsciiChar3('c', 'a', 't');

        Assert.True(a.Equals(b));
        Assert.True(a == b);
        Assert.False(a != b);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void Equals_DifferentValue_ReturnsFalse()
    {
        var a = new AsciiChar3('c', 'a', 't');
        var b = new AsciiChar3('d', 'o', 'g');

        Assert.False(a.Equals(b));
        Assert.True(a != b);
    }

    [Fact]
    public void EqualsIgnoreCase_MatchesDifferentCase()
    {
        var a = new AsciiChar3('C', 'A', 'T');
        var b = new AsciiChar3('c', 'a', 't');

        Assert.True(a.EqualsIgnoreCase(b));
        Assert.False(a.Equals(b));
    }

    [Fact]
    public void EqualsIgnoreCase_String_Match()
    {
        var a = new AsciiChar3('C', 'A', 'T');

        Assert.True(a.EqualsIgnoreCase("cat"));
    }

    [Fact]
    public void CompareTo_UsesOrdinalOrdering()
    {
        var abc = new AsciiChar3('a', 'b', 'c');
        var abd = new AsciiChar3('a', 'b', 'd');

        Assert.True(abc.CompareTo(abd) < 0);
        Assert.True(abd.CompareTo(abc) > 0);
        Assert.Equal(0, abc.CompareTo(abc));
    }

    [Fact]
    public void CompareToIgnoreCase_TreatsCasesAsEqual()
    {
        var upper = new AsciiChar3('A', 'B', 'C');
        var lower = new AsciiChar3('a', 'b', 'c');

        Assert.Equal(0, upper.CompareToIgnoreCase(lower));
    }

    [Fact]
    public void ToUpper_UppercasesAsciiLetters()
    {
        var v = new AsciiChar3('a', 'b', 'c');

        Assert.Equal("ABC", v.ToUpper().ToString());
    }

    [Fact]
    public void ToLower_LowercasesAsciiLetters()
    {
        var v = new AsciiChar3('X', 'Y', 'Z');

        Assert.Equal("xyz", v.ToLower().ToString());
    }

    [Fact]
    public void ToChar3_ProducesEquivalentChar3()
    {
        var v = new AsciiChar3('a', 'b', 'c');

        var c3 = v.ToChar3();

        Assert.Equal("abc", c3.ToString());
    }

    [Fact]
    public void ToCharArray_ReturnsCharSequence()
    {
        var v = new AsciiChar3('x', 'y', 'z');

        Assert.Equal(['x', 'y', 'z'], v.ToCharArray());
    }

    [Theory]
    [InlineData("abc", true)]
    [InlineData("XYZ", true)]
    [InlineData("", false)]
    [InlineData("ab", false)]
    [InlineData("abcd", false)]
    [InlineData("abé", false)]
    public void TryParse_Char_Validates(string input, bool expectedSuccess)
    {
        var success = AsciiChar3.TryParse(input.AsSpan(), null, out var result);

        Assert.Equal(expectedSuccess, success);

        if (success)
        {
            Assert.Equal(input, result.ToString());
        }
    }

    [Fact]
    public void Parse_InvalidInput_Throws()
    {
        _ = Assert.Throws<FormatException>(() => AsciiChar3.Parse("ab", null));
    }

    [Fact]
    public void Parse_NullString_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => AsciiChar3.Parse((string)null!, null));
    }

    [Fact]
    public void FromString_Valid_Succeeds()
    {
        var v = AsciiChar3.FromString("abc");

        Assert.Equal("abc", v.ToString());
    }

    [Fact]
    public void TryFormat_BufferTooSmall_ReturnsFalse()
    {
        var v = new AsciiChar3('a', 'b', 'c');
        Span<char> buffer = stackalloc char[2];

        var success = v.TryFormat(buffer, out var charsWritten, default, null);

        Assert.False(success);
        Assert.Equal(0, charsWritten);
    }

    [Fact]
    public void TryFormat_SufficientBuffer_WritesChars()
    {
        var v = new AsciiChar3('a', 'b', 'c');
        Span<char> buffer = stackalloc char[3];

        var success = v.TryFormat(buffer, out var charsWritten, default, null);

        Assert.True(success);
        Assert.Equal(3, charsWritten);
        Assert.Equal("abc", buffer[..3].ToString());
    }

    [Fact]
    public void ToStringLower_ReturnsLowercase()
    {
        var v = new AsciiChar3('A', 'B', 'C');

        Assert.Equal("abc", v.ToStringLower());
    }

    [Fact]
    public void ToStringUpper_ReturnsUppercase()
    {
        var v = new AsciiChar3('a', 'b', 'c');

        Assert.Equal("ABC", v.ToStringUpper());
    }

    [Fact]
    public void ImplicitConversionToString_ReturnsValue()
    {
        var v = new AsciiChar3('f', 'o', 'o');

        string s = v;

        Assert.Equal("foo", s);
    }

    [Fact]
    public void ExplicitConversionFromString_Parses()
    {
        var v = (AsciiChar3)"bar";

        Assert.Equal("bar", v.ToString());
    }

    [Fact]
    public void GetRepeatableHashCode_EqualValuesAreEqual()
    {
        var a = new AsciiChar3('c', 'a', 't');
        var b = new AsciiChar3('c', 'a', 't');

        Assert.Equal(a.GetRepeatableHashCode(), b.GetRepeatableHashCode());
    }
}
