// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values;

public class EmailAddressTests
{
    [Theory]
    [InlineData("email@example.com")]
    [InlineData("firstname.lastname@example.com")]
    [InlineData("email@subdomain.example.com")]
    [InlineData("firstname+lastname@example.com")]
    [InlineData("1234567890@example.com")]
    [InlineData("email@example-one.com")]
    [InlineData("_______@example.com")]
    [InlineData("email@example.name")]
    [InlineData("email@example.museum")]
    [InlineData("email@example.co.jp")]
    [InlineData("firstname-lastname@example.com")]
    [InlineData("x@example.com")]
    [InlineData("long.email-address-with-hyphens@and.subdomains.example.com")]
    [InlineData("user.name+tag+sorting@example.com")]
    [InlineData("name/surname@example.com")]
    [InlineData("example@s.example")]
    [InlineData("user-@example.org")]
    [InlineData("user+mailbox@example.com")]
    [InlineData("customer/department=shipping@example.com")]
    [InlineData("$A12345@example.com")]
    [InlineData("!def!xyz%abc@example.com")]
    [InlineData("_somename@example.com")]
    [InlineData("email@a.b.c.d.e.f.g.h.i.j.k.example.com")]
    public void TryParse_WithValidEmailAddress_ReturnsTrue(string address)
    {
        Assert.True(EmailAddress.TryParse(address, out _));
    }

    [Theory]
    [InlineData("plainaddress")]
    [InlineData("#@%^%#$@#$@#.com")]
    [InlineData("@example.com")]
    [InlineData("Joe Smith <email@example.com>")]
    [InlineData("email.example.com")]
    [InlineData("email@example@example.com")]
    [InlineData(".email@example.com")]
    [InlineData("email.@example.com")]
    [InlineData("email..email@example.com")]
    [InlineData("あいうえお@example.com")]
    [InlineData("email@example.com (Joe Smith)")]
    [InlineData("email@example")]
    [InlineData("email@-example.com")]
    // [InlineData("email@example.web")] `.web` is not a valid TLD https://en.wikipedia.org/wiki/List_of_Internet_top-level_domains
    [InlineData("email@111.222.333.44444")]
    [InlineData("email@example..com")]
    [InlineData("Abc..123@example.com")]
    [InlineData("”(),:;<>[\\]@example.com")]
    [InlineData("just”not”right@example.com")]
    [InlineData("this\\ is\"really\"not\\\allowed@example.com ")]
    public void TryParse_WithInvalidEmailAddress_ShouldReturnFalse(string address)
    {
        Assert.False(EmailAddress.TryParse(address, out _));
    }

    [Theory]
    [InlineData("email@123.123.123.123")]
    [InlineData("email@[123.123.123.123]")]
    [InlineData("\"email\"@example.com")]
    [InlineData("\"john..doe\"@example.org")]
    [InlineData("user@localhost")]
    public void TryParse_WithUnsupportedEmailAddress_ShouldReturnFalse(string address)
    {
        Assert.False(EmailAddress.TryParse(address, out _));
    }

    [Fact]
    public void Length()
    {
        Assert.Equal(26, EmailAddress.Parse("hello@dseinternational.org", CultureInfo.InvariantCulture).Length);
    }

    [Fact]
    public void LocalPart()
    {
        Assert.Equal("hello", EmailAddress.Parse("hello@dseinternational.org", CultureInfo.InvariantCulture).LocalPart());
    }

    [Fact]
    public void DomainPart()
    {
        Assert.Equal("dseinternational.org", EmailAddress.Parse("hello@dseinternational.org", CultureInfo.InvariantCulture).DomainPart());
    }

    [Fact]
    public void Parse_WithSurroundingWhitespace_ShouldTrimValue()
    {
        var address = EmailAddress.Parse("  hello@dseinternational.org  ", CultureInfo.InvariantCulture);

        Assert.Equal("hello@dseinternational.org", address.ToString());
    }

    [Fact]
    public void TryParse_WithEmptySpan_ShouldReturnTrueAndDefaultResult()
    {
        var success = EmailAddress.TryParse(ReadOnlySpan<char>.Empty, CultureInfo.InvariantCulture, out var result);

        Assert.True(success);
        Assert.Equal(EmailAddress.Empty, result);
    }

    [Fact]
    public void TryParse_WithNullString_ShouldReturnFalseAndDefaultResult()
    {
        var success = EmailAddress.TryParse(null, CultureInfo.InvariantCulture, out var result);

        Assert.False(success);
        Assert.Equal(EmailAddress.Empty, result);
    }

    [Fact]
    public void TryFormat_WithExactBuffer_ShouldWriteValue()
    {
        var address = EmailAddress.Parse("hello@dseinternational.org", CultureInfo.InvariantCulture);
        Span<char> buffer = stackalloc char[address.Length];

        var success = address.TryFormat(buffer, out var charsWritten);

        Assert.True(success);
        Assert.Equal(address.Length, charsWritten);
        Assert.Equal(address.ToString(), buffer.ToString());
    }

    [Fact]
    public void TryFormat_WithShortBuffer_ShouldReturnFalse()
    {
        var address = EmailAddress.Parse("hello@dseinternational.org", CultureInfo.InvariantCulture);
        Span<char> buffer = stackalloc char[address.Length - 1];

        var success = address.TryFormat(buffer, out var charsWritten);

        Assert.False(success);
        Assert.Equal(0, charsWritten);
    }

    [Fact]
    public void Contains_DefaultComparison_ShouldIgnoreCase()
    {
        var address = EmailAddress.Parse("Hello@DseInternational.Org", CultureInfo.InvariantCulture);

#pragma warning disable CA1307 // Intentionally verifies the default comparison used by this overload.
        Assert.True(address.Contains("dseinternational.org"));
#pragma warning restore CA1307
    }

    [Fact]
    public void Contains_WithOrdinalComparison_ShouldBeCaseSensitive()
    {
        var address = EmailAddress.Parse("Hello@DseInternational.Org", CultureInfo.InvariantCulture);

        Assert.False(address.Contains("dseinternational.org", StringComparison.Ordinal));
        Assert.True(address.Contains("DseInternational".AsSpan(), StringComparison.Ordinal));
    }

    [Fact]
    public void Contains_WithNullString_ShouldThrowArgumentNullException()
    {
        var address = EmailAddress.Parse("hello@dseinternational.org", CultureInfo.InvariantCulture);

        _ = Assert.Throws<ArgumentNullException>(() => address.Contains(null!, StringComparison.Ordinal));
    }

    [Fact]
    public void Equals_StringAndMemoryOverloads_ShouldUseOrdinalComparison()
    {
        var address = EmailAddress.Parse("hello@dseinternational.org", CultureInfo.InvariantCulture);

        Assert.True(address.Equals("hello@dseinternational.org"));
        Assert.True(address.Equals("hello@dseinternational.org".AsMemory()));
        Assert.False(address.Equals("HELLO@DSEINTERNATIONAL.ORG"));
    }
}
