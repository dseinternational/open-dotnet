from sentence_transformers import SentenceTransformer
from collections.abc import Buffer
from typing import Optional


def get_sentence_transformer(
    model_name_or_path: str,
    device: Optional[str] = None,
    cache_dir: Optional[str] = None,
    revision: Optional[str] = "main",
    trust_remote_code: bool = False,
) -> SentenceTransformer:
    """
    Loads or creates a SentenceTransformer model that can be used to map sentences / text
    to embeddings.

    Args:
        model (str): If it is a filepath on disc, it loads the model from that path. If it
            is not a path, it first tries to download a pre-trained SentenceTransformer model.
            If that fails, tries to construct a model from the Hugging Face Hub with that name.
        device (Optional[str]): Device (like “cuda”, “cpu”, “mps”, “npu”) that should be
            used for computation. If None, checks if a GPU can be used.
        cache_folder  (Optional[str]): The directory to cache the model files.
        revision (Optional[str]): The specific model revision to use, default is "main".
        trust_remote_code (bool): Whether or not to allow for custom models defined on the Hub
            in their own modeling files. This option should only be set to True for repositories
            you trust and in which you have read the code, as it will execute code present on
            the Hub on your local machine.

    Returns:
        SentenceTransformer: The loaded SentenceTransformer model.

    """
    return SentenceTransformer(
        model_name_or_path,
        device=device,
        cache_folder=cache_dir,
        revision=revision,
        trust_remote_code=trust_remote_code,
    )


def encode_sentence_collection(
    model: SentenceTransformer, sentences: list[str], prompt: Optional[str]
) -> Buffer:
    """
    Computes sentence embeddings using the specified SentenceTransformer model.

    Args:
        model (SentenceTransformer): The SentenceTransformer model to use for encoding.
        sentences (list[str]): The sentences to embed.
        prompt (Optional[str]): An optional prompt to use for encoding. For example,
            if the prompt is "query":, then the sentence “What is the capital of France?”
            will be encoded as “query: What is the capital of France?” because the
            sentence is appended to the prompt.

    Returns:
        A 2d numpy array with shape [num_inputs, output_dimension] is returned.

    """
    return model.encode(sentences, prompt=prompt)


def encode_sentence(model: SentenceTransformer, sentence: str) -> Buffer:
    """
    Computes a sentence embedding using the specified SentenceTransformer model.

    Args:
        model (SentenceTransformer): The SentenceTransformer model to use for encoding.
        sentence (str): The sentence to embed.

    Returns:
        A 1d array with shape [output_dimension].

    """
    return model.encode(sentence)


def get_device_type(model: SentenceTransformer) -> str:
    """
    Returns the device on which the SentenceTransformer model is loaded.

    Args:
        model (SentenceTransformer): The SentenceTransformer model.

    Returns:
        str: The device on which the model is loaded, e.g., "cuda", "cpu", etc.
    """
    return model.device.type
