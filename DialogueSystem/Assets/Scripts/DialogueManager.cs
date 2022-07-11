using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager _instance;
    public static DialogueManager Instance { get { return _instance; } }

    [SerializeField] private DialogueHolder[] dialogueHolders;
    private DialogueHolder activeDialogueHolder;

    private Queue<OneDialogue> oneDialogueQue;

    private int dialogueIndex;

    [SerializeField] private float fastWriteSpeed = 0.04f;
    private bool isCoroutineEnd = true;
    private bool fastWrite = false;

    [SerializeField] private string startDialogueAnimationStateName;
    [SerializeField] private string endDialogueAnimationStateName;

    public Action OnStartDialogueActions;
    public Action OnCustomDialogueActions;
    public Action OnEndDialogueActions;

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

    public void StartDialogue(Dialogue dialogue, int activeTextIndexInScene) // For Creating New Dialogue
    {

        SetActiveTextInScene(activeTextIndexInScene);

        OnStartDialogueActions?.Invoke();
        StartDialogueCustomActions();

        oneDialogueQue.Clear();
        dialogueIndex = 0;

        foreach (string sentence in dialogue.sentences)
        {
            oneDialogueQue.Enqueue(new OneDialogue(dialogue.charcterName, dialogue.sentences[dialogueIndex], dialogue.images[dialogueIndex], 
                dialogue.textWriteSpeeds[dialogueIndex], dialogue.textAudios[dialogueIndex], dialogue.textEffects[dialogueIndex]));

            dialogueIndex++;
        }

        DisplayNextSentence();

    }

    public void DisplayNextSentence() // Reference with clicking or Button Push
    {

        if (isCoroutineEnd == false)
        {
            fastWrite = true;
            return;
        }

        if (oneDialogueQue.Count == 0)
        {
            EndDialogue();
            return; 
        }

        OneDialogue currDialogue = oneDialogueQue.Dequeue();

        activeDialogueHolder.SetEtextEffects(currDialogue.textEffects);

        activeDialogueHolder.dialogueHolderImage.sprite = currDialogue.image.sprite;

        if (currDialogue.image.GetComponent<Animator>() != null)
            currDialogue.image.GetComponent<Animator>().enabled = true;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(currDialogue));

    }

    IEnumerator TypeSentence(OneDialogue currDialogue)
    {
        activeDialogueHolder.dialogueHolderText.text = String.Empty;
        isCoroutineEnd = false;

        AudioSource newAudioSource = Instantiate(currDialogue.textAudio, transform.position, Quaternion.identity);
       
        if(newAudioSource.loop == true)
            newAudioSource.Play();

        WaitForSeconds wfs = new WaitForSeconds(currDialogue.textWriteSpeed);
        WaitForSeconds wfsFast = new WaitForSeconds(fastWriteSpeed);


        foreach (char letter in currDialogue.sentence)
        {

            activeDialogueHolder.dialogueHolderText.text += letter; // her char basildigiginda effect calismiyor

            if (letter != ' ')
            {
                if (newAudioSource.loop == false)   
                    newAudioSource.Play();

               
                if(fastWrite)
                    yield return wfsFast;
                else
                    yield return wfs;
            }

        }

        fastWrite = false;
        isCoroutineEnd = true;
        Destroy(newAudioSource);
    }

    private void EndDialogue()
    {

        OnEndDialogueActions?.Invoke();
        EndDialogueCustomActions();

    }


    private void SetActiveTextInScene(int index)
    {
        activeDialogueHolder = dialogueHolders[index];
    }

    private void EndDialogueCustomActions() { }
    private void StartDialogueCustomActions() { }

}
