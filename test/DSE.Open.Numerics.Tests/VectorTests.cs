// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Numerics.Serialization;
using DSE.Open.Testing.Xunit;

namespace DSE.Open.Numerics;

public partial class VectorTests : LoggedTestsBase
{
    public VectorTests(ITestOutputHelper output) : base(output)
    {
    }

    private static Vector<T> TestCreate<T>(T[] elements)
        where T : notnull, IEquatable<T>
    {
        Vector<T> vector = [.. elements];
        Assert.Equal(elements.Length, vector.Length);
        Assert.Equivalent(elements, vector.ToArray());
        return vector;
    }

    private void TestSerializeDeserialize<T>(T[] elements, JsonSerializerOptions serializerOptions)
        where T : notnull, IEquatable<T>
    {
        Vector<T> vector = [.. elements];
        var json = JsonSerializer.Serialize(vector, serializerOptions);
        Assert.NotNull(json);

        Output.WriteLine(json);

        var deserialized = JsonSerializer.Deserialize<Vector<T>>(json, serializerOptions);
        Assert.NotNull(deserialized);
        Assert.Equivalent(vector, deserialized);
    }

    [Fact]
    public void Create_Char()
    {
        _ = TestCreate(['a', 'b', 'c', 'd', 'e']);
    }

    [Fact]
    public void CreateString()
    {
        _ = TestCreate(["one", "two", "three", "four", "five"]);
    }

    [Fact]
    public void CreateInt8()
    {
        _ = TestCreate<sbyte>([0, 1, 2, 3, -4]);
    }

    [Fact]
    public void CreateInt16()
    {
        _ = TestCreate<short>([0, 1, 2, 3, -4]);
    }

    [Fact]
    public void CreateNaInt16()
    {
        var v = TestCreate<NaInt<short>>([0, 1, 2, 3, -4, null]);
        Assert.True(v.IsNullable);
    }

    [Fact]
    public void CreateInt32()
    {
        _ = TestCreate([0, 1, 2, 3, -4]);
    }

    [Fact]
    public void CreateInt64()
    {
        _ = TestCreate<long>([0, 1, 2, 3, -4]);
    }

    [Fact]
    public void CreateUInt16()
    {
        _ = TestCreate<ushort>([0, 1, 2, 3, 4]);
    }

    [Fact]
    public void CreateUInt32()
    {
        _ = TestCreate<uint>([0, 1, 2, 3, 4]);
    }

    [Fact]
    public void CreateUInt64()
    {
        _ = TestCreate<ulong>([0, 1, 2, 3, 4]);
    }

    [Fact]
    public void CreateFloat16()
    {
        _ = TestCreate([(Half)0.496f, (Half)1.235f, (Half)200.8469874f, (Half)(-4682.169845f), (Half)981635.123548715f]);
    }

    [Fact]
    public void CreateFloat32()
    {
        _ = TestCreate([0.496f, 1.235f, 200.8469874f, -4682.169845f, 981635.123548715f]);
    }

    [Fact]
    public void CreateFloat64()
    {
        _ = TestCreate([0.496, 1.235, 200.8469874, -4682.169845, 981635.123548715]);
    }

    [Fact]
    public void CreateNaInt8()
    {
        _ = TestCreate<NaInt<sbyte>>([0, 1, 2, 3, -4, null]);
    }

    [Fact]
    public void CreateNaInt32()
    {
        _ = TestCreate<NaInt<int>>([0, 1, 2, 3, -4, null]);
    }

    [Fact]
    public void CreateNaInt64()
    {
        _ = TestCreate<NaInt<long>>([0, 1, 2, 3, -4, null]);
    }

    [Fact]
    public void CreateNaUInt16()
    {
        _ = TestCreate<NaInt<ushort>>([0, 1, 2, 3, 4, null]);
    }

