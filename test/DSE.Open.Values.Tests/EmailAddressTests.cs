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
        Assert.Equal(26, EmailAddress.Parse("hello@dseinternational.org").Length);
    }

    [Fact]
    public void LocalPart()
    {
        Assert.Equal("hello", EmailAddress.Parse("hello@dseinternational.org").LocalPart());
    }

    [Fact]
    public void DomainPart()
    {
        Assert.Equal("dseinternational.org", EmailAddress.Parse("hello@dseinternational.org").DomainPart());
    }
}
