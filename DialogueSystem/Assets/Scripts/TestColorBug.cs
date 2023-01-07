using System;
using TMPro;
using UnityEngine;
using System.Collections;

public class TestColorBug : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private int index;
    
    void Start()
    {
        //ChangeWordColor(text, index, Color.red);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ChangeWordColor(text, index, Color.red);
        }
    }

    public void ChangeWordColor(TMP_Text dialogueHolderText, int wordColorIndex, Color color)
    {
        var textInfo = dialogueHolderText.textInfo;

        if(wordColorIndex <= textInfo.wordCount-1)
        {
            Debug.Log(textInfo.wordInfo[wordColorIndex].GetWord());
            Debug.Log(textInfo.wordInfo[wordColorIndex].firstCharacterIndex);
            Debug.Log(textInfo.wordInfo[wordColorIndex].lastCharacterIndex);

            for (int i = textInfo.wordInfo[wordColorIndex].firstCharacterIndex; i <= textInfo.wordInfo[wordColorIndex].lastCharacterIndex; i++)
            {
                var charInfo = textInfo.characterInfo[i];
                
                Debug.Log(charInfo.index);

                int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                Color32[] vertexColors = textInfo.meshInfo[charInfo.materialReferenceIndex].colors32;

                vertexColors[vertexIndex + 0] = color;
                vertexColors[vertexIndex + 1] = color;
                vertexColors[vertexIndex + 2] = color;
                vertexColors[vertexIndex + 3] = color;
            }
        }

        dialogueHolderText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

    }
    
}
