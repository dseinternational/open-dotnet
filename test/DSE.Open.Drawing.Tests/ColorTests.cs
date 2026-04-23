// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Drawing.Tests;

public class ColorTests
{
    // ------------------------------------------------------------------
    //  Construction & component access
    // ------------------------------------------------------------------

    [Fact]
    public void FromRgb_byte_sets_components_with_full_alpha()
    {
        var c = Color.FromRgb(10, 20, 30);
        Assert.Equal(255, c.A);
        Assert.Equal(10, c.R);
        Assert.Equal(20, c.G);
        Assert.Equal(30, c.B);
    }

    [Fact]
    public void FromRgba_byte_sets_all_components()
    {
        var c = Color.FromRgba(10, 20, 30, 128);
        Assert.Equal(128, c.A);
        Assert.Equal(10, c.R);
        Assert.Equal(20, c.G);
        Assert.Equal(30, c.B);
    }

    [Fact]
    public void FromArgb_byte_sets_all_components()
    {
        var c = Color.FromArgb(128, 10, 20, 30);
        Assert.Equal(128, c.A);
        Assert.Equal(10, c.R);
        Assert.Equal(20, c.G);
        Assert.Equal(30, c.B);
    }

    [Fact]
    public void FromRgba_span_sets_components()
    {
        ReadOnlySpan<byte> bytes = [10, 20, 30, 128];
        var c = Color.FromRgba(bytes);
        Assert.Equal(128, c.A);
        Assert.Equal(10, c.R);
        Assert.Equal(20, c.G);
        Assert.Equal(30, c.B);
    }

    [Fact]
    public void FromArgb_span_sets_components()
    {
        ReadOnlySpan<byte> bytes = [128, 10, 20, 30];
        var c = Color.FromArgb(bytes);
        Assert.Equal(128, c.A);
        Assert.Equal(10, c.R);
        Assert.Equal(20, c.G);
        Assert.Equal(30, c.B);
    }

    // ------------------------------------------------------------------
    //  Float factories
    // ------------------------------------------------------------------

    [Fact]
    public void FromRgb_float_boundary_zero()
    {
        var c = Color.FromRgb(0f, 0f, 0f);
        Assert.Equal(0, c.R);
        Assert.Equal(0, c.G);
        Assert.Equal(0, c.B);
    }

    [Fact]
    public void FromRgb_float_boundary_one()
    {
        var c = Color.FromRgb(1f, 1f, 1f);
        Assert.Equal(255, c.R);
        Assert.Equal(255, c.G);
        Assert.Equal(255, c.B);
    }

