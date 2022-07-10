using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager _instance;
    public static DialogueManager Instance { get { return _instance; } }

    [SerializeField] private DialogueHolder[] dialogueHolders;
    private DialogueHolder activeDialogueHolder;

    private Queue<OneDialogue> oneDialogueQue;

    private int dialogueIndex;
    private bool isLastDialogue;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    private void Start()
    {
        oneDialogueQue = new Queue<OneDialogue>();
    }

    public void StartDialogue(Dialogue dialogue) // For Creating New Dialogue
    {
        oneDialogueQue.Clear();
        dialogueIndex = 0;
        isLastDialogue = false;

        isLastDialogue = dialogue.lastDialogueInRow;

        foreach (string sentence in dialogue.sentences)
        {
            oneDialogueQue.Enqueue(new OneDialogue(dialogue.charcterName, dialogue.sentences[dialogueIndex], dialogue.images[dialogueIndex], 
                dialogue.textWriteSpeeds[dialogueIndex], dialogue.textAudios[dialogueIndex]));

            dialogueIndex++;
        }

        DisplayNextSentence();

    }

    public void DisplayNextSentence() // Reference with clicking or Button Push
    {
        if(oneDialogueQue.Count == 0)
        {
            EndDialogue(isLastDialogue);
            return; 
        }

        OneDialogue currDialogue = oneDialogueQue.Dequeue();

        activeDialogueHolder.dialogueHolderImage.sprite = currDialogue.image.sprite;

        if (currDialogue.image.GetComponent<Animator>() != null)
            currDialogue.image.GetComponent<Animator>().enabled = true;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(currDialogue));

 
    }

    IEnumerator TypeSentence(OneDialogue currDialogue)
    {
        activeDialogueHolder.dialogueHolderText.text = String.Empty;
        AudioSource newAudioSource = Instantiate(currDialogue.textAudio, transform.position, Quaternion.identity);

        WaitForSeconds wfs = new WaitForSeconds(currDialogue.textWriteSpeed);

        foreach(char letter in currDialogue.sentence)
        {
            activeDialogueHolder.dialogueHolderText.text += letter;

            if(letter != ' ')
            {
                newAudioSource.Play();
                yield return wfs;
            }

        }

        Destroy(newAudioSource);
    }

    private void EndDialogue(bool isLastDialogue) // Play EndDialogue Animation
    {
        if (isLastDialogue)
            activeDialogueHolder.enabled = false;
 
    }


    public void SetActiveTextInScene(int index)
    {
        activeDialogueHolder = dialogueHolders[index];
    }

}
