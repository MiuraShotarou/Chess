using UnityEditor;
using UnityEngine;

public class ExpansionWindow : EditorWindow
{
    [MenuItem("Window/ExpansionWindow")]
    static void Open()
    {
        var window = GetWindow<ExpansionWindow>();
        window.titleContent = new GUIContent("オリジナルのウィンドウ");
    }
}
