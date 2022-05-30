using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using ESM = UnityEditor.SceneManagement.EditorSceneManager;

namespace AdditiveSceneHelper
{
    public class SceneInfoInEditor
    {
        int oldLevelIndex;
        readonly LevelManager.ScenesInfo scenesInfo;
        bool disableRemoveButton;

        public SceneInfoInEditor(LevelManager.ScenesInfo scenesInfo)
        {
            this.scenesInfo = scenesInfo;
            this.oldLevelIndex = scenesInfo.LevelIndex;

            disableRemoveButton = !LevelManagerEditor.OpenedSceneFolderPathHasCodeList.Contains(scenesInfo.SceneFolderPathHashCode);
        }

        public void InspectorGUI()
        {
            EditorGUILayout.BeginHorizontal();
                scenesInfo.LevelIndex = EditorGUILayout.IntSlider(scenesInfo.name + " Index", scenesInfo.LevelIndex, 0, (scenesInfo.NumScenes - 1).NoNegative());
                EditorGUI.BeginDisabledGroup(disableRemoveButton);
                    RButtonDown();
                EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            if (!LevelIndexIsChanged()) return;

            if (scenesInfo.name == "")
            {
                Debug.LogError("Scene Info name does not assign!");
                return;
            }

            int sceneIndexOnHierarchy = LevelManagerEditor.GetSceneIndex(scenesInfo.SceneFolderPathHashCode);
            bool hasValidSceneAtIndex = sceneIndexOnHierarchy != -1;
            if (hasValidSceneAtIndex)
            {
                SaveAndCloseOpenedScene(sceneIndexOnHierarchy);
            }

            UnityEngine.SceneManagement.Scene openingScene = ESM.OpenScene(scenesInfo.CurrScenePath, OpenSceneMode.Additive);

            if (hasValidSceneAtIndex) ESM.MoveSceneAfter(openingScene, ESM.GetSceneAt(sceneIndexOnHierarchy - 1)); // The scene which loaded is highest index in the hierarchy(loaded at the bottom). Therebefore must rearrange laoded scenes.

            disableRemoveButton = false;
        }

        bool LevelIndexIsChanged()
        {
            if (oldLevelIndex != scenesInfo.LevelIndex)
            {
                oldLevelIndex = scenesInfo.LevelIndex;
                return true;
            }
            return false;
        }

        void SaveAndCloseOpenedScene(int sceneIndexOnHierarchy)
        {
            UnityEngine.SceneManagement.Scene loadedSceneAtIndex = ESM.GetSceneAt(sceneIndexOnHierarchy);
            if (loadedSceneAtIndex.isDirty) ESM.SaveScene(loadedSceneAtIndex); // Save if the scene has changed
            ESM.CloseScene(loadedSceneAtIndex, true); // Close the existing scene at index
        }

        public void RButtonDown()
        {
            if (GUILayout.Button("R", GUILayout.Width(25)))
            {
                disableRemoveButton = true;
                int sceneIndexWithESM = LevelManagerEditor.GetSceneIndex(scenesInfo.SceneFolderPathHashCode);
                if (sceneIndexWithESM != -1) SaveAndCloseOpenedScene(sceneIndexWithESM);
            }
        }
    }
}