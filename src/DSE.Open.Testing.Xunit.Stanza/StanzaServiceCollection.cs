// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Xunit;

namespace DSE.Open.Testing.Xunit.Stanza;

[CollectionDefinition("StanzaService")]
public class StanzaServiceCollection : ICollectionFixture<StanzaServiceFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
