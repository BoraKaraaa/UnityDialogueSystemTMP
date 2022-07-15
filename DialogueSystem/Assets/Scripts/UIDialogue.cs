using UnityEngine;

[CreateAssetMenu(fileName = "UIDialogueData", menuName = "ScriptableObjects/UIDialogueDataScriptableObject", order = 1)]
public class UIDialogue : Dialogue
{
    public int[] characterCounts;
    public Sprite[] sprites;
    public RuntimeAnimatorController[] animators;
    public string[] animatorStateNames;
    public int[] diffColorWordIndex;
    public Color[] diffColor;

}

public class UIOneDialogue : OneDialogue
{
    public int characterCount;
    public Sprite sprite;
    public RuntimeAnimatorController animator;
    public string animatorStateName;
    public int diffColorWordIndex;
    public Color diffColor;

    public UIOneDialogue(string sentence, float textWriteSpeed, AudioClip textAudio, ETextEffects textEffects, bool overWrite, Sprite sprite, RuntimeAnimatorController animator,  
        string animatorStateName, int diffColorWordIndex, Color diffColor) 
        : base(sentence, textWriteSpeed, textAudio, textEffects, overWrite)
    {
        this.sprite = sprite;
        this.animator = animator;
        this.animatorStateName = animatorStateName;
        this.diffColorWordIndex = diffColorWordIndex;
        this.diffColor = diffColor;
    }

}