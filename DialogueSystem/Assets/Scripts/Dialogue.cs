using UnityEngine;

public abstract class Dialogue : ScriptableObject
{
    public string characterName;

    [TextArea(3, 10)]
    public string[] sentences;

    public float[] textWriteSpeeds;
    public AudioSource[] textAudios;
    public ETextEffects[] textEffects;
    public bool[] overWrite;

}

public abstract class OneDialogue
{
    public string characterName;
    public string sentence;
    public float textWriteSpeed;
    public AudioSource textAudio;
    public ETextEffects textEffects;
    public bool overWrite;

    public OneDialogue(string characterName, string sentence, float textWriteSpeed, AudioSource textAudio, ETextEffects textEffects, bool overWrite)
    {
        this.characterName = characterName;
        this.sentence = sentence;
        this.textWriteSpeed = textWriteSpeed;
        this.textAudio = textAudio;
        this.textEffects = textEffects;
        this.overWrite = overWrite;
    }

}
