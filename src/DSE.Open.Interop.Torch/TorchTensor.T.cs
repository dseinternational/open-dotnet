// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using DSE.Open.Numerics;
using TorchSharp;

namespace DSE.Open.Interop.Torch;

public abstract class TorchTensor<T> : IDisposable
    where T : unmanaged, INumber<T>
{
    private readonly torch.Tensor _tensor;
    private bool _disposed;

    protected internal TorchTensor(torch.Tensor tensor)
    {
        Debug.Assert(TorchTensor.IsValidElementType(typeof(T)));
        _tensor = tensor;
    }

    internal torch.Tensor Tensor => _tensor;

    public uint Rank => (uint)_tensor.dim();

    public ReadOnlySpan<uint> Shape => MemoryMarshal.Cast<long, uint>(_tensor.shape);

    protected abstract TorchTensor<T> CreateNew(torch.Tensor tensor);

    protected abstract torch.Tensor CreateNewNative(TensorSpan<T> tensor);

    public abstract TorchTensor<T> Add(T scalar);

    public TorchTensor<T> Add(TensorSpan<T> other)
    {
        return CreateNew(torch.add(Tensor, CreateNewNative(other)));
    }

    public TorchTensor<T> Add(TorchTensor<T> other)
    {
        ArgumentNullException.ThrowIfNull(other);
        return CreateNew(torch.add(Tensor, other.Tensor));
    }

    public T[] ToArray()
    {
        return [.. _tensor.data<T>()];
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _tensor.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
