using UnityEngine;

[System.Serializable]
public class BonusData : IWeight
{
    [Range(0, 10)]
    public int _Weight;
    public string Key;
    public int BonusValue;
    public GameObject prefab;
    public int Weight { get => _Weight; set => _Weight = value; }
}