// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit.Stanza;

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

public class StanzaContextDisposeTests
{
    [Fact]
    public void CanCreateAndDispose()
    {
        var context = new StanzaContext(TestingPythonContext.Instance);
        context.Dispose();
    }
}
