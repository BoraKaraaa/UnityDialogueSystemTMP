using UnityEditor;

[CustomEditor(typeof(UIDialogue))]
public class UIDialogueEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
            
        /*
        UIDialogue uiDialogue = (UIDialogue)target;
        
        foreach (var sentence in uiDialogue.sentences)
        {
            EditorGUILayout.TextArea(sentence, GUILayout.Width(400), GUILayout.Height(100));
        }
        */

    }
}
