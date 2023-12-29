using UnityEngine;
using Munkur;

public class TriggerDialogueViaButton : MonoBehaviour
{
    private void TriggerDialogue()
    {
        DialogueTrigger.Instance.TriggerDialogue();
    }
}
