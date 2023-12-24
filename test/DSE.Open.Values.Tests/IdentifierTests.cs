// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Tests;

public class IdentifierTests
{
    public ITestOutputHelper Output { get; }

    public IdentifierTests(ITestOutputHelper output)
    {
        Output = output;
    }

    [Theory]
    [InlineData("sg_YTRBuEVPtNac6vF")]
    public void EqualsReturnsTrueForEqualValues(string id)
    {
        var id1 = Identifier.Parse(id, CultureInfo.InvariantCulture);
        var id2 = Identifier.Parse(id, CultureInfo.InvariantCulture);
        Assert.Equal(id1, id2);
    }

    [Theory]
    [InlineData("sg_YTRBuEVPtNac6vF")]
    public void GetHasCodeReturnsEqualValueForSameIdValues(string id)
    {
        var hash1 = Identifier.Parse(id, CultureInfo.InvariantCulture).GetHashCode();
        var hash2 = Identifier.Parse(id, CultureInfo.InvariantCulture).GetHashCode();
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void Generate()
    {
        for (var i = 0; i < 50; i++)
        {
            var id = Identifier.New();
            Output.WriteLine(id.ToString());
        }
    }

    [Fact]
    public void GenerateWithPrefix()
    {
        for (var i = 0; i < 50; i++)
        {
            var id = Identifier.New("prefix");
            Output.WriteLine(id.ToString());
        }
    }

    [Fact]
    public void StartsWith()
    {
        for (var i = 0; i < 50; i++)
        {
            var id = Identifier.New("prefix");
            Assert.True(id.StartsWith("prefix"));
        }
    }

    [Fact]
    public void UidEquals()
    {
        for (var i = 0; i < 50; i++)
        {
            var id1 = Identifier.New();
            Assert.Equal(id1, id1);
        }
    }

    [Fact]
    public void ToStringParseEquals()
    {
        for (var i = 0; i < 50; i++)
        {
            var id1 = Identifier.New();
            var id1Str = id1.ToString();
            Assert.True(Identifier.TryParse(id1Str, out var id2));
            Assert.Equal(id1, id2);
        }
    }

    [Fact]
    public void ToStringParseEqualsPrefix()
    {
        for (var i = 0; i < 50; i++)
        {
            var id1 = Identifier.New();
            var id1Str = "pre_fix_" + id1;
            Assert.True(Identifier.TryParse(id1Str, out _));
        }
    }

    [Theory]
    [InlineData("a2oZ6OmS7aO9HrrppH5Jth3CICti8fan0gHuRUJ66CQVyuRm")]
    [InlineData("1Thm00exvYX2me2MyJYqk8uYaudlt2scEWthgJLRQNdacqIf")]
    [InlineData("ZY3KBZnj42RRC0gTWMDjIXhnndZ0br8NQv2QpQSqkXIoveLs")]
    [InlineData("8rYvGTPddMvAV1eiOxoI1ya3a7r9qKFdPBLvs4VF8PexpyGq")]
    [InlineData("PeFtqLJZdAn30WQQictqZSTZVefiP66HPWlkEEQPxr0hKQ7d")]
    [InlineData("xmDUNsRSgYmA8Zacl86dMkqtOIGA0IZupgQtdP2cFMFF7ERn")]
    [InlineData("pXfx6HhWpB7iG9KCfn8sI8S8GEIxEkWmomB7vioSo5s67nXM")]
    [InlineData("bi9WcoBcddGpr9GuiyUu5EWraDy2gErPDojrDA7r0yKeJOwp")]
    [InlineData("aDE361irj1wUn1CO1P9QwvR9I6EbjLGe29ediMiE4QlAxX7D")]
    [InlineData("jNFgVRPrScE61Kl2RKObWyZXRqDfte6LeLUNsJuegIm6ILl1")]
    [InlineData("oNBu0vtmlhTRT8bdW1m03xo4F8soeVgAilxAOLuY1GkGRLC5")]
    [InlineData("Oac8ADVPX5aVqoRgHeVL2cIyKKx7r1jWbonfjrMQWqgJoTxg")]
    [InlineData("1xgc56VaXQqCKceVIkZDEmlKcuc32voL9YUswhVGOncXkHiY")]
    [InlineData("1lVtHNpkuUUvJPFVNAi3P1m6DppErkWZ7MCitQoAhSVyBfPF")]
    [InlineData("UZo9jidQn2O5lk8Wn0LPEYj3Himsn4DQXQ3SMxnj2iqiMWkH")]
    [InlineData("ZUQ5R3HU44Dp6efVpajx0CPcN3xQMixVaIbTqX1yZT98n7H3")]
    [InlineData("hqB9WcaKwNGdmWSMRVZuygL9vPblAw0mpOptOK3oQvgNgxtp")]
    [InlineData("8gM1dYjJGtmKqNotgpsV9gq2y3hvlYuxocIu5emIcmwT7NMd")]
    [InlineData("Tq9pGnvZjO7n6uRoxa2j0g7xKCpqZEf80wUNvKol8gHpm0om")]
    [InlineData("KwPxTF6DkoFsE5NjhX2K3LWcKlg8umcKpDbqp8hSl3Z2a6sr")]
    [InlineData("iioupAEuwTZHl1ZvDWAsNRidAZ4ZFWvXFYK5Iocts9Ajfue2")]
    [InlineData("4GjaCs9oinLlEhyNWNlnKBaAr3o8WTlaPdGMkaxQ03sOqGxD")]
    [InlineData("AAQj5Po1MMN16LuK3jjNyipiRcE9KJMm0M8jfEOJfvsoGXiG")]
    [InlineData("ELiieyIjs6M9NxYFWTyUX8NLCfiFrBY1X8C2W5aB1lIsBI4H")]
    [InlineData("TtnOnfI5WhI004vuE02tWxNdY7vNGFLmbbrREfwMrICxh42m")]
    [InlineData("wmenWIZvjEhH2k2M5Tf9hKOlxNeRFVBlTGFSxH8PyhX7hnCv")]
    [InlineData("9gTDJgTdhG8urv1y21gdr77SBEXeC5cPnqHv1oYqY2TD8uUk")]
    [InlineData("ltvmsdF0iiUSDSbOdiaqwCc3mbHrTDBv3HiHA0SEtoIoWtMv")]
    [InlineData("9nL8sU6ONBh8LBcK6A9vfqy2bD5DCmlCNpEUSEm58BxJ6jim")]
    [InlineData("JMyLTWa2hXLaD15XJ5tMS4oeEASZD1dOXDkNYGcLJ2hQkvsj")]
    [InlineData("Q1wrILujiUlyMbRTjUZFm8uo07s3PX3PfAxfG2ggBTnZElwX")]
    [InlineData("k6bWiZe1Ed2VrfcuTQOTqH7a4V1R9eH4wjCJaH03dKOkoPpS")]
    [InlineData("1R8aurDO6vMSH6OTNo7S8isMTKa2S9jG6NbXZh5CD4w0MI1L")]
    [InlineData("IeEdle2Q4qILBjyZOR5uhtjhQ3EgRp6qqBSQnhC25xaKYS8v")]
    [InlineData("DyqMtbCphT0Z495pIRUcfZHx9BJlPP9kW3ogBun9xanObgGU")]
    [InlineData("2Mt78Ps38ieWik90GQTIcYSt3lYCyfsPCu7ToS5YUacHNCur")]
    [InlineData("5EwBtcYQJdoovWjSJJUKw5L7yIBKPAVeSJwA1Sxi9WCd75V8")]
    [InlineData("htRBaMHnRpDsngMBSqYlcPupdF8Ir5DfsWDBYAQd5rFWOdQ3")]
    [InlineData("VauxOkUZeNWdxgEUgti3u1qJhLKpoyCpy8vKjPXFwk3vRPmS")]
    [InlineData("dngyuSU4l0UVIoufSKy8eSsqQTfhTIwni7ugRtdpoSQXJGr8")]
    [InlineData("gHfPMrcK2m3VVe0Vnij9HOisWhxRrLHimSKMpGmVjstwd2UK")]
    [InlineData("fofbJugqpdP8n8fA79ULgjeHlbqg1HK2A0uB8SIjHgfRUsOR")]
    [InlineData("9E7W5Zusb1KMvkJ4eSK6BugOZp6JoDE2Rx9fYbg4G4dbwTuT")]
    [InlineData("rjwT9qSfdI5bH14wWC3D0hSv9pMPAEJ4iZEqE61TZeDdrDQ8")]
    [InlineData("nIULQC0Lrifcr3YTpETbQS5QXypbrCQitutOfCKWBMGuUNOa")]
    [InlineData("wOlGZ8uha4VasT0BEpHZtOE3OuQaJeHiMTvhETgfpZMrpVlc")]
    [InlineData("uIsmV2EbTWdwoVfS8qXlvnuawdyckfiKeiEBhugQTRV3j2Ph")]
    [InlineData("3VmMlc8IKMNqqPvjh1K65pZWAWaj7tZA3S1W7l6QaWtNZ2cK")]
    [InlineData("HhQHBLIq94CgLpvx39VtnNtZxLiswAOSv8Vuk9ntJ9w4Of99")]
    [InlineData("BsGP27ATn0QNS6ZNxPggLKTj1qPKYCdyGAj35mWlQCH04KqO")]
    [InlineData("in_1L96W0H8j6F17S8Erqm21SKw")]
    [InlineData("stripe_in_1L96W0H8j6F17S8Erqm21SKw")]
    public void TryParseValid(string value)
    {
        var success = Identifier.TryParse(value, out var uid);
        Assert.True(success);
        Assert.NotEqual(Identifier.Empty, uid);
    }

    [Theory]
    [InlineData("dse_sub_VjHlsZTVmKRGglRRjSkPeQL71M17c7sQ")]
    [InlineData("12345678901234567890123_VjHlsZTVmKRGglRRjSkPeQL71M17c7sQ")]
    [InlineData("VjHlsZTVmKRGglRRjSkPeQL71M17c7sQ")]
    [InlineData("12_4567_9012_45678_0123_VjHlsZTVmKRGglRRjSkPeQL71M17c7sQ")]
    public void IsValid(string value)
    {
        Assert.True(Identifier.IsValid(value));
    }

    [Theory]
    [InlineData("SkPeQL71M1")]
    [InlineData("123456789012345678901234_VjHlsZTVmKRGglRRjSkPeQL71M17c7sQ")]
    [InlineData("_2345678901234567890123_VjHlsZTVmKRGglRRjSkPeQL71M17c7sQ")]
    public void IsNotValid(string value)
    {
        Assert.False(Identifier.IsValid(value));
    }

    [Theory]
    [InlineData("origin_id_C7bj8qkIIAfCnKlhbqatRt8ibFtVmFDT1qU1uM")]
    public void TryParseValidWithPrefix(string value)
    {
        var success = Identifier.TryParse(value, out var uid);
        Assert.True(success);
        Assert.NotEqual(Identifier.Empty, uid);
    }

    [Theory]
    [InlineData("a2oZ6OmS7aO9!!rppH5Jth3CICti8fan0gHuRUJ66CQVyuRm")]
    [InlineData("1Thm00exvYX2me2MyJYqk8uYaudlt2sc(Wthg)LRQNdacqIf")]
    [InlineData("ZY3KBZnj4^2RR&C0gTDjIXhnndZ0br8NQv2QpQSqkXIoveLs")]
    [InlineData("PeFtqL")]
    [InlineData("xm_dMk_upgQt")]
    public void TryParseInvalid(string value)
    {
        var success = Identifier.TryParse(value, out var uid);
        Assert.False(success);
        Assert.Equal(Identifier.Empty, uid);
    }

    [Fact]
    public void TryParse_WithNullString_ShouldReturnFalse()
    {
        // Arrange
        string? val = null;

        // Act
        var success = Identifier.TryParse(val, out _);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void Parse_WithInvalidCode_ShouldThrowFormatException()
    {
        // Arrange
        const string code = "invalid";

        // Assert
        _ = Assert.Throws<FormatException>(() => Identifier.Parse(code, null));
    }

    [Fact]
    public void New_WithIdLengthTooLong_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        const int length = Identifier.MaxIdLength + 1;

        // Act
        static void New()
        {
            _ = Identifier.New(length, Span<char>.Empty);
        }

        // Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(New);
    }

    [Fact]
    public void NewWith_WithIdLengthTooShort_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        const int length = Identifier.MinIdLength - 1;

        // Act
        static void New()
        {
            _ = Identifier.New(length, Span<char>.Empty);
        }

        // Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(New);
    }

