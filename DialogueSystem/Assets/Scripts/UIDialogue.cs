using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "UIDialogueData", menuName = "ScriptableObjects/UIDialogueDataScriptableObject", order = 1)]
public class UIDialogue : Dialogue
{
    public Image[] images;
    public int[] wordIndexToChangeColor;
}

public class UIOneDialogue : OneDialogue
{
    public Image image;
    //public int wordIndexToChangeColor;

    public UIOneDialogue(string characterName, string sentence, float textWriteSpeed, AudioSource textAudio, ETextEffects textEffects, Image image) 
        : base(characterName, sentence, textWriteSpeed, textAudio, textEffects)
    {
        this.image = image;

        //this.wordIndexToChangeColor = wordIndexToChangeColor;
    }

}