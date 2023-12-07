// Copyright (c) Down Syndrome Education Enterprises CIC. All Rights Reserved.
// Information contained herein is PROPRIETARY AND CONFIDENTIAL.

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

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

[CollectionDefinition(nameof(StanzaContextCollection))]
public class StanzaContextCollection : ICollectionFixture<StanzaContextFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
