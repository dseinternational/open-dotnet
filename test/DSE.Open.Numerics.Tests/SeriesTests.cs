// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Numerics.Serialization;
using DSE.Open.Testing.Xunit;

namespace DSE.Open.Numerics;

public partial class SeriesTests : LoggedTestsBase
{
    public SeriesTests(ITestOutputHelper output) : base(output)
    {
    }

    private void TestSerializeDeserialize<T>(T[] elements, JsonSerializerOptions serializerOptions)
        where T : notnull, IEquatable<T>
    {
        var vector = Series.Create("test", [.. elements]);
        var json = JsonSerializer.Serialize(vector, serializerOptions);
        Assert.NotNull(json);

        Output.WriteLine(json);

        var deserialized = JsonSerializer.Deserialize<Series<T>>(json, serializerOptions);
        Assert.NotNull(deserialized);
        Assert.Equivalent(vector, deserialized);
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
}
