// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Testing.IO;

/// <summary>
/// A <see cref="Stream"/> wrapper for testing asynchronous code paths. Asynchronous operations have an artificial delay
/// to force asynchronous completion.
/// </summary>
public sealed class LazyStream : Stream
{
    private readonly Stream _inner;
    private readonly int _delay;

    /// <summary></summary>
    /// <param name="inner">The stream to wrap</param>
    /// <param name="delay">The artificial delay to add to asynchronous operations.</param>
    public LazyStream(Stream inner, int delay = 100)
    {
        ArgumentNullException.ThrowIfNull(inner);
        ArgumentOutOfRangeException.ThrowIfZero(delay);
        ArgumentOutOfRangeException.ThrowIfNegative(delay);

        _inner = inner;
        _delay = delay;
    }

    public override bool CanRead => _inner.CanRead;
    public override bool CanSeek => _inner.CanSeek;
    public override bool CanWrite => _inner.CanWrite;
    public override long Length => _inner.Length;

    public override long Position
    {
        get => _inner.Position;
        set => _inner.Position = value;
    }

    public override void Flush()
    {
        _inner.Flush();
    }

    public override async Task FlushAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(_delay, cancellationToken).ConfigureAwait(false);
        await base.FlushAsync(cancellationToken).ConfigureAwait(false);
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        return _inner.Read(buffer, offset, count);
    }

    public override async ValueTask<int> ReadAsync(
        Memory<byte> buffer,
        CancellationToken cancellationToken = default)
    {
        await Task.Delay(_delay, cancellationToken).ConfigureAwait(false);
        return await base.ReadAsync(buffer, cancellationToken).ConfigureAwait(false);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        return _inner.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
        _inner.SetLength(value);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        _inner.Write(buffer, offset, count);
    }

    public override async ValueTask WriteAsync(
        ReadOnlyMemory<byte> buffer,
        CancellationToken cancellationToken = default)
    {
        await Task.Delay(_delay, cancellationToken).ConfigureAwait(false);
        await base.WriteAsync(buffer, cancellationToken).ConfigureAwait(false);
    }

    public override async Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
    {
        await Task.Delay(_delay, cancellationToken).ConfigureAwait(false);
        await base.CopyToAsync(destination, bufferSize, cancellationToken).ConfigureAwait(false);
    }

    protected override void Dispose(bool disposing)
    {
        _inner.Dispose();
        base.Dispose(disposing);
    }
}
