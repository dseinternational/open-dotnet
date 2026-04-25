// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// Behavioural coverage for the lazy name-to-index map on <see cref="DataFrame"/>
/// and the eager <see cref="System.Collections.Frozen.FrozenDictionary{TKey, TValue}"/>
/// on <see cref="ReadOnlyDataFrame"/>. The map is a perf optimization, but it must
/// produce the same results as the original O(N) linear scan it replaced — across
/// every mutation that can change which column owns a given name.
/// </summary>
public class DataFrameNameIndexTests
{
    // ─────────────────────────────────────────────────────────────────────
    // DataFrame
    // ─────────────────────────────────────────────────────────────────────

    [Fact]
    public void DataFrame_LookupByName_ReturnsCorrectColumn()
    {
        var a = Series.Create([1, 2, 3], "a");
        var b = Series.Create([4, 5, 6], "b");
        var c = Series.Create([7, 8, 9], "c");

        var frame = new DataFrame { a, b, c };

        Assert.Same(a, frame["a"]);
        Assert.Same(b, frame["b"]);
        Assert.Same(c, frame["c"]);
    }

    [Fact]
    public void DataFrame_LookupByName_UnknownName_ReturnsNull()
    {
        var frame = new DataFrame
        {
            Series.Create([1, 2, 3], "a"),
        };

        Assert.Null(frame["missing"]);
    }

    [Fact]
    public void DataFrame_LookupByName_NullName_Throws()
    {
        var frame = new DataFrame();
        Assert.Throws<ArgumentNullException>(() => frame[null!]);
    }

    [Fact]
    public void DataFrame_LookupAfterAdd_FindsNewColumn()
    {
        var frame = new DataFrame
        {
            Series.Create([1, 2, 3], "a"),
        };

        // Force the lazy index to build.
        Assert.NotNull(frame["a"]);

        var b = Series.Create([4, 5, 6], "b");
        frame.Add(b);

        Assert.Same(b, frame["b"]);
    }

    [Fact]
    public void DataFrame_LookupAfterInsert_FindsNewColumn_AtCorrectIndex()
    {
        var a = Series.Create([1, 2, 3], "a");
        var c = Series.Create([7, 8, 9], "c");
        var frame = new DataFrame { a, c };

        Assert.NotNull(frame["a"]);

        var b = Series.Create([4, 5, 6], "b");
        frame.Insert(1, b);

        Assert.Same(a, frame["a"]);
        Assert.Same(b, frame["b"]);
        Assert.Same(c, frame["c"]);
        Assert.Equal(0, frame.IndexOf(a));
        Assert.Equal(1, frame.IndexOf(b));
        Assert.Equal(2, frame.IndexOf(c));
    }

    [Fact]
    public void DataFrame_LookupAfterRemove_NoLongerFindsColumn()
    {
        var a = Series.Create([1, 2, 3], "a");
        var b = Series.Create([4, 5, 6], "b");
        var frame = new DataFrame { a, b };

        Assert.NotNull(frame["a"]);
        Assert.True(frame.Remove(a));

        Assert.Null(frame["a"]);
        Assert.Same(b, frame["b"]);
    }

    [Fact]
    public void DataFrame_LookupAfterRemoveAt_NoLongerFindsColumn()
    {
        var a = Series.Create([1, 2, 3], "a");
        var b = Series.Create([4, 5, 6], "b");
        var frame = new DataFrame { a, b };

        Assert.NotNull(frame["a"]);
        frame.RemoveAt(0);

        Assert.Null(frame["a"]);
        Assert.Same(b, frame["b"]);
    }

    [Fact]
    public void DataFrame_LookupAfterClear_FindsNothing()
    {
        var frame = new DataFrame
        {
            Series.Create([1, 2, 3], "a"),
            Series.Create([4, 5, 6], "b"),
        };

        Assert.NotNull(frame["a"]);
        frame.Clear();

        Assert.Null(frame["a"]);
        Assert.Null(frame["b"]);
    }

    [Fact]
    public void DataFrame_LookupAfterIndexerSetByInt_FindsNewColumn()
    {
        var frame = new DataFrame
        {
            Series.Create([1, 2, 3], "a"),
            Series.Create([4, 5, 6], "b"),
        };

        Assert.NotNull(frame["a"]);

        var replacement = Series.Create([7, 8, 9], "renamed");
        frame[0] = replacement;

        Assert.Null(frame["a"]);
        Assert.Same(replacement, frame["renamed"]);
    }

    [Fact]
    public void DataFrame_LookupAfterIndexerSetByName_FindsReplacement()
    {
        var frame = new DataFrame
        {
            Series.Create([1, 2, 3], "a"),
            Series.Create([4, 5, 6], "b"),
        };

        var replacement = Series.Create([7, 8, 9]);
        frame["a"] = replacement;

        Assert.Same(replacement, frame["a"]);
        Assert.Equal("a", replacement.Name);
    }

    [Fact]
    public void DataFrame_DuplicateNames_FirstWins()
    {
        // Matches the previous FirstOrDefault(s => s.Name == name) semantics.
        var first = Series.Create([1, 2, 3], "dup");
        var second = Series.Create([4, 5, 6], "dup");

        var frame = new DataFrame { first, second };

        Assert.Same(first, frame["dup"]);
    }

