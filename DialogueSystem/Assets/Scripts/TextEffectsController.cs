using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextEffectsController : MonoBehaviour
{
    private static TextEffectsController _instance;
    public static TextEffectsController Instance { get { return _instance; } }

    private const int TMP_PRO_VERTICES = 4;

    private ETextEffects textEffect = ETextEffects.None;

    //private int wordColorIndex = 0;
    private int k = 1;

    private float increaseAmount = 0;

    // CUSTOM TESTING VARIABLES 

    [SerializeField] float A;
    [SerializeField] float B;
    [SerializeField] float C;

    [SerializeField] float D;
    [SerializeField] float E;
    [SerializeField] float F;

    // --------------------------

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    /*
    private void ChangeWordColor()
    {
        dialogueHolderText.ForceMeshUpdate();

        var textInfo = dialogueHolderText.textInfo;

        for (int i = 0; i < textInfo.wordCount; i++)
        {
            if (i == wordColorIndex)
            {
                for (int j = 0; j < textInfo.wordInfo[wordColorIndex].characterCount; j++)
                {
                    var charInfo = textInfo.characterInfo[j];

                    int vertexIndex = textInfo.characterInfo[charInfo.index].vertexIndex;

                    Color32[] vertexColors = textInfo.meshInfo[charInfo.materialReferenceIndex].colors32;
                    vertexColors[vertexIndex + 0] = Color.red;
                    vertexColors[vertexIndex + 1] = Color.red;
                    vertexColors[vertexIndex + 2] = Color.red;
                    vertexColors[vertexIndex + 3] = Color.red;
                }

            }
        }

        dialogueHolderText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

    }
    */

    public void DoTextEffect(TMP_Text dialogueHolderText)
    {

        dialogueHolderText.ForceMeshUpdate();

        var textInfo = dialogueHolderText.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible)
                continue;

            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            if (textEffect == ETextEffects.Wiggle)
                WiggleTextEffect(verts, charInfo);
            else if (textEffect == ETextEffects.Vibration)
                VibrationTextEffect(verts, charInfo);
            else if (textEffect == ETextEffects.Rage)
                RageTextEffect(verts, charInfo);
            else if (textEffect == ETextEffects.Glitch)
                GlitchTextEffect(verts, charInfo);
            else if (textEffect == ETextEffects.LightGlitch)
                LightGlitchTextEffect(verts, charInfo);
            else if (textEffect == ETextEffects.PingPong)
                PingPongTextEffect(verts, charInfo);
            else if (textEffect == ETextEffects.Custom) // CUSTOMIZE
                Custom(verts, charInfo);

        }

        ChangeWorkingCopy(dialogueHolderText, textInfo);
    }

    public void ChangeWorkingCopy(TMP_Text dialogueHolderText, TMP_TextInfo textInfo)
    {
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            dialogueHolderText.UpdateGeometry(meshInfo.mesh, i);
        }

    }

    public void SetEtextEffects(ETextEffects textEffect)
    {
        this.textEffect = textEffect;
    }

    private void WiggleTextEffect(Vector3[] verts, TMP_CharacterInfo charInfo)
    {
        k *= -1;

        for (int j = 0; j < TMP_PRO_VERTICES; j++)
        {
            var orig = verts[charInfo.vertexIndex + j];
            verts[charInfo.vertexIndex + j] = orig + new Vector3(Mathf.Cos(Time.time * 2 + orig.x * 0.7f) * 3 * k, Mathf.Sin(Time.time * 8) * 0.6f * k, 0);
        }
    }

    private void VibrationTextEffect(Vector3[] verts, TMP_CharacterInfo charInfo)
    {
        k *= -1;

        for (int j = 0; j < TMP_PRO_VERTICES; j++)
        {
            var orig = verts[charInfo.vertexIndex + j];
            verts[charInfo.vertexIndex + j] = orig + new Vector3(Mathf.Sin(Time.time * 80) * 0.6f * k, Mathf.Sin(Time.time * 60) * 0.6f * k, 0);
        }
    }

    private void RageTextEffect(Vector3[] verts, TMP_CharacterInfo charInfo) // NOT READY
    {
        k *= -1;

        if (increaseAmount < 5)
            increaseAmount += Time.deltaTime / 50;

        for (int j = 0; j < TMP_PRO_VERTICES; j++)
        {
            var orig = verts[charInfo.vertexIndex + j];
            verts[charInfo.vertexIndex + j] = orig + new Vector3(Mathf.Tan(Time.time * 1 + orig.x * increaseAmount) * increaseAmount * k,
                Mathf.Tan(Time.time * 1 + orig.x * increaseAmount) * increaseAmount * k, 0);
        }
    }

    private void GlitchTextEffect(Vector3[] verts, TMP_CharacterInfo charInfo)
    {
        k *= -1;

        for (int j = 0; j < TMP_PRO_VERTICES; j++)
        {
            var orig = verts[charInfo.vertexIndex + j];
            verts[charInfo.vertexIndex + j] = orig + new Vector3(Mathf.Tan(Time.time + orig.x * 50) * 0.1f * k, Mathf.Tan(Time.time + orig.x * 50) * 0.1f * k, 0);
        }
    }

    private void LightGlitchTextEffect(Vector3[] verts, TMP_CharacterInfo charInfo)
    {
        k *= -1;

        for (int j = 0; j < TMP_PRO_VERTICES; j++)
        {
            var orig = verts[charInfo.vertexIndex + j];
            verts[charInfo.vertexIndex + j] = orig + new Vector3(Mathf.Tan(Time.time * 2 + orig.x) * 0.01f * k, Mathf.Tan(Time.time * 2 + orig.x) * 0.01f * k, 0);
        }
    }

    private void PingPongTextEffect(Vector3[] verts, TMP_CharacterInfo charInfo) // NOT READY
    {
        k *= -1;

        for (int j = 0; j < TMP_PRO_VERTICES; j++)
        {
            var orig = verts[charInfo.vertexIndex + j];
            verts[charInfo.vertexIndex + j] = orig + new Vector3(Mathf.Log(Time.time + orig.x * 5) * k, Mathf.Log(Time.time + orig.x * -15) * k, 0);
        }
    }

    private void Custom(Vector3[] verts, TMP_CharacterInfo charInfo) // FOR NEW EFFECTS
    {
        k *= -1;

        for (int j = 0; j < TMP_PRO_VERTICES; j++)
        {
            var orig = verts[charInfo.vertexIndex + j];
            verts[charInfo.vertexIndex + j] = orig + new Vector3(Mathf.Log(Time.time * A + orig.x * B) * C * k, Mathf.Log(Time.time * D + orig.x * E) * F * k, 0);
        }
    }


}
