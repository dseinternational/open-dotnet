// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.IO;

namespace DSE.Open.Tests.IO;

public class StreamExtensionsTests
{
    [Fact]
    public async Task ReadToEndAsStringAsync_ReadsCorrectly()
    {
        var stream = new MemoryStream(1024);
        var bytes = "Hello, World!"u8.ToArray();
        await stream.WriteAsync(bytes);
        stream.Position = 0;

        var result = await stream.ReadToEndAsStringAsync();

        Assert.Equal("Hello, World!", result);
        Assert.Throws<ObjectDisposedException>(() => stream.Write("Hi!"u8.ToArray()));
    }
}
