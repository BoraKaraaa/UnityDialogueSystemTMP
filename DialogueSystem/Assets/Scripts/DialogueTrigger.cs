using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] Dialogue[] dialogues;
    [SerializeField] int effectTextIndex;

    private int i = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            if (i++ == 0)
                TriggerDialogue(0);
            else
                DialogueManager.Instance.DisplayNextSentence();
        }
            
    }

    public virtual void TriggerDialogue(int dialogueIndex)
    {
        DialogueManager.Instance.SetActiveTextInScene(effectTextIndex);
        DialogueManager.Instance.StartDialogue(dialogues[dialogueIndex]);
    }





}
