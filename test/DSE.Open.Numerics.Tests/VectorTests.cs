// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json;
using DSE.Open.Numerics.Serialization;
using DSE.Open.Testing.Xunit;

namespace DSE.Open.Numerics;

public class VectorTests : LoggedTestsBase
{
    public VectorTests(ITestOutputHelper output) : base(output)
    {
    }

    private void TestCreate<T>(T[] elements)
        where T : notnull
    {
        var vector = Vector.Create(elements);

        Assert.NotNull(vector);
        Assert.Equal(elements.Length, vector.Length);
        Assert.Equivalent(elements, vector.ToArray());
    }

    private void TestCreateNumeric<T>(T[] elements)
        where T : struct, INumber<T>
    {
        var vector = Vector.CreateNumeric(elements);

        Assert.NotNull(vector);
        Assert.Equal(elements.Length, vector.Length);
        Assert.Equivalent(elements, vector.ToArray());
    }

    private void TestSerializeDeserializeNumeric<T>(T[] elements, JsonSerializerOptions serializerOptions)
        where T : struct, INumber<T>
    {
        var vector = Vector.CreateNumeric(elements);

        var json = JsonSerializer.Serialize(vector, serializerOptions);

        Assert.NotNull(json);

        Output.WriteLine(json);

        var deserialized = JsonSerializer.Deserialize<NumericVector<T>>(json, serializerOptions);

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
    public void CreateWithKnownNumericTypeReturnsNumericVectorInt32()
    {
        var vector = Vector.Create([1, 2, 3, 4, 5]);
        var numVector = Assert.IsType<NumericVector<int>>(vector);
        Assert.NotNull(numVector);
    }

    [Fact]
    public void CreateWithKnownNumericTypeReturnsNumericVectorDouble()
    {
        var vector = Vector.Create([1.0, 2.84685, -0.000083, 4, 5]);
        var numVector = Assert.IsType<NumericVector<double>>(vector);
        Assert.NotNull(numVector);
    }
}
