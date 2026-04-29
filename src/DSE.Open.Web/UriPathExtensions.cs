// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Globalization;
using DSE.Open.Values;
using Microsoft.AspNetCore.Http;

namespace DSE.Open.Web;

/// <summary>
/// Provides extension methods for converting URI path values to ASP.NET Core <see cref="PathString"/> instances.
/// </summary>
public static class UriPathExtensions
{
#pragma warning disable CS0618 // Type or member is obsolete — UriPath is being phased out in favour of UriSlug
    /// <summary>
    /// Returns a <see cref="PathString"/> representing the specified <see cref="UriPath"/> prefixed with a forward slash,
    /// or <see cref="PathString.Empty"/> if <paramref name="path"/> is empty.
    /// </summary>
    public static PathString ToPathString(this UriPath path)
    {
        return path.IsEmpty ? PathString.Empty : new($"/{path}");
    }

    /// <summary>
    /// Returns a <see cref="PathString"/> consisting of the specified <paramref name="language"/> tag followed by
    /// the specified <see cref="UriPath"/>, each prefixed with a forward slash.
    /// </summary>
    public static PathString ToLanguagePrefixedPathString(this UriPath path, LanguageTag language)
    {
        return path.IsEmpty ? new($"/{language:L}") : new PathString($"/{language:L}/{path}");
    }
#pragma warning restore CS0618 // Type or member is obsolete

    /// <summary>
    /// Returns a <see cref="PathString"/> representing the specified <see cref="UriSlug"/> prefixed with a forward slash,
    /// or <see cref="PathString.Empty"/> if <paramref name="path"/> is empty.
    /// </summary>
    public static PathString ToPathString(this UriSlug path)
    {
        return path.IsEmpty ? PathString.Empty : new($"/{path}");
    }

    /// <summary>
    /// Returns a <see cref="PathString"/> consisting of the specified <paramref name="language"/> tag followed by
    /// the specified <see cref="UriSlug"/>, each prefixed with a forward slash.
    /// </summary>
    public static PathString ToLanguagePrefixedPathString(this UriSlug path, LanguageTag language)
    {
        return path.IsEmpty ? new($"/{language:L}") : new PathString($"/{language:L}/{path}");
    }

    /// <summary>
    /// Returns a <see cref="PathString"/> representing the specified <see cref="UriAsciiPath"/> prefixed with a forward slash,
    /// or <see cref="PathString.Empty"/> if <paramref name="path"/> is empty.
    /// </summary>
    public static PathString ToPathString(this UriAsciiPath path)
    {
        return path.IsEmpty ? PathString.Empty : new($"/{path}");
    }

    /// <summary>
    /// Returns a <see cref="PathString"/> consisting of the specified <paramref name="language"/> tag followed by
    /// the specified <see cref="UriAsciiPath"/>, each prefixed with a forward slash.
    /// </summary>
    public static PathString ToLanguagePrefixedPathString(this UriAsciiPath path, LanguageTag language)
    {
        return path.IsEmpty ? new($"/{language:L}") : new PathString($"/{language:L}/{path}");
    }
}
