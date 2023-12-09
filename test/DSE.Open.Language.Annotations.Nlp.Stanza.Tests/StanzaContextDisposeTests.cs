// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Interop.Python;

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

public class StanzaContextDisposeTests
{
    [Fact]
    public void CanCreateAndDispose()
    {
#pragma warning disable CA2000 // Dispose objects before losing scope
        var context = new StanzaContext(
            new PythonContext(
                new PythonContextConfiguration()));
#pragma warning restore CA2000 // Dispose objects before losing scope

        context.Dispose();
    }
}
