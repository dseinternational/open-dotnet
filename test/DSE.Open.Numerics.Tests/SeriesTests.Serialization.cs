// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

public partial class SeriesTests
{
    private void TestSerializeDeserialize<T>(T[] elements, JsonSerializerOptions serializerOptions)
        where T : IEquatable<T>
    {
        var vector = Series.Create([.. elements], "test");
        var json = JsonSerializer.Serialize(vector, serializerOptions);
        Assert.NotNull(json);

        Output.WriteLine(json);

        var deserialized = JsonSerializer.Deserialize<Series<T>>(json, serializerOptions);
        Assert.NotNull(deserialized);
        Assert.Equivalent(vector, deserialized);
    }

    private void TestSerializeDeserialize<T>(T[] elements, T[] categories, JsonSerializerOptions serializerOptions)
        where T : IEquatable<T>
    {
        var vector = Series.Create([.. elements], "test", [.. categories]);
        var json = JsonSerializer.Serialize(vector, serializerOptions);
        Assert.NotNull(json);

        Output.WriteLine(json);

        var deserialized = JsonSerializer.Deserialize<Series<T>>(json, serializerOptions);
        Assert.NotNull(deserialized);
        Assert.Equivalent(vector, deserialized);
        Assert.Equivalent(vector.Categories, deserialized.Categories);
    }

    private void TestSerializeDeserialize<T>(T[] elements, T[] categories, ValueLabel<T>[] valueLabels, JsonSerializerOptions serializerOptions)
        where T : IEquatable<T>
    {
        var vector = Series.Create([.. elements], "test", [.. categories], [.. valueLabels]);
        var json = JsonSerializer.Serialize(vector, serializerOptions);
        Assert.NotNull(json);

        Output.WriteLine(json);

        var deserialized = JsonSerializer.Deserialize<Series<T>>(json, serializerOptions);
        Assert.NotNull(deserialized);
        Assert.Equivalent(vector, deserialized);
        Assert.Equivalent(vector.Categories, deserialized.Categories);
        Assert.Equivalent(vector.ValueLabels, deserialized.ValueLabels);
    }

