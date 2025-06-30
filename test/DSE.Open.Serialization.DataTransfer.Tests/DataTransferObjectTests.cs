// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Text.Json;

namespace DSE.Open.Serialization.DataTransfer.Tests;

public class DataTransferObjectTests
{
    [Fact]
    [Obsolete("Obsolete")]
    public void Serialize()
    {
        var dto = new DataTransferObjectFake
        {
            TimeSpan = TimeSpan.FromDays(1),
        };

        var json = JsonSerializer.Serialize(dto, JsonSharedOptions.RelaxedJsonEscaping);

        Assert.NotNull(json);
    }
}

[Obsolete("Obsolete")]
public record DataTransferObjectFake : DataTransferObject
{
    public TimeSpan TimeSpan { get; init; }
}
