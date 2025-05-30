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

    /// <summary>
    /// Gets the type of device on which the model is loaded, e.g., "cuda" or "cpu"
    /// </summary>
    public string DeviceType => _service.GetDeviceType(InnerObject);

    /// <summary>
    /// Computes a sentence embedding.
    /// </summary>
    /// <param name="sentence"></param>
    /// <returns>
    /// </returns>
    public ReadOnlySpan<float> GetEmbedding(string sentence)
    {
        ArgumentNullException.ThrowIfNull(sentence);

        var result = _service.EncodeSentence(InnerObject, sentence);
        return result.AsReadOnlySpan<float>();
    }

    /// <summary>
    /// Computes sentence embeddings.
    /// </summary>
    /// <param name="sentences"></param>
    /// <param name="prompt"></param>
    /// <returns>
    /// A 2d tensor with shape [sentences.Count, embedding_length].
    /// </returns>
    public ReadOnlyTensorSpan<float> GetEmbeddings(
        IReadOnlyList<string> sentences,
        string? prompt = null)
    {
        ArgumentNullException.ThrowIfNull(sentences);

        if (sentences.Count == 0)
        {
            return new ReadOnlyTensorSpan<float>([], [0, 0], [0, 0]);
        }

        var result = _service.EncodeSentenceCollection(InnerObject, sentences, prompt);
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
