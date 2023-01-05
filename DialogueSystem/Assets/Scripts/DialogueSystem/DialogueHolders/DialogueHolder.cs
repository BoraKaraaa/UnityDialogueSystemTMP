using UnityEngine;
using TMPro;
using System;
using System.Collections;

public abstract class DialogueHolder : MonoBehaviour
{
    public TMP_Text dialogueHolderText;
    public AudioSource audioSource;
    public EDialogueEnd dialogueEnd;

    private ETextEffects textEffect = ETextEffects.None;
    private WordColorIndex wordColorIndex = null;
    
    public Action HolderOnStartDialogueActions;
    public Action<RealDialogue, int> HolderOnCustomDialogueActions;
    public Action HolderOnOneDialogueEndActions;
    public Action HolderOnEndDialogueActions;
    
    public bool DialogueStopGame;

    protected bool StopTextEffect { get; set; } = false;
    protected bool StopChangeColor { get; set; } = false;

    private RealDialogue realDialogue = null;
    private Dialogue dialogue = null;

    private Coroutine textEffectRoutine = null;
    private Coroutine changeWordColorRoutine = null;
    
    /// <summary>
    ///   <para> Subscribe Dialogue Manager Actions
    ///             Called by Dialogue Manager When this --DialogueHolder-- active </para>
    /// </summary>
    public void SubsActions()
    {
        DialogueManager.Instance.OnStartDialogueActions += OnStartDialogueActions;
        DialogueManager.Instance.OnCustomDialogueActions += OnCustomDialogueActions;
        DialogueManager.Instance.OnEndDialogueActions += OnEndDialogueActions;
        DialogueManager.Instance.OnOneDialogueEndActions += OnOneDialogueEndActions;
    }
    
    /// <summary>
    ///   <para> UnSubscribe Dialogue Manager Actions
    ///             Called by Dialogue Manager When this --DialogueHolder-- inactive </para>
    /// </summary>
    public void UnSubsActions()
    {
        DialogueManager.Instance.OnStartDialogueActions -= OnStartDialogueActions;
        DialogueManager.Instance.OnCustomDialogueActions -= OnCustomDialogueActions;
        DialogueManager.Instance.OnEndDialogueActions -= OnEndDialogueActions;
        DialogueManager.Instance.OnOneDialogueEndActions -= OnOneDialogueEndActions;
    }
    
    private void OnDestroy()
    {
        DialogueManager.Instance.OnStartDialogueActions -= OnStartDialogueActions;
        DialogueManager.Instance.OnCustomDialogueActions -= OnCustomDialogueActions;
        DialogueManager.Instance.OnEndDialogueActions -= OnEndDialogueActions;
        DialogueManager.Instance.OnOneDialogueEndActions -= OnOneDialogueEndActions;
    }

    protected void SetEtextEffects(ETextEffects textEffect) => this.textEffect = textEffect;
    protected void SetWordColorIndex(WordColorIndex wordColorIndex) => this.wordColorIndex = wordColorIndex;
    protected abstract void InitReferences(ref RealDialogue realDialogue);
    
    protected virtual void OnStartDialogueActions(Dialogue dialogue)
    {
        if (DialogueStopGame)
        {
            DialogueManager.Instance.DialogueStopGame = true;
            Time.timeScale = 0f;
        }

        this.dialogue = dialogue;

        InitReferences(ref realDialogue);
        
        this.gameObject.SetActive(true);
        
        StopTextEffect = false;
        StopChangeColor = false;
        
        HolderOnStartDialogueActions?.Invoke();
    }

    protected virtual RealDialogue OnCustomDialogueActions(int index)
    {
        TextColorController.Instance.ChangeWholeColor(dialogueHolderText, 
            realDialogue.diffColor[index]);
        
        SetEtextEffects(realDialogue.textEffects[index]);
        CheckAndStartTextEffect();
        
        SetWordColorIndex(realDialogue.wordColorIndices[index]);
        CheckAndStartWordColorEffect();

        return realDialogue;
    }

    protected virtual void OnOneDialogueEndActions()
    {

    }

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
    
