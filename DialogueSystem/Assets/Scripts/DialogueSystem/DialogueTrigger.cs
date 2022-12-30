using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueTrigger : Singletonn<DialogueTrigger>
{
    [Space(5)]
    [Header("Add Dialogue Scriptable Objects In Executing Order")]
    [Space(3)]
    [SerializeField] List<Dialogue> dialogueList;
    
    [Space(5)]
    
    [Header("Target DialogueHolder Index on  --DialogueManager--")]
    [Space(3)]
    [SerializeField] List<int> targetDialogueHolderIndex;

    private Queue<Dialogue> dialogueQueue;

    private int index = 0;

    public Action OnDialogudeTriggerInit;

    private void Start()
    {
        dialogueQueue = new Queue<Dialogue>();

        int startIndex = 0;

        for (int i = startIndex; i < dialogueList.Count; i++)
        {
            dialogueQueue.Enqueue(dialogueList[i]);
        }
        
        OnDialogudeTriggerInit?.Invoke();
    }
    
    public void SetDialogue(Dialogue newDialogue, int effectDialogueHolderIndex)
    {
        dialogueList.Add(newDialogue);
        dialogueQueue.Enqueue(newDialogue);
        targetDialogueHolderIndex.Add(effectDialogueHolderIndex);
    }

    public void TriggerDialogue()
    {
        if(DialogueManager.Instance.IsDialogueStarted || dialogueQueue.Count > 0)
        {
            if (!DialogueManager.Instance.IsDialogueStarted)
            {
                DialogueManager.Instance.StartDialogue(dialogueQueue.Dequeue(), targetDialogueHolderIndex[index++]);
            }
            else
            {
                DialogueManager.Instance.DisplayNextSentence();
            }
        }
    }   

}
