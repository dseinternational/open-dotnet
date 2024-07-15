// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Units;

public class UnitOfLengthTests
{
    [Fact]
    public void TryParseMillimetre()
    {
        Assert.True(UnitOfLength.TryParse("mm", out var unit));
        Assert.Equal(UnitOfLength.Millimetre, unit);
    }

    [Fact]
    public void TryParseCentimetre()
    {
        Assert.True(UnitOfLength.TryParse("cm", out var unit));
        Assert.Equal(UnitOfLength.Centimetre, unit);
    }

    [Fact]
    public void TryParseMetre()
    {
        Assert.True(UnitOfLength.TryParse("m", out var unit));
        Assert.Equal(UnitOfLength.Metre, unit);
    }

    [Fact]
    public void TryParseKilometre()
    {
        Assert.True(UnitOfLength.TryParse("km", out var unit));
        Assert.Equal(UnitOfLength.Kilometre, unit);
    }
}
