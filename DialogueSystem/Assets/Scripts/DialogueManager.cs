using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager _instance;
    public static DialogueManager Instance { get { return _instance; } }

    [SerializeField] private DialogueHolder[] dialogueHolders;
    public DialogueHolder activeDialogueHolder;

    public Queue<OneDialogue> oneDialogueQue;

    [SerializeField] private float fastWriteSpeed = 0.04f;
    private bool isCoroutineEnd = true;
    private bool fastWrite = false;

    [SerializeField] private string startDialogueAnimationStateName;
    [SerializeField] private string endDialogueAnimationStateName;

    public Action<Dialogue> OnStartDialogueActions;
    public Func<OneDialogue> OnCustomDialogueActions;
    public Action OnEndDialogueActions;

    public bool isDialogueStarted = false;

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
        isDialogueStarted = true;

        SetActiveTextInScene(activeTextIndexInScene);

        if (activeDialogueHolder.GetComponent<Animator>() != null)
            activeDialogueHolder.GetComponent<Animator>().Play(startDialogueAnimationStateName, 0);

        oneDialogueQue.Clear();

        OnStartDialogueActions?.Invoke(dialogue);
        StartDialogueCustomActions();

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
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

        OneDialogue newOneDialogue =  OnCustomDialogueActions?.Invoke();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(newOneDialogue));

    }

    IEnumerator TypeSentence(OneDialogue currDialogue)
    {
        if(!currDialogue.overWrite)
            activeDialogueHolder.dialogueHolderText.text = String.Empty;

        isCoroutineEnd = false;

        AudioSource newAudioSource = Instantiate(currDialogue.textAudio, transform.position, Quaternion.identity);
       
        if(newAudioSource.loop == true)
            newAudioSource.Play();

        WaitForSeconds wfs = new WaitForSeconds(currDialogue.textWriteSpeed);
        WaitForSeconds wfsFast = new WaitForSeconds(fastWriteSpeed);


        foreach (char letter in currDialogue.sentence)
        {

            activeDialogueHolder.dialogueHolderText.text += letter; 

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
        Destroy(newAudioSource.gameObject);
    }

    private void EndDialogue()
    {
        isDialogueStarted = false;

        if (activeDialogueHolder.GetComponent<Animator>() != null)
            activeDialogueHolder.GetComponent<Animator>().Play(endDialogueAnimationStateName, 0);

        OnEndDialogueActions?.Invoke();
        EndDialogueCustomActions();

    }

    private void SetActiveTextInScene(int index)
    {
        activeDialogueHolder = dialogueHolders[index];
        activeDialogueHolder.SubsActions();

        for(int i = 0; i < dialogueHolders.Length; i++)
        {
            if (i != index)
                dialogueHolders[i].UnSubsActions();
        }
            
    }

    private void EndDialogueCustomActions() { }
    private void StartDialogueCustomActions() { }

}
