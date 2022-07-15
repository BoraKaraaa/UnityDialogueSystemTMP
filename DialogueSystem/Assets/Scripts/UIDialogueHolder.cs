using UnityEngine;
using UnityEngine.UI;

public class UIDialogueHolder : DialogueHolder
{
    [SerializeField] private Image dialogueHolderImage;
    [SerializeField] private Animator dialogueAnimator;

    private Color color = Color.white;
    private int diffColorWordIndex = -1; 
    
    public void Update()
    {
        if (textEffect != ETextEffects.None)
            TextEffectsController.Instance.DoTextEffect(dialogueHolderText, textEffect);

        if (diffColorWordIndex != -1)
            TextEffectsController.Instance.ChangeWordColor(dialogueHolderText, diffColorWordIndex, color);

    }

    public override void OnStartDialogueActions(Dialogue dialogue)
    {
        this.gameObject.SetActive(true);

        int dialogueIndex = 0;

        UIDialogue UIDialogue = dialogue as UIDialogue;

        foreach (string sentence in dialogue.sentences)
        {
            DialogueManager.Instance.oneDialogueQue.Enqueue(new UIOneDialogue(UIDialogue.sentences[dialogueIndex],
                UIDialogue.textWriteSpeeds[dialogueIndex], UIDialogue.textAudios[dialogueIndex], UIDialogue.textEffects[dialogueIndex], UIDialogue.overWrite[dialogueIndex],
                UIDialogue.sprites[UIDialogue.characterCounts[dialogueIndex]], UIDialogue.animators[UIDialogue.characterCounts[dialogueIndex]], UIDialogue.animatorStateNames[dialogueIndex],
                UIDialogue.diffColorWordIndex[dialogueIndex], UIDialogue.diffColor[dialogueIndex]));

            dialogueIndex++;
        }
    }

    public override OneDialogue OnCustomDialogueActions()
    {
        UIOneDialogue newUIOneDialogue =  DialogueManager.Instance.oneDialogueQue.Dequeue() as UIOneDialogue;

        dialogueHolderImage.sprite = newUIOneDialogue.sprite;
        dialogueAnimator.runtimeAnimatorController = newUIOneDialogue.animator;

        dialogueAnimator.Play(newUIOneDialogue.animatorStateName);

        diffColorWordIndex = newUIOneDialogue.diffColorWordIndex;
        color = newUIOneDialogue.diffColor;

        SetEtextEffects(newUIOneDialogue.textEffects);

        return newUIOneDialogue;
    }

    public override void OnOneDialogueEndActions()
    {
        dialogueAnimator.Play("NotTalking");
    }

    public override void OnEndDialogueActions()
    {
        DialogueTrigger.Instance.TriggerDialogue();
        //this.gameObject.SetActive(false);
    }

}
