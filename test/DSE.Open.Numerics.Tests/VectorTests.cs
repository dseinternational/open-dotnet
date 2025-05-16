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

    private static void TestCreate<T>(T[] elements)
        where T : notnull, IEquatable<T>
    {
        Vector<T> vector = [.. elements];
        Assert.Equal(elements.Length, vector.Length);
        Assert.Equivalent(elements, vector.ToArray());
    }

    private static void TestSerializeDeserialize<T>(T[] elements, JsonSerializerOptions serializerOptions)
        where T : notnull, IEquatable<T>
    {
        Vector<T> vector = [.. elements];
        var json = JsonSerializer.Serialize(vector, serializerOptions);
        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<Vector<T>>(json, serializerOptions);
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
        TestCreate([0, 1, 2, 3, 4]);
    }

    [Fact]
    public void CreateNumericFloat()
    {
        TestCreate([0.496f, 1.235f, 200.8469874f, -4682.169845f, 981635.123548715f]);
    }

    [Fact]
    public void CreateNumericDouble()
    {
        TestCreate([0.496, 1.235, 200.8469874, -4682.169845, 981635.123548715]);
    }

    [Fact]
    public void SerializeDeserializeReflectedFloat()
    {
        TestSerializeDeserialize(
            [0.496f, 1.235f, 200.8469874f, -4682.169845f, 981635.123548715f],
            NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserializeSourceGeneratedFloat()
    {
        TestSerializeDeserialize(
            [0.496f, 1.235f, 200.8469874f, -4682.169845f, 981635.123548715f],
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

    //[Fact]
    //public void VectorEquality()
    //{
    //    Vector<int> v1 = [1, 2, 3, 4, 5, 6];
    //    var v2 = Vector.Create([1, 2, 3, 4, 5, 6]);
    //    Assert.Equal(v1, v2);
    //}

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

    /*
    [Fact]
    public void AdditionOperator()
    {
        var v1 = Vector.CreateOnes<int>(6);
        var v2 = Vector.CreateOnes<int>(6);
        var v3 = v1 + v2;
        Assert.Equal(6, v3.Length);
        Assert.True(v3.AsSpan().SequenceEqual([2, 2, 2, 2, 2, 2]));
    }

    [Fact]
    public void AdditionOperatorScalar()
    {
        var v1 = Vector.CreateOnes<int>(6);
        var v2 = v1 + 1;
        Assert.Equal(6, v2.Length);
        Assert.True(v2.AsSpan().SequenceEqual([2, 2, 2, 2, 2, 2]));
    }

    [Fact]
    public void SubtractionOperator()
    {
        var v1 = Vector.CreateOnes<int>(6);
        var v2 = Vector.CreateOnes<int>(6);
        var v3 = v1 - v2;
        Assert.Equal(6, v3.Length);
        Assert.True(v3.AsSpan().SequenceEqual([0, 0, 0, 0, 0, 0]));
    }

    [Fact]
    public void SubtractionOperatorScalar()
    {
        var v1 = Vector.CreateOnes<int>(6);
        var v2 = v1 - 1;
        Assert.Equal(6, v2.Length);
        Assert.True(v2.AsSpan().SequenceEqual([0, 0, 0, 0, 0, 0]));
    }
    */


}
