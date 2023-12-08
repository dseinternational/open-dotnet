// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language.Annotations.Nlp.Stanza;

namespace DSE.Open.Language.Readability;

[CollectionDefinition(nameof(StanzaContextCollection))]
public class StanzaContextCollection : ICollectionFixture<StanzaContextFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}