    [Fact]
    public void FromRgb_float_rejects_negative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Color.FromRgb(-0.1f, 0f, 0f));
    }

    [Fact]
    public void FromRgb_float_rejects_greater_than_one()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Color.FromRgb(0f, 1.1f, 0f));
    }

    [Fact]
    public void FromRgb_float_rejects_NaN()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Color.FromRgb(float.NaN, 0f, 0f));
    }

    [Fact]
    public void FromRgb_float_rejects_PositiveInfinity()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Color.FromRgb(float.PositiveInfinity, 0f, 0f));
    }

    [Fact]
    public void FromArgb_float_rejects_NaN_alpha()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Color.FromArgb(float.NaN, 0f, 0f, 0f));
    }

    [Fact]
    public void FromArgb_float_boundary_values()
    {
        var c = Color.FromArgb(0.5f, 0f, 0.5f, 1f);
        Assert.Equal(127, c.A);
        Assert.Equal(0, c.R);
        Assert.Equal(127, c.G);
        Assert.Equal(255, c.B);
    }

    // ------------------------------------------------------------------
    //  Byte array accessors
    // ------------------------------------------------------------------

    [Fact]
    public void AsArgbBytes_returns_correct_order()
    {
        var c = Color.FromArgb(10, 20, 30, 40);
        var bytes = c.AsArgbBytes();
        Assert.Equal([10, 20, 30, 40], bytes);
    }

    [Fact]
    public void AsRrgbaBytes_returns_correct_order()
    {
        var c = Color.FromArgb(10, 20, 30, 40);
        var bytes = c.AsRrgbaBytes();
        Assert.Equal([20, 30, 40, 10], bytes);
    }

    [Fact]
    public void AsRrgbBytes_returns_correct_order()
    {
        var c = Color.FromArgb(10, 20, 30, 40);
        var bytes = c.AsRrgbBytes();
        Assert.Equal([20, 30, 40], bytes);
    }

    // ------------------------------------------------------------------
    //  ToString / formatting
    // ------------------------------------------------------------------

    [Theory]
    [InlineData(0, 0, 0, 0, "#00000000")]
    [InlineData(0, 255, 0, 0, "#FF000000")]
    [InlineData(0, 255, 255, 255, "#FFFFFF00")]
    [InlineData(254, 0, 0, 0, "#000000FE")]
    [InlineData(254, 255, 0, 0, "#FF0000FE")]
    [InlineData(254, 255, 255, 255, "#FFFFFFFE")]
    [InlineData(255, 0, 0, 0, "#000000")]
    [InlineData(255, 255, 0, 0, "#FF0000")]
    [InlineData(255, 255, 255, 255, "#FFFFFF")]
    public void ToString_outputs_hex_format(byte a, byte r, byte g, byte b, string expected)
    {
        var c = Color.FromArgb(a, r, g, b);
        Assert.Equal(expected, c.ToString());
    }

    [Fact]
    public void ToRgbHexString_always_7_chars()
    {
        var c = Color.FromRgba(255, 128, 0, 200);
        var s = c.ToRgbHexString();
        Assert.Equal("#FF8000", s);
        Assert.Equal(Color.RgbHexFormatLength, s.Length);
    }

    [Fact]
    public void ToRgbaHexString_always_9_chars()
    {
        var c = Color.FromRgba(255, 128, 0, 200);
        var s = c.ToRgbaHexString();
        Assert.Equal("#FF8000C8", s);
        Assert.Equal(Color.RgbaHexFormatLength, s.Length);
    }

    [Fact]
    public void ToArgbHexString_always_9_chars()
    {
        var c = Color.FromArgb(200, 255, 128, 0);
        var s = c.ToArgbHexString();
        Assert.Equal("#C8FF8000", s);
        Assert.Equal(Color.ArgbHexFormatLength, s.Length);
    }

    [Fact]
    public void TryFormat_insufficient_buffer_returns_false()
    {
        var c = Color.FromRgb(255, 0, 0);
        Span<char> buf = stackalloc char[3];
        Assert.False(c.TryFormat(buf, out var written, [], null));
        Assert.Equal(0, written);
    }

    // ------------------------------------------------------------------
    //  Parsing
    // ------------------------------------------------------------------

    [Theory]
    [InlineData("#FF0000", 255, 255, 0, 0)]
    [InlineData("#00FF00", 255, 0, 255, 0)]
    [InlineData("#0000FF", 255, 0, 0, 255)]
    [InlineData("#FFFFFF", 255, 255, 255, 255)]
    [InlineData("#000000", 255, 0, 0, 0)]
    public void Parse_rgb_hex(string input, byte expectedA, byte expectedR, byte expectedG, byte expectedB)
    {
        var c = Color.Parse(input, null);
        Assert.Equal(expectedA, c.A);
        Assert.Equal(expectedR, c.R);
        Assert.Equal(expectedG, c.G);
        Assert.Equal(expectedB, c.B);
    }

    [Theory]
    [InlineData("#FF000080", 128, 255, 0, 0)]
    [InlineData("#00FF00FF", 255, 0, 255, 0)]
    public void Parse_rgba_hex(string input, byte expectedA, byte expectedR, byte expectedG, byte expectedB)
    {
        var c = Color.Parse(input, null);
        Assert.Equal(expectedA, c.A);
        Assert.Equal(expectedR, c.R);
        Assert.Equal(expectedG, c.G);
        Assert.Equal(expectedB, c.B);
    }

    [Fact]
    public void TryParse_null_returns_false()
    {
        Assert.False(Color.TryParse(null, out _));
    }

    [Fact]
    public void TryParse_empty_returns_false()
    {
        Assert.False(Color.TryParse("", out _));
    }

    [Fact]
    public void TryParse_no_hash_returns_false()
    {
        Assert.False(Color.TryParse("FF0000", out _));
    }

    [Fact]
    public void TryParse_invalid_hex_chars_returns_false_not_throws()
    {
        Assert.False(Color.TryParse("#GGGGGG", out _));
    }

    [Fact]
    public void TryParse_invalid_8char_hex_returns_false_not_throws()
    {
        Assert.False(Color.TryParse("#GGGGGGGG", out _));
    }

    [Fact]
    public void TryParse_wrong_length_returns_false()
    {
        Assert.False(Color.TryParse("#FFF", out _));
    }

    [Fact]
    public void Parse_invalid_throws_FormatException()
    {
        Assert.Throws<FormatException>(() => Color.Parse("invalid", null));
    }

    // ------------------------------------------------------------------
    //  Round-trip: ToString ↔ Parse
    // ------------------------------------------------------------------

    [Theory]
    [InlineData(255, 255, 0, 0)]
    [InlineData(255, 0, 128, 255)]
    [InlineData(128, 64, 32, 16)]
    public void RoundTrip_default_format(byte a, byte r, byte g, byte b)
    {
        var original = Color.FromArgb(a, r, g, b);
        var text = original.ToString();
        var parsed = Color.Parse(text, null);
        Assert.Equal(original, parsed);
    }

    [Fact]
    public void RoundTrip_rgb_hex()
    {
        var c = Color.FromRgb(100, 200, 50);
        var text = c.ToRgbHexString();
        var parsed = Color.Parse(text, null);
        Assert.Equal(c, parsed);
    }

    [Fact]
    public void RoundTrip_rgba_hex()
    {
        var c = Color.FromRgba(100, 200, 50, 128);
        var text = c.ToRgbaHexString();
        var parsed = Color.Parse(text, null);
        Assert.Equal(c, parsed);
    }

    // ------------------------------------------------------------------
    //  JSON serialization round-trip
    // ------------------------------------------------------------------

    [Theory]
    [InlineData(255, 255, 0, 0)]
    [InlineData(128, 0, 255, 128)]
    public void Json_roundtrip(byte a, byte r, byte g, byte b)
    {
        var original = Color.FromArgb(a, r, g, b);
        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<Color>(json);
        Assert.Equal(original, deserialized);
    }

    // ------------------------------------------------------------------
    //  System.Drawing.Color interop
    // ------------------------------------------------------------------

    [Fact]
    public void Roundtrip_system_drawing_color()
    {
        var original = Color.FromArgb(200, 100, 150, 50);
        var sdc = original.ToSystemDrawingColor();
        var back = Color.FromSystemDrawingColor(sdc);
        Assert.Equal(original, back);
    }

    [Fact]
    public void Implicit_conversion_to_system_drawing()
    {
        var c = Color.FromRgb(10, 20, 30);
        System.Drawing.Color sdc = c;
        Assert.Equal(10, sdc.R);
        Assert.Equal(20, sdc.G);
        Assert.Equal(30, sdc.B);
    }

    [Fact]
    public void Implicit_conversion_from_system_drawing()
    {
        var sdc = System.Drawing.Color.FromArgb(255, 10, 20, 30);
        Color c = sdc;
        Assert.Equal(10, c.R);
        Assert.Equal(20, c.G);
        Assert.Equal(30, c.B);
    }

    // ------------------------------------------------------------------
    //  HSL conversion
    // ------------------------------------------------------------------

    [Fact]
    public void ToHsl_black()
    {
        var c = Color.FromRgb(0, 0, 0);
        c.ToHsl(out _, out _, out var l);
        Assert.Equal(0f, l);
    }

    [Fact]
    public void ToHsl_white()
    {
        var c = Color.FromRgb(255, 255, 255);
        c.ToHsl(out _, out var s, out var l);
        Assert.Equal(1f, l);
        Assert.Equal(0f, s);
    }

    [Fact]
    public void FromHsl_roundtrip_red()
    {
        var original = Color.FromRgb(255, 0, 0);
        original.ToHsl(out var h, out var s, out var l);
        var roundTripped = Color.FromHsl(h, s, l);
        Assert.Equal(original.R, roundTripped.R);
        Assert.Equal(original.G, roundTripped.G);
        Assert.Equal(original.B, roundTripped.B);
    }

    // ------------------------------------------------------------------
    //  Equality
    // ------------------------------------------------------------------

    [Fact]
    public void Same_color_is_equal()
    {
        var a = Color.FromRgb(1, 2, 3);
        var b = Color.FromRgb(1, 2, 3);
        Assert.Equal(a, b);
        Assert.True(a == b);
    }

    [Fact]
    public void Different_colors_are_not_equal()
    {
        var a = Color.FromRgb(1, 2, 3);
        var b = Color.FromRgb(3, 2, 1);
        Assert.NotEqual(a, b);
        Assert.True(a != b);
    }

    // ------------------------------------------------------------------
    //  Named colors
    // ------------------------------------------------------------------

    [Fact]
    public void MediumPurple_matches_css_standard()
    {
        // CSS MediumPurple: #9370DB = rgb(147, 112, 219)
        Assert.Equal(147, Colors.MediumPurple.R);
        Assert.Equal(112, Colors.MediumPurple.G);
        Assert.Equal(219, Colors.MediumPurple.B);
    }

    [Fact]
    public void Transparent_has_zero_alpha()
    {
        Assert.Equal(0, Colors.Transparent.A);
    }

    [Fact]
    public void White_is_all_255()
    {
        Assert.Equal(255, Colors.White.A);
        Assert.Equal(255, Colors.White.R);
        Assert.Equal(255, Colors.White.G);
        Assert.Equal(255, Colors.White.B);
    }

    [Fact]
    public void Black_is_all_zero_with_full_alpha()
    {
        Assert.Equal(255, Colors.Black.A);
        Assert.Equal(0, Colors.Black.R);
        Assert.Equal(0, Colors.Black.G);
        Assert.Equal(0, Colors.Black.B);
    }
}
