using UnityEngine;
using UnityEditor;
using System.Collections;

namespace AdditiveSceneHelper
{
    public class QuicklyOpenLevelManager : EditorWindow
    {
        /*
            Alt + a => Quickly open/close LevelManager
            DERLENDİKTEN SONRA PENCERE AÇIKSA ONENABLE ÇALIŞYOR AMA ONDİSABLE ÇALIŞMIYOR
        */

        static LevelManager levelManager;
        static Editor levelManagerEditor;
        static EditorWindow window;

        [MenuItem("Tools/Level Manager &a")] // & == Alt
        private static void ShowWindow()
        {
            if (window != null)
            {
                window.Close();
                return;
            }

            if (levelManagerEditor != null)
                Debug.LogError("levelManagerEditor nul değil ");

            levelManager = Resources.Load<LevelManager>("Scriptables/LevelManager 00");
            levelManagerEditor = Editor.CreateEditor(levelManager);

            window = GetWindow<QuicklyOpenLevelManager>();
            window.Show();
            window.titleContent = new GUIContent(levelManager.name);
        }

        private void OnDisable()
        {
            // You need to manually destroy any inspectors created from Editor.CreateEditor! More information: https://stackoverflow.com/questions/16054955/unity-custom-inspector-on-custom-window   
            DestroyImmediate(levelManagerEditor);
        }

        private void OnGUI()
        {
            if (levelManagerEditor != null) // levelManagerEditor is null If the window is open and scripts are recompile
            {
                levelManagerEditor.OnInspectorGUI();
            }
        }

        private void OnDestroy() { } // Does not call when the window is open and scripts are recompiled
    }
}