// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Testing.Xunit;

namespace DSE.Open.Numerics;

public partial class SeriesTests : LoggedTestsBase
{
    public SeriesTests(ITestOutputHelper output) : base(output)
    {
    }

    // TODO

    [Fact]
    public void Serialize()
    {
        var series = Series.Create("test", [1, 2, 3, 4, 5, 6, 7, 8, 9]);
        var json = JsonSerializer.Serialize(series);

        Assert.NotNull(json);

        Output.WriteLine(json);
    }
}
