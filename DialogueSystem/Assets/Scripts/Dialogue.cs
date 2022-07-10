using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DialogueData", menuName = "ScriptableObjects/DialogueDataScriptableObject", order = 1)]
public class Dialogue : ScriptableObject
{
    public string charcterName;

    [TextArea(3, 10)]
    public string[] sentences;

    public Image[] images;
    public float[] textWriteSpeeds;
    public AudioSource[] textAudios;
    public bool lastDialogueInRow;
    public ETextEffects[] textEffects;

}

public class OneDialogue
{
    public string charcterName;
    public string sentence;
    public Image image;
    public float textWriteSpeed;
    public AudioSource textAudio;

    public OneDialogue(string characterName, string sentence, Image image, float textWriteSpeed, AudioSource textAudio)
    {
        this.charcterName = characterName;
        this.sentence = sentence;
        this.image = image;
        this.textWriteSpeed = textWriteSpeed;
        this.textAudio = textAudio;
    }

}
