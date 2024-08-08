// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Observations;

public sealed class ObservationIdTests
{
    [Fact]
    public void SerializesToNumber()
    {
        var id = ObservationId.FromInt64(667420532491);
        var json = JsonSerializer.Serialize(id);
        Assert.Equal("667420532491", json);
    }
}
