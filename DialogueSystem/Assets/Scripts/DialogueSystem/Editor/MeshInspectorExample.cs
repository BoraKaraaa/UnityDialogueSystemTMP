using TMPro;
using UnityEditor;
using UnityEngine;

public class MeshInspectorExample : EditorWindow
{
    private Mesh targetMesh;
    private Material previewMaterial;
    private GameObject previewObject;
    private PreviewRenderUtility previewRender;

    private void OnEnable()
    {
        previewRender = new PreviewRenderUtility();
        //previewObject = new GameObject("MeshPreviewObject");
    }

    private void OnDisable()
    {
        DestroyImmediate(previewObject);
        previewRender.Cleanup();
    }

    private void OnGUI()
    {
        GUILayout.Label("Mesh Inspector Example");

        previewObject = EditorGUILayout.ObjectField("Default TMP Object", previewObject, typeof(GameObject),
            true) as GameObject;
        //targetMesh = EditorGUILayout.ObjectField("Target Mesh", targetMesh, typeof(Mesh), false) as Mesh;
        
        if (previewObject != null)
        {
            Test();
            //ShowMeshPreview();
        }
    }

    private void Test()
    {
        MeshFilter meshFilter = previewObject.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = previewObject.GetComponent<MeshRenderer>();
        
        meshFilter.sharedMesh = targetMesh;
        meshRenderer.sharedMaterial = previewMaterial;

        previewObject.transform.rotation = Quaternion.identity;

        Rect previewRect = GUILayoutUtility.GetRect(200, 200);
        previewRender.BeginPreview(previewRect, GUIStyle.none);
        previewRender.DrawMesh(targetMesh, Matrix4x4.identity, previewMaterial, 0);
        previewRender.Render(false);
        Texture previewTexture = previewRender.EndPreview();

        GUI.DrawTexture(previewRect, previewTexture, ScaleMode.ScaleToFit);
    }

    private void ShowMeshPreview()
    {
        if (previewMaterial == null)
        {
            previewMaterial = new Material(Shader.Find("Unlit/Color"));
            previewMaterial.color = Color.gray;
        }

        MeshFilter meshFilter = previewObject.GetComponent<MeshFilter>();
        if (meshFilter == null)
            meshFilter = previewObject.AddComponent<MeshFilter>();
        
        MeshRenderer meshRenderer = previewObject.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
            meshRenderer = previewObject.AddComponent<MeshRenderer>();

        meshFilter.sharedMesh = targetMesh;
        meshRenderer.sharedMaterial = previewMaterial;

        previewObject.transform.rotation = Quaternion.identity;

        Rect previewRect = GUILayoutUtility.GetRect(200, 200);
        previewRender.BeginPreview(previewRect, GUIStyle.none);
        previewRender.DrawMesh(targetMesh, Matrix4x4.identity, previewMaterial, 0);
        previewRender.Render(false);
        Texture previewTexture = previewRender.EndPreview();

        GUI.DrawTexture(previewRect, previewTexture, ScaleMode.ScaleToFit);
    }

    [MenuItem("Window/Mesh Inspector Example")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(MeshInspectorExample));
    }
}


