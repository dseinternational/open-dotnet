// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using CSnakes.Runtime;
using CSnakes.Runtime.Python;

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

// https://github.com/stanfordnlp/stanza/blob/af3d42b70ef2d82d96f410214f98dd17dd983f51/stanza/pipeline/core.py#L176

public class Pipeline : StanzaObject
{
    internal Pipeline(PyObject pyPipeline, IStanzaService stanza) : base(pyPipeline, stanza)
    {
    }

    public IReadOnlyList<string> GetLoadedProcessorsDescriptions()
    {
        return [.. Stanza.GetLoadedProcessors(InnerObject).Select(p => p.ToString())];
    }

    [RequiresDynamicCode("Calls DSE.Open.Interop.Python.PyObjectExtensions.AsNullable<T>()")]
    public Document ProcessText(string text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        var pyDocument = Stanza.ProcessText(InnerObject, text);
        return new Document(pyDocument, Stanza);
    }
}
