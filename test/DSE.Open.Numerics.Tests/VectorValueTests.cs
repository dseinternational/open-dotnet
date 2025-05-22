// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;

namespace DSE.Open.Numerics;

public partial class VectorValueTests : LoggedTestsBase
{
    public VectorValueTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Create_Float64()
    {
        VectorValue val = -1.358;
        Assert.Equal(-1.358, val.ToFloat64(), 3);
    }
}
