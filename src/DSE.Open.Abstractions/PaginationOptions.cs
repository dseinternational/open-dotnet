// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace DSE.Open;

[StructLayout(LayoutKind.Auto)]
public readonly record struct PaginationOptions
{
    public const int DefaultPageSize = 50;
    public const int DefaultPageNumber = 1;

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

    [JsonPropertyName("page_size")]
    public int PageSize => _pageSize > 0 ? _pageSize : DefaultPageSize;

    [JsonPropertyName("page_number")]
    public int PageNumber => _pageNumber > 0 ? _pageNumber : DefaultPageNumber;

    [JsonIgnore]
    public int SkipCount
    {
        get
        {
            if (PageNumber == 1)
            {
                return 0;
            }

            return PageSize * (PageNumber - 1);
        }
    }

}
