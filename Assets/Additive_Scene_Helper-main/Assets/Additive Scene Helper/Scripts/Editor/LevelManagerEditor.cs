using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using ESM = UnityEditor.SceneManagement.EditorSceneManager;

namespace AdditiveSceneHelper
{
    [CustomEditor(typeof(LevelManager))]
    public class LevelManagerEditor : Editor
    {
        public static List<int> OpenedSceneFolderPathHasCodeList => Enumerable.Range(0, ESM.sceneCount).Select(i => ESM.GetSceneAt(i).path.GetFolderPathFromScenePath().GetHashCode()).ToList();

        LevelManager levelManager;
        SceneInfoInEditor[] scenesInfoInEditors;

        private void OnEnable()
        {
            Init();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Application.isPlaying) return;

            if (GUILayout.Button("Remove All Additive Scene"))
            {
                Enumerable.Range(1, ESM.sceneCount - 1).Reverse().ToList().ForEach(i => ESM.CloseScene(ESM.GetSceneAt(i), true));
                Init();
            }

            for (int i = 0; i < scenesInfoInEditors.Length; i++) scenesInfoInEditors[i].InspectorGUI();
        }

        private void Init()
        {
            levelManager = target as LevelManager;

            scenesInfoInEditors = new SceneInfoInEditor[levelManager.sceneInfos.Length];
            for (int i = 0; i < scenesInfoInEditors.Length; i++)
                scenesInfoInEditors[i] = new SceneInfoInEditor(levelManager.sceneInfos[i]);
        }

        public static int GetSceneIndex(int sceneFolderPathHashCode) => OpenedSceneFolderPathHasCodeList.FindIndex(1, x => x == sceneFolderPathHashCode);
    }
}