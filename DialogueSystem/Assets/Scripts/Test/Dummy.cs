using UnityEngine;
using Munkur;

public class Dummy : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            DialogueTrigger.Instance.TriggerDialogue();
        }
        
        if (Input.GetKeyDown(KeyCode.N))
        {
            DialogueManager.Instance.DisplayNextSentence();
        }
    }
}
