using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Ins { get; private set; }

    protected virtual void Awake()
    {
        if (Ins != null)
        {
            Debug.LogError("İki tane " + gameObject.name + " var");
            Destroy(gameObject);
            return;
        }
        Ins = GetComponent<T>();
    }
}