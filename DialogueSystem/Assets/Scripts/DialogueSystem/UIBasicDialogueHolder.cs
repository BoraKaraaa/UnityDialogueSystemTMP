using UnityEngine;

public class UIBasicDialogueHolder : DialogueHolder
{
    private RealUIBasicDialogue realUIBasicDialogue;
    private UIBasicDialogue uiBasicDialogue;

    public override void OnStartDialogueActions(Dialogue dialogue)
    {
        if (DialogueStopGame)
        {
            DialogueManager.Instance.DialogueStopGame = true;
            Time.timeScale = 0f;
        }
        
        this.gameObject.SetActive(true);
        
        HolderOnStartDialogueActions?.Invoke();
        
        realUIBasicDialogue = new RealUIBasicDialogue();
        
        realUIBasicDialogue.Init(dialogue);
        
        uiBasicDialogue = dialogue as UIBasicDialogue;
        
        
        StopTextEffect = false;
        StopChangeColor = false;
        
        CheckAndStartTextEffect();
    }
    
    private void SetDefaultValues(int index)
    {
        realUIBasicDialogue.SetText(index, uiBasicDialogue.sentences[index]); //Set Texts

        realUIBasicDialogue.SetCustomTextWriteSpeed(index, uiBasicDialogue.defTextWriteSpeeds);
        realUIBasicDialogue.SetCustomTextAudio(index, uiBasicDialogue.defTextAudios);
        realUIBasicDialogue.SetCustomTextEffect(index, uiBasicDialogue.defTextEffects);
        realUIBasicDialogue.SetCustomOverWrite(index, uiBasicDialogue.defOverWrites);
        realUIBasicDialogue.SetCustomDiffColorWordIndex(index, uiBasicDialogue.defDiffColorWordIndex);
        realUIBasicDialogue.SetCustomDiffColor(index, uiBasicDialogue.defDiffColor);
    }

    private void ControlCustomValues(int index)
    {
        if (index < uiBasicDialogue.textWriteSpeeds.Count)  
            realUIBasicDialogue.SetCustomTextWriteSpeed(uiBasicDialogue.textWriteSpeeds[index].id, uiBasicDialogue.textWriteSpeeds[index].textWriteSpeed);
        if (index < uiBasicDialogue.textAudios.Count)  
            realUIBasicDialogue.SetCustomTextAudio(uiBasicDialogue.textAudios[index].id, uiBasicDialogue.textAudios[index].textAudio);
        if (index < uiBasicDialogue.textEffects.Count)  
            realUIBasicDialogue.SetCustomTextEffect(uiBasicDialogue.textEffects[index].id, uiBasicDialogue.textEffects[index].textEffect);
        if (index < uiBasicDialogue.overWrites.Count)  
            realUIBasicDialogue.SetCustomOverWrite(uiBasicDialogue.overWrites[index].id, uiBasicDialogue.overWrites[index].overWrite);
        if (index < uiBasicDialogue.diffColorWordIndex.Count)  
            realUIBasicDialogue.SetCustomDiffColorWordIndex(uiBasicDialogue.diffColorWordIndex[index].id, uiBasicDialogue.diffColorWordIndex[index].diffColorWordIndex);
        if (index < uiBasicDialogue.diffColor.Count)  
            realUIBasicDialogue.SetCustomDiffColor(uiBasicDialogue.diffColor[index].id, uiBasicDialogue.diffColor[index].diffColor);
    }

    public override RealDialogue OnCustomDialogueActions(int index)
    {
        SetDefaultValues(index);
        ControlCustomValues(index);
        
        HolderOnCustomDialogueActions?.Invoke(realUIBasicDialogue, index);
        
        diffColorWordIndex = realUIBasicDialogue.diffColorWordIndex[index];
        color = realUIBasicDialogue.diffColor[index];

        SetEtextEffects(realUIBasicDialogue.textEffects[index]);

        return realUIBasicDialogue;
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
    
    
    

}
