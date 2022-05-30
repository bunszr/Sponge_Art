using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ESM = UnityEditor.SceneManagement.EditorSceneManager;

namespace AdditiveSceneHelper
{
    public static class Utility
    {
        public static string GetFolderPathFromScenePath(this string path)
        {
            return path.Substring(0, GetSceneFolderPathCount(path));
        }

        public static int GetSceneFolderPathCount(string path)
        {
            for (int i = path.Length - 1; i >= 0; i--)
            {
                if (path[i] == '/')
                    return i;
            }
            return -1;
        }

        public static int NoNegative(this int value) => value < 0 ? 0 : value;
    }
}