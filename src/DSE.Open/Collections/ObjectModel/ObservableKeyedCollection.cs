// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DSE.Open.Collections.ObjectModel;

// Adapted from https://github.com/dotnet/corefx/blob/master/src/System.ObjectModel/src/System/Collections/ObjectModel/KeyedCollection.cs

[DebuggerDisplay("Count = {Count}")]
public abstract class ObservableKeyedCollection<TKey, TItem> : ObservableCollection<TItem>,
    IObservableKeyedCollection<TKey, TItem> where TKey : notnull
{
    private readonly Dictionary<TKey, TItem> _dictionary;

    protected ObservableKeyedCollection() : this(null)
    {
    }

    protected ObservableKeyedCollection(IEqualityComparer<TKey>? comparer)
    {
        comparer ??= EqualityComparer<TKey>.Default;
        _dictionary = new Dictionary<TKey, TItem>();
        Comparer = comparer;
    }

    public IEqualityComparer<TKey> Comparer { get; }

    protected IDictionary<TKey, TItem> Dictionary => _dictionary;

    public TItem this[TKey key] => key is null ? throw new ArgumentNullException(nameof(key)) : _dictionary[key];

    /// <summary>Enables the use of foreach internally without allocations using
    ///     <see cref="List{T}" />'s struct enumerator.</summary>
    private new List<TItem> Items
    {
        get
        {
            Debug.Assert(base.Items is List<TItem>);

            return (List<TItem>)base.Items;
        }
    }

    private void AddKey(TKey key, TItem item) => _dictionary.Add(key, item);

    protected void ChangeItemKey(TItem item, TKey newKey)
    {
        // Check if the item exists in the collection
        if (!ContainsItem(item))
        {
            throw new ArgumentException(null, nameof(item));
        }

        var oldKey = GetKeyForItem(item);

        if (Comparer.Equals(oldKey, newKey))
        {
            return;
        }

        AddKey(newKey, item);
        RemoveKey(oldKey);
    }

    protected override void ClearItems()
    {
        base.ClearItems();

        _dictionary.Clear();
    }

    public bool Contains(TKey key) => key is null ? throw new ArgumentNullException(nameof(key)) : _dictionary.ContainsKey(key);

    private bool ContainsItem(TItem item)
    {
        var key = GetKeyForItem(item);
        var exist = _dictionary.TryGetValue(key, out var itemInDict);
        return exist && EqualityComparer<TItem>.Default.Equals(itemInDict, item);
    }

    protected abstract TKey GetKeyForItem(TItem item);

    protected override void InsertItem(int index, TItem item)
    {
        var key = GetKeyForItem(item);
        AddKey(key, item);
        base.InsertItem(index, item);
    }

    public bool Remove(TKey key)
    {
        return key is null
            ? throw new ArgumentNullException(nameof(key))
            : _dictionary.TryGetValue(key, out var item) && Remove(item);
    }

    protected override void RemoveItem(int index)
    {
        var key = GetKeyForItem(Items[index]);
        RemoveKey(key);
        base.RemoveItem(index);
    }

    private void RemoveKey(TKey key) => _ = _dictionary.Remove(key);

    protected override void SetItem(int index, TItem item)
    {
        var newKey = GetKeyForItem(item);
        var oldKey = GetKeyForItem(Items[index]);

        if (Comparer.Equals(oldKey, newKey))
        {
            _dictionary[newKey] = item;
        }
        else
        {
            AddKey(newKey, item);
            RemoveKey(oldKey);
        }

        base.SetItem(index, item);
    }
}
