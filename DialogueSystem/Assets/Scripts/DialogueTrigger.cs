using System.Collections;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] Dialogue dialogue;
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
        DialogueManager.Instance.StartDialogue(dialogue, effectTextIndex);
    }



}
