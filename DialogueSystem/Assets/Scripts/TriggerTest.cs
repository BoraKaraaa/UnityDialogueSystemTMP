using UnityEngine;

public class TriggerTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DialogueTrigger.Instance.TriggerDialogue();
        }
    }
}
