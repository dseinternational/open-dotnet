// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Globalization;
using DSE.Open.Values;
using Microsoft.AspNetCore.Http;

namespace DSE.Open.Web;

public static class AsciiPathExtensions
{
    public static PathString ToPathString(this AsciiPath path)
        => path.IsEmpty ? PathString.Empty : new($"/{path}");

    public static PathString ToLanguagePrefixedPathString(this AsciiPath path, LanguageTag language)
        => path.IsEmpty ? new($"/{language:L}") : new($"/{language:L}/{path}");
}
