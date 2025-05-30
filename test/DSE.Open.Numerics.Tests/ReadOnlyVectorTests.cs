// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json;
using DSE.Open.Numerics.Serialization;
using DSE.Open.Testing.Xunit;

namespace DSE.Open.Numerics;

public sealed class ReadOnlyVectorTests : LoggedTestsBase
{
    public ReadOnlyVectorTests(ITestOutputHelper output) : base(output)
    {
    }

    private static void TestCreate<T>(T[] elements)
        where T : notnull, IEquatable<T>
    {
        var vector = Vector.Create(elements).AsReadOnly();

        Assert.NotNull(vector);
        Assert.Equal(elements.Length, vector.Length);
        Assert.Equivalent(elements, vector.ToArray());
    }

    private void TestSerializeDeserializeNumeric<T>(T[] elements, JsonSerializerOptions serializerOptions)
        where T : struct, INumber<T>
    {
        var vector = Vector.Create(elements).AsReadOnly();

        var json = JsonSerializer.Serialize(vector, serializerOptions);

        Assert.NotNull(json);

        Output.WriteLine(json);

        var deserialized = JsonSerializer.Deserialize<Vector<T>>(json, serializerOptions);

        Assert.NotNull(deserialized);
        Assert.Equivalent(vector, deserialized);
    }

    private void TestSerializeDeserializeReadOnlyNumeric<T>(T[] elements, JsonSerializerOptions serializerOptions)
        where T : struct, INumber<T>
    {
        var vector = Vector.Create(elements).AsReadOnly();

        var json = JsonSerializer.Serialize(vector, serializerOptions);

        Assert.NotNull(json);

        Output.WriteLine(json);

        var deserialized = JsonSerializer.Deserialize<ReadOnlyVector<T>>(json, serializerOptions);

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
    public void CreateInt32()
    {
        TestCreate([0, 1, 2, 3, 4]);
    }

    [Fact]
    public void CreateFloat()
    {
        TestCreate([0.496f, 1.235f, 200.8469874f, -4682.169845f, 981635.123548715f]);
    }

    [Fact]
    public void CreateDouble()
    {
        TestCreate([0.496, 1.235, 200.8469874, -4682.169845, 981635.123548715]);
    }

    [Fact]
    public void SerializeDeserializeReflectedFloat()
    {
        TestSerializeDeserializeNumeric(
            [0.496f, 1.235f, 200.8469874f, -4682.169845f, 981635.123548715f],
            NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserializeReflectedFloatReadOnly()
    {
        TestSerializeDeserializeReadOnlyNumeric(
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
    public void SerializeDeserializeSourceGeneratedFloatReadOnly()
    {
        TestSerializeDeserializeReadOnlyNumeric(
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
    public void Init()
    {
        ReadOnlyVector<int> v1 = [1, 2, 3, 4, 5, 6];
        var v2 = ReadOnlyVector.Create([1, 2, 3, 4, 5, 6]);

        Assert.Equal(6, v1.Length);
        Assert.Equal(6, v2.Length);

        Assert.True(v1.AsSpan().SequenceEqual(v2.AsSpan()));
    }

    [Fact]
    public void JsonRoundtrip()
    {
        // Arrange
        var vector = ReadOnlyVector.Create([1, 2, 3, 4, 5, 6]);

        // Act
        var json = JsonSerializer.Serialize(vector);

        Output.WriteLine(json);

        var deserializedVector = JsonSerializer.Deserialize<ReadOnlyVector<int>>(json);

        // Assert
        Assert.NotNull(deserializedVector);
        Assert.True(vector.SequenceEqual(deserializedVector));
    }
}
