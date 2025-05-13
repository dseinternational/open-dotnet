// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json;
using DSE.Open.Numerics.Serialization;
using DSE.Open.Testing.Xunit;

namespace DSE.Open.Numerics;

public partial class VectorTests : LoggedTestsBase
{
    public VectorTests(ITestOutputHelper output) : base(output)
    {
    }

    private static void TestCreate<T>(T[] elements)
        where T : notnull
    {
        var vector = Series.Create(elements);

        Assert.NotNull(vector);
        Assert.Equal(elements.Length, vector.Length);
        Assert.Equivalent(elements, vector.ToArray());
    }

    private static void TestCreateNumeric<T>(T[] elements)
        where T : struct, INumber<T>
    {
        var vector = Series.Create(elements);

        Assert.NotNull(vector);
        Assert.Equal(elements.Length, vector.Length);
        Assert.Equivalent(elements, vector.ToArray());
    }

    private static void TestSerializeDeserializeNumeric<T>(T[] elements, JsonSerializerOptions serializerOptions)
        where T : struct, INumber<T>
    {
        var vector = Series.Create(elements);

        var json = JsonSerializer.Serialize(vector, serializerOptions);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<Series<T>>(json, serializerOptions);

        Assert.NotNull(deserialized);
        Assert.Equivalent(vector, deserialized);
    }

    [Fact]
    public void Create_Char()
    {
        TestCreate(['a', 'b', 'c', 'd', 'e']);
    }

    [Fact]
    public void CreateString()
    {
        TestCreate(["one", "two", "three", "four", "five"]);
    }

    [Fact]
    public void CreateNumericInt32()
    {
        TestCreateNumeric([0, 1, 2, 3, 4]);
    }

    [Fact]
    public void CreateNumericFloat()
    {
        TestCreateNumeric([0.496f, 1.235f, 200.8469874f, -4682.169845f, 981635.123548715f]);
    }

    [Fact]
    public void CreateNumericDouble()
    {
        TestCreateNumeric([0.496, 1.235, 200.8469874, -4682.169845, 981635.123548715]);
    }

    [Fact]
    public void SerializeDeserializeReflectedFloat()
    {
        TestSerializeDeserializeNumeric(
            [0.496f, 1.235f, 200.8469874f, -4682.169845f, 981635.123548715f],
            NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserializeSourceGeneratedFloat()
    {
        TestSerializeDeserializeNumeric(
            [0.496f, 1.235f, 200.8469874f, -4682.169845f, 981635.123548715f],
            NumericsJsonSharedOptions.SourceGenerated);
    }

    [Fact]
    public void SerializeDeserializeReflectedDate64()
    {
        var elements = Enumerable.Range(1, 20).Select(i => new DateTime64(i * 1000L * 60 * 60 * 24 * 365)).ToArray();
        TestSerializeDeserializeNumeric(elements, NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserializeSourceGeneratedDate64()
    {
        var elements = Enumerable.Range(1, 20).Select(i => new DateTime64(i * 1000L * 60 * 60 * 24 * 365)).ToArray();
        TestSerializeDeserializeNumeric(elements, NumericsJsonSharedOptions.SourceGenerated);
    }

    [Fact]
    public void CreateWithKnownNumericTypeReturnsVectorInt32()
    {
        var vector = Series.Create([1, 2, 3, 4, 5]);
        var numVector = Assert.IsType<Series<int>>(vector);
        Assert.NotNull(numVector);
    }

    [Fact]
    public void CreateWithKnownNumericTypeReturnsVectorDouble()
    {
        var vector = Series.Create([1.0, 2.84685, -0.000083, 4, 5]);
        var numVector = Assert.IsType<Series<double>>(vector);
        Assert.NotNull(numVector);
    }
    [Fact]
    public void Init()
    {
        Series<int> v1 = [1, 2, 3, 4, 5, 6];

        var v2 = Series.Create([1, 2, 3, 4, 5, 6]);

        Assert.Equal(6, v1.Length);
        Assert.Equal(6, v2.Length);

        Assert.True(v1.AsSpan().SequenceEqual(v2.AsSpan()));
    }

    [Fact]
    public void CreateDefault()
    {
        var v1 = Series.Create<int>(6);
        Assert.Equal(6, v1.Length);
        Assert.True(v1.AsSpan().SequenceEqual(new int[6]));
    }

    [Fact]
    public void CreateZeroes()
    {
        var v1 = Series.CreateZeroes<int>(6);
        Assert.Equal(6, v1.Length);
        Assert.True(v1.AsSpan().SequenceEqual(new int[6]));
    }

    [Fact]
    public void CreateOnes()
    {
        var v1 = Series.CreateOnes<int>(6);
        Assert.Equal(6, v1.Length);
        Assert.True(v1.AsSpan().SequenceEqual([1, 1, 1, 1, 1, 1]));
    }

    [Fact]
    public void Equality()
    {
        var v1 = Series.CreateOnes<int>(6);
        var v2 = Series.CreateOnes<int>(6);
        Assert.Equal(v1, v2);
    }
    /*
    [Fact]
    public void AdditionOperator()
    {
        var v1 = Series.CreateOnes<int>(6);
        var v2 = Series.CreateOnes<int>(6);
        var v3 = v1 + v2;
        Assert.Equal(6, v3.Length);
        Assert.True(v3.AsSpan().SequenceEqual([2, 2, 2, 2, 2, 2]));
    }

    [Fact]
    public void AdditionOperatorScalar()
    {
        var v1 = Series.CreateOnes<int>(6);
        var v2 = v1 + 1;
        Assert.Equal(6, v2.Length);
        Assert.True(v2.AsSpan().SequenceEqual([2, 2, 2, 2, 2, 2]));
    }

    [Fact]
    public void SubtractionOperator()
    {
        var v1 = Series.CreateOnes<int>(6);
        var v2 = Series.CreateOnes<int>(6);
        var v3 = v1 - v2;
        Assert.Equal(6, v3.Length);
        Assert.True(v3.AsSpan().SequenceEqual([0, 0, 0, 0, 0, 0]));
    }

    [Fact]
    public void SubtractionOperatorScalar()
    {
        var v1 = Series.CreateOnes<int>(6);
        var v2 = v1 - 1;
        Assert.Equal(6, v2.Length);
        Assert.True(v2.AsSpan().SequenceEqual([0, 0, 0, 0, 0, 0]));
    }
    */

}
