using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "LevelConfig", order = 51)]
public class LevelConfig : ScriptableObject
{
    public List<Chunk> Chunks;

    public Chunk GetRandomChunk => Chunks.RandomElementByIWeight();
    public Chunk StartChunk;
}