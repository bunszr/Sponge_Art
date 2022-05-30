using UnityEngine;

public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    static T ins;
    public static T Ins
    {
        get
        {
            if (ins == null) ins = Resources.Load<T>(typeof(T).ToString());
            return ins;
        }
    }
}