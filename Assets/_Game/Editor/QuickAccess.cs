// using UnityEngine;
// using UnityEditor;
// using Sirenix.OdinInspector.Editor;
// using Sirenix.OdinInspector;
// using System.Linq;

// public class QuickAccess : OdinEditorWindow
// {
//     public Object[] objects;

//     [MenuItem("Tools/QuickAccess")]
//     public static void ShowWindow()
//     {
//         GetWindow(typeof(QuickAccess));

//     }

//     [Button(ButtonSizes.Small)]
//     public void SelectGameManager()
//     {
//         Selection.activeGameObject = FindObjectOfType<GameManager>().gameObject;
//     }

//     [HorizontalGroup("1")]
//     public string typeName = "Collider";

//     [HorizontalGroup("1"), Button(ButtonSizes.Small)]
//     public void SelectWithName()
//     {
//         Selection.objects = FindObjectsOfType<Component>().Where(x => x.GetType().Name == typeName).Select(x => (Object)x.gameObject).ToArray(); // If we write x instead of x.gameObject, it only selects that component. This kind of Transform selects together with meshFilter etc.
//     }
// }