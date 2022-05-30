using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;

namespace AdditiveSceneHelper
{
    [CreateAssetMenu(fileName = "LevelManager", menuName = "Additive Scene Manager/LevelManager", order = 0)]
    public class LevelManager : ScriptableObject
    {
        [SerializeField] string MAIN_SCENE_NAME = "_Main Scene";

        public ScenesInfo[] sceneInfos;

        public ScenesInfo LevelSceneInfo => sceneInfos[0];

        public void JumpNextLevel()
        {
            IncrementLevelIndex();
            SOHolder.Ins.events.onPressedNextLevelButton.Raise();
            LoadSceneAccordingInfo();
        }

        public void ReloadScene()
        {
            SOHolder.Ins.events.onPressedRestartLevelButton.Raise();
            LoadSceneAccordingInfo();
        }

        public void LoadSceneAccordingInfo()
        {
            SceneManager.LoadScene(MAIN_SCENE_NAME);
            SceneManager.LoadScene(LevelSceneInfo.CurrSceneName, LoadSceneMode.Additive);
        }

        void IncrementLevelIndex() => LevelSceneInfo.LevelIndex = (LevelSceneInfo.LevelIndex + 1) % LevelSceneInfo.NumScenes;

        public void RunShortcutCoroutine(MonoBehaviour monoBehaviour) => monoBehaviour.StartCoroutine(UpdateShortCut());
        IEnumerator UpdateShortCut()
        {
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.R)) ReloadScene();
                else if (Input.GetKeyDown(KeyCode.N)) JumpNextLevel();
#if UNITY_EDITOR
                else if (Input.GetKeyDown(KeyCode.P)) UnityEditor.EditorApplication.isPaused = true;
#endif
                yield return null;
            }
        }

        [System.Serializable]
        public class ScenesInfo
        {
            public string name;
            public string sceneFolderPath;
            [SerializeField] Object[] sceneObjects = new Object[] { };

            public int NumScenes => sceneObjects.Length;
            public string CurrSceneName => sceneObjects[LevelIndex].name;
            public string CurrScenePath => sceneFolderPath + "/" + sceneObjects[LevelIndex].name + ".unity";
            public int SceneFolderPathHashCode => sceneFolderPath.GetHashCode();

            public int LevelIndex
            {
                get => PlayerPrefs.GetInt(name + "Key");
                set => PlayerPrefs.SetInt(name + "Key", value);
            }
        }
    }
}
