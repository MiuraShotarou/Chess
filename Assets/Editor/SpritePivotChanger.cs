using UnityEngine;
using UnityEditor;

public class SpritePivotChanger : EditorWindow
{
    private SpriteAlignment pivotAlignment = SpriteAlignment.BottomCenter;
    private Vector2 customPivot = new Vector2(0.5f, 0f);

    [MenuItem("Tools/Sprite Pivot Changer")]
    public static void ShowWindow()
    {
        GetWindow<SpritePivotChanger>("Sprite Pivot Changer");
    }

    void OnGUI()
    {
        GUILayout.Label("Sprite Pivot Batch Changer", EditorStyles.boldLabel);

        pivotAlignment = (SpriteAlignment)EditorGUILayout.EnumPopup("Pivot Type", pivotAlignment);

        if (pivotAlignment == SpriteAlignment.Custom)
        {
            customPivot = EditorGUILayout.Vector2Field("Custom Pivot", customPivot);
        }

        if (GUILayout.Button("Change Selected Sprites"))
        {
            ChangeSelectedSpritesPivot();
        }

        if (GUILayout.Button("Change All Sprites in Folder"))
        {
            Debug.Log("Change All Sprites in Folder");
            ChangeAllSpritesInFolder();
        }
    }

    void ChangeSelectedSpritesPivot()
    {
        foreach (Object obj in Selection.objects)
        {
            if (obj is Texture2D)
            {
                ChangePivot(obj as Texture2D);
            }
        }
    }

    void ChangeAllSpritesInFolder()
    {
        string[] guids = AssetDatabase.FindAssets("Idle", new string[] { "Assets/ImportAssets/Characters/K_Asset/Sprites" });//ここの中身を指定する必要がある
        //string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { GetSelectedFolderPath() });//ここの中身を指定する必要がある

        foreach (string guid in guids)
        {
            Debug.Log(guid);
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            ChangePivot(texture);
        }
    }
    void ChangePivot(Texture2D texture)
    {
        string path = AssetDatabase.GetAssetPath(texture);
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

        if (importer != null)
        {
            Debug.Log("TextureImporterが取得出来ている");
            importer.spritePivot = GetPivotVector(pivotAlignment, customPivot);
            Debug.Log(importer.spritePivot);
            importer.SaveAndReimport();
        }
    }
    Vector2 GetPivotVector(SpriteAlignment alignment, Vector2 customPivot)
    {
        switch (alignment)
        {
            case SpriteAlignment.TopLeft: return new Vector2(0f, 1f);
            case SpriteAlignment.TopCenter: return new Vector2(0.5f, 1f);
            case SpriteAlignment.TopRight: return new Vector2(1f, 1f);
            case SpriteAlignment.LeftCenter: return new Vector2(0f, 0.5f);
            case SpriteAlignment.Center: return new Vector2(0.5f, 0.5f);
            case SpriteAlignment.RightCenter: return new Vector2(1f, 0.5f);
            case SpriteAlignment.BottomLeft: return new Vector2(0f, 0f);
            case SpriteAlignment.BottomCenter: return new Vector2(0.5f, 0f);
            case SpriteAlignment.BottomRight: return new Vector2(1f, 0f);
            case SpriteAlignment.Custom: return customPivot;
            default: return new Vector2(0.5f, 0.5f);
        }
    }

    string GetSelectedFolderPath()
    {
        foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
        {
            if (!obj)
            {
                Debug.Log("objがunll");
            }
            string path = AssetDatabase.GetAssetPath(obj);
            if (System.IO.Directory.Exists(path))
            {
                return path;
            }
        }
        return "Assets";
    }
}