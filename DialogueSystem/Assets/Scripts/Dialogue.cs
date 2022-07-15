using UnityEngine;

public abstract class Dialogue : ScriptableObject
{
    [TextArea(3, 10)]
    public string[] sentences;

    public float[] textWriteSpeeds;
    public AudioClip[] textAudios;
    public ETextEffects[] textEffects;
    public bool[] overWrite;

}

public abstract class OneDialogue
{
    public string sentence;
    public float textWriteSpeed;
    public AudioClip textAudio;
    public ETextEffects textEffects;
    public bool overWrite;

    public OneDialogue(string sentence, float textWriteSpeed, AudioClip textAudio, ETextEffects textEffects, bool overWrite)
    {
        this.sentence = sentence;
        this.textWriteSpeed = textWriteSpeed;
        this.textAudio = textAudio;
        this.textEffects = textEffects;
        this.overWrite = overWrite;
    }

}
