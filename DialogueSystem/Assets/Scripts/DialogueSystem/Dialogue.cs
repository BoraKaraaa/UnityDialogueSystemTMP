using System.Collections.Generic;
using UnityEngine;

public abstract class Dialogue : ScriptableObject
{
    [Header("Dialogue Essential Values")] 
    [Space(10)]
    
    [TextArea(3, 10)]
    public string[] sentences;
    
    public int[] characterCounts;

    [Header("Dialogue Default Values")] 
    [Space(10)]
    public float defTextWriteSpeeds;
    public AudioClip defTextAudios;
    public ETextEffects defTextEffects;
    public bool defOverWrites;
    public int defDiffColorWordIndex;
    public Color defDiffColor;

    [Header("Dialogue Specific Values")]
    [Space(10)]
    public List<TextWriteSpeedDic> textWriteSpeeds;
    public List<AudioClipDic> textAudios;
    public List<ETextEffectsDic> textEffects;
    public List<OverWriteDic> overWrites;
    public List<DiffColorWordIndexDic> diffColorWordIndex;
    public List<DiffColorDic> diffColor;
    
}

public abstract class RealDialogue
{
    public string[] sentences;
    public float[] textWriteSpeeds;
    public AudioClip[] textAudios;
    public ETextEffects[] textEffects;
    public bool[] overWrite;
    public int[] diffColorWordIndex;
    public Color[] diffColor;

    private int arraysLength;
    public virtual void Init(Dialogue dialogue)
    { 
        arraysLength = dialogue.sentences.Length;
        
        sentences = new string[arraysLength];
        textWriteSpeeds = new float[arraysLength];
        textAudios = new AudioClip[arraysLength];
        textEffects = new ETextEffects[arraysLength];
        overWrite = new bool[arraysLength];
        diffColorWordIndex = new int[arraysLength];
        diffColor = new Color[arraysLength];
    }

    public void SetText(int index, string sentence)
    {
        sentences[index] = sentence;
    }

    public void SetCustomTextWriteSpeed(int index, float customTextWriteSpeed)
    {
        textWriteSpeeds[index] = customTextWriteSpeed;
    }
    
    public void SetCustomTextAudio(int index, AudioClip customTextAudio)
    {
        textAudios[index] = customTextAudio;
    }
    
    public void SetCustomTextEffect(int index, ETextEffects customTextEffect)
    {
        textEffects[index] = customTextEffect;
    }
    
    public void SetCustomOverWrite(int index, bool customOverWrite = false)
    {
        overWrite[index] = customOverWrite;
    }
    
    public void SetCustomDiffColorWordIndex(int index, int customDiffColorWordIndex)
    {
        this.diffColorWordIndex[index] = customDiffColorWordIndex;
    }
    
    public void SetCustomDiffColor(int index, Color customDiffColor)
    {
        this.diffColor[index] = customDiffColor;
    }

}