    [Fact]
    public void SerializeDeserialize_Int16_Reflected()
    {
        TestSerializeDeserialize<short>([-1, -2, 3, 4, 5, 6, 7, 8, 9], NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserialize_Int16_SourceGenerated()
    {
        TestSerializeDeserialize<short>([-1, -2, 3, 4, 5, 6, 7, 8, 9], NumericsJsonSharedOptions.SourceGenerated);
    }

    [Fact]
    public void SerializeDeserialize_Int32_Reflected()
    {
        TestSerializeDeserialize([1, 2, 3, 4, 5, 6, 7, -8, -9], NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserialize_Int32_SourceGenerated()
    {
        TestSerializeDeserialize([1, 2, 3, 4, 5, 6, 7, -8, -9], NumericsJsonSharedOptions.SourceGenerated);
    }

    [Fact]
    public void SerializeDeserialize_Int64_Reflected()
    {
        TestSerializeDeserialize<long>([-1, -2, 3, 4, 5, 6, 7, 8, 9], NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserialize_Int64_SourceGenerated()
    {
        TestSerializeDeserialize<long>([-1, -2, 3, 4, 5, 6, 7, 8, 9], NumericsJsonSharedOptions.SourceGenerated);
    }

    [Fact]
    public void SerializeDeserialize_Float64_Reflected()
    {
        TestSerializeDeserialize([-685142.2547851, -2, 3, 4, 5, 6, 7, 8, double.NaN], NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserialize_Float64_SourceGenerated()
    {
        TestSerializeDeserialize([-685142.2547851, -2, 3, 4, 5, 6, 7, 8, double.NaN], NumericsJsonSharedOptions.SourceGenerated);
    }

    [Fact]
    public void SerializeDeserialize_String_Reflected()
    {
        TestSerializeDeserialize(
            [.. Enumerable.Range(1, 100).Select(i => $"item {i}")],
            NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserialize_String_SourceGenerated()
    {
        TestSerializeDeserialize(
            [.. Enumerable.Range(1, 100).Select(i => $"item {i}")],
            NumericsJsonSharedOptions.SourceGenerated);
    }

    [Fact]
    public void SerializeDeserialize_Char_Reflected()
    {
        TestSerializeDeserialize(
            [.. Enumerable.Range(48, 591).Select(i => (char)i)],
            NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserialize_Char_SourceGenerated()
    {
        TestSerializeDeserialize(
            [.. Enumerable.Range(48, 591).Select(i => (char)i)],
            NumericsJsonSharedOptions.SourceGenerated);
    }

    [Fact]
    public void SerializeDeserialize_Boolean_Reflected()
    {
        TestSerializeDeserialize(
            [.. Enumerable.Range(1, 100).Select(i => i % 2 == 0)],
            NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserialize_Boolean_SourceGenerated()
    {
        TestSerializeDeserialize(
            [.. Enumerable.Range(1, 100).Select(i => i % 2 == 0)],
            NumericsJsonSharedOptions.SourceGenerated);
    }

    [Fact]
    public void SerializeDeserialize_NaInt16_Reflected()
    {
        TestSerializeDeserialize<NaInt<short>>([null, -1, -2, 3, 4, 5, 6, 7, 8, null], NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserialize_NaInt16_SourceGenerated()
    {
        TestSerializeDeserialize<NaInt<short>>([null, -1, -2, 3, 4, 5, 6, 7, 8, null], NumericsJsonSharedOptions.SourceGenerated);
    }

    [Fact]
    public void SerializeDeserialize_NaInt32_Reflected()
    {
        TestSerializeDeserialize<NaInt<int>>([null, -1, -2, 3, 4, 5, 6, 7, 8, null], NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserialize_NaInt32_SourceGenerated()
    {
        TestSerializeDeserialize<NaInt<int>>([null, -1, -2, 3, 4, 5, 6, 7, 8, null], NumericsJsonSharedOptions.SourceGenerated);
    }

    [Fact]
    public void SerializeDeserialize_Int16_Categorical_Reflected()
    {
        TestSerializeDeserialize<short>([-1, -2, 3, 4], [-1, -2, 3, 4], NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserialize_Int16_Categorical_SourceGenerated()
    {
        TestSerializeDeserialize<short>([-1, -2, 3, 4], [-1, -2, 3, 4], NumericsJsonSharedOptions.SourceGenerated);
    }

    [Fact]
    public void SerializeDeserialize_Int32_Categorical_Reflected()
    {
        TestSerializeDeserialize([-1, -2, 3, 4], [-1, -2, 3, 4], NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserialize_Int32_Categorical_SourceGenerated()
    {
        TestSerializeDeserialize([-1, -2, 3, 4], [-1, -2, 3, 4], NumericsJsonSharedOptions.SourceGenerated);
    }

    [Fact]
    public void SerializeDeserialize_Int64_Categorical_Reflected()
    {
        TestSerializeDeserialize<long>([-1, -2, 3, 4], [-1, -2, 3, 4], NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserialize_Int64_Categorical_SourceGenerated()
    {
        TestSerializeDeserialize<long>([-1, -2, 3, 4], [-1, -2, 3, 4], NumericsJsonSharedOptions.SourceGenerated);
    }

    [Fact]
    public void SerializeDeserialize_Int64_Categorical_Labelled_Reflected()
    {
        TestSerializeDeserialize(
            [-1, -2, 3, 4],
            [-1, -2, 3, 4],
            [ValueLabel.Create(-1L, "Minus 1"), ValueLabel.Create(-2L, "Minus 2")],
            NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void SerializeDeserialize_Int64_Categorical_Labelled_SourceGenerated()
    {
        TestSerializeDeserialize(
            [-1, -2, 3, 4],
            [-1, -2, 3, 4],
            [ValueLabel.Create(-1L, "Minus 1"), ValueLabel.Create(-2L, "Minus 2")],
            NumericsJsonSharedOptions.SourceGenerated);
    }
}
