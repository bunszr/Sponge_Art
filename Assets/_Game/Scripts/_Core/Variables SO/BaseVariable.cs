using UnityEngine;

// It does not work properly on phone If BaseVariable become generic(<T>) !!!!!!!!!!!!!!!!!

public abstract class BaseVariable : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif
}