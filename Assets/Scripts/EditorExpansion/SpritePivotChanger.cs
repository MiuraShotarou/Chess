using UnityEngine;
using UnityEditor;

public class SpritePivotChanger : EditorWindow
{
    private SpriteAlignment pivotAlignment = SpriteAlignment.Bottom;
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
        string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { GetSelectedFolderPath() });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            ChangePivot(texture);
        }
    }

    void ChangePivot(Texture2D texture)
    {
        string path = AssetDatabase.GetAssetPath(texture);
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

        if (importer != null && importer.textureType == TextureImporterType.Sprite)
        {
            importer.spriteAlignment = (int)pivotAlignment;

            if (pivotAlignment == SpriteAlignment.Custom)
            {
                importer.spritePivot = customPivot;
            }

            importer.SaveAndReimport();
        }
    }

    string GetSelectedFolderPath()
    {
        foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
        {
            string path = AssetDatabase.GetAssetPath(obj);
            if (System.IO.Directory.Exists(path))
            {
                return path;
            }
        }
        return "Assets";
    }
}