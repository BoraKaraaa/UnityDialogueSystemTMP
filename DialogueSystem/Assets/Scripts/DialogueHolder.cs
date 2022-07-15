using UnityEngine;
using TMPro;

public abstract class DialogueHolder : MonoBehaviour
{

    public TMP_Text dialogueHolderText;
    public AudioSource audioSource;

    protected ETextEffects textEffect = ETextEffects.None;

    public void SubsActions()
    {
        DialogueManager.Instance.OnStartDialogueActions += OnStartDialogueActions;
        DialogueManager.Instance.OnCustomDialogueActions += OnCustomDialogueActions;
        DialogueManager.Instance.OnEndDialogueActions += OnEndDialogueActions;
        DialogueManager.Instance.OnOneDialogueEndActions += OnOneDialogueEndActions;
    }

    public void UnSubsActions()
    {
        DialogueManager.Instance.OnStartDialogueActions -= OnStartDialogueActions;
        DialogueManager.Instance.OnCustomDialogueActions -= OnCustomDialogueActions;
        DialogueManager.Instance.OnEndDialogueActions -= OnEndDialogueActions;
        DialogueManager.Instance.OnOneDialogueEndActions += OnOneDialogueEndActions;
    }

    private void OnDestroy()
    {
        DialogueManager.Instance.OnStartDialogueActions -= OnStartDialogueActions;
        DialogueManager.Instance.OnCustomDialogueActions -= OnCustomDialogueActions;
        DialogueManager.Instance.OnEndDialogueActions -= OnEndDialogueActions;
        DialogueManager.Instance.OnOneDialogueEndActions += OnOneDialogueEndActions;
    }

    public void SetEtextEffects(ETextEffects textEffect) => this.textEffect = textEffect;

    public abstract void OnStartDialogueActions(Dialogue dialogue);
    public abstract OneDialogue OnCustomDialogueActions();
    public abstract void OnOneDialogueEndActions();
    public abstract void OnEndDialogueActions();



}
