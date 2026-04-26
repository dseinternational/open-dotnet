// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Globalization;
using DSE.Open.Values;
using Microsoft.AspNetCore.Http;

namespace DSE.Open.Web;

public static class UriPathExtensions
{
#pragma warning disable CS0618 // Type or member is obsolete — UriPath is being phased out in favour of UriSlug
    public static PathString ToPathString(this UriPath path)
    {
        return path.IsEmpty ? PathString.Empty : new($"/{path}");
    }

    public static PathString ToLanguagePrefixedPathString(this UriPath path, LanguageTag language)
    {
        return path.IsEmpty ? new($"/{language:L}") : new PathString($"/{language:L}/{path}");
    }
#pragma warning restore CS0618 // Type or member is obsolete

    public static PathString ToPathString(this UriSlug path)
    {
        return path.IsEmpty ? PathString.Empty : new($"/{path}");
    }

    public static PathString ToLanguagePrefixedPathString(this UriSlug path, LanguageTag language)
    {
        return path.IsEmpty ? new($"/{language:L}") : new PathString($"/{language:L}/{path}");
    }

    public static PathString ToPathString(this UriAsciiPath path)
    {
        return path.IsEmpty ? PathString.Empty : new($"/{path}");
    }

    public static PathString ToLanguagePrefixedPathString(this UriAsciiPath path, LanguageTag language)
    {
        return path.IsEmpty ? new($"/{language:L}") : new PathString($"/{language:L}/{path}");
    }
}
