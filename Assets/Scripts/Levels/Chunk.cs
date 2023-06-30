using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Chunk : IWeight
{
    [Range(1, 10)]
    public int _Weight;
    public List<BlockType> Block;
    public int Weight { get => _Weight; set => _Weight = value; }
}