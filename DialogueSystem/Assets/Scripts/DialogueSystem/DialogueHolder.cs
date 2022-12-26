using UnityEngine;
using TMPro;
using System;
using System.Collections;

public abstract class DialogueHolder : MonoBehaviour
{
    public TMP_Text dialogueHolderText;
    public AudioSource audioSource;
    public EDialogueEnd dialogueEnd;
    
    public Color color = Color.white;
    public int diffColorWordIndex = -1;

    protected ETextEffects textEffect = ETextEffects.None;
    
    public Action HolderOnStartDialogueActions;
    public Action<RealDialogue, int> HolderOnCustomDialogueActions;
    public Action HolderOnOneDialogueEndActions;
    public Action HolderOnEndDialogueActions;
    
    public bool DialogueStopGame;

    public bool StopTextEffect { get; set; } = false;
    public bool StopChangeColor { get; set; } = false;

    private Coroutine _textEffectRoutine = null;
    private Coroutine _changeWordColorRoutine = null;

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
        DialogueManager.Instance.OnOneDialogueEndActions -= OnOneDialogueEndActions;
    }
    public void SetEtextEffects(ETextEffects textEffect) => this.textEffect = textEffect;
    public abstract void OnStartDialogueActions(Dialogue dialogue);
    public abstract RealDialogue OnCustomDialogueActions(int index);
    public abstract void OnOneDialogueEndActions();
    public abstract void OnEndDialogueActions();

    protected virtual void CheckAndStartTextEffect()
    {
        if (textEffect != ETextEffects.None && _textEffectRoutine == null)
        {
            if (DialogueStopGame)
            {
                _textEffectRoutine = StartCoroutine(UnScaledDoTextEffect());
            }
            else
            {
                _textEffectRoutine = StartCoroutine(ScaledDoTextEffect());
            }
        }

        if (diffColorWordIndex != -1 && _changeWordColorRoutine == null)
        {
            if (DialogueStopGame)
            {
                _changeWordColorRoutine = StartCoroutine(UnScaledChangeWordColor());
            }
            else
            {
                _changeWordColorRoutine = StartCoroutine(ScaledChangeWordColor());
            }
        }
    }

    private IEnumerator ScaledDoTextEffect()
    {
        while (true)
        {
            if (StopTextEffect)
            {
                yield return null;
                yield break;
            }
            
            TextEffectsController.Instance.DoTextEffect(dialogueHolderText, textEffect);
        }
    }

    private IEnumerator UnScaledDoTextEffect()
    {
        while (true)
        {
            if (StopTextEffect)
            {
                yield return null;
                yield break;
            }
            
            UnScaledTextEffectController.Instance.DoTextEffect(dialogueHolderText, textEffect);
        }
    }

    private IEnumerator ScaledChangeWordColor()
    {
        while (true)
        {
            if (StopChangeColor)
            {
                yield return null;
                yield break;
            }
            
            TextEffectsController.Instance.ChangeWordColor(dialogueHolderText, diffColorWordIndex, color);
        }
    }

    private IEnumerator UnScaledChangeWordColor()
    {
        while (true)
        {
            if (StopChangeColor)
            {
                yield return null;
                yield break;
            }
            
            UnScaledTextEffectController.Instance.ChangeWordColor(dialogueHolderText, diffColorWordIndex, color);
        }
    }

}
