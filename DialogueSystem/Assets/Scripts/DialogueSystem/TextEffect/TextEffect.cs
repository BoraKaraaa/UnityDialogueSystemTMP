using UnityEngine;
using TMPro;

public class TextEffect : MonoBehaviour
{
    [SerializeField] private ETextEffects customTextEffect;
    [SerializeField] private int diffColorWordIndex = -1;
    [SerializeField] private Color textColor;
    [SerializeField] private TMP_Text tmpText;
    
    void Update()
    {
        if (customTextEffect != ETextEffects.None)
            TextEffectsController.Instance.DoTextEffect(tmpText, customTextEffect);

        if (diffColorWordIndex != -1)
            TextEffectsController.Instance.ChangeWordColor(tmpText, diffColorWordIndex, textColor);
    }
}
