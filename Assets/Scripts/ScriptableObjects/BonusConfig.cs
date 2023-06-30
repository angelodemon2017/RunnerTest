using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "BonusConfig", menuName = "BonusConfig", order = 51)]
public class BonusConfig : ScriptableObject
{
    [Range(0f, 100f)]
    public float ChanceSpawn;

    public List<BonusData> bonusDatas;

    public BonusData GetByKey(string key)
    {
        return bonusDatas.FirstOrDefault(x => x.Key == key);
    }
}