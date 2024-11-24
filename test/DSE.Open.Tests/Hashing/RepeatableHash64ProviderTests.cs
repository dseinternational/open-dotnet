// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Hashing;

public class RepeatableHash64ProviderTests
{
    [Fact]
    public void GetRepeatableHashCode_ReadOnlySpanByte_ReturnsExpected()
    {
        var value = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        var expected = 1653410307359580823u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_Int32_ReturnsExpected()
    {
        var value = 42;
        var expected = 2392174772787195229u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_Decimal_ReturnsExpected()
    {
        var value = 42.42m;
        var expected = 16175059879511794072u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_UInt32_ReturnsExpected()
    {
        var value = 424242424242u;
        var expected = 5997179928398961107u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_String_ReturnsExpected()
    {
        var value = "42";
        var expected = 15131234264966358245u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_StringEmpty_ReturnsExpected()
    {
        var value = "";
        var expected = 3244421341483603138u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_ReadOnlySpanChar_ReturnsExpected()
    {
        var value = "A test value".ToCharArray();
        var expected = 11473665220532529127u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_ReadOnlySpanChar_BigEndian_ReturnsExpected()
    {
        var value = "A test value".ToCharArray();
        var expected = 394367666448625864u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value, true);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_ReadOnlySpanInt16_ReturnsExpected()
    {
        short[] value = [-42, 0, 1, 42];
        var expected = 6686604391042738473u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_ReadOnlySpanInt16_BigEndian_ReturnsExpected()
    {
        short[] value = [-42, 0, 1, 42];
        var expected = 8125135128953856518u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value, true);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_ReadOnlySpanInt32_ReturnsExpected()
    {
        int[] value = [-42, 0, 1, 42];
        var expected = 14201497061795295795u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_ReadOnlySpanInt32_BigEndian_ReturnsExpected()
    {
        int[] value = [-42, 0, 1, 42];
        var expected = 18378802383672441854u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value, true);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_ReadOnlySpanInt64_ReturnsExpected()
    {
        long[] value = [long.MinValue, 0, 1, long.MaxValue];
        var expected = 16322748243600739498u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_ReadOnlySpanInt64_BigEndian_ReturnsExpected()
    {
        long[] value = [long.MinValue, 0, 1, long.MaxValue];
        var expected = 6917305357192507555u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value, true);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_Int32_Dispersion()
    {
        var hashes = Enumerable.Range(0, 1000)
            .Select(RepeatableHash64Provider.Default.GetRepeatableHashCode)
            .ToArray();

        var distinct = hashes.Distinct().Count();

        Assert.Equal(hashes.Length, distinct);
        /*
        using var output = File.OpenWrite("../../hashes.csv");
        using var writer = new StreamWriter(output);

        for (var i = 0; i < hashes.Length; i++)
        {
            writer.WriteLine($"{i},{hashes[i]}");
        }
        */
    }

    [Fact]
    public void Combine_Dispersion()
    {
        var hashes1 = Enumerable.Range(0, 1000)
            .Select(RepeatableHash64Provider.Default.GetRepeatableHashCode)
            .ToArray();

        var hashes2 = Enumerable.Range(100000, 1000)
            .Select(RepeatableHash64Provider.Default.GetRepeatableHashCode)
            .ToArray();

        var combined = hashes1.Select((h, i) => RepeatableHash64Provider.Default.CombineHashCodes(h, hashes2[i])).ToArray();
        /*
        using var output = File.OpenWrite("../../combined_hashes.csv");
        using var writer = new StreamWriter(output);

        for (var i = 0; i < combined.Length; i++)
        {
            writer.WriteLine($"{i},{combined[i]}");
        }
        */
    }
}
