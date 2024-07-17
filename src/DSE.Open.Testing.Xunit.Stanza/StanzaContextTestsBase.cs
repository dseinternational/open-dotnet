// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language.Annotations.Nlp.Stanza;
using Xunit.Abstractions;

namespace DSE.Open.Testing.Xunit.Stanza;

public abstract class StanzaContextTestsBase : LoggedTestsBase
{
    protected StanzaContextTestsBase(StanzaContextFixture fixture, ITestOutputHelper output) : base(output)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        StanzaContext = fixture.StanzaContext;
    }

    public StanzaContext StanzaContext { get; }
}
