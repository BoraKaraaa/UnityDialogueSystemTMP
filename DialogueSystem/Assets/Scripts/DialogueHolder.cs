using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class DialogueHolder : MonoBehaviour
{

    public TMP_Text dialogueHolderText;
    private ETextEffects textEffect = ETextEffects.None;

    private void Start()
    {
        DialogueManager.Instance.OnStartDialogueActions += OnStartDialogueActions;
        DialogueManager.Instance.OnCustomDialogueActions += OnCustomDialogueActions;
        DialogueManager.Instance.OnEndDialogueActions += OnEndDialogueActions;
    }

    private void OnDestroy()
    {
        DialogueManager.Instance.OnStartDialogueActions -= OnStartDialogueActions;
        DialogueManager.Instance.OnCustomDialogueActions -= OnCustomDialogueActions;
        DialogueManager.Instance.OnEndDialogueActions -= OnEndDialogueActions;
    }

    public virtual void Update()
    {
        if(textEffect != ETextEffects.None)
            TextEffectsController.Instance.DoTextEffect(dialogueHolderText);
    }

    public void SetEtextEffects(ETextEffects textEffect)
    {
        TextEffectsController.Instance.SetEtextEffects(textEffect);
        this.textEffect = textEffect;
    }

    public virtual void OnStartDialogueActions()
    {

    }

    public virtual void OnCustomDialogueActions()
    {

    }

    public virtual void OnEndDialogueActions()
    {

    }


}
