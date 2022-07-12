using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private static DialogueTrigger _instance;
    public static DialogueTrigger Instance { get { return _instance; } }

    [SerializeField] List<Dialogue> dialogueList;
    [SerializeField] int[] effectTextIndex;

    private Queue<Dialogue> dialogueQueue;

    private int index = 0;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    private void Start()
    {
        dialogueQueue = new Queue<Dialogue>();

        foreach (Dialogue dialogue in dialogueList)
            dialogueQueue.Enqueue(dialogue);
    }

    public void TriggerDialogue()
    {
        DialogueManager dialogueManager = DialogueManager.Instance;

        if(dialogueManager.isDialogueStarted || dialogueQueue.Count > 0)
        {
            if (!dialogueManager.isDialogueStarted)
                dialogueManager.StartDialogue(dialogueQueue.Dequeue(), effectTextIndex[index++]);
            else
                dialogueManager.DisplayNextSentence();
        }
    }   



}