    /// <summary>
    ///   <para> Sets the dialogue Scriptable Object's default values to realDialogue </para>
    /// <param name="index"> Corresponding sentences index </param>
    /// </summary>
    protected virtual void SetDefaultValues(int index)
    {
        realDialogue.SetText(index, dialogue.sentences[index]); //Set Texts

        realDialogue.SetCustomTextWriteSpeed(index, dialogue.defTextWriteSpeeds[dialogue.characterCounts[index]]);
        realDialogue.SetCustomTextAudio(index, dialogue.defTextAudios[dialogue.characterCounts[index]]);
        realDialogue.SetCustomTextEffect(index, dialogue.defTextEffects[dialogue.characterCounts[index]]);
        realDialogue.SetCustomOverWrite(index, dialogue.defOverWrites);
        realDialogue.SetCustomDiffColor(index, dialogue.defDiffColor[dialogue.characterCounts[index]]);
    }
    
    /// <summary>
    ///   <para> Sets the dialogue Scriptable Object's custom values to realDialogue </para>
    /// <param name="index"> Corresponding sentences index </param>
    /// </summary>
    protected virtual void ControlCustomValues(int index)
    {
        if (index < dialogue.textWriteSpeeds.Count)
        {
            realDialogue.SetCustomTextWriteSpeed(dialogue.textWriteSpeeds[index].id, 
                dialogue.textWriteSpeeds[index].textWriteSpeed);
        }
        if (index < dialogue.textAudios.Count)
        {
            realDialogue.SetCustomTextAudio(dialogue.textAudios[index].id, 
                dialogue.textAudios[index].textAudio);
        }
        if (index < dialogue.textEffects.Count)
        {
            realDialogue.SetCustomTextEffect(dialogue.textEffects[index].id, 
                dialogue.textEffects[index].textEffect);
        }
        if (index < dialogue.overWrites.Count)
        {
            realDialogue.SetCustomOverWrite(dialogue.overWrites[index].id, 
                dialogue.overWrites[index].overWrite);
        }
        if (index < dialogue.diffColor.Count)
        {   
            realDialogue.SetCustomDiffColor(dialogue.diffColor[index].id, 
                dialogue.diffColor[index].diffColor);
        }
        if (index < dialogue.WordColorIndices.Count)
        {
            realDialogue.SetDiffWordColorDic(dialogue.WordColorIndices[index].id, 
                dialogue.WordColorIndices[index]);
        }
    }

    private void CheckAndStartTextEffect()
    {
        if (textEffect != ETextEffects.None && textEffectRoutine == null)
        {
            if (DialogueStopGame)
            {
                textEffectRoutine = StartCoroutine(UnScaledDoTextEffect());
            }
            else
            {
                textEffectRoutine = StartCoroutine(ScaledDoTextEffect());
            }
        }
    }

    private void CheckAndStartWordColorEffect()
    {

        if (wordColorIndex != null && changeWordColorRoutine == null)
        {
            changeWordColorRoutine = StartCoroutine(ScaledChangeWordColor());
        }
    }

    private IEnumerator ScaledDoTextEffect()
    {
        while (true)
        {
            if (StopTextEffect)
            {
                yield break;
            }
            
            TextEffectsController.Instance.DoTextEffect(dialogueHolderText, textEffect);
            yield return null;
        }
    }

    private IEnumerator UnScaledDoTextEffect()
    {
        while (true)
        {
            if (StopTextEffect)
            {
                yield break;
            }
            
            UnScaledTextEffectController.Instance.DoTextEffect(dialogueHolderText, textEffect);
            yield return null;
        }
    }

    private IEnumerator ScaledChangeWordColor()
    {
        while (true)
        {
            if (StopChangeColor)
            {
                yield break;
            }

            for (int i = 0; ( wordColorIndex != null && i < wordColorIndex.diffWordColorDics.Count ); i++)
            {
                foreach (int wordIndex in wordColorIndex.diffWordColorDics[i].wordId)
                {
                    TextColorController.Instance.ChangeWordColor(dialogueHolderText, wordIndex, 
                        wordColorIndex.diffWordColorDics[i].diffColor);
                }
            }

            yield return null;
        }
    }

}
