// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Localization.Resources;

public interface ILocalizedResourceProvider
{
    CultureInfo LookupCulture { get; }

    string GetString(string name, CultureInfo? cultureInfo = null);

    Stream GetStream(string name, CultureInfo? cultureInfo = null);
}
