// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public class ValueLabelCollectionTests
{
    [Fact]
    public void Remove_middle_item_preserves_label_lookup_for_shifted_entries()
    {
        // Regression: Remove previously re-inserted the *removed* label into
        // LabelIndexLookup for every shifted entry, corrupting label lookups.
        var collection = new ValueLabelCollection<int>
        {
            { 1, "one" },
            { 2, "two" },
            { 3, "three" },
        };

        Assert.True(collection.Remove((2, "two")));

        Assert.True(collection.TryGetValue("three", out var threeValue));
        Assert.Equal(3, threeValue);

        Assert.True(collection.TryGetValue("one", out var oneValue));
        Assert.Equal(1, oneValue);

        Assert.False(collection.ContainsLabel("two"));
        Assert.True(collection.ContainsLabel("three"));
    }

    [Fact]
    public void Remove_middle_item_preserves_value_lookup_for_shifted_entries()
    {
        var collection = new ValueLabelCollection<int>
        {
            { 1, "one" },
            { 2, "two" },
            { 3, "three" },
        };

        Assert.True(collection.Remove((2, "two")));

        Assert.True(collection.TryGetLabel(3, out var label3));
        Assert.Equal("three", label3);

        Assert.True(collection.TryGetLabel(1, out var label1));
        Assert.Equal("one", label1);

        Assert.False(collection.ContainsValue(2));
    }

    [Fact]
    public void Add_distinct_values_with_colliding_hash_codes_does_not_throw()
    {
        // Regression: lookups were keyed only by hash, so two distinct values
        // with the same GetHashCode() were treated as duplicates.
        var collection = new ValueLabelCollection<CollidingKey>
        {
            { new CollidingKey(1), "one" },
        };

        collection.Add(new CollidingKey(2), "two");

        Assert.Equal(2, collection.Count);
    }

    [Fact]
    public void TryGetLabel_returns_correct_label_when_hash_codes_collide()
    {
        var collection = new ValueLabelCollection<CollidingKey>
        {
            { new CollidingKey(1), "one" },
            { new CollidingKey(2), "two" },
        };

        Assert.True(collection.TryGetLabel(new CollidingKey(1), out var label1));
        Assert.Equal("one", label1);

        Assert.True(collection.TryGetLabel(new CollidingKey(2), out var label2));
        Assert.Equal("two", label2);
    }

    [Fact]
    public void Contains_uses_equality_not_hash_when_hash_codes_collide()
    {
        var collection = new ValueLabelCollection<CollidingKey>
        {
            { new CollidingKey(1), "one" },
        };

        Assert.Contains(new ValueLabel<CollidingKey>(new CollidingKey(1), "one"), collection);
        Assert.DoesNotContain(new ValueLabel<CollidingKey>(new CollidingKey(2), "two"), collection);
    }

    [Fact]
    public void ReadOnly_TryGetLabel_returns_correct_label_when_hash_codes_collide()
    {
        var collection = ReadOnlyValueLabelCollection.Create(
        [
            new ValueLabel<CollidingKey>(new CollidingKey(1), "one"),
            new ValueLabel<CollidingKey>(new CollidingKey(2), "two"),
        ]);

        Assert.True(collection.TryGetLabel(new CollidingKey(1), out var label1));
        Assert.Equal("one", label1);

        Assert.True(collection.TryGetLabel(new CollidingKey(2), out var label2));
        Assert.Equal("two", label2);
    }

    [Fact]
    public void Indexer_set_adds_new_label_and_duplicate_value_throws()
    {
        var collection = new ValueLabelCollection<int>();

        collection[1] = "one";

        Assert.Equal("one", collection[1]);
        _ = Assert.Throws<ArgumentException>(() => collection[1] = "uno");
    }

    [Fact]
    public void ExplicitCopyTo_copies_labels_at_requested_offset()
    {
        var collection = new ValueLabelCollection<int>
        {
            { 1, "one" },
            { 2, "two" },
        };
        var destination = new ValueLabel<int>[3];

        ((ICollection<ValueLabel<int>>)collection).CopyTo(destination, 1);

        Assert.Equal(default, destination[0]);
        Assert.Equal(new ValueLabel<int>(1, "one"), destination[1]);
        Assert.Equal(new ValueLabel<int>(2, "two"), destination[2]);
    }

    [Fact]
    public void ExplicitCopyTo_WithTooSmallDestination_ShouldThrowArgumentException()
    {
        var collection = new ValueLabelCollection<int>
        {
            { 1, "one" },
            { 2, "two" },
        };

        _ = Assert.Throws<ArgumentException>(() => ((ICollection<ValueLabel<int>>)collection).CopyTo(new ValueLabel<int>[1], 0));
    }

    [Fact]
    public void TryGetValue_WithNullLabel_ShouldThrowArgumentNullException()
    {
        var collection = new ValueLabelCollection<int>();

        _ = Assert.Throws<ArgumentNullException>(() => collection.TryGetValue(null!, out _));
    }

    private readonly struct CollidingKey : IEquatable<CollidingKey>
    {
        public CollidingKey(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public bool Equals(CollidingKey other) => Value == other.Value;

        public override bool Equals(object? obj) => obj is CollidingKey other && Equals(other);

        public override int GetHashCode() => 0;

        public static bool operator ==(CollidingKey left, CollidingKey right) => left.Equals(right);

        public static bool operator !=(CollidingKey left, CollidingKey right) => !left.Equals(right);
    }
}
