// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open;

public class VariantTests
{
    [Fact]
    public void Variant_Null()
    {
        var variant = Variant.Null;
        Assert.True(variant.IsNull);
        Assert.False(variant.IsInteger);
        Assert.False(variant.IsFloatingPoint);
        Assert.False(variant.IsText);
        Assert.False(variant.IsBoolean);
    }

    [Fact]
    public void Variant_Integer()
    {
        var variant = new Variant(42);
        Assert.False(variant.IsNull);
        Assert.True(variant.IsInteger);
        Assert.False(variant.IsFloatingPoint);
        Assert.False(variant.IsText);
        Assert.False(variant.IsBoolean);
        Assert.Equal(42, variant.Integer);
    }

    [Fact]
    public void Variant_FloatingPoint()
    {
        var variant = new Variant(3.14);
        Assert.False(variant.IsNull);
        Assert.False(variant.IsInteger);
        Assert.True(variant.IsFloatingPoint);
        Assert.False(variant.IsText);
        Assert.False(variant.IsBoolean);
        Assert.Equal(3.14, variant.FloatingPoint);
    }

    [Fact]
    public void Variant_Text()
    {
        var variant = new Variant("Hello, World!");
        Assert.False(variant.IsNull);
        Assert.False(variant.IsInteger);
        Assert.False(variant.IsFloatingPoint);
        Assert.True(variant.IsText);
        Assert.False(variant.IsBoolean);
        Assert.Equal("Hello, World!", variant.Text);
    }

    [Fact]
    public void Variant_Boolean()
    {
        var variant = new Variant(true);
        Assert.False(variant.IsNull);
        Assert.False(variant.IsInteger);
        Assert.False(variant.IsFloatingPoint);
        Assert.False(variant.IsText);
        Assert.True(variant.IsBoolean);
        Assert.True(variant.Boolean);
    }

    [Fact]
    public void Variant_GetValue()
    {
        var variant = new Variant(42);
        Assert.Equal(42L, variant.GetValue());
    }

    [Fact]
    public void Variant_ToString()
    {
        var variant = new Variant(42);
        Assert.Equal("42", variant.ToString());
    }

    [Fact]
    public void Variant_ToString_Format()
    {
        var variant = new Variant(42);
        Assert.Equal("42", variant.ToString("G", null));
    }

    [Fact]
    public void Variant_ToString_Format_Culture()
    {
        var variant = new Variant(42);
        Assert.Equal("42", variant.ToString("G", CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Variant_String_Json_SerializeAndDeserialize()
    {
        var variant = new Variant("Hello, World!");
        var json = JsonSerializer.Serialize(variant);
        Assert.Equal("\"Hello, World!\"", json);
        var deserialized = JsonSerializer.Deserialize<Variant>(json);
        Assert.True(deserialized.IsText);
        Assert.Equal(variant.Text, deserialized.Text);
    }

    [Fact]
    public void Variant_Integer_Json_SerializeAndDeserialize()
    {
        var variant = new Variant(42);
        var json = JsonSerializer.Serialize(variant);
        Assert.Equal("42", json);
        var deserialized = JsonSerializer.Deserialize<Variant>(json);
        Assert.True(deserialized.IsInteger);
        Assert.Equal(variant.Integer, deserialized.Integer);
    }

    [Fact]
    public void Variant_FloatingPoint_Json_SerializeAndDeserialize()
    {
        var variant = new Variant(3.14);
        var json = JsonSerializer.Serialize(variant);
        Assert.Equal("3.14", json);
        var deserialized = JsonSerializer.Deserialize<Variant>(json);
        Assert.True(deserialized.IsFloatingPoint);
        Assert.Equal(variant.FloatingPoint, deserialized.FloatingPoint);
    }

    [Fact]
    public void Variant_Boolean_Json_SerializeAndDeserialize()
    {
        var variant = new Variant(true);
        var json = JsonSerializer.Serialize(variant);
        Assert.Equal("true", json);
        var deserialized = JsonSerializer.Deserialize<Variant>(json);
        Assert.True(deserialized.IsBoolean);
        Assert.Equal(variant.Boolean, deserialized.Boolean);
    }

    [Fact]
    public void Variant_Null_Json_SerializeAndDeserialize()
    {
        var variant = Variant.Null;
        var json = JsonSerializer.Serialize(variant);
        Assert.Equal("null", json);
        var deserialized = JsonSerializer.Deserialize<Variant>(json);
        Assert.True(deserialized.IsNull);
    }
}
