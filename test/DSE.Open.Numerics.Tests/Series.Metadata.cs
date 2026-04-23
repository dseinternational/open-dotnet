// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public class SeriesMetadataTests
{
    // -------- Series<T>.WithName --------

    [Fact]
    public void WithName_Returns_NewInstance_With_UpdatedName()
    {
        var series = new Series<int>([1, 2, 3]) { Name = "original" };
        var renamed = series.WithName("updated");

        Assert.Equal("updated", renamed.Name);
        Assert.Equal("original", series.Name);
        Assert.NotSame(series, renamed);
    }

    [Fact]
    public void WithName_Null_Clears_Name()
    {
        var series = new Series<int>([1, 2, 3]) { Name = "original" };
        var renamed = series.WithName(null);

        Assert.Null(renamed.Name);
    }

    [Fact]
    public void WithName_Preserves_Categories_And_ValueLabels_ByReference()
    {
        var categories = new CategorySet<int>([1, 2, 3]);
        var labels = new ValueLabelCollection<int>();
        labels.Add(1, "one");

        var series = new Series<int>([1, 2, 3], name: "x", categories: categories, valueLabels: labels);
        var renamed = series.WithName("y");

        Assert.Same(categories, renamed.Categories);
        Assert.Same(labels, renamed.ValueLabels);
    }

    [Fact]
    public void WithName_Shares_Underlying_Buffer()
    {
        var series = new Series<int>([1, 2, 3]);
        var renamed = series.WithName("x");

        // Mutating one reflects in the other — same underlying vector memory.
        renamed[0] = 99;
        Assert.Equal(99, series[0]);
    }

    // -------- Series<T>.WithCategories --------

    [Fact]
    public void WithCategories_Returns_NewInstance_With_UpdatedCategories()
    {
        var original = new CategorySet<int>([1, 2, 3]);
        var series = new Series<int>([1, 2, 3], categories: original);

        var replacement = new CategorySet<int>([1, 2, 3, 4]);
        var updated = series.WithCategories(replacement);

        Assert.Same(replacement, updated.Categories);
        Assert.Same(original, series.Categories);
    }

    [Fact]
    public void WithCategories_Null_Removes_Categorical_Constraint()
    {
        var categories = new CategorySet<int>([1, 2, 3]);
        var series = new Series<int>([1, 2, 3], categories: categories);

        var uncategorised = series.WithCategories(null);

        Assert.False(uncategorised.IsCategorical);
    }

    [Fact]
    public void WithCategories_Validates_Against_New_Set()
    {
        var series = new Series<int>([1, 2, 3]);
        var tooSmall = new CategorySet<int>([1, 2]); // 3 not allowed

        _ = Assert.Throws<NumericsArgumentException>(() => series.WithCategories(tooSmall));
    }

    [Fact]
    public void WithCategories_Preserves_Name_And_ValueLabels_ByReference()
    {
        var labels = new ValueLabelCollection<int>();
        labels.Add(1, "one");
        var series = new Series<int>([1, 2, 3], name: "x", valueLabels: labels);

        var categories = new CategorySet<int>([1, 2, 3]);
        var updated = series.WithCategories(categories);

        Assert.Equal("x", updated.Name);
        Assert.Same(labels, updated.ValueLabels);
    }

    // -------- Series<T>.WithValueLabels --------

    [Fact]
    public void WithValueLabels_Returns_NewInstance_With_UpdatedLabels()
    {
        var original = new ValueLabelCollection<int>();
        original.Add(1, "one");
        var series = new Series<int>([1, 2, 3], valueLabels: original);

        var replacement = new ValueLabelCollection<int>();
        replacement.Add(1, "uno");
        var updated = series.WithValueLabels(replacement);

        Assert.Same(replacement, updated.ValueLabels);
        Assert.Same(original, series.ValueLabels);
    }

    [Fact]
    public void WithValueLabels_Null_Clears_Labels()
    {
        var labels = new ValueLabelCollection<int>();
        labels.Add(1, "one");
        var series = new Series<int>([1, 2, 3], valueLabels: labels);

        var cleared = series.WithValueLabels(null);

        Assert.False(cleared.HasValueLabels);
    }

    [Fact]
    public void WithValueLabels_Preserves_Name_And_Categories_ByReference()
    {
        var categories = new CategorySet<int>([1, 2, 3]);
        var series = new Series<int>([1, 2, 3], name: "x", categories: categories);

        var labels = new ValueLabelCollection<int>();
        labels.Add(1, "one");
        var updated = series.WithValueLabels(labels);

        Assert.Equal("x", updated.Name);
        Assert.Same(categories, updated.Categories);
    }

    // -------- ReadOnlySeries<T>.WithName --------

    [Fact]
    public void ReadOnly_WithName_Returns_NewInstance_With_UpdatedName()
    {
        var series = new Series<int>([1, 2, 3]) { Name = "original" }.AsReadOnly();
        var renamed = series.WithName("updated");

        Assert.Equal("updated", renamed.Name);
        Assert.Equal("original", series.Name);
        Assert.NotSame(series, renamed);
    }

    [Fact]
    public void ReadOnly_WithName_Preserves_Categories_And_ValueLabels_ByReference()
    {
        var categories = new CategorySet<int>([1, 2, 3]);
        var labels = new ValueLabelCollection<int>();
        labels.Add(1, "one");

        var series = new Series<int>([1, 2, 3], name: "x", categories: categories, valueLabels: labels)
            .AsReadOnly();
        var renamed = series.WithName("y");

        Assert.Same(series.Categories, renamed.Categories);
        Assert.Same(series.ValueLabels, renamed.ValueLabels);
    }

    // -------- ReadOnlySeries<T>.WithCategories --------

    [Fact]
    public void ReadOnly_WithCategories_Returns_NewInstance_With_UpdatedCategories()
    {
        var series = new Series<int>([1, 2, 3]).AsReadOnly();
        var replacement = new CategorySet<int>([1, 2, 3]).AsReadOnly();

        var updated = series.WithCategories(replacement);

        Assert.Same(replacement, updated.Categories);
    }

    [Fact]
    public void ReadOnly_WithCategories_Null_Removes_Categorical_Constraint()
    {
        var categories = new CategorySet<int>([1, 2, 3]).AsReadOnly();
        var series = new ReadOnlySeries<int>([1, 2, 3], categories: categories);

        var uncategorised = series.WithCategories(null);

        Assert.False(uncategorised.IsCategorical);
    }

    // -------- ReadOnlySeries<T>.WithValueLabels --------

    [Fact]
    public void ReadOnly_WithValueLabels_Returns_NewInstance_With_UpdatedLabels()
    {
        var labels = new ValueLabelCollection<int>();
        labels.Add(1, "one");
        var roLabels = labels.AsReadOnly();

        var series = new ReadOnlySeries<int>([1, 2, 3]);
        var updated = series.WithValueLabels(roLabels);

        Assert.Same(roLabels, updated.ValueLabels);
    }
}
