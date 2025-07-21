// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.IO;

#pragma warning disable CA5394 // Do not use insecure randomness

public class StreamExtensionsTests
{
    [Fact]
    public async Task ReadToEndAsStringAsync_ReadsCorrectly()
    {
        var stream = new MemoryStream(1024);
        var bytes = "Hello, World!"u8.ToArray();
        await stream.WriteAsync(bytes, TestContext.Current.CancellationToken);
        stream.Position = 0;

        var result = await stream.ReadToEndAsStringAsync();

        Assert.Equal("Hello, World!", result);
        _ = Assert.Throws<ObjectDisposedException>(() => stream.Write("Hi!"u8.ToArray()));
    }

    [Fact]
    public async Task ReadToEndAsync_ReadsSequence()
    {
        var stream = new MemoryStream(1024);
        var bytes = new byte[1024];
        Random.Shared.NextBytes(bytes);
        stream.Write(bytes);
        stream.Position = 0;

        var result = await stream.ReadToEndAsync(cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(bytes, result);
    }

    [Fact]
    public void ReadToEnd_ReadsSequence()
    {
        var stream = new MemoryStream(1024);
        var bytes = new byte[1024];
        Random.Shared.NextBytes(bytes);
        stream.Write(bytes);
        stream.Position = 0;

        var result = stream.ReadToEnd();

        Assert.Equal(bytes, result);
    }
}
