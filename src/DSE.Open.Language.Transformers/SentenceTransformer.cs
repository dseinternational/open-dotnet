// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics.Tensors;
using CSnakes.Runtime;
using CSnakes.Runtime.Python;
using DSE.Open.Interop.Python;

namespace DSE.Open.Language.Transformers;


/// <summary>
/// A managed wrapper over a Python sentence-transformers model that produces sentence embeddings.
/// </summary>
public class SentenceTransformer : PyObjectWrapper
{
    private readonly ISentenceTransformersService _service;

    private SentenceTransformer(PyObject pyObject, string modelName, ISentenceTransformersService service) : base(pyObject)
    {
        ModelName = modelName;
        _service = service;
    }

    /// <summary>
    /// Gets the type of device on which the model is loaded, e.g., "cuda" or "cpu"
    /// </summary>
    public string DeviceType => _service.GetDeviceType(InnerObject);

    /// <summary>
    /// Gets the name (or path) of the loaded sentence-transformers model.
    /// </summary>
    public string ModelName { get; }

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
            return new ReadOnlyTensorSpan<float>();
        }

        for (var i = 0; i < sentences.Count; i++)
        {
            if (sentences[i] is null)
            {
                throw new ArgumentException("Sentence entries cannot be null.", nameof(sentences));
            }
        }

        var result = _service.EncodeSentenceCollection(InnerObject, sentences, prompt);

        // todo: avoid copy - this is a workaround an issue in CSnakes
        var encodingDimensions = (int)_service.GetSentenceEmbeddingDimension(InnerObject);
        var data = new float[sentences.Count * encodingDimensions];
        result.AsSpan2D<float>().CopyTo(data);
        return new ReadOnlyTensorSpan<float>(data, [sentences.Count, encodingDimensions]);
        // return result.AsTensorSpan<float>();
    }

    /// <summary>
    /// Loads the named sentence-transformers model from the supplied Python environment.
    /// </summary>
    /// <param name="pythonEnvironment">The Python environment hosting the sentence-transformers package.</param>
    /// <param name="modelName">The model name or path passed to sentence-transformers.</param>
    /// <param name="trustExternalCode">
    /// When <see langword="true"/>, allows the model to execute remote code (passed as
    /// <c>trust_remote_code</c>). Defaults to <see langword="false"/>.
    /// </param>
    public static SentenceTransformer Create(
        IPythonEnvironment pythonEnvironment,
        string modelName,
        bool trustExternalCode = false)
    {
        ArgumentNullException.ThrowIfNull(pythonEnvironment);
        ArgumentException.ThrowIfNullOrWhiteSpace(modelName);

        var service = pythonEnvironment.SentenceTransformersService();
        var transformer = service.GetSentenceTransformer(modelName, trustRemoteCode: trustExternalCode);
        return new SentenceTransformer(transformer, modelName, service);
    }
}
