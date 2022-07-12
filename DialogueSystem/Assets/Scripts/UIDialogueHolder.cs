using UnityEngine;
using UnityEngine.UI;

public class UIDialogueHolder : DialogueHolder
{
    [SerializeField] private Image dialogueHolderImage;

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
            DialogueManager.Instance.oneDialogueQue.Enqueue(new UIOneDialogue(dialogue.characterName, dialogue.sentences[dialogueIndex],
                dialogue.textWriteSpeeds[dialogueIndex], dialogue.textAudios[dialogueIndex], dialogue.textEffects[dialogueIndex], dialogue.overWrite[dialogueIndex],
                ((UIDialogue)dialogue).images[dialogueIndex], ((UIDialogue)dialogue).diffColorWordIndex[dialogueIndex], ((UIDialogue)dialogue).diffColor[dialogueIndex]));

            dialogueIndex++;
        }
    }

    public override OneDialogue OnCustomDialogueActions()
    {
        UIOneDialogue newUIOneDialogue =  DialogueManager.Instance.oneDialogueQue.Dequeue() as UIOneDialogue;

        dialogueHolderImage.sprite = newUIOneDialogue.image.sprite;

        diffColorWordIndex = newUIOneDialogue.diffColorWordIndex;
        color = newUIOneDialogue.diffColor;

        SetEtextEffects(newUIOneDialogue.textEffects);

        if (newUIOneDialogue.image.GetComponent<Animator>() != null)
            newUIOneDialogue.image.GetComponent<Animator>().enabled = true;

        return newUIOneDialogue;
    }

    public override void OnEndDialogueActions()
    {
        this.gameObject.SetActive(false);
    }


}
