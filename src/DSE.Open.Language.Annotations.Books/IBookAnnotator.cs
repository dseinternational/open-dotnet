// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language.Annotations.Books.Sources;

namespace DSE.Open.Language.Annotations.Books;

/// <summary>
/// Produces an annotated <see cref="Book"/> from a source representation.
/// </summary>
public interface IBookAnnotator
{
    /// <summary>
    /// Annotates the supplied <paramref name="book"/> source and returns the resulting <see cref="Book"/>.
    /// </summary>
    /// <param name="book">The source representation of the book to annotate.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>The annotated <see cref="Book"/>.</returns>
    Task<Book> AnnotateBookAsync(BookSource book, CancellationToken cancellationToken = default);
}