    [Fact]
    public void DataFrame_DuplicateNames_RemoveFirst_LookupFindsSecond()
    {
        var first = Series.Create([1, 2, 3], "dup");
        var second = Series.Create([4, 5, 6], "dup");

        var frame = new DataFrame { first, second };

        // Prime the index.
        Assert.Same(first, frame["dup"]);

        Assert.True(frame.Remove(first));

        Assert.Same(second, frame["dup"]);
    }

    [Fact]
    public void DataFrame_NameMutatedExternally_LookupByOldName_ReturnsNull()
    {
        // Series.Name is publicly settable, so a caller can rename a column without
        // the frame observing it. The trust-but-verify lookup must not return the
        // wrong column, even on a stale index hit.
        var a = Series.Create([1, 2, 3], "a");
        var frame = new DataFrame { a };

        // Prime the lazy index with the old name.
        Assert.Same(a, frame["a"]);

        a.Name = "renamed";

        Assert.Null(frame["a"]);
    }

    [Fact]
    public void DataFrame_NameMutatedExternally_LookupByNewName_ReturnsColumn()
    {
        // After an external rename, the new name should be findable — the lookup
        // path falls back to a linear scan when the cached entry is stale and
        // rebuilds the index for subsequent lookups.
        var a = Series.Create([1, 2, 3], "a");
        var frame = new DataFrame { a };

        Assert.Same(a, frame["a"]);

        a.Name = "renamed";

        Assert.Same(a, frame["renamed"]);
        // And again, to exercise the rebuilt index.
        Assert.Same(a, frame["renamed"]);
    }

    [Fact]
    public void DataFrame_NullColumnNames_DoNotPolluteIndex()
    {
        // The frame's constructor assigns positional names ("0", "1", …) when the
        // input columns lack names, but the lazy index code path must still cope
        // with null names (e.g., a series whose name was cleared after Add).
        var a = Series.Create([1, 2, 3], "a");
        var frame = new DataFrame { a };

        a.Name = null;

        Assert.Null(frame["a"]);
    }

    [Fact]
    public void DataFrame_LookupBeforeAndAfterMutation_AreConsistent()
    {
        // End-to-end smoke test: a sequence of mutations interleaved with lookups.
        var frame = new DataFrame();
        var a = Series.Create([1, 2, 3], "a");
        var b = Series.Create([4, 5, 6], "b");
        var c = Series.Create([7, 8, 9], "c");

        frame.Add(a);
        Assert.Same(a, frame["a"]);

        frame.Add(b);
        Assert.Same(b, frame["b"]);

        frame.Insert(1, c);
        Assert.Same(a, frame["a"]);
        Assert.Same(b, frame["b"]);
        Assert.Same(c, frame["c"]);

        frame.Remove(a);
        Assert.Null(frame["a"]);
        Assert.Same(b, frame["b"]);
        Assert.Same(c, frame["c"]);
    }

    // ─────────────────────────────────────────────────────────────────────
    // ReadOnlyDataFrame
    // ─────────────────────────────────────────────────────────────────────

    [Fact]
    public void ReadOnlyDataFrame_LookupByName_ReturnsCorrectColumn()
    {
        ReadOnlyDataFrame frame =
        [
            Series.Create([1, 2, 3], "a").AsReadOnly(),
            Series.Create([4, 5, 6], "b").AsReadOnly(),
            Series.Create([7, 8, 9], "c").AsReadOnly(),
        ];

        Assert.Equal("a", frame["a"]?.Name);
        Assert.Equal("b", frame["b"]?.Name);
        Assert.Equal("c", frame["c"]?.Name);
    }

    [Fact]
    public void ReadOnlyDataFrame_LookupByName_UnknownName_ReturnsNull()
    {
        ReadOnlyDataFrame frame =
        [
            Series.Create([1, 2, 3], "a").AsReadOnly(),
        ];

        Assert.Null(frame["missing"]);
    }

    [Fact]
    public void ReadOnlyDataFrame_LookupByName_NullName_Throws()
    {
        ReadOnlyDataFrame frame =
        [
            Series.Create([1, 2, 3], "a").AsReadOnly(),
        ];

        Assert.Throws<ArgumentNullException>(() => frame[null!]);
    }

    [Fact]
    public void ReadOnlyDataFrame_LookupByName_EmptyFrame_ReturnsNull()
    {
        var frame = ReadOnlyDataFrame.Empty;
        Assert.Null(frame["anything"]);
    }

    [Fact]
    public void ReadOnlyDataFrame_DuplicateNames_FirstWins()
    {
        ReadOnlyDataFrame frame =
        [
            Series.Create([1, 2, 3], "dup").AsReadOnly(),
            Series.Create([4, 5, 6], "dup").AsReadOnly(),
        ];

        Assert.Same(frame[0], frame["dup"]);
    }

    [Fact]
    public void ReadOnlyDataFrame_FromMutableFrame_NameLookupsAgree()
    {
        var mutable = new DataFrame
        {
            Series.Create([1, 2, 3], "a"),
            Series.Create([4, 5, 6], "b"),
        };

        var ro = mutable.AsReadOnly();

        Assert.Equal(mutable["a"]!.Name, ro["a"]?.Name);
        Assert.Equal(mutable["b"]!.Name, ro["b"]?.Name);
        Assert.Null(ro["c"]);
    }
}
