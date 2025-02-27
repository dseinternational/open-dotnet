// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json;
using DSE.Open.Numerics.Serialization;
using DSE.Open.Testing.Xunit;
using DSE.Open.Text.Json;

namespace DSE.Open.Numerics;

public class VectorTests : LoggedTestsBase
{
    private static readonly Lazy<JsonSerializerOptions> s_jsonOptions = new(() =>
    {
        var options = new JsonSerializerOptions(JsonSharedOptions.RelaxedJsonEscaping);
        options.AddDefaultNumericsJsonConverters();
        return options;
    });

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

    private void TestSerializeDeserialize<T>(T[] elements)
        where T : notnull
    {
        var vector = Vector.Create(elements);

        var json = JsonSerializer.Serialize(vector, s_jsonOptions.Value);

        Assert.NotNull(json);

        Output.WriteLine(json);

        var deserialized = JsonSerializer.Deserialize<Vector<T>>(json, s_jsonOptions.Value);

        Assert.NotNull(deserialized);
        Assert.Equivalent(vector, deserialized);
    }

    private void TestSerializeDeserializeNumeric<T>(T[] elements)
        where T : struct, INumber<T>
    {
        var vector = Vector.CreateNumeric(elements);

        var json = JsonSerializer.Serialize(vector, s_jsonOptions.Value);

        Assert.NotNull(json);

        Output.WriteLine(json);

        var deserialized = JsonSerializer.Deserialize<NumericVector<T>>(json, s_jsonOptions.Value);

        Assert.NotNull(deserialized);
        Assert.Equivalent(vector, deserialized);
    }

    [Fact]
    public void Create_Char()
    {
        TestCreate(['a', 'b', 'c', 'd', 'e']);
    }

    [Fact]
    public void CreateNumeric_Int32()
    {
        TestCreateNumeric([0, 1, 2, 3, 4]);
    }

    [Fact]
    public void CreateNumeric_Float()
    {
        TestCreateNumeric([0.496f, 1.235f, 200.8469874f, -4682.169845f, 981635.123548715f]);
    }

    [Fact]
    public void SerializeDeserialize_Char()
    {
        TestSerializeDeserialize(['a', 'b', 'c', 'd', 'e']);
    }

    [Fact]
    public void SerializeDeserialize_Int32()
    {
        TestSerializeDeserializeNumeric([0, 1, 2, 3, 4]);
    }

    [Fact]
    public void SerializeDeserialize_Int64()
    {
        TestSerializeDeserializeNumeric([0L, 1L, 2L, 3L, 4L]);
    }

    [Fact]
    public void SerializeDeserialize_Float()
    {
        TestSerializeDeserializeNumeric([0.496f, 1.235f, 200.8469874f, -4682.169845f, 981635.123548715f]);
    }

    [Fact]
    public void SerializeDeserialize_Date64()
    {
        var elements = Enumerable.Range(1, 20).Select(i => new Date64(i * 1000L * 60 * 60 * 24 * 365)).ToArray();
        TestSerializeDeserializeNumeric(elements);
    }
}
