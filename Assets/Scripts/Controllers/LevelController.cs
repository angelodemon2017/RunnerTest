using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;

    public LevelConfig levelConfig;
    public BonusConfig bonusConfig;
    public Transform ParentBlocks;
    public List<BlockConfig> blockConfigs;

    private Direction DirectionGenerate;
    private int Zposition = 0;
    private int Xposition = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void StartLevel()
    {
        DirectionGenerate = Direction.Zpositive;
        Zposition = 0;
        Xposition = 0;
        foreach (Transform child in ParentBlocks)
        {
            Destroy(child.gameObject);
        }
        SpawnChunk(levelConfig.StartChunk);
    }

    public void AddingPath()
    {
        if (ParentBlocks.childCount < 12)
        {
            SpawnChunk(levelConfig.GetRandomChunk);
        }
    }

    private void SpawnChunk(Chunk chunk)
    {
        foreach (var block in chunk.Block)
        {
            SpawnBlock(block);
        }
    }

    private void SpawnBlock(BlockType blockType)
    {
        var bc = GetPrefabByType(blockType);
        var eb = Instantiate(bc.BlockPrefab, new Vector3(Xposition, 0f, Zposition), DirectionGenerate.GetQuaternionirection(), ParentBlocks);
        if (blockType == BlockType.rotater)
        {
            RandomRotate();
        }
        eb.Instantiate(bc, DirectionGenerate);

        switch (DirectionGenerate)
        {
            case Direction.Zpositive:
                Zposition += bc.Size;
                break;
            case Direction.Znegative:
                Zposition -= bc.Size;
                break;
            case Direction.Xpositive:
                Xposition += bc.Size;
                break;
            case Direction.Xnegative:
                Xposition -= bc.Size;
                break;
        }
        if (bc.IsCanGenBonus)
        {
            TrySpawnBonus(eb);
        }
    }

    private void RandomRotate()
    {
        switch (DirectionGenerate)
        {
            case Direction.Zpositive:
                DirectionGenerate = Random.RandomRange(0, 100f) < 50f ? Direction.Xpositive : Direction.Xnegative;
                break;
            case Direction.Znegative:
                DirectionGenerate = Random.RandomRange(0, 100f) < 50f ? Direction.Xpositive : Direction.Xnegative;
                break;
            case Direction.Xpositive:
                DirectionGenerate = Random.RandomRange(0, 100f) < 50f ? Direction.Zpositive : Direction.Znegative;
                break;
            case Direction.Xnegative:
                DirectionGenerate = Random.RandomRange(0, 100f) < 50f ? Direction.Zpositive : Direction.Znegative;
                break;
        }
    }

    private void TrySpawnBonus(EntityBlock eb)
    {
        if (Random.Range(0f, 100f) <= bonusConfig.ChanceSpawn)
        {
            var tempBonus = bonusConfig.bonusDatas.RandomElementByIWeight();
            var go = Instantiate(tempBonus.prefab, new Vector3(Xposition, 1f, Zposition), Quaternion.Euler(90f, eb.gameObject.transform.eulerAngles.y + 90f, 0f), transform);
            eb.BonusObject = go;
        }
    }

    private BlockConfig GetPrefabByType(BlockType blockType)
    {
        return blockConfigs.FirstOrDefault(x => x.blockType == blockType);
    }
}