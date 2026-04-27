// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Security;

namespace DSE.Open.EntityFrameworkCore.Tests.Storage.ValueConversion;

public class SecureTokenToStringConverterTests
{
    [Fact]
    public void RoundTrip()
    {
        var converter = SecureTokenToStringConverter.Default;
        var token = SecureToken.New();
        var stored = converter.ConvertToProvider(token)?.ToString();
        Assert.NotNull(stored);
        var roundTripped = (SecureToken)(converter.ConvertFromProvider(stored) ?? throw new InvalidOperationException());
        Assert.Equal(token, roundTripped);
    }
}
