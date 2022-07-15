using UnityEngine;

[CreateAssetMenu(fileName = "UIDBasicialogueData", menuName = "ScriptableObjects/UIBasicDialogueDataScriptableObject", order = 1)]
public class UIBasicDialogue : Dialogue
{
    public int[] diffColorWordIndex;
    public Color[] diffColor;
}

public class UIOneBasicDialogue : OneDialogue
{
    public int diffColorWordIndex;
    public Color diffColor;

    public UIOneBasicDialogue(string sentence, float textWriteSpeed, AudioClip textAudio, ETextEffects textEffects, bool overWrite, int diffColorWordIndex, Color diffColor)
        : base(sentence, textWriteSpeed, textAudio, textEffects, overWrite)
    {
        this.diffColorWordIndex = diffColorWordIndex;
        this.diffColor = diffColor;
    }

}