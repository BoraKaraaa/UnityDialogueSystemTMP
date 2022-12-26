using UnityEngine;

public class UISimpDialogueHolder : DialogueHolder
{
    [SerializeField] private Animator dialogueAnimator;

    private RealUISimpDialogue realUISimpDialogue;
    private UISimpDialogue uiSimpDialogue;

    public override void OnStartDialogueActions(Dialogue dialogue)
    {
        if (DialogueStopGame)
        {
            DialogueManager.Instance.DialogueStopGame = true;
            Time.timeScale = 0f;
        }
        
        dialogueAnimator.enabled = true;
        
        this.gameObject.SetActive(true);
        
        HolderOnStartDialogueActions?.Invoke();
            
        realUISimpDialogue = new RealUISimpDialogue();
        
        realUISimpDialogue.Init(dialogue);
        
        uiSimpDialogue = dialogue as UISimpDialogue;
        
        
        StopTextEffect = false;
        StopChangeColor = false;
        
        CheckAndStartTextEffect();
    }
    
    private void SetDefaultValues(int index)
    {
        realUISimpDialogue.SetText(index, uiSimpDialogue.sentences[index]); //Set Texts
        
        realUISimpDialogue.SetCustomTextWriteSpeed(index, uiSimpDialogue.defTextWriteSpeeds);
        realUISimpDialogue.SetCustomTextAudio(index, uiSimpDialogue.defTextAudios);
        realUISimpDialogue.SetCustomTextEffect(index, uiSimpDialogue.defTextEffects);
        realUISimpDialogue.SetCustomOverWrite(index, uiSimpDialogue.defOverWrites);
        realUISimpDialogue.SetCustomAnimatorStateName(index, uiSimpDialogue.defAnimatorStateNames);
        realUISimpDialogue.SetCustomDiffColorWordIndex(index, uiSimpDialogue.defDiffColorWordIndex);
        realUISimpDialogue.SetCustomDiffColor(index, uiSimpDialogue.defDiffColor);
    }

    private void ControlCustomValues(int index)
    {
        if (index < uiSimpDialogue.textWriteSpeeds.Count)  
            realUISimpDialogue.SetCustomTextWriteSpeed(uiSimpDialogue.textWriteSpeeds[index].id, uiSimpDialogue.textWriteSpeeds[index].textWriteSpeed);
        if (index < uiSimpDialogue.textAudios.Count)  
            realUISimpDialogue.SetCustomTextAudio(uiSimpDialogue.textAudios[index].id, uiSimpDialogue.textAudios[index].textAudio);
        if (index < uiSimpDialogue.textEffects.Count)  
            realUISimpDialogue.SetCustomTextEffect(uiSimpDialogue.textEffects[index].id, uiSimpDialogue.textEffects[index].textEffect);
        if (index < uiSimpDialogue.overWrites.Count)  
            realUISimpDialogue.SetCustomOverWrite(uiSimpDialogue.overWrites[index].id, uiSimpDialogue.overWrites[index].overWrite);
        if (index < uiSimpDialogue.diffColorWordIndex.Count)  
            realUISimpDialogue.SetCustomDiffColorWordIndex(uiSimpDialogue.diffColorWordIndex[index].id, uiSimpDialogue.diffColorWordIndex[index].diffColorWordIndex);
        if (index < uiSimpDialogue.diffColor.Count)  
            realUISimpDialogue.SetCustomDiffColor(uiSimpDialogue.diffColor[index].id, uiSimpDialogue.diffColor[index].diffColor);
        if (index < uiSimpDialogue.animatorStateNames.Count)  
            realUISimpDialogue.SetCustomAnimatorStateName(uiSimpDialogue.animatorStateNames[index].id, uiSimpDialogue.animatorStateNames[index].animationStateName);
    }

    public override RealDialogue OnCustomDialogueActions(int index)
    {
        SetDefaultValues(index);
        ControlCustomValues(index);
        
        HolderOnCustomDialogueActions?.Invoke(realUISimpDialogue, index);
        
        diffColorWordIndex = realUISimpDialogue.diffColorWordIndex[index];
        color = realUISimpDialogue.diffColor[index];
        
        dialogueAnimator.runtimeAnimatorController = uiSimpDialogue.animators[uiSimpDialogue.characterCounts[index]];
        
        dialogueAnimator.Play(realUISimpDialogue.animatorStateNames[index]);
        
        SetEtextEffects(realUISimpDialogue.textEffects[index]);

        return realUISimpDialogue;
    }

    public override void OnOneDialogueEndActions()
    {
        HolderOnOneDialogueEndActions?.Invoke();
    }

    public override void OnEndDialogueActions()
    {
        if (DialogueStopGame)
        {
            DialogueManager.Instance.DialogueStopGame = false;
            Time.timeScale = 0f;
        }
        
        if (dialogueEnd == EDialogueEnd.None)
        {
            gameObject.SetActive(false);
        }
        else if (dialogueEnd == EDialogueEnd.NextDialogue)
        {
            DialogueTrigger.Instance.TriggerDialogue();
            gameObject.SetActive(false);
        }
        else if (dialogueEnd == EDialogueEnd.LoadScene)
        {
            
        }
        
        StopTextEffect = true;
        StopChangeColor = true;
        HolderOnEndDialogueActions?.Invoke();
    }

}
