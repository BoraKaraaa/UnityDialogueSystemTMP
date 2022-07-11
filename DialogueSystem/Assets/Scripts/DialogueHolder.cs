using UnityEngine;
using TMPro;

public abstract class DialogueHolder : MonoBehaviour
{

    public TMP_Text dialogueHolderText;
    protected ETextEffects textEffect = ETextEffects.None;

    public void SubsActions()
    {
        DialogueManager.Instance.OnStartDialogueActions += OnStartDialogueActions;
        DialogueManager.Instance.OnCustomDialogueActions += OnCustomDialogueActions;
        DialogueManager.Instance.OnEndDialogueActions += OnEndDialogueActions;
    }

    public void UnSubsActions()
    {
        DialogueManager.Instance.OnStartDialogueActions -= OnStartDialogueActions;
        DialogueManager.Instance.OnCustomDialogueActions -= OnCustomDialogueActions;
        DialogueManager.Instance.OnEndDialogueActions -= OnEndDialogueActions;
    }

    private void OnDestroy()
    {
        DialogueManager.Instance.OnStartDialogueActions -= OnStartDialogueActions;
        DialogueManager.Instance.OnCustomDialogueActions -= OnCustomDialogueActions;
        DialogueManager.Instance.OnEndDialogueActions -= OnEndDialogueActions;
    }

    public void SetEtextEffects(ETextEffects textEffect) => this.textEffect = textEffect;

    public abstract void OnStartDialogueActions(Dialogue dialogue);
    public abstract OneDialogue OnCustomDialogueActions();
    public abstract void OnEndDialogueActions();



}
