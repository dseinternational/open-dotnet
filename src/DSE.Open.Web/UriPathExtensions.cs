// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Globalization;
using DSE.Open.Values;
using Microsoft.AspNetCore.Http;

namespace DSE.Open.Web;

public static class UriPathExtensions
{
    [Obsolete("Use UriAsciiPath")]
    public static PathString ToPathString(this AsciiPath path)
        => path.IsEmpty ? PathString.Empty : new($"/{path}");

    [Obsolete("Use UriAsciiPath")]
    public static PathString ToLanguagePrefixedPathString(this AsciiPath path, LanguageTag language)
        => path.IsEmpty ? new($"/{language:L}") : new($"/{language:L}/{path}");

    public static PathString ToPathString(this UriPath path)
        => path.IsEmpty ? PathString.Empty : new($"/{path}");

    public static PathString ToLanguagePrefixedPathString(this UriPath path, LanguageTag language)
        => path.IsEmpty ? new($"/{language:L}") : new($"/{language:L}/{path}");

    public static PathString ToPathString(this UriAsciiPath path)
        => path.IsEmpty ? PathString.Empty : new($"/{path}");

    public static PathString ToLanguagePrefixedPathString(this UriAsciiPath path, LanguageTag language)
        => path.IsEmpty ? new($"/{language:L}") : new($"/{language:L}/{path}");
}
