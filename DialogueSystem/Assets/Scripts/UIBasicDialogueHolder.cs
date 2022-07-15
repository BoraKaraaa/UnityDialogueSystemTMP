using UnityEngine;
using UnityEngine.UI;

public class UIBasicDialogueHolder : DialogueHolder
{

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

        foreach (string sentence in dialogue.sentences)
        {
            DialogueManager.Instance.oneDialogueQue.Enqueue(new UIOneBasicDialogue(dialogue.sentences[dialogueIndex],
                dialogue.textWriteSpeeds[dialogueIndex], dialogue.textAudios[dialogueIndex], dialogue.textEffects[dialogueIndex], dialogue.overWrite[dialogueIndex],
                ((UIBasicDialogue)dialogue).diffColorWordIndex[dialogueIndex],
                ((UIBasicDialogue)dialogue).diffColor[dialogueIndex]));

            dialogueIndex++;
        }
    }

    public override OneDialogue OnCustomDialogueActions()
    {
        UIOneBasicDialogue newUIOneDialogue = DialogueManager.Instance.oneDialogueQue.Dequeue() as UIOneBasicDialogue;

        diffColorWordIndex = newUIOneDialogue.diffColorWordIndex;
        color = newUIOneDialogue.diffColor;

        SetEtextEffects(newUIOneDialogue.textEffects);

        return newUIOneDialogue;
    }

    public override void OnOneDialogueEndActions()
    {
       
    }

    public override void OnEndDialogueActions()
    {
         this.gameObject.SetActive(false);
    }

}
