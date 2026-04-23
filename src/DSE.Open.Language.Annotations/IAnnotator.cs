// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Globalization;

namespace DSE.Open.Language.Annotations;

/// <summary>
/// Annotates a piece of text in a given language and returns a
/// <see cref="Document"/> describing its sentences, tokens and words.
/// </summary>
public interface IAnnotator
{
    /// <summary>
    /// Annotates <paramref name="text"/> as <paramref name="language"/>.
    /// </summary>
    /// <param name="language">The language of <paramref name="text"/>.</param>
    /// <param name="text">The text to annotate.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
    Task<Document> AnnotateTextAsync(LanguageTag language, string text, CancellationToken cancellationToken = default);
}
