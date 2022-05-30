using UnityEngine;

namespace AdditiveSceneHelper
{
    public class LevelManagerMono : MonoBehaviour
    {
        public LevelManager levelManager;

        private void Start()
        {
#if UNITY_EDITOR
            levelManager.RunShortcutCoroutine(this);
#endif
        }
    }
}