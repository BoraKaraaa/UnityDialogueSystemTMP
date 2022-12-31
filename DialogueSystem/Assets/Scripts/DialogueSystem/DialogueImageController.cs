using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueImageController : MonoBehaviour
{
    [SerializeField] private List<TextToSpriteKVP> textToSpriteKVPs;

    private Dictionary<string, string> wordToSpriteDictionary;

    private void Awake()
    {
        foreach (var keyValuePair in textToSpriteKVPs)
        {
            // map to each pairs value to proper string value
            
            
            //wordToSpriteDictionary.Add(keyValuePair.textKey, keyValuePair.spriteValue);
        }
    }
    public TMP_SpriteAsset tmpSprite;
    public bool TryToAddImage(string word) => wordToSpriteDictionary.ContainsKey(word);
    
    // Call Only After TryToAddImage
    public void AddImageAfterWord(string word)
    {
        
    }
    
}

[Serializable]
public class TextToSpriteKVP
{
    public string textKey;
    public TMP_SpriteAsset tmpSprite;
}
