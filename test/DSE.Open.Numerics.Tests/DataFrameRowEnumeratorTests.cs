// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// Behavioural and allocation-shape coverage for the struct enumerators on
/// <see cref="DataFrameRow"/>, <see cref="DataFrameRowCollection"/>,
/// <see cref="ReadOnlyDataFrameRow"/> and <see cref="ReadOnlyDataFrameRowCollection"/>.
/// The enumerators replaced compiler-generated <c>yield return</c> state machines;
/// these tests confirm iteration order/values still match and that the public
/// <c>GetEnumerator()</c> overload is the struct (so <c>foreach</c> binds to it
/// via duck typing instead of falling back to <c>IEnumerable&lt;T&gt;</c>).
/// </summary>
public class DataFrameRowEnumeratorTests
{
    private static DataFrame BuildSampleFrame()
    {
        return new DataFrame
        {
            Series.Create([1, 2, 3], "ints"),
            Series.Create([1.5, 2.5, 3.5], "doubles"),
            Series.Create(["a", "b", "c"], "strings"),
        };
    }

    // ─────────────────────────────────────────────────────────────────────
    // DataFrameRowCollection
    // ─────────────────────────────────────────────────────────────────────

    [Fact]
    public void RowCollection_Foreach_VisitsEveryRow_InOrder()
    {
        var frame = BuildSampleFrame();

        var visited = 0;
        foreach (var row in frame.Rows)
        {
            // Reading row[0] confirms each iteration sees the right index, since
            // ints column is 1-based.
            Assert.Equal((VectorValue)(visited + 1), row[0]);
            visited++;
        }

        Assert.Equal(3, visited);
    }

    [Fact]
    public void RowCollection_GetEnumerator_ReturnsStructEnumerator()
    {
        // The whole point of the change is that `foreach (var row in df.Rows)`
        // binds to a struct enumerator. The compiler picks the public
        // GetEnumerator() — confirm that overload returns the named struct type.
        var frame = BuildSampleFrame();
        var enumerator = frame.Rows.GetEnumerator();

        Assert.IsType<DataFrameRowCollection.Enumerator>(enumerator);
    }

    [Fact]
    public void RowCollection_GetEnumerator_OnEmptyFrame_ProducesNoRows()
    {
        var frame = new DataFrame();
        var rows = frame.Rows;

        var enumerator = rows.GetEnumerator();
        Assert.False(enumerator.MoveNext());
    }

    [Fact]
    public void RowCollection_LinqInterop_StillWorks()
    {
        // LINQ (Count, ToList, etc.) goes through IEnumerable<T>, which boxes the
        // struct enumerator. Behaviour-wise it must be identical to the foreach path.
        var frame = BuildSampleFrame();
        var asList = System.Linq.Enumerable.ToList(frame.Rows);
        Assert.Equal(3, asList.Count);
    }

    [Fact]
    public void RowCollection_Enumerator_ResetThrowsNotSupported()
    {
        var frame = BuildSampleFrame();
        // Box deliberately: Reset is on the explicit IEnumerator implementation,
        // and reaching it via the boxed interface is the only way to call it.
        // CA1859 warns about avoiding interface dispatch for perf — not relevant here.
#pragma warning disable CA1859
        IEnumerator<DataFrameRow> enumerator = frame.Rows.GetEnumerator();
#pragma warning restore CA1859

        Assert.True(enumerator.MoveNext());
        Assert.Throws<NotSupportedException>(enumerator.Reset);
    }

    [Fact]
    public void RowCollection_Enumerator_MoveNextAfterEnd_KeepsReturningFalse()
    {
        var frame = BuildSampleFrame();
        var enumerator = frame.Rows.GetEnumerator();

        for (var i = 0; i < frame.Rows.Count; i++)
        {
            Assert.True(enumerator.MoveNext());
        }

        Assert.False(enumerator.MoveNext());
        Assert.False(enumerator.MoveNext());
    }

    // ─────────────────────────────────────────────────────────────────────
    // DataFrameRow
    // ─────────────────────────────────────────────────────────────────────

