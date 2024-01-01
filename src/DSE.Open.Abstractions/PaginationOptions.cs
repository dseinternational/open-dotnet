// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace DSE.Open;

/// <summary>
/// Represents pagination options, usually used in a request.
/// </summary>
[StructLayout(LayoutKind.Auto)]
public readonly record struct PaginationOptions
{
    /// <summary>
    /// The default page size, used when no page size is specified.
    /// </summary>
    public const int DefaultPageSize = 50;

    /// <summary>
    /// The default page number, used when no page number is specified.
    /// </summary>
    public const int DefaultPageNumber = 1;

    /// <summary>
    /// The default pagination options, with a page size of <see cref="DefaultPageSize"/> and a page number of <see cref="DefaultPageNumber"/>.
    /// </summary>
    public static readonly PaginationOptions Default;

    private readonly int _pageSize;
    private readonly int _pageNumber;

    [JsonConstructor]
    public PaginationOptions(int pageSize, int pageNumber = DefaultPageNumber)
    {
        Guard.IsGreaterThan(pageSize, 0);
        Guard.IsGreaterThan(pageNumber, 0);

        _pageSize = pageSize;
        _pageNumber = pageNumber;
    }

    /// <summary>
    /// The number of items in a single page.
    /// </summary>
    [JsonPropertyName("page_size")]
    public int PageSize => _pageSize > 0 ? _pageSize : DefaultPageSize;

    /// <summary>
    /// The one-based page number.
    /// </summary>
    [JsonPropertyName("page_number")]
    public int PageNumber => _pageNumber > 0 ? _pageNumber : DefaultPageNumber;

    /// <summary>
    /// The number of items to skip to get to the current page, based on the page size and page number.
    /// </summary>
    [JsonIgnore]
    public int SkipCount => PageSize * (PageNumber - 1);
}
