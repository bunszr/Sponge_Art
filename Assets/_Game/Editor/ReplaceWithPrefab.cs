using UnityEngine;
using UnityEditor;

// https://forum.unity.com/threads/a-script-replacing-one-prefab-with-another.681805/#:~:text=4-,Talked%20to%20some%20of%20my%20Engineer%20buddies%2C%20but%20they%20aren%27t%20familiar%20with%20Unity.%20I%20have%20no%20errors%20or%20warnings%2C%20but%20I%20just%20can%27t%20get%20it%20to%20work%20like%20the%20video%20above.,-Code%20(CSharp)%3A 
public class ReplaceWithPrefab : EditorWindow
{
    [SerializeField] private GameObject prefab;

    [MenuItem("Tools/Replace With Prefab")]
    static void CreateReplaceWithPrefab()
    {
        EditorWindow.GetWindow<ReplaceWithPrefab>();
    }

    private void OnGUI()
    {
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);

        if (GUILayout.Button("Replace"))
        {
            var selection = Selection.gameObjects;

            for (var i = selection.Length - 1; i >= 0; --i)
            {
                var selected = selection[i];
                var prefabType = PrefabUtility.GetPrefabType(prefab);
                GameObject newObject;

                if (prefabType == PrefabType.Prefab)
                {
                    newObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                }
                else
                {
                    newObject = Instantiate(prefab);
                    newObject.name = prefab.name;
                }

                if (newObject == null)
                {
                    Debug.LogError("Error instantiating prefab");
                    break;
                }

                Undo.RegisterCreatedObjectUndo(newObject, "Replace With Prefabs");
                newObject.transform.parent = selected.transform.parent;
                newObject.transform.localPosition = selected.transform.localPosition;
                newObject.transform.localRotation = selected.transform.localRotation;
                newObject.transform.localScale = selected.transform.localScale;
                newObject.transform.SetSiblingIndex(selected.transform.GetSiblingIndex());
                Undo.DestroyObjectImmediate(selected);
            }
        }

        GUI.enabled = false;
        EditorGUILayout.LabelField("Selection count: " + Selection.objects.Length);
    }
}