    [Fact]
    public void SerializeDeserializeReflectedInt8()
    {
        TestSerializeDeserialize(
            [.. Enumerable.Range(-10, 120).Select(i => (sbyte)i)],
            NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserializeSourceGeneratedInt8()
    {
        TestSerializeDeserialize(
            [.. Enumerable.Range(-10, 120).Select(i => (sbyte)i)],
            NumericsJsonSharedOptions.SourceGenerated);
    }

    [Fact]
    public void SerializeDeserializeReflectedInt16()
    {
        TestSerializeDeserialize(
            [.. Enumerable.Range(-10, 500).Select(i => (short)i)],
            NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserializeSourceGeneratedInt16()
    {
        TestSerializeDeserialize(
            [.. Enumerable.Range(-10, 500).Select(i => (short)i)],
            NumericsJsonSharedOptions.SourceGenerated);
    }

    [Fact]
    public void SerializeDeserializeReflectedInt32()
    {
        TestSerializeDeserialize(
            [.. Enumerable.Range(-10, 500)],
            NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserializeSourceGeneratedInt32()
    {
        TestSerializeDeserialize(
            [.. Enumerable.Range(-10, 500)],
            NumericsJsonSharedOptions.SourceGenerated);
    }

    [Fact]
    public void SerializeDeserializeReflectedInt64()
    {
        TestSerializeDeserialize(
            [.. Enumerable.Range(-10, 500).Select(i => (long)i)],
            NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserializeSourceGeneratedInt64()
    {
        TestSerializeDeserialize(
            [.. Enumerable.Range(-10, 500).Select(i => (long)i)],
            NumericsJsonSharedOptions.SourceGenerated);
    }

    [Fact]
    public void SerializeDeserializeReflectedFloat32()
    {
        TestSerializeDeserialize(
            [0.496f, 1.235f, 200.8469874f, -4682.169845f, 981635.123548715f],
            NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserializeSourceGeneratedFloat32()
    {
        TestSerializeDeserialize(
            [0.496f, 1.235f, 200.8469874f, -4682.169845f, 981635.123548715f, float.NaN, float.NegativeInfinity, float.PositiveInfinity],
            NumericsJsonSharedOptions.SourceGenerated);
    }

    [Fact]
    public void SerializeDeserializeReflectedFloat64()
    {
        TestSerializeDeserialize(
            [0.496, 1.235, 200.8469874, -4682.169845, 981635.123548715, double.NaN, double.NegativeInfinity, double.PositiveInfinity],
            NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserializeSourceGeneratedFloat64()
    {
        TestSerializeDeserialize(
            [0.496, 1.235, 200.8469874, -4682.169845, 981635.123548715],
            NumericsJsonSharedOptions.SourceGenerated);
    }

    [Fact]
    public void SerializeDeserializeReflectedDate64()
    {
        var elements = Enumerable.Range(1, 20).Select(i => new DateTime64(i * 1000L * 60 * 60 * 24 * 365)).ToArray();
        TestSerializeDeserialize(elements, NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserializeSourceGeneratedDate64()
    {
        var elements = Enumerable.Range(1, 20).Select(i => new DateTime64(i * 1000L * 60 * 60 * 24 * 365)).ToArray();
        TestSerializeDeserialize(elements, NumericsJsonSharedOptions.SourceGenerated);
    }

    [Fact]
    public void SerializeDeserializeReflectedString()
    {
        TestSerializeDeserialize(
            [.. Enumerable.Range(1, 500).Select(i => $"item {i}")],
            NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserializeSourceGeneratedString()
    {
        TestSerializeDeserialize(
            [.. Enumerable.Range(1, 500).Select(i => $"item {i}")],
            NumericsJsonSharedOptions.SourceGenerated);
    }

    [Fact]
    public void SerializeDeserializeReflectedCharAscii()
    {
        TestSerializeDeserialize(
            [.. Enumerable.Range(48, 78).Select(i => (char)i)],
            NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserializeSourceGeneratedCharAscii()
    {
        TestSerializeDeserialize(
            [.. Enumerable.Range(48, 78).Select(i => (char)i)],
            NumericsJsonSharedOptions.SourceGenerated);
    }

    [Fact]
    public void SerializeDeserializeReflectedCharUnicode()
    {
        TestSerializeDeserialize(
            [.. Enumerable.Range(48, 591).Select(i => (char)i)],
            NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserializeSourceGeneratedCharUnicode()
    {
        TestSerializeDeserialize(
            [.. Enumerable.Range(48, 591).Select(i => (char)i)],
            NumericsJsonSharedOptions.SourceGenerated);
    }

    [Fact]
    public void SerializeDeserializeReflectedNaCharUnicode()
    {
        TestSerializeDeserialize(
            [.. Enumerable.Range(48, 591).Select(i => (NaValue<char>)(char)i)],
            NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserializeSourceGeneratedNaCharUnicode()
    {
        TestSerializeDeserialize(
            [.. Enumerable.Range(48, 591).Select(i => (NaValue<char>)(char)i)],
            NumericsJsonSharedOptions.SourceGenerated);
    }
    [Fact]
    public void SerializeDeserializeReflectedBoolean()
    {
        TestSerializeDeserialize(
            [.. Enumerable.Range(1, 100).Select(i => i % 2 == 0)],
            NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserializeSourceGeneratedBoolean()
    {
        TestSerializeDeserialize(
            [.. Enumerable.Range(1, 100).Select(i => i % 2 == 0)],
            NumericsJsonSharedOptions.SourceGenerated);
    }

    [Fact]
    public void SerializeDeserializeReflectedNaInt32()
    {
        TestSerializeDeserialize(
            [.. Enumerable.Range(-10, 500).Select(i => (NaInt<int>)i)],
            NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserializeSourceGeneratedNaInt32()
    {
        TestSerializeDeserialize(
            [.. Enumerable.Range(-10, 500).Select(i => (NaInt<int>)i)],
            NumericsJsonSharedOptions.SourceGenerated);
    }

    [Fact]
    public void SerializeDeserializeReflectedNaInt64()
    {
        TestSerializeDeserialize(
            [.. Enumerable.Range(-10, 500).Select(i => (NaInt<long>)i)],
            NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserializeSourceGeneratedNaInt64()
    {
        TestSerializeDeserialize(
            [.. Enumerable.Range(-10, 500).Select(i => (NaInt<long>)i)],
            NumericsJsonSharedOptions.SourceGenerated);
    }

    [Fact]
    public void CreateWithKnownNumericTypeReturnsVectorInt32()
    {
        var vector = Vector.Create([1, 2, 3, 4, 5]);
        var numVector = Assert.IsType<Vector<int>>(vector);
        Assert.NotNull(numVector);
    }

    [Fact]
    public void CreateWithKnownNumericTypeReturnsVectorDouble()
    {
        var vector = Vector.Create([1.0, 2.84685, -0.000083, 4, 5]);
        var numVector = Assert.IsType<Vector<double>>(vector);
        Assert.NotNull(numVector);
    }

    [Fact]
    public void CreateDefault()
    {
        var v1 = Vector.Create<int>(6);
        Assert.Equal(6, v1.Length);
        Assert.True(v1.AsSpan().SequenceEqual(new int[6]));
    }

    [Fact]
    public void CreateZeroes()
    {
        var v1 = Vector.CreateZeroes<int>(6);
        Assert.Equal(6, v1.Length);
        Assert.True(v1.AsSpan().SequenceEqual(new int[6]));
    }

    [Fact]
    public void CreateOnes()
    {
        var v1 = Vector.CreateOnes<int>(6);
        Assert.Equal(6, v1.Length);
        Assert.True(v1.AsSpan().SequenceEqual([1, 1, 1, 1, 1, 1]));
    }

    [Fact]
    public void Equality()
    {
        var v1 = Vector.CreateOnes<int>(6);
        var v2 = Vector.CreateOnes<int>(6);
        Assert.Equal(v1, v2);
    }

    [Fact]
    public void Equality_2()
    {
        Vector<int> v1 = [1, 2, 3, 4, 5, 6];
        var v2 = Vector.Create([1, 2, 3, 4, 5, 6]);
        Assert.Equal(v1, v2);
    }

    [Fact]
    public void SerializeDeserializeReflected()
    {
        var series = Vector.Create([1, 2, 3, 4, 5]);

        var json = JsonSerializer.Serialize(series, NumericsJsonSharedOptions.Reflected);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<Vector<int>>(json, NumericsJsonSharedOptions.Reflected);

        Assert.NotNull(deserialized);
        Assert.Equivalent(series, deserialized);
    }

    [Fact]
    public void SerializeDeserializeSourceGenerated()
    {
        var series = Vector.Create([1, 2, 3, 4, 5]);

        var json = JsonSerializer.Serialize(series, NumericsJsonSharedOptions.Reflected);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<Vector<int>>(json, NumericsJsonSharedOptions.SourceGenerated);

        Assert.NotNull(deserialized);
        Assert.Equivalent(series, deserialized);
    }

    [Fact]
    public void SerializeDeserializeReflectedPolymorphic()
    {
        var series = Vector.Create([1, 2, 3, 4, 5]);

        var json = JsonSerializer.Serialize(series, NumericsJsonSharedOptions.Reflected);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<Vector>(json, NumericsJsonSharedOptions.Reflected);

        Assert.NotNull(deserialized);
        var series2 = Assert.IsType<Vector<int>>(deserialized);
        Assert.Equivalent(series, series2);
    }

    [Fact]
    public void SerializeDeserializeSourceGeneratedPolymorphic()
    {
        var series = Vector.Create([1, 2, 3, 4, 5]);

        var json = JsonSerializer.Serialize(series, NumericsJsonSharedOptions.Reflected);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<Vector>(json, NumericsJsonSharedOptions.SourceGenerated);

        Assert.NotNull(deserialized);
        var series2 = Assert.IsType<Vector<int>>(deserialized);
        Assert.Equivalent(series, series2);
    }

    [Fact]
    public void Add()
    {
        var v1 = Vector.CreateOnes<int>(6);
        var v2 = Vector.CreateOnes<int>(6);
        var v3 = v1.Add(v2);
        Assert.Equal(6, v3.Length);
        Assert.True(v3.AsSpan().SequenceEqual([2, 2, 2, 2, 2, 2]));
    }

    [Fact]
    public void AddScalar()
    {
        var v1 = Vector.CreateOnes<int>(6);
        var v2 = v1.Add(1);
        Assert.Equal(6, v2.Length);
        Assert.True(v2.AsSpan().SequenceEqual([2, 2, 2, 2, 2, 2]));
    }

    [Fact]
    public void Subtract()
    {
        var v1 = Vector.CreateOnes<int>(6);
        var v2 = Vector.CreateOnes<int>(6);
        var v3 = v1.Subtract(v2);
        Assert.Equal(6, v3.Length);
        Assert.True(v3.AsSpan().SequenceEqual([0, 0, 0, 0, 0, 0]));
    }

    [Fact]
    public void SubtractScalar()
    {
        var v1 = Vector.CreateOnes<int>(6);
        var v2 = v1.Subtract(1);
        Assert.Equal(6, v2.Length);
        Assert.True(v2.AsSpan().SequenceEqual([0, 0, 0, 0, 0, 0]));
    }

    [Fact]
    public void Create_EmptyArray()
    {
        var vector = Vector.Create(Array.Empty<int>());
        Assert.Equal(0, vector.Length);
        Assert.True(vector.IsEmpty);
    }

    [Fact]
    public void Create_LargeArray()
    {
        var largeArray = Enumerable.Range(0, 1_000_000).ToArray();
        var vector = Vector.Create(largeArray);
        Assert.Equal(1_000_000, vector.Length);
        Assert.True(vector.AsSpan().SequenceEqual(largeArray));
    }

    [Fact]
    public void Slice_ValidRange()
    {
        Vector<int> vector = [1, 2, 3, 4, 5];
        var slice = vector.Slice(1, 3);
        Assert.Equal(3, slice.Length);
        Assert.True(slice.AsSpan().SequenceEqual([2, 3, 4]));
    }

    [Fact]
    public void Slice_OutOfRange_Throws()
    {
        Vector<int> vector = [1, 2, 3, 4, 5];
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => vector.Slice(6, 2));
    }

    [Fact]
    public void AsSpan_ModifiesVector()
    {
        Vector<int> vector = [1, 2, 3, 4, 5];
        var span = vector.AsSpan();
        span[0] = 42;
        Assert.Equal(42, vector[0]);
    }

    [Fact]
    public void GetHashCode_EqualVectors_ReturnSameHashCode()
    {
        Vector<int> v1 = [1, 2, 3, 4, 5];
        Vector<int> v2 = [1, 2, 3, 4, 5];
        Assert.Equal(v1.GetHashCode(), v2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_UnequalVectors_ReturnDifferentHashCodes()
    {
        Vector<int> v1 = [1, 2, 3, 4, 5];
        Vector<int> v2 = [5, 4, 3, 2, 1];
        Assert.NotEqual(v1.GetHashCode(), v2.GetHashCode());
    }

    [Fact]
    public void Equals_ReadOnlySpan_Equal()
    {
        Vector<int> vector = [1, 2, 3, 4, 5];
        ReadOnlySpan<int> span = [1, 2, 3, 4, 5];
        Assert.True(vector.Equals(span));
    }

    [Fact]
    public void Equals_ReadOnlySpan_NotEqual()
    {
        Vector<int> vector = [1, 2, 3, 4, 5];
        ReadOnlySpan<int> span = [5, 4, 3, 2, 1];
        Assert.False(vector.Equals(span));
    }

    [Fact]
    public void SerializeDeserialize_EmptyVector()
    {
        var vector = Vector<int>.Empty;
        var json = JsonSerializer.Serialize(vector, NumericsJsonSharedOptions.Reflected);
        var deserialized = JsonSerializer.Deserialize<Vector<int>>(json, NumericsJsonSharedOptions.Reflected);
        Assert.NotNull(deserialized);
        Assert.True(deserialized.IsEmpty);
    }

    [Fact]
    public void EmptyVector_IsEmpty()
    {
        var empty = Vector<int>.Empty;
        Assert.Equal(0, empty.Length);
        Assert.True(empty.IsEmpty);
    }
}
