// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Globalization;

namespace DSE.Open.Language.Annotations;

public interface IAnnotator
{
    Task<Document> AnnotateTextAsync(LanguageTag language, string text, CancellationToken cancellationToken = default);
}
