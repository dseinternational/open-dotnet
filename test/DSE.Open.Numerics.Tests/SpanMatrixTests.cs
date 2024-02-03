// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public class SpanMatrixTests
{
    [Fact]
    public void Init_Span()
    {
        var m = new SpanMatrix<int>([1, 2, 3, 4, 5, 6, 7, 8], 2, 4);

        Assert.Equal(2, m.RowCount);
        Assert.Equal(4, m.ColumnCount);

        var rowIterator = m.Row(0).GetEnumerator();

        var i = 1;

        while (rowIterator.MoveNext())
        {
            Assert.Equal(i++, rowIterator.Current);
        }
    }

    [Fact]
    public void Init_Array_Jagged()
    {
        var m = new SpanMatrix<int>([[1, 2, 3, 4], [5, 6, 7, 8]]);

        Assert.Equal(2, m.RowCount);
        Assert.Equal(4, m.ColumnCount);
    }

    [Fact]
    public void Row()
    {
        var m = new SpanMatrix<int>([1, 2, 3, 4, 5, 6, 7, 8], 2, 4);

        Assert.Equal(1, m.Row(0)[0]);
        Assert.Equal(2, m.Row(0)[1]);
        Assert.Equal(3, m.Row(0)[2]);
        Assert.Equal(4, m.Row(0)[3]);
        Assert.Equal(5, m.Row(1)[0]);
        Assert.Equal(6, m.Row(1)[1]);
        Assert.Equal(7, m.Row(1)[2]);
        Assert.Equal(8, m.Row(1)[3]);
    }

    [Fact]
    public void Enumerate_RowItems()
    {
        var m = new SpanMatrix<int>([1, 2, 3, 4, 5, 6, 7, 8], 2, 4);

        var val = 1;

        for (var r = 0; r < 2; r++)
        {
            var rowIterator = m.RowItems(r).GetEnumerator();

            while (rowIterator.MoveNext())
            {
                Assert.Equal(val++, rowIterator.Current);
            }
        }
    }

    [Fact]
    public void Enumerate_ColumnItems()
    {
        var m = new SpanMatrix<int>([1, 2, 3, 4, 5, 6, 7, 8], 2, 4);

        for (var c = 0; c < 4; c++)
        {
            var colIterator = m.ColumnItems(c).GetEnumerator();

            var val = c + 1;

            while (colIterator.MoveNext())
            {
                Assert.Equal(val, colIterator.Current);
                val += 4;
            }
        }
    }

    [Fact]
    public void Add_Value()
    {
        var m1 = new SpanMatrix<int>([[1, 2, 3, 4], [5, 6, 7, 8]]);
        var m2 = m1.Add(1);

        for (var r = 0; r < 2; r++)
        {
            for (var c = 0; c < 4; c++)
            {
                Assert.Equal(m1[r, c] + 1, m2[r, c]);
            }
        }
    }
}
