using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "UIDialogueData", menuName = "ScriptableObjects/UIDialogueDataScriptableObject", order = 1)]
public class UIDialogue : Dialogue
{
    public Image[] images;
}

public class UIOneDialogue : OneDialogue
{
    public Image image;

    public UIOneDialogue(string characterName, string sentence, float textWriteSpeed, AudioSource textAudio, ETextEffects textEffects, bool overWrite, Image image) 
        : base(characterName, sentence, textWriteSpeed, textAudio, textEffects, overWrite)
    {
        this.image = image;
    }

}