using UnityEngine;
using UnityEngine.UI;

public class UIDialogueHolder : DialogueHolder
{
    [SerializeField] private Image dialogueHolderImage;
    [SerializeField] private Animator dialogueAnimator;

    private RealUIDialogue realUIDialogue;
    private UIDialogue uiDialogue;

    public override void OnStartDialogueActions(Dialogue dialogue)
    {
        if (DialogueStopGame)
        {
            DialogueManager.Instance.DialogueStopGame = true;
            Time.timeScale = 0f;
        }

        this.gameObject.SetActive(true);
        
        HolderOnStartDialogueActions?.Invoke();
        
        realUIDialogue = new RealUIDialogue();
        
        realUIDialogue.Init(dialogue);
        
        uiDialogue = dialogue as UIDialogue;
        
        StopTextEffect = false;
        StopChangeColor = false;
        
        CheckAndStartTextEffect();
    }

    private void SetDefaultValues(int index)
    { 
        realUIDialogue.SetText(index, uiDialogue.sentences[index]); //Set Texts

        realUIDialogue.SetCustomTextWriteSpeed(index, uiDialogue.defTextWriteSpeeds[uiDialogue.characterCounts[index]]);
        realUIDialogue.SetCustomTextAudio(index, uiDialogue.defTextAudios[uiDialogue.characterCounts[index]]);
        realUIDialogue.SetCustomTextEffect(index, uiDialogue.defTextEffects[uiDialogue.characterCounts[index]]);
        realUIDialogue.SetCustomOverWrite(index, uiDialogue.defOverWrites);
        realUIDialogue.SetCustomAnimatorStateName(index, uiDialogue.defAnimatorStateNames);
        realUIDialogue.SetCustomDiffColorWordIndex(index, uiDialogue.defDiffColorWordIndex[uiDialogue.characterCounts[index]]);
        realUIDialogue.SetCustomDiffColor(index, uiDialogue.defDiffColor[uiDialogue.characterCounts[index]]);
    }

    private void ControlCustomValues(int index)
    {
        if (index < uiDialogue.textWriteSpeeds.Count)  
            realUIDialogue.SetCustomTextWriteSpeed(uiDialogue.textWriteSpeeds[index].id, uiDialogue.textWriteSpeeds[index].textWriteSpeed);
        if (index < uiDialogue.textAudios.Count)  
            realUIDialogue.SetCustomTextAudio(uiDialogue.textAudios[index].id, uiDialogue.textAudios[index].textAudio);
        if (index < uiDialogue.textEffects.Count)  
            realUIDialogue.SetCustomTextEffect(uiDialogue.textEffects[index].id, uiDialogue.textEffects[index].textEffect);
        if (index < uiDialogue.overWrites.Count)  
            realUIDialogue.SetCustomOverWrite(uiDialogue.overWrites[index].id, uiDialogue.overWrites[index].overWrite);
        if (index < uiDialogue.animatorStateNames.Count)  
            realUIDialogue.SetCustomAnimatorStateName(uiDialogue.animatorStateNames[index].id, uiDialogue.animatorStateNames[index].animationStateName);
        if (index < uiDialogue.diffColorWordIndex.Count)  
            realUIDialogue.SetCustomDiffColorWordIndex(uiDialogue.diffColorWordIndex[index].id, uiDialogue.diffColorWordIndex[index].diffColorWordIndex);
        if (index < uiDialogue.diffColor.Count)  
            realUIDialogue.SetCustomDiffColor(uiDialogue.diffColor[index].id, uiDialogue.diffColor[index].diffColor);
  
    }

    public override RealDialogue OnCustomDialogueActions(int index)
    {
        SetDefaultValues(index);
        ControlCustomValues(index);
        
        HolderOnCustomDialogueActions?.Invoke(realUIDialogue, index);
        
        dialogueHolderImage.sprite = uiDialogue.sprites[uiDialogue.characterCounts[index]];
        dialogueAnimator.runtimeAnimatorController = uiDialogue.animators[uiDialogue.characterCounts[index]];

        dialogueAnimator.Play(realUIDialogue.animatorStateNames[index]);

        diffColorWordIndex = realUIDialogue.diffColorWordIndex[index];
        color = realUIDialogue.diffColor[index];

        SetEtextEffects(realUIDialogue.textEffects[index]);
        
        return realUIDialogue;
    }

    public override void OnOneDialogueEndActions()
    {
        dialogueAnimator.Play("NotTalking");
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
