// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language.Annotations.Nlp.Stanza;

namespace DSE.Open.Testing.Xunit.Stanza;

public sealed class StanzaContextFixture : IDisposable
{
    public StanzaContextFixture()
    {
        PythonContext = new PythonContext(new PythonContextConfiguration());

        StanzaContext = new StanzaContext(PythonContext);

        StanzaContext.Initialize();

        // assuming pre-downloaded models
    }

    public PythonContext PythonContext { get; }

    public StanzaContext StanzaContext { get; }

    public void Dispose()
    {
        StanzaContext.Dispose();
        PythonContext.Dispose();
    }
}
