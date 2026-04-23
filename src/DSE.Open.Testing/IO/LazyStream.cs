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

    /// <summary>
    /// Initialises a new <see cref="LazyStream"/> that wraps <paramref name="inner"/> and
    /// delays each asynchronous operation by <paramref name="delay"/> milliseconds.
    /// </summary>
    /// <param name="inner">The stream to wrap.</param>
    /// <param name="delay">The artificial delay, in milliseconds, to add to asynchronous operations.
    /// Must be positive.</param>
    /// <exception cref="ArgumentNullException"><paramref name="inner"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="delay"/> is zero or negative.</exception>
    public LazyStream(Stream inner, int delay = 100)
    {
        ArgumentNullException.ThrowIfNull(inner);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(delay);

        _inner = inner;
        _delay = delay;
    }

    /// <inheritdoc />
    public override bool CanRead => _inner.CanRead;

    /// <inheritdoc />
    public override bool CanSeek => _inner.CanSeek;

    /// <inheritdoc />
    public override bool CanWrite => _inner.CanWrite;

    /// <inheritdoc />
    public override long Length => _inner.Length;

    /// <inheritdoc />
    public override long Position
    {
        get => _inner.Position;
        set => _inner.Position = value;
    }

    /// <inheritdoc />
    public override void Flush()
    {
        _inner.Flush();
    }

    /// <inheritdoc />
    public override async Task FlushAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(_delay, cancellationToken).ConfigureAwait(false);
        await base.FlushAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public override int Read(byte[] buffer, int offset, int count)
    {
        return _inner.Read(buffer, offset, count);
    }

    /// <inheritdoc />
    public override async ValueTask<int> ReadAsync(
        Memory<byte> buffer,
        CancellationToken cancellationToken = default)
    {
        await Task.Delay(_delay, cancellationToken).ConfigureAwait(false);
        return await base.ReadAsync(buffer, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public override long Seek(long offset, SeekOrigin origin)
    {
        return _inner.Seek(offset, origin);
    }

    /// <inheritdoc />
    public override void SetLength(long value)
    {
        _inner.SetLength(value);
    }

    /// <inheritdoc />
    public override void Write(byte[] buffer, int offset, int count)
    {
        _inner.Write(buffer, offset, count);
    }

    /// <inheritdoc />
    public override async ValueTask WriteAsync(
        ReadOnlyMemory<byte> buffer,
        CancellationToken cancellationToken = default)
    {
        await Task.Delay(_delay, cancellationToken).ConfigureAwait(false);
        await base.WriteAsync(buffer, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public override async Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
    {
        await Task.Delay(_delay, cancellationToken).ConfigureAwait(false);
        await base.CopyToAsync(destination, bufferSize, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _inner.Dispose();
        }

        base.Dispose(disposing);
    }
}
