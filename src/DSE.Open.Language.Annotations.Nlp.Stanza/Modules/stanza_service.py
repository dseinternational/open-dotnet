import stanza
import stanza.pipeline
import stanza.pipeline.processor


# Stanza NLP


def download(lang: str = "en", logging_level: str = "WARN"):
    """
    Download the Stanza pipeline for the specified language.

    Args:
        lang (str): The language code for which to download the Stanza pipeline.
    """
    stanza.download(lang, logging_level=logging_level)


def create_pipeline(
    lang: str = "en", processors: str = "tokenize,mwt,pos,lemma,depparse") -> stanza.Pipeline:
    """
    Create a Stanza pipeline for the specified language and processors.

    Args:
        lang (str): The language code for which to create the Stanza pipeline.
        processors (str): A comma-separated string of processors to include in the pipeline.

    Returns:
        stanza.Pipeline: The created Stanza pipeline.
    """
    pipeline = stanza.Pipeline(lang=lang, processors=processors)
    return pipeline


# Pipeline


def get_loaded_processors(
    pipeline: stanza.Pipeline) -> list[stanza.pipeline.processor.Processor]:
    """
    Returns a list of currently loaded Stanza processors.
    """
    return pipeline.loaded_processors


def process_text(pipeline: stanza.Pipeline, text: str) -> stanza.Document:
    """
    Process the input text using the Stanza pipeline and return a Document object.

    Args:
        pipeline (stanza.Pipeline): The Stanza pipeline to use for processing.
        text (str): The input text to process.

    Returns:
        stanza.Document: The processed document.
    """
    document = pipeline(text)
    return document