    [Fact]
    public void New_WithInvalidPrefixCharacters_ShouldThrowArgumentException()
    {
        // Arrange
        const string prefix = "aa*";

        // Act
        static void New()
        {
            _ = Identifier.New(Identifier.MaxIdLength, prefix);
        }

        // Assert
        _ = Assert.Throws<ArgumentException>(New);
    }

    [Fact]
    public void New_WithPrefixTooShort_ShouldThrowArgumentException()
    {
        // Arrange
        var prefix = new string('a', Identifier.MinPrefixLength - 1);

        // Act
        void New()
        {
            _ = Identifier.New(Identifier.MaxIdLength, prefix);
        }

        // Assert
        _ = Assert.Throws<ArgumentException>(New);
    }

    [Fact]
    public void New_WithValidPrefix_ShouldFillWithRandomBytes()
    {
        // Arrange
        var prefix = new string('a', Identifier.MinPrefixLength);

        // Act
        var code = Identifier.New(Identifier.MaxIdLength, prefix);

        // Assert
        Assert.StartsWith(prefix, code.ToString(), StringComparison.Ordinal);
    }

    [Fact]
    public void Split_WithValidCode_ShouldSplitCorrectly()
    {
        // Arrange
        const string prefixText = "prefix";
        var code = Identifier.New(Identifier.MaxIdLength, prefixText);

        // Act
        var (prefix, id) = Identifier.Split(code.ToString().AsMemory());

        // Assert
        Assert.Equal(prefixText, prefix.ToString());
        Assert.Equal(Identifier.MaxIdLength, id.Length);
    }
}
