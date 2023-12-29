using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public enum ETextEffectTimeVariable
{
    SCALED,
    UNSCALED
}

public class TextEffectsController : Singleton<TextEffectsController>
{
    private const int TMP_PRO_VERTICES = 4;

    private int neg = 1;
    private float increaseAmount = 0;

    private const int RESET_VERTEX_VALUE = 500;

    public void DoTextEffect(TMP_Text dialogueHolderText, ETextEffects textEffect, ETextEffectTimeVariable textEffectTimeVariable, int resetCounter)
    {
        float TIME_VARIABLE = textEffectTimeVariable == ETextEffectTimeVariable.SCALED ? Time.time : Time.unscaledTime;

        if (resetCounter % RESET_VERTEX_VALUE == 0)
        {
            dialogueHolderText.ForceMeshUpdate();
        }

        var textInfo = dialogueHolderText.textInfo;
        
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible)
            {
                continue;
            }

            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            if (textEffect == ETextEffects.Wiggle)
            {
                WiggleTextEffect(verts, charInfo, TIME_VARIABLE);
            }
            else if (textEffect == ETextEffects.Vibration)
            {
                VibrationTextEffect(verts, charInfo, TIME_VARIABLE);
            }
            else if (textEffect == ETextEffects.VibrationV2)
            {
                VibrationTextEffectV2(verts, charInfo, TIME_VARIABLE);
            }
            else if (textEffect == ETextEffects.Rage)
            {
                RageTextEffect(verts, charInfo, TIME_VARIABLE);
            }
            else if (textEffect == ETextEffects.Glitch)
            {
                GlitchTextEffect(verts, charInfo, TIME_VARIABLE);
            }
            else if (textEffect == ETextEffects.LightGlitch)
            {
                LightGlitchTextEffect(verts, charInfo, TIME_VARIABLE);
            }
        }

        ChangeWorkingCopy(dialogueHolderText, textInfo);
    }

    public void DoTextEffect(TMP_Text dialogueHolderText, int wordColorIndex, ETextEffects textEffect, ETextEffectTimeVariable textEffectTimeVariable, int resetCounter)
    {
        float TIME_VARIABLE = textEffectTimeVariable == ETextEffectTimeVariable.SCALED ? Time.time : Time.unscaledTime;
        
        if (resetCounter % RESET_VERTEX_VALUE == 0)
        {
            dialogueHolderText.ForceMeshUpdate();
        }
        
        var textInfo = dialogueHolderText.textInfo;

        if (wordColorIndex <= textInfo.wordCount - 1)
        {
            ChangeWorkingCopy(dialogueHolderText, textInfo);
                
            for (int i = textInfo.wordInfo[wordColorIndex].firstCharacterIndex;
                 i <= textInfo.wordInfo[wordColorIndex].lastCharacterIndex; i++)
            {
                var charInfo = textInfo.characterInfo[i];

                if (!charInfo.isVisible)
                {
                    continue;
                }
                
                var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

                if (textEffect == ETextEffects.Wiggle)
                {
                    WiggleTextEffect(verts, charInfo, TIME_VARIABLE);
                }
                else if (textEffect == ETextEffects.Vibration)
                {
                    VibrationTextEffect(verts, charInfo, TIME_VARIABLE);
                }
                else if (textEffect == ETextEffects.VibrationV2)
                {
                    VibrationTextEffectV2(verts, charInfo, TIME_VARIABLE);
                }
                else if (textEffect == ETextEffects.Rage)
                {
                    RageTextEffect(verts, charInfo, TIME_VARIABLE);
                }
                else if (textEffect == ETextEffects.Glitch)
                {
                    GlitchTextEffect(verts, charInfo, TIME_VARIABLE);
                }
                else if (textEffect == ETextEffects.LightGlitch)
                {
                    LightGlitchTextEffect(verts, charInfo, TIME_VARIABLE);
                }
            }
            
            ChangeWorkingCopy(dialogueHolderText, textInfo);
        }
    }

    private void ChangeWorkingCopy(TMP_Text dialogueHolderText, TMP_TextInfo textInfo)
    {
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var meshInfo = textInfo.meshInfo[i];

            if (meshInfo.mesh != null)
            {
                meshInfo.mesh.vertices = meshInfo.vertices;
                dialogueHolderText.UpdateGeometry(meshInfo.mesh, i);
            }
        }
    }

    private void WiggleTextEffect(Vector3[] verts, TMP_CharacterInfo charInfo, float TIME_VARIABLE)
    {
        neg *= -1;

        for (int j = 0; j < TMP_PRO_VERTICES; j++)
        {
            var orig = verts[charInfo.vertexIndex + j];
            orig.z = 0;
            verts[charInfo.vertexIndex + j] = orig + Vector3.right * (Mathf.Cos(TIME_VARIABLE * 2 + orig.x * 0.7f) * 3 * neg)
                                                   + Vector3.up * (Mathf.Sin(TIME_VARIABLE * 8) * 0.6f * neg);
        }
    }

    private void VibrationTextEffect(Vector3[] verts, TMP_CharacterInfo charInfo, float TIME_VARIABLE)
    {
        neg *= -1;

        for (int j = 0; j < TMP_PRO_VERTICES; j++)
        {
            var orig = verts[charInfo.vertexIndex + j];
            orig.z = 0;
            verts[charInfo.vertexIndex + j] = orig + Vector3.right * (Mathf.Sin(TIME_VARIABLE * 80) * 0.6f * neg)
                                                   + Vector3.up * (Mathf.Sin(TIME_VARIABLE * 60) * 0.6f * neg);
        }
    }

    private void VibrationTextEffectV2(Vector3[] verts, TMP_CharacterInfo charInfo, float TIME_VARIABLE)
    {
        neg *= -1;

        for (int j = 0; j < TMP_PRO_VERTICES; j++)
        {
            var orig = verts[charInfo.vertexIndex + j];
            orig.z = 0;
            verts[charInfo.vertexIndex + j] = orig + Vector3.right * (Mathf.Sin(TIME_VARIABLE * 2 + orig.x * 2) * 3 * neg)
                                                   + Vector3.up * (Mathf.Sin(TIME_VARIABLE * 3 + orig.x * 1.5f) * 4f * neg);
        }
    }

    private void RageTextEffect(Vector3[] verts, TMP_CharacterInfo charInfo, float TIME_VARIABLE)
    {
        neg *= -1;

        if (increaseAmount < 5)
        {
            increaseAmount += Time.deltaTime / 50;
        }

        for (int j = 0; j < TMP_PRO_VERTICES; j++)
        {
            var orig = verts[charInfo.vertexIndex + j];
            orig.z = 0;
            verts[charInfo.vertexIndex + j] = orig + Vector3.right * (Mathf.Tan(TIME_VARIABLE + orig.x * increaseAmount) * increaseAmount * neg)
                                                   + Vector3.up * (Mathf.Tan(TIME_VARIABLE + orig.x * increaseAmount) * increaseAmount * neg);
        }
    }

    private void GlitchTextEffect(Vector3[] verts, TMP_CharacterInfo charInfo, float TIME_VARIABLE)
    {
        neg *= -1;

        for (int j = 0; j < TMP_PRO_VERTICES; j++)
        {
            var orig = verts[charInfo.vertexIndex + j];
            orig.z = 0;
            verts[charInfo.vertexIndex + j] = orig + Vector3.right * (Mathf.Tan(TIME_VARIABLE + orig.x * 50) * 0.1f * neg)
                                                   + Vector3.up * (Mathf.Tan(TIME_VARIABLE + orig.x * 50) * 0.1f * neg);
        }
    }

    private void LightGlitchTextEffect(Vector3[] verts, TMP_CharacterInfo charInfo, float TIME_VARIABLE)
    {
        neg *= -1;

        for (int j = 0; j < TMP_PRO_VERTICES; j++)
        {
            var orig = verts[charInfo.vertexIndex + j];
            orig.z = 0;
            verts[charInfo.vertexIndex + j] = orig + Vector3.right * (Mathf.Tan(TIME_VARIABLE * 2 + orig.x) * 0.01f * neg)
                                                   + Vector3.up * (Mathf.Tan(TIME_VARIABLE * 2 + orig.x) * 0.01f * neg);
        }
    }


}
