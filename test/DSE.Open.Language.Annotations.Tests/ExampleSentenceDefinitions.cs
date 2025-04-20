// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Annotations;

public static class ExampleSentenceDefinitions
{
    /// <summary>
    /// Example sentence with 3 multiple modal verbs.
    /// </summary>
    public const string HeCanSwimAndMustPractiseEveryDay = """
        # text = He can swim and must practise every day, but he might rest tomorrow.
        # 
        1	He	he	PRON	PRP	Case=Nom|Gender=Masc|Number=Sing|Person=3|PronType=Prs	3	nsubj	_	_
        2	can	can	AUX	MD	VerbForm=Fin	3	aux	_	_
        3	swim	swim	VERB	VB	VerbForm=Inf	0	root	_	_
        4	and	and	CCONJ	CC	_	6	cc	_	_
        5	must	must	AUX	MD	VerbForm=Fin	6	aux	_	_
        6	practise	practise	VERB	VB	VerbForm=Inf	3	conj	_	_
        7	every	every	DET	DT	PronType=Tot	8	det	_	_
        8	day	day	NOUN	NN	Number=Sing	6	obl:unmarked	_	_
        9	,	,	PUNCT	,	_	13	punct	_	_
        10	but	but	CCONJ	CC	_	13	cc	_	_
        11	he	he	PRON	PRP	Case=Nom|Gender=Masc|Number=Sing|Person=3|PronType=Prs	13	nsubj	_	_
        12	might	might	AUX	MD	VerbForm=Fin	13	aux	_	_
        13	rest	rest	VERB	VB	VerbForm=Inf	3	conj	_	_
        14	tomorrow	tomorrow	NOUN	NN	Number=Sing	13	obl:unmarked	_	_
        15	.	.	PUNCT	.	_	3	punct	_	_
        """;

    public const string TheDogIsRunningAndTheHorseIsWalking = """
        # text = The dog is running and the horse is walking, but the snail is crawling and the duck is swimming and the bird is flying.
        # 
        1	The	the	DET	DT	Definite=Def|PronType=Art	2	det	_	_
        2	dog	dog	NOUN	NN	Number=Sing	4	nsubj	_	_
        3	is	be	AUX	VBZ	Mood=Ind|Number=Sing|Person=3|Tense=Pres|VerbForm=Fin	4	aux	_	_
        4	running	run	VERB	VBG	Tense=Pres|VerbForm=Part	0	root	_	_
        5	and	and	CCONJ	CC	_	9	cc	_	_
        6	the	the	DET	DT	Definite=Def|PronType=Art	7	det	_	_
        7	horse	horse	NOUN	NN	Number=Sing	9	nsubj	_	_
        8	is	be	AUX	VBZ	Mood=Ind|Number=Sing|Person=3|Tense=Pres|VerbForm=Fin	9	aux	_	_
        9	walking	walk	VERB	VBG	Tense=Pres|VerbForm=Part	4	conj	_	_
        10	,	,	PUNCT	,	_	15	punct	_	_
        11	but	but	CCONJ	CC	_	15	cc	_	_
        12	the	the	DET	DT	Definite=Def|PronType=Art	13	det	_	_
        13	snail	snail	NOUN	NN	Number=Sing	15	nsubj	_	_
        14	is	be	AUX	VBZ	Mood=Ind|Number=Sing|Person=3|Tense=Pres|VerbForm=Fin	15	aux	_	_
        15	crawling	crawl	VERB	VBG	Tense=Pres|VerbForm=Part	4	conj	_	_
        16	and	and	CCONJ	CC	_	20	cc	_	_
        17	the	the	DET	DT	Definite=Def|PronType=Art	18	det	_	_
        18	duck	duck	NOUN	NN	Number=Sing	20	nsubj	_	_
        19	is	be	AUX	VBZ	Mood=Ind|Number=Sing|Person=3|Tense=Pres|VerbForm=Fin	20	aux	_	_
        20	swimming	swim	VERB	VBG	Tense=Pres|VerbForm=Part	15	conj	_	_
        21	and	and	CCONJ	CC	_	25	cc	_	_
        22	the	the	DET	DT	Definite=Def|PronType=Art	23	det	_	_
        23	bird	bird	NOUN	NN	Number=Sing	25	nsubj	_	_
        24	is	be	AUX	VBZ	Mood=Ind|Number=Sing|Person=3|Tense=Pres|VerbForm=Fin	25	aux	_	_
        25	flying	fly	VERB	VBG	Tense=Pres|VerbForm=Part	20	conj	_	_
        26	.	.	PUNCT	.	_	4	punct	_	_
        """;
}
