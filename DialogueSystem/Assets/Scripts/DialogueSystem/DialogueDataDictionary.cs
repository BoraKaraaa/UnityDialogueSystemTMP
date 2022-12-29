using System.Collections.Generic;
using UnityEngine;

public class DialogueDataDictionary
{

}

/// <summary>
///   <para>Dialogue Data in dictionary structure for using initialize dialogue</para>
/// </summary>

/// Dialogue Data

[System.Serializable]
public class TextWriteSpeedDic {
    public int id;
    public float textWriteSpeed;
}

[System.Serializable]
public class AudioClipDic {
    public int id;
    public AudioClip textAudio;
}

[System.Serializable]
public class ETextEffectsDic {
    public int id;
    public ETextEffects textEffect;
}

[System.Serializable]
public class OverWriteDic {
    public int id;
    public bool overWrite;
}

[System.Serializable]
public class DiffColorDic {
    public int id;
    public Color diffColor;
}

[System.Serializable]
public class WordColorIndex
{
    public int id;
    public List<DiffColorDic> aaa;
}

/// Additional Dialogue Data

[System.Serializable]
public class AnimatorStateNamesDic {
    public int id;
    public string animationStateName;
}