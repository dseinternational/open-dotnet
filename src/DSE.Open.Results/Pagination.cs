// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace DSE.Open.Results;

public readonly record struct Pagination
{
    public static readonly Pagination None;

    [JsonConstructor]
    public Pagination(int totalItems, int pageSize, int currentPage)
    {
        Guard.IsGreaterThanOrEqualTo(totalItems, 0);
        Guard.IsGreaterThanOrEqualTo(pageSize, 1);

        if (currentPage < 1 || (totalItems > 0 && currentPage > (int)Math.Ceiling((double)totalItems / pageSize)))
        {
            throw new ArgumentOutOfRangeException(nameof(currentPage));
        }

        TotalItems = totalItems;
        PageSize = pageSize;
        CurrentPage = currentPage;
    }

    /// <summary>
    /// Gets the total number of items in the result set. May be zero (0) if no items in the result set.
    /// </summary>
    [JsonPropertyName("total_items")]
    public int TotalItems { get; }

    /// <summary>
    /// Gets the number of items requested in each page of the result set.
    /// </summary>
    [JsonPropertyName("page_size")]
    public int PageSize { get; }

    /// <summary>
    /// Gets the current page of the result set. Will be at least one (1).
    /// </summary>
    [JsonPropertyName("current_page")]
    public int CurrentPage { get; }

    /// <summary>
    /// Gets the total number of pages to cover all the items available in the result set. Will be zero
    /// if there are no items in the result set.
    /// </summary>
    [JsonPropertyName("total_pages")]
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

    /// <summary>
    /// Gets the next page number, or null if no page number is available.
    /// </summary>
    [JsonPropertyName("next_page")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? NextPage => TotalPages > CurrentPage ? CurrentPage + 1 : null;

    /// <summary>
    /// Gets the next page number, or null if no page number is available.
    /// </summary>
    [JsonPropertyName("previous_page")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? PreviousPage => CurrentPage > 1 ? CurrentPage - 1 : null;

    /// <summary>
    /// Gets the count of items in the current page.
    /// </summary>
    [JsonPropertyName("item_count")]
    public int ItemCount => TotalItems <= PageSize
        ? TotalItems
        : CurrentPage < TotalPages
            ? PageSize
            : TotalItems % PageSize;

    /// <summary>
    /// Gets a human-readable description.
    /// </summary>
    /// <returns></returns>
#pragma warning disable CA1024 // Use properties where appropriate
    public string GetDescription()
#pragma warning restore CA1024 // Use properties where appropriate
    {
        if (ItemCount == 0)
        {
            return "No items";
        }

        var startItem = ((CurrentPage - 1) * PageSize) + 1;
        var endItem = startItem + ItemCount - 1;
        return $"{startItem}-{endItem} of {TotalItems}";
    }
}
