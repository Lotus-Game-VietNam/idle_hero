using Lotus.CoreFramework;
using UnityEditor;
using UnityEngine;

public class AdminPanelWindows : EditorWindow
{

    [MenuItem("Tools/Admin Panel")]
    public static void OpenWindow()
    {
        AdminPanelWindows window = (AdminPanelWindows)GetWindow(typeof(AdminPanelWindows));
        window.minSize = new UnityEngine.Vector2(600, 1000);
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        var buttonStyle = new GUIStyle(GUI.skin.button) { fontStyle = FontStyle.Bold, fixedHeight = 30, fixedWidth = 150 };
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add 10000 Gems", buttonStyle))
        {
            ResourceManager.Gem += 10000;
        }
        GUILayout.EndHorizontal();
    }
}
