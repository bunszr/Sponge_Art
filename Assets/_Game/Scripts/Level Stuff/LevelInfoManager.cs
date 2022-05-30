using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelInfoManager", menuName = "Sponge_Art/LevelInfoManager", order = 0)]
public class LevelInfoManager : ScriptableObject
{
    public LevelInfo[] levelInfos;

    [System.NonSerialized] Dictionary<string, LevelInfo> dictionaryLevelInfo;

    public LevelInfo CurrLevelInfo
    {
        get
        {
            if (dictionaryLevelInfo == null) dictionaryLevelInfo = levelInfos.ToDictionary(x => x.levelName, y => y);

            LevelInfo levelInfo;
            if (!dictionaryLevelInfo.TryGetValue(SOHolder.Ins.importants.levelManager.LevelSceneInfo.CurrSceneName, out levelInfo)) return null;
            return levelInfo;
        }
    }

}