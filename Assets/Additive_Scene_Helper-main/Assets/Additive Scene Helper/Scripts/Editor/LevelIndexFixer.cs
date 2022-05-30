#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using AdditiveSceneHelper;
using System.Linq;

// In runtime, LevelIndex variable are being changed when level scene changed. But when the runtime is exited, the scene we started playing for the first time exists in the editor. Therefore levelIndex variable and level scene do not match. This class is for solving this problem 

[InitializeOnLoad]
public static class LevelIndexFixer
{
    static LevelInfoHolder[] levelInfoHolders;

    static LevelIndexFixer()
    {
        EditorApplication.playModeStateChanged += ModeChanged;

        LevelManager[] levelManagers = Resources.LoadAll<LevelManager>("");
        levelInfoHolders = new LevelInfoHolder[levelManagers.Length];
        for (int i = 0; i < levelManagers.Length; i++) levelInfoHolders[i] = new LevelInfoHolder(levelManagers[i]);
    }

    static void ModeChanged(PlayModeStateChange playModeState)
    {
        if (playModeState == PlayModeStateChange.EnteredEditMode)
        {
            levelInfoHolders.ToList().ForEach(x => x.EnteredEditMode());
        }
    }

    class LevelInfoHolder
    {
        int defaultLevelIndex;
        LevelManager levelManager;

        public LevelInfoHolder(LevelManager levelManager)
        {
            this.levelManager = levelManager;
            defaultLevelIndex = levelManager.LevelSceneInfo.LevelIndex;
        }

        public void EnteredEditMode()
        {
            levelManager.LevelSceneInfo.LevelIndex = defaultLevelIndex;
        }
    }
}

#endif