// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public interface IMatrix : IReadOnlyMatrix
{
}

public interface IMatrix<T> : IMatrix
{
    T this[int row, int column] { get; set; }
}
