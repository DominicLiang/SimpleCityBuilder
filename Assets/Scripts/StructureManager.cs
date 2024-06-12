using System;
using System.Linq;
using SVS;
using UnityEngine;

public class StructureManager : MonoBehaviour
{
    public StructurePrefabWeight[] housePrefab;
    public StructurePrefabWeight[] specialPrefab;
    public StructurePrefabWeight[] bigStructurePrefab;

    private PlacementManager placementManager;

    private float[] houseWeight;
    private float[] specialWeight;
    private float[] bigStructureWeight;

    private void Awake()
    {
        placementManager = GetComponent<PlacementManager>();
    }

    private void Start()
    {
        houseWeight = housePrefab.Select(x => x.weight).ToArray();
        specialWeight = specialPrefab.Select(x => x.weight).ToArray();
        bigStructureWeight = bigStructurePrefab.Select(x => x.weight).ToArray();
    }

    public void PlaceHouse(Vector3Int position)
    {
        if (!CheckPositionBeforePlacement(position)) return;
        var randomIndex = GetRandomWeightIndex(houseWeight);
        placementManager.PlaceObjectOnMap(position, housePrefab[randomIndex].prefab, CellType.Structure);
        AudioPlayer.instance.PlayPlacementSound();
    }

    public void PlaceSpecial(Vector3Int position)
    {
        if (!CheckPositionBeforePlacement(position)) return;
        var randomIndex = GetRandomWeightIndex(specialWeight);
        placementManager.PlaceObjectOnMap(position, specialPrefab[randomIndex].prefab, CellType.SpecialStructure);
        AudioPlayer.instance.PlayPlacementSound();
    }

    public void PlaceBigStructure(Vector3Int position)
    {
        var width = 2;
        var height = 2;
        if (!CheckBigStructure(position, width, height)) return;
        var randomIndex = GetRandomWeightIndex(bigStructureWeight);
        placementManager.PlaceObjectOnMap(position, bigStructurePrefab[randomIndex].prefab, CellType.Structure, width, height);
        AudioPlayer.instance.PlayPlacementSound();
    }

    private bool CheckBigStructure(Vector3Int position, int width, int height)
    {
        var nearRoad = false;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var newPosition = position + new Vector3Int(x, 0, y);
                if (!DefaultCheck(newPosition)) return false;
                if (!nearRoad) nearRoad = RoadCheck(newPosition);
            }
        }

        return nearRoad;
    }

    private int GetRandomWeightIndex(float[] weight)
    {
        var sum = 0f;
        for (int i = 0; i < weight.Length; i++)
        {
            sum += weight[i];
        }
        var randomValue = UnityEngine.Random.Range(0, sum);
        var tempSum = 0f;
        for (int i = 0; i < weight.Length; i++)
        {
            if (randomValue >= tempSum && randomValue < tempSum + weight[i])
            {
                return i;
            }
            tempSum += weight[i];
        }
        return 0;
    }

    private bool CheckPositionBeforePlacement(Vector3Int position)
    {
        if (!DefaultCheck(position)) return false;
        if (!RoadCheck(position)) return false;

        return true;
    }

    private bool RoadCheck(Vector3Int position)
    {
        if (placementManager.GetNeighborTypeFor(position, CellType.Road).Count() <= 0) return false;
        return true;
    }

    private bool DefaultCheck(Vector3Int position)
    {
        if (!placementManager.CheckPositionInBound(position)) return false;
        if (!placementManager.CheckPositionIsFree(position)) return false;
        return true;
    }
}

[Serializable]
public struct StructurePrefabWeight
{
    public GameObject prefab;
    [Range(0, 1)]
    public float weight;
}