// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

#undef OUTPUT_CSV
// #define OUTPUT_CSV

namespace DSE.Open.Hashing;

public sealed class RepeatableHash64ProviderTests
{
    [Fact]
    public void GetRepeatableHashCode_ReadOnlySpanByte_ReturnsExpected()
    {
        ReadOnlySpan<byte> value = [1, 2, 3, 4, 5, 6, 7, 8];
        const ulong expected = 1653410307359580823u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_Int32_ReturnsExpected()
    {
        const int value = 42;
        const ulong expected = 2392174772787195229u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_Decimal_ReturnsExpected()
    {
        const decimal value = 42.42m;
        const ulong expected = 16175059879511794072u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_UInt32_ReturnsExpected()
    {
        const ulong value = 424242424242u;
        const ulong expected = 5997179928398961107u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_String_ReturnsExpected()
    {
        const string value = "42";
        const ulong expected = 15131234264966358245u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_StringEmpty_ReturnsExpected()
    {
        const string value = "";
        const ulong expected = 3244421341483603138u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_ReadOnlySpanChar_ReturnsExpected()
    {
        ReadOnlySpan<char> value = "A test value";
        const ulong expected = 11473665220532529127u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_ReadOnlySpanChar_BigEndian_ReturnsExpected()
    {
        ReadOnlySpan<char> value = "A test value";
        const ulong expected = 9838420940477837270u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value, true);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_ReadOnlySpanInt16_ReturnsExpected()
    {
        ReadOnlySpan<short> value = [-42, 0, 1, 42];
        const ulong expected = 6686604391042738473u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_ReadOnlySpanInt16_BigEndian_ReturnsExpected()
    {
        ReadOnlySpan<short> value = [-42, 0, 1, 42];
        const ulong expected = 17659265717773422113u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value, true);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_ReadOnlySpanInt32_ReturnsExpected()
    {
        ReadOnlySpan<int> value = [-42, 0, 1, 42];
        const ulong expected = 14201497061795295795u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_ReadOnlySpanInt32_BigEndian_ReturnsExpected()
    {
        ReadOnlySpan<int> value = [-42, 0, 1, 42];
        const ulong expected = 1543474647417320130u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value, true);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_ReadOnlySpanInt64_ReturnsExpected()
    {
        ReadOnlySpan<long> value = [long.MinValue, 0, 1, long.MaxValue];
        const ulong expected = 16322748243600739498u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_ReadOnlySpanInt64_BigEndian_ReturnsExpected()
    {
        ReadOnlySpan<long> value = [long.MinValue, 0, 1, long.MaxValue];
        const ulong expected = 11982527232425785625u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value, true);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_ReadOnlySpanUInt16_ReturnsExpected()
    {
        ReadOnlySpan<ushort> value = [0, 1, 42];
        const ulong expected = 8683154451414937794u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_ReadOnlySpanUInt16_BigEndian_ReturnsExpected()
    {
        ReadOnlySpan<ushort> value = [0, 1, 42];
        const ulong expected = 1011185303802023267u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value, true);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_ReadOnlySpanUInt32_ReturnsExpected()
    {
        ReadOnlySpan<uint> value = [0, 1, 42];
        const ulong expected = 13229281443940912080u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_ReadOnlySpanUInt32_BigEndian_ReturnsExpected()
    {
        ReadOnlySpan<uint> value = [0, 1, 42];
        const ulong expected = 1609210568001055618u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value, true);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_ReadOnlySpanUInt64_ReturnsExpected()
    {
        ReadOnlySpan<ulong> value = [0, 1, long.MaxValue, ulong.MaxValue];
        const ulong expected = 10129015545246210565u;
        var actual = RepeatableHash64Provider.Default.GetRepeatableHashCode(value);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRepeatableHashCode_ReadOnlySpanUInt64_BigEndian_ReturnsExpected()
    {
        ReadOnlySpan<ulong> value = [0, 1, long.MaxValue, ulong.MaxValue];
        const ulong expected = 6043940069377282815u;
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

#if OUTPUT_CSV
        using var output = File.OpenWrite("../../hashes.csv");
        using var writer = new StreamWriter(output);

        for (var i = 0; i < hashes.Length; i++)
        {
            writer.WriteLine($"{i},{hashes[i]}");
        }
#endif // OUTPUT_CSV
    }

#if OUTPUT_CSV
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

        using var output = File.OpenWrite("../../combined_hashes.csv");
        using var writer = new StreamWriter(output);

        for (var i = 0; i < combined.Length; i++)
        {
            writer.WriteLine($"{i},{combined[i]}");
        }
    }
#endif // OUTPUT_CSV
}
