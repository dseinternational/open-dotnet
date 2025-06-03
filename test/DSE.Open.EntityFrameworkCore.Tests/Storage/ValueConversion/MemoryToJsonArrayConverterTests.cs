// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Tests.Storage.ValueConversion;

public class MemoryToJsonArrayConverterTests
{
    [Fact]
    public void Serializes_Int32()
    {
        var value = new Memory<int>([1, 2, 3, 4, 5, 6, 7, 8]);
        var converter = new MemoryToJsonArrayConverter<int>();
        var serialized = converter.ConvertToProviderExpression.Compile().Invoke(value);
        Assert.Equal("[1,2,3,4,5,6,7,8]", serialized);
    }

    [Fact]
    public void Serializes_Float32()
    {
        var value = new Memory<float>([1.54f, 2.222f, 3.15f, 4.0f, 5, 6, 7, 8]);
        var converter = new MemoryToJsonArrayConverter<float>();
        var serialized = converter.ConvertToProviderExpression.Compile().Invoke(value);
        Assert.Equal("[1.54,2.222,3.15,4,5,6,7,8]", serialized);
    }
}
