using UnityEngine;
using UnityEngine.UI;

public abstract class Dialogue : ScriptableObject
{
    public string charcterName;

    [TextArea(3, 10)]
    public string[] sentences;

    public float[] textWriteSpeeds;
    public AudioSource[] textAudios;
    public ETextEffects[] textEffects;

}

public abstract class OneDialogue
{
    public string charcterName;
    public string sentence;
    public float textWriteSpeed;
    public AudioSource textAudio;
    public ETextEffects textEffects;

    public OneDialogue(string characterName, string sentence, float textWriteSpeed, AudioSource textAudio, ETextEffects textEffects)
    {
        this.charcterName = characterName;
        this.sentence = sentence;
        this.textWriteSpeed = textWriteSpeed;
        this.textAudio = textAudio;
        this.textEffects = textEffects;
    }

}
