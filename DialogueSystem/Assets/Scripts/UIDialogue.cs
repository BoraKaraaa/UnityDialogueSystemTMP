using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "UIDialogueData", menuName = "ScriptableObjects/UIDialogueDataScriptableObject", order = 1)]
public class UIDialogue : Dialogue
{
    public Image[] images;
    public int[] diffColorWordIndex;
    public Color[] diffColor;

}

public class UIOneDialogue : OneDialogue
{
    public Image image;
    public int diffColorWordIndex;
    public Color diffColor;

    public UIOneDialogue(string sentence, float textWriteSpeed, AudioSource textAudio, ETextEffects textEffects, bool overWrite, Image image, int diffColorWordIndex, Color diffColor) 
        : base(sentence, textWriteSpeed, textAudio, textEffects, overWrite)
    {
        this.image = image;
        this.diffColorWordIndex = diffColorWordIndex;
        this.diffColor = diffColor;
    }

}