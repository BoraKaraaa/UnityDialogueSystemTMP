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

    private ETextEffects textEffect = ETextEffects.None;
    
    public Action HolderOnStartDialogueActions;
    public Action<RealDialogue, int> HolderOnCustomDialogueActions;
    public Action HolderOnOneDialogueEndActions;
    public Action HolderOnEndDialogueActions;
    
    public bool DialogueStopGame;

    protected bool StopTextEffect { get; set; } = false;
    protected bool StopChangeColor { get; set; } = false;

    private RealDialogue _realDialogue = null;
    private Dialogue _dialogue = null;

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

    protected void SetEtextEffects(ETextEffects textEffect) => this.textEffect = textEffect;
    protected abstract void InitReferences(ref RealDialogue realDialogue);

    protected virtual void OnStartDialogueActions(Dialogue dialogue)
    {
        if (DialogueStopGame)
        {
            DialogueManager.Instance.DialogueStopGame = true;
            Time.timeScale = 0f;
        }

        _dialogue = dialogue;

        InitReferences(ref _realDialogue);
        
        this.gameObject.SetActive(true);
        
        StopTextEffect = false;
        StopChangeColor = false;
        
        CheckAndStartTextEffect();
        HolderOnStartDialogueActions?.Invoke();
    }

    protected virtual RealDialogue OnCustomDialogueActions(int index)
    {
        //diffColorWordIndex = _realDialogue.diffColorWordIndex[index];
        //color = _realDialogue.diffColorWord[index];

        SetEtextEffects(_realDialogue.textEffects[index]);

        return _realDialogue;
    }
    protected abstract void OnOneDialogueEndActions();

    protected virtual void OnEndDialogueActions()
    {
        if (DialogueStopGame)
        {
            DialogueManager.Instance.DialogueStopGame = false;
            Time.timeScale = 1f;
        }
        
        if (dialogueEnd == EDialogueEnd.None)
        {
            this.gameObject.SetActive(false);
        }
        else if (dialogueEnd == EDialogueEnd.NextDialogue)
        {
            DialogueTrigger.Instance.TriggerDialogue();
            this.gameObject.SetActive(false);
        }
        else if (dialogueEnd == EDialogueEnd.LoadScene)
        {
       
        }
        
        StopTextEffect = true;
        StopChangeColor = true;
        HolderOnEndDialogueActions?.Invoke();
    }

    protected virtual void SetDefaultValues(int index)
    {
        _realDialogue.SetText(index, _dialogue.sentences[index]); //Set Texts

        _realDialogue.SetCustomTextWriteSpeed(index, _dialogue.defTextWriteSpeeds[_dialogue.characterCounts[index]]);
        _realDialogue.SetCustomTextAudio(index, _dialogue.defTextAudios[_dialogue.characterCounts[index]]);
        _realDialogue.SetCustomTextEffect(index, _dialogue.defTextEffects[_dialogue.characterCounts[index]]);
        _realDialogue.SetCustomOverWrite(index, _dialogue.defOverWrites);
        _realDialogue.SetCustomDiffColor(index, _dialogue.defDiffColor[_dialogue.characterCounts[index]]);
    }

    protected virtual void ControlCustomValues(int index)
    {
        if (index < _dialogue.textWriteSpeeds.Count)  
            _realDialogue.SetCustomTextWriteSpeed(_dialogue.textWriteSpeeds[index].id, _dialogue.textWriteSpeeds[index].textWriteSpeed);
        if (index < _dialogue.textAudios.Count)  
            _realDialogue.SetCustomTextAudio(_dialogue.textAudios[index].id, _dialogue.textAudios[index].textAudio);
        if (index < _dialogue.textEffects.Count)  
            _realDialogue.SetCustomTextEffect(_dialogue.textEffects[index].id, _dialogue.textEffects[index].textEffect);
        if (index < _dialogue.overWrites.Count)  
            _realDialogue.SetCustomOverWrite(_dialogue.overWrites[index].id, _dialogue.overWrites[index].overWrite);
        if (index < _dialogue.diffColor.Count)  
            _realDialogue.SetCustomDiffColor(_dialogue.diffColor[index].id, _dialogue.diffColor[index].diffColor);
    }

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
