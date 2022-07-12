using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private static DialogueTrigger _instance;
    public static DialogueTrigger Instance { get { return _instance; } }

    [SerializeField] Queue<Dialogue> dialogue;
    [SerializeField] int[] effectTextIndex;

    private int index = 0;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue.Dequeue(), effectTextIndex[index++]);
    }



}
