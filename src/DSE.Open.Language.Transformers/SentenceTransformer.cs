// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics.Tensors;
using CSnakes.Runtime;
using CSnakes.Runtime.Python;
using DSE.Open.Interop.Python;

namespace DSE.Open.Language.Transformers;

#pragma warning disable SYSLIB5001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

public class SentenceTransformer : PyObjectWrapper
{
    private readonly ISentenceTransformersService _service;

    private SentenceTransformer(PyObject pyObject, ISentenceTransformersService service) : base(pyObject)
    {
        _service = service;
    }

    public ReadOnlyTensorSpan<float> Encode(
        IReadOnlyList<string> sentences,
        string? prompt = null)
    {
        var result = _service.Encode(InnerObject, sentences, prompt);
        return result.AsReadOnlyTensorSpan<float>();
    }

    public static SentenceTransformer Create(
        IPythonEnvironment pythonEnvironment,
        string modelName,
        bool trustExternalCode = false)
    {
        ArgumentNullException.ThrowIfNull(pythonEnvironment);
        ArgumentException.ThrowIfNullOrWhiteSpace(modelName);

        var service = pythonEnvironment.SentenceTransformersService();
        var transformer = service.GetSentenceTransformer(modelName, trustRemoteCode: trustExternalCode);
        return new SentenceTransformer(transformer, service);
    }
}
