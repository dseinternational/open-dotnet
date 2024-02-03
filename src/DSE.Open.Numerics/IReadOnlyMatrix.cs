// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public interface IReadOnlyMatrix
{
    int RowCount { get; }

    int ColumnCount { get; }
}
public interface IReadOnlyMatrix<T> : IReadOnlyMatrix
{
    T this[int row, int column] { get; }
}
