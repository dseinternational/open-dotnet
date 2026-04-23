// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Text;
using DSE.Open.Testing.IO;

namespace DSE.Open.Testing.Tests.IO;

public class LazyStreamTests
{
    [Fact]
    public void Ctor_NullInner_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => new LazyStream(null!));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Ctor_NonPositiveDelay_Throws(int delay)
    {
        using var inner = new MemoryStream();
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new LazyStream(inner, delay));
    }

    [Fact]
    public void CapabilityFlags_ReflectInner()
    {
        using var inner = new MemoryStream();
        using var lazy = new LazyStream(inner);
        Assert.Equal(inner.CanRead, lazy.CanRead);
        Assert.Equal(inner.CanSeek, lazy.CanSeek);
        Assert.Equal(inner.CanWrite, lazy.CanWrite);
    }

    [Fact]
    public void Length_ReflectsInner()
    {
        using var inner = new MemoryStream(new byte[42]);
        using var lazy = new LazyStream(inner);
        Assert.Equal(42, lazy.Length);
    }

    [Fact]
    public void Position_GetSet_ReflectsInner()
    {
        using var inner = new MemoryStream(new byte[100]);
        using var lazy = new LazyStream(inner);

        lazy.Position = 10;
        Assert.Equal(10, inner.Position);
        Assert.Equal(10, lazy.Position);
    }

    [Fact]
    public void Read_DelegatesToInner()
    {
        var payload = Encoding.UTF8.GetBytes("hello world");
        using var inner = new MemoryStream(payload);
        using var lazy = new LazyStream(inner);

        var buffer = new byte[payload.Length];
        var read = lazy.Read(buffer, 0, buffer.Length);

        Assert.Equal(payload.Length, read);
        Assert.Equal(payload, buffer);
    }

    [Fact]
    public void Write_DelegatesToInner()
    {
        var payload = Encoding.UTF8.GetBytes("hello");
        using var inner = new MemoryStream();
        using var lazy = new LazyStream(inner);

        lazy.Write(payload, 0, payload.Length);

        Assert.Equal(payload, inner.ToArray());
    }

    [Fact]
    public void Flush_DelegatesToInner()
    {
        var inner = new FlushTrackingStream();
        using var lazy = new LazyStream(inner);
        lazy.Flush();
        Assert.Equal(1, inner.FlushCount);
    }

    [Fact]
    public void Seek_DelegatesToInner()
    {
        using var inner = new MemoryStream(new byte[100]);
        using var lazy = new LazyStream(inner);

        var pos = lazy.Seek(20, SeekOrigin.Begin);
        Assert.Equal(20, pos);
        Assert.Equal(20, inner.Position);
    }

    [Fact]
    public void SetLength_DelegatesToInner()
    {
        using var inner = new MemoryStream();
        using var lazy = new LazyStream(inner);

        lazy.SetLength(50);
        Assert.Equal(50, inner.Length);
    }

    [Fact]
    public async Task ReadAsync_AddsDelayAndReturnsData()
    {
        const int delayMs = 50;
        var payload = Encoding.UTF8.GetBytes("hello");
        using var inner = new MemoryStream(payload);
        await using var lazy = new LazyStream(inner, delayMs);

        var buffer = new byte[payload.Length];
        var sw = Stopwatch.StartNew();
        var total = 0;
        while (total < buffer.Length)
        {
            var read = await lazy.ReadAsync(buffer.AsMemory(total), TestContext.Current.CancellationToken);
            if (read == 0)
            {
                break;
            }
            total += read;
        }
        sw.Stop();

        Assert.Equal(payload.Length, total);
        Assert.Equal(payload, buffer);
        Assert.True(sw.ElapsedMilliseconds >= delayMs - 10,
            $"Expected at least {delayMs}ms delay, got {sw.ElapsedMilliseconds}ms");
    }

    [Fact]
    public async Task WriteAsync_AddsDelayAndWritesData()
    {
        const int delayMs = 50;
        var payload = Encoding.UTF8.GetBytes("hello");
        using var inner = new MemoryStream();
        await using var lazy = new LazyStream(inner, delayMs);

        var sw = Stopwatch.StartNew();
        await lazy.WriteAsync(payload, TestContext.Current.CancellationToken);
        sw.Stop();

        Assert.Equal(payload, inner.ToArray());
        Assert.True(sw.ElapsedMilliseconds >= delayMs - 10);
    }

    [Fact]
    public async Task FlushAsync_AddsDelay()
    {
        const int delayMs = 50;
        using var inner = new MemoryStream();
        await using var lazy = new LazyStream(inner, delayMs);

        var sw = Stopwatch.StartNew();
        await lazy.FlushAsync(TestContext.Current.CancellationToken);
        sw.Stop();

        Assert.True(sw.ElapsedMilliseconds >= delayMs - 10);
    }

    [Fact]
    public async Task CopyToAsync_AddsDelayAndCopiesData()
    {
        const int delayMs = 50;
        var payload = Encoding.UTF8.GetBytes("hello world");
        using var inner = new MemoryStream(payload);
        using var destination = new MemoryStream();
        await using var lazy = new LazyStream(inner, delayMs);

        var sw = Stopwatch.StartNew();
        await lazy.CopyToAsync(destination, TestContext.Current.CancellationToken);
        sw.Stop();

        Assert.Equal(payload, destination.ToArray());
        Assert.True(sw.ElapsedMilliseconds >= delayMs - 10);
    }

    [Fact]
    public async Task ReadAsync_CancelledBeforeDelay_Throws()
    {
        using var inner = new MemoryStream(new byte[] { 1, 2, 3 });
        await using var lazy = new LazyStream(inner, delay: 1000);

        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        var buffer = new byte[3];
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            async () =>
            {
                _ = await lazy.ReadAsync(buffer, cts.Token);
            });
    }

    [Fact]
    public void Dispose_DisposesInner()
    {
        var inner = new DisposeTrackingStream();
        var lazy = new LazyStream(inner);

        lazy.Dispose();

        Assert.True(inner.Disposed);
    }

    [Fact]
    public void DoubleDispose_DoesNotThrow()
    {
        var inner = new MemoryStream();
        var lazy = new LazyStream(inner);
        lazy.Dispose();
        lazy.Dispose();
    }

    private sealed class FlushTrackingStream : MemoryStream
    {
        public int FlushCount { get; private set; }

        public override void Flush()
        {
            FlushCount++;
            base.Flush();
        }
    }

    private sealed class DisposeTrackingStream : MemoryStream
    {
        public bool Disposed { get; private set; }

        protected override void Dispose(bool disposing)
        {
            Disposed = true;
            base.Dispose(disposing);
        }
    }
}
