// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Memory;

namespace DSE.Open.Numerics;

public class MatrixTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(42, 4)]
    [InlineData(4, 42)]
    public void Init_Array_Rows_Columns(int rows, int columns)
    {
        var array = new int[rows * columns];

        var m = new Matrix<int>(array, rows, columns);

        Assert.Equal(rows, m.RowCount);
        Assert.Equal(columns, m.ColumnCount);
    }

    [Fact]
    public void Can_init_with_jagged_array()
    {
        var m = new Matrix<int>( [ [1, 2], [3, 4], [5, 6] ]);

        Assert.Equal(3, m.RowCount);
        Assert.Equal(2, m.ColumnCount);
    }

    [Fact]
    public void Can_init_with_span()
    {
        Span<int> s = [1,2,3,4,5,6];
        var m = Matrix.Create(s, 3, 2);

        Assert.Equal(3, m.RowCount);
        Assert.Equal(2, m.ColumnCount);
    }

    [Fact]
    public void Add()
    {
        var m1 = Matrix.Create([1, 2, 3, 4, 5, 6], 3, 2);
        var m2 = Matrix.Create([1, 2, 3, 4, 5, 6], 3, 2);

        var m3 = m1.Add( m2);

        Assert.Equal(3, m3.RowCount);
        Assert.Equal(2, m3.ColumnCount);
        Assert.Equal(2, m3[0, 0]);
        Assert.Equal(4, m3[0, 1]);
        Assert.Equal(6, m3[1, 0]);
        Assert.Equal(8, m3[1, 1]);
        Assert.Equal(10, m3[2, 0]);
        Assert.Equal(12, m3[2, 1]);
    }
}
