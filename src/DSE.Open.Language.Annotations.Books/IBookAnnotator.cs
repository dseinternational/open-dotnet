// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language.Annotations.Books.Sources;

namespace DSE.Open.Language.Annotations.Books;

public interface IBookAnnotator
{
    Task<Book> AnnotateTextAsync(BookSource book, CancellationToken cancellationToken = default);
}
