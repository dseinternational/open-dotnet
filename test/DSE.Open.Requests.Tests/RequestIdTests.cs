// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Requests.Tests;

public class RequestIdTests
{
    [Fact]
    public void Ctor_ValidValue_Succeeds()
    {
        var id = new RequestId("abc123");
        Assert.Equal("abc123", id.ToString());
    }

    [Fact]
    public void Ctor_EmptyValue_Throws()
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new RequestId(""));
    }

    [Fact]
    public void Ctor_NullValue_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => new RequestId(null!));
    }

    [Fact]
    public void Ctor_WhitespaceValue_Throws()
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new RequestId("   "));
    }

    [Fact]
    public void Ctor_ControlCharacters_Throws()
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new RequestId("abc\r\n123"));
    }

    [Fact]
    public void Ctor_MaxLength_Succeeds()
    {
        var value = new string('x', RequestId.MaxSerializedCharLength);
        var id = new RequestId(value);
        Assert.Equal(value, id.ToString());
    }

    [Fact]
    public void Ctor_ExceedsMaxLength_Throws()
    {
        var value = new string('x', RequestId.MaxSerializedCharLength + 1);
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new RequestId(value));
    }

    [Fact]
    public void IsValidValue_Empty_ReturnsFalse()
    {
        Assert.False(RequestId.IsValidValue(default));
    }

    [Theory]
    [InlineData("   ")]
    [InlineData("abc\t123")]
    [InlineData("abc\r\n123")]
    public void IsValidValue_InvalidText_ReturnsFalse(string value)
    {
        Assert.False(RequestId.IsValidValue(value));
    }

    [Fact]
    public void IsValidValue_WithinBounds_ReturnsTrue()
    {
        Assert.True(RequestId.IsValidValue("hello"));
    }

    [Fact]
    public void ExplicitCast_FromString_Succeeds()
    {
        var id = (RequestId)"abc";
        Assert.Equal("abc", id.ToString());
    }

    [Fact]
    public void Equality_SameValue_Equal()
    {
        var a = new RequestId("same");
        var b = new RequestId("same");
        Assert.Equal(a, b);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void Equality_DifferentValue_NotEqual()
    {
        var a = new RequestId("one");
        var b = new RequestId("two");
        Assert.NotEqual(a, b);
    }

    [Fact]
    public void SerializeDeserialize_Roundtrips()
    {
        var id = new RequestId("abc123");
        var json = JsonSerializer.Serialize(id);
        var deserialized = JsonSerializer.Deserialize<RequestId>(json);
        Assert.Equal(id, deserialized);
    }
}
