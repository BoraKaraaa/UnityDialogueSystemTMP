using System.Collections;
using UnityEngine;
using System;

public class DialogueManager : Singletonn<DialogueManager>
{
    [Space(5)]
    [SerializeField] private DialogueHolder[] dialogueHolders;

    private DialogueHolder activeDialogueHolder = null;
    public DialogueHolder ActiveDialogueHolder => activeDialogueHolder;

    private Animator activeDialogueHolderAnimator = null;

    [SerializeField] private float fastWriteSpeed = 0.04f;
    private bool isCoroutineEnd = true;
    private bool fastWrite = false;

    private int dialogueIndex = 0;
    private int maxSize;
    
    [Space(5)]
    [SerializeField] private string startDialogueAnimationStateName;
    [SerializeField] private string endDialogueAnimationStateName;

    public Action<Dialogue> OnStartDialogueActions;
    public Func<int, RealDialogue> OnCustomDialogueActions;
    public Action OnOneDialogueEndActions;
    public Action OnEndDialogueActions;

    private bool isDialogueStarted = false;
    public bool IsDialogueStarted => isDialogueStarted;

    public bool DialogueStopGame { get; set; } = false;
    
    public void StartDialogue(Dialogue dialogue, int activeTextIndexInScene)
    {
        isDialogueStarted = true;
        maxSize = dialogue.sentences.Length;
        
        SetActiveTextInScene(activeTextIndexInScene);

        if (activeDialogueHolderAnimator != null)
        {
            activeDialogueHolderAnimator.Play(startDialogueAnimationStateName, 0);
        }

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

        if (dialogueIndex == maxSize)
        {
            EndDialogue();
            return; 
        }

        RealDialogue realDialogue =  OnCustomDialogueActions?.Invoke(dialogueIndex);

        if (realDialogue == null)
        {
            Debug.Log("REAL DIALOGUE VALUE NULL");
        }
        else
        {
            activeDialogueHolder.audioSource.clip = realDialogue.textAudios[dialogueIndex];
        }
        
        
        StopAllCoroutines();
        StartCoroutine(TypeSentence(realDialogue));

    }

    IEnumerator TypeSentence(RealDialogue realDialogue)
    {
        if (!realDialogue.overWrite[dialogueIndex])
        {
            activeDialogueHolder.dialogueHolderText.text = String.Empty;
        }

        isCoroutineEnd = false;

        if (activeDialogueHolder.audioSource.loop == true)
        {
            activeDialogueHolder.audioSource.Play();
        }

        WaitForSecondsRealtime wfs = new WaitForSecondsRealtime(realDialogue.textWriteSpeeds[dialogueIndex]);
        WaitForSecondsRealtime wfsFast = new WaitForSecondsRealtime(fastWriteSpeed);


        foreach (char letter in realDialogue.sentences[dialogueIndex])
        {

            activeDialogueHolder.dialogueHolderText.text += letter;

            if (letter != ' ')
            {
                if (activeDialogueHolder.audioSource.loop == false)
                {
                    activeDialogueHolder.audioSource.Play();
                }
         
                if(fastWrite)
                    yield return wfsFast;
                else
                    yield return wfs;
            }

        }

        dialogueIndex++;
        fastWrite = false;
        isCoroutineEnd = true;
        OnOneDialogueEndActions?.Invoke();
    }

    private void EndDialogue()
    {
        isDialogueStarted = false;
        dialogueIndex = 0;

        if (activeDialogueHolderAnimator != null)
        {
            activeDialogueHolderAnimator.Play(endDialogueAnimationStateName, 0);
        }

        OnEndDialogueActions?.Invoke();
        EndDialogueCustomActions();

    }

    private void SetActiveTextInScene(int index)
    {
        if (activeDialogueHolder != null)
        {
            activeDialogueHolder.UnSubsActions();
        }
        
        activeDialogueHolder = dialogueHolders[index];
        activeDialogueHolder.SubsActions();

        activeDialogueHolderAnimator = activeDialogueHolder.GetComponent<Animator>();
    }

    public void SkipDialogue()
    {
        isCoroutineEnd = true;
        StopAllCoroutines();
        EndDialogue();
    }

    private void StartDialogueCustomActions() { }

    private void EndDialogueCustomActions() { }
    
}