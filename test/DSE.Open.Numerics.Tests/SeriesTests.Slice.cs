// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class SeriesTests
{
    // -------- Slice preserves Name (existing behaviour, re-asserted) --------

    [Fact]
    public void Slice_Preserves_Name()
    {
        var series = new Series<int>([1, 2, 3, 4, 5]) { Name = "x" };
        var slice = series.Slice(1, 3);
        Assert.Equal("x", slice.Name);
    }

    // -------- Slice preserves Categories (#264) --------

    [Fact]
    public void Slice_Preserves_Categories_ByReference()
    {
        var categories = new CategorySet<int>([1, 2, 3]);
        var series = new Series<int>([1, 2, 3, 2, 1], name: "x", categories: categories);

        var slice = series.Slice(1, 3);

        Assert.True(slice.IsCategorical);
        Assert.Same(categories, slice.Categories);
    }

    [Fact]
    public void Slice_Preserves_ValueLabels_ByReference()
    {
        var labels = new ValueLabelCollection<int>();
        labels.Add(1, "one");
        labels.Add(2, "two");
        var series = new Series<int>([1, 2, 1, 2, 1], name: "x", valueLabels: labels);

        var slice = series.Slice(1, 3);

        Assert.True(slice.HasValueLabels);
        Assert.Same(labels, slice.ValueLabels);
    }

    [Fact]
    public void Slice_SingleArg_Preserves_Categories_ByReference()
    {
        var categories = new CategorySet<int>([1, 2, 3]);
        var series = new Series<int>([1, 2, 3], name: "x", categories: categories);

        var slice = series.Slice(1);

        Assert.Same(categories, slice.Categories);
    }

    // -------- Slice(..., copy: true) isolates metadata --------

    [Fact]
    public void Slice_WithCopyTrue_Copies_Categories()
    {
        var categories = new CategorySet<int>([1, 2, 3]);
        var series = new Series<int>([1, 2, 3, 2, 1], name: "x", categories: categories);

        var slice = series.Slice(1, 3, copy: true);

        Assert.True(slice.IsCategorical);
        Assert.NotSame(categories, slice.Categories);
        // Contents match at the moment of copy.
        Assert.True(slice.Categories.SetEquals(categories));
    }

    [Fact]
    public void Slice_WithCopyTrue_Copies_ValueLabels()
    {
        var labels = new ValueLabelCollection<int>();
        labels.Add(1, "one");
        var series = new Series<int>([1, 1, 1], name: "x", valueLabels: labels);

        var slice = series.Slice(0, 2, copy: true);

        Assert.True(slice.HasValueLabels);
        Assert.NotSame(labels, slice.ValueLabels);
        Assert.Equal("one", slice.ValueLabels[1]);
    }

    [Fact]
    public void Slice_WithCopyTrue_ParentCategoryMutation_DoesNotAffectSlice()
    {
        var categories = new CategorySet<int>([1, 2, 3]);
        var series = new Series<int>([1, 2, 3], name: "x", categories: categories);

        var slice = series.Slice(0, 2, copy: true);
        _ = categories.Add(99); // mutate the parent's set

        Assert.DoesNotContain(99, (ISet<int>)slice.Categories);
        Assert.Contains(99, (ISet<int>)series.Categories);
    }

    [Fact]
    public void Slice_WithCopyFalse_ParentCategoryMutation_IsVisible()
    {
        // Documents the share-by-reference behaviour (the #252 caveat in action).
        var categories = new CategorySet<int>([1, 2, 3]);
        var series = new Series<int>([1, 2, 3], name: "x", categories: categories);

        var slice = series.Slice(0, 2);
        _ = categories.Add(99);

        Assert.Contains(99, (ISet<int>)slice.Categories);
        Assert.Same(categories, slice.Categories);
    }

    [Fact]
    public void Slice_WithCopyTrue_NoCategories_ReturnsSliceWithNullCategories()
    {
        var series = new Series<int>([1, 2, 3]) { Name = "x" };

        var slice = series.Slice(0, 2, copy: true);

        Assert.False(slice.IsCategorical);
    }

    // -------- ReadOnlySeries<T>.Slice --------

    [Fact]
    public void ReadOnlySlice_Preserves_Categories_ByReference()
    {
        var categories = new CategorySet<int>([1, 2, 3]);
        var roCategories = categories.AsReadOnly();
        var series = new ReadOnlySeries<int>([1, 2, 3, 2, 1], name: "x", categories: roCategories);

        var slice = series.Slice(1, 3);

        Assert.True(slice.IsCategorical);
        Assert.Same(roCategories, slice.Categories);
    }

    [Fact]
    public void ReadOnlySlice_WithCopyTrue_Copies_Categories()
    {
        var roCategories = new CategorySet<int>([1, 2, 3]).AsReadOnly();
        var series = new ReadOnlySeries<int>([1, 2, 3, 2, 1], name: "x", categories: roCategories);

        var slice = series.Slice(1, 3, copy: true);

        Assert.True(slice.IsCategorical);
        Assert.NotSame(roCategories, slice.Categories);
    }
}