    [Fact]
    public void DataFrameRow_Foreach_VisitsEveryCell_InColumnOrder()
    {
        var frame = BuildSampleFrame();
        var row = frame.Rows[1];

        var seen = new List<VectorValue>();
        foreach (var cell in row)
        {
            seen.Add(cell);
        }

        Assert.Equal(3, seen.Count);
        Assert.Equal(row[0], seen[0]);
        Assert.Equal(row[1], seen[1]);
        Assert.Equal(row[2], seen[2]);
    }

    [Fact]
    public void DataFrameRow_GetEnumerator_ReturnsStructEnumerator()
    {
        var frame = BuildSampleFrame();
        var enumerator = frame.Rows[0].GetEnumerator();

        Assert.IsType<DataFrameRow.Enumerator>(enumerator);
    }

    // ─────────────────────────────────────────────────────────────────────
    // ReadOnlyDataFrameRowCollection / ReadOnlyDataFrameRow
    // ─────────────────────────────────────────────────────────────────────

    [Fact]
    public void ReadOnlyRowCollection_Foreach_VisitsEveryRow_InOrder()
    {
        var frame = BuildSampleFrame().AsReadOnly();

        var visited = 0;
        foreach (var row in frame.Rows)
        {
            Assert.Equal((VectorValue)(visited + 1), row[0]);
            visited++;
        }

        Assert.Equal(3, visited);
    }

    [Fact]
    public void ReadOnlyRowCollection_GetEnumerator_ReturnsStructEnumerator()
    {
        var frame = BuildSampleFrame().AsReadOnly();
        var enumerator = frame.Rows.GetEnumerator();

        Assert.IsType<ReadOnlyDataFrameRowCollection.Enumerator>(enumerator);
    }

    [Fact]
    public void ReadOnlyRow_GetEnumerator_ReturnsStructEnumerator()
    {
        var frame = BuildSampleFrame().AsReadOnly();
        var enumerator = frame.Rows[0].GetEnumerator();

        Assert.IsType<ReadOnlyDataFrameRow.Enumerator>(enumerator);
    }

    [Fact]
    public void ReadOnlyRow_Foreach_VisitsEveryCell_InColumnOrder()
    {
        var frame = BuildSampleFrame().AsReadOnly();
        var row = frame.Rows[2];

        var seen = new List<VectorValue>();
        foreach (var cell in row)
        {
            seen.Add(cell);
        }

        Assert.Equal(3, seen.Count);
        Assert.Equal(row[0], seen[0]);
        Assert.Equal(row[1], seen[1]);
        Assert.Equal(row[2], seen[2]);
    }

    // ─────────────────────────────────────────────────────────────────────
    // Allocation contract
    // ─────────────────────────────────────────────────────────────────────

    [Fact]
    public void RowCollection_Foreach_DoesNotAllocateOnHeap()
    {
        // The previous yield-return-based implementation allocated a state machine
        // per foreach. Confirm the struct-enumerator path holds the line by
        // measuring allocated bytes around a hot loop. Use a generous tolerance
        // so unrelated GC noise doesn't flake this — the previous implementation
        // would have allocated O(rows × frames) bytes and easily blow this budget.
        var frame = BuildSampleFrame();

        // Warm-up: trip JIT and any one-time allocations (boxed VectorValues etc.).
        long warmSum = 0;
        foreach (var row in frame.Rows)
        {
            warmSum += row.Count;
        }

        Assert.True(warmSum > 0); // keep the warm-up alive

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        var before = GC.GetAllocatedBytesForCurrentThread();

        long sum = 0;
        for (var trial = 0; trial < 100; trial++)
        {
            foreach (var row in frame.Rows)
            {
                sum += row.Count;
            }
        }

        var after = GC.GetAllocatedBytesForCurrentThread();
        var allocated = after - before;

        // Make sum observable to avoid the loop being optimized away.
        Assert.True(sum > 0);

        // Budget: a few hundred bytes, well below the previous yield-state-machine
        // cost (~80 bytes × 100 trials = 8 KB minimum, plus per-row state).
        Assert.True(
            allocated < 2_000,
            $"foreach over DataFrame.Rows allocated {allocated} bytes — expected near zero");
    }
}
