// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Tests.Storage.ValueConversion;

public class DiagnosticCodeToStringConverterTests
{
    [Fact]
    public void ConvertFrom()
    {
        var converter = DiagnosticCodeToStringConverter.Default;
        var result = (Diagnostics.DiagnosticCode)(converter.ConvertFromProvider("ABCD123456") ?? throw new InvalidOperationException());
        Assert.Equal(Diagnostics.DiagnosticCode.Parse("ABCD123456"), result);
    }

    [Fact]
    public void ConvertTo()
    {
        var converter = DiagnosticCodeToStringConverter.Default;
        var result = converter.ConvertToProvider(Diagnostics.DiagnosticCode.Parse("ABCD123456"))?.ToString();
        Assert.Equal("ABCD123456", result);
    }
}
