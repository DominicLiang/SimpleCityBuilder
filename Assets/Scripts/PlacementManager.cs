using System;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public Transform StructureRoot;
    public int width;
    public int height;
    private Grid placementGrid;

    private void Awake()
    {
        placementGrid = new(width, height);
    }

    public bool CheckPositionInBound(Vector3Int pos)
    {
        var xInBound = pos.x >= 0 && pos.x < width;
        var zInBound = pos.z >= 0 && pos.z < height;
        return xInBound && zInBound;
    }

    public bool CheckPositionIsFree(Vector3Int pos)
    {
        return CheckPositionIsOfType(pos, CellType.Empty);
    }

    private bool CheckPositionIsOfType(Vector3Int pos, CellType type)
    {
        return placementGrid[pos.x, pos.z] == type;
    }

    public void PlaceTemporaryStructure(Vector3Int pos, GameObject gameObject, CellType type)
    {
        placementGrid[pos.x, pos.z] = type;
        var newStructure = Instantiate(gameObject, StructureRoot);
        newStructure.transform.SetPositionAndRotation(pos, Quaternion.identity);
    }
}