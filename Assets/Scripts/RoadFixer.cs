using System;
using System.Linq;
using UnityEngine;

public class RoadFixer : MonoBehaviour
{
    public GameObject deadEnd;
    public GameObject straight;
    public GameObject corner;
    public GameObject threeWay;
    public GameObject fourWay;

    public void FixRoadAtPosition(PlacementManager placementManager, Vector3Int temporaryPosition)
    {
        var result = placementManager.GetNeighborTypeFor(temporaryPosition);
        var roadCount = 0;
        roadCount = result.Where(x => x == CellType.Road).Count();
        switch (roadCount)
        {
            case 0:
            case 1:
                CreateDeadEnd(placementManager, result, temporaryPosition);
                break;
            case 2:
                var isStraight = CreateStraightRoad(placementManager, result, temporaryPosition);
                if (isStraight) break;
                CreateCorner(placementManager, result, temporaryPosition);
                break;
            case 3:
                CreateThreeWay(placementManager, result, temporaryPosition);
                break;
            case 4:
                CreateFourWay(placementManager, temporaryPosition);
                break;
        }
    }

    private void CreateDeadEnd(PlacementManager placementManager, CellType[] result, Vector3Int temporaryPosition)
    {
        var rotation = Quaternion.identity;
        if (result[2] == CellType.Road)
        {
            rotation = Quaternion.identity;
        }
        else if (result[3] == CellType.Road)
        {
            rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (result[0] == CellType.Road)
        {
            rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (result[1] == CellType.Road)
        {
            rotation = Quaternion.Euler(0, 270, 0);
        }
        placementManager.ModifyStructureModel(temporaryPosition, deadEnd, rotation);
    }

    private bool CreateStraightRoad(PlacementManager placementManager, CellType[] result, Vector3Int temporaryPosition)
    {
        var rotation = Quaternion.identity;
        var isStraight = false;
        if (result[0] == CellType.Road && result[2] == CellType.Road)
        {
            rotation = Quaternion.identity;
            isStraight = true;
        }
        else if (result[1] == CellType.Road && result[3] == CellType.Road)
        {
            rotation = Quaternion.Euler(0, 90, 0);
            isStraight = true;
        }
        if (!isStraight) return false;
        placementManager.ModifyStructureModel(temporaryPosition, straight, rotation);
        return true;
    }

    private void CreateCorner(PlacementManager placementManager, CellType[] result, Vector3Int temporaryPosition)
    {
        var rotation = Quaternion.identity;
        if (result[0] == CellType.Road && result[1] == CellType.Road)
        {
            rotation = Quaternion.identity;
        }
        else if (result[1] == CellType.Road && result[2] == CellType.Road)
        {
            rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (result[2] == CellType.Road && result[3] == CellType.Road)
        {
            rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (result[3] == CellType.Road && result[0] == CellType.Road)
        {
            rotation = Quaternion.Euler(0, 270, 0);
        }
        placementManager.ModifyStructureModel(temporaryPosition, corner, rotation);
    }

    private void CreateThreeWay(PlacementManager placementManager, CellType[] result, Vector3Int temporaryPosition)
    {
        var rotation = Quaternion.identity;
        if (result[0] != CellType.Road)
        {
            rotation = Quaternion.identity;
        }
        else if (result[1] != CellType.Road)
        {
            rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (result[2] != CellType.Road)
        {
            rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (result[3] != CellType.Road)
        {
            rotation = Quaternion.Euler(0, 270, 0);
        }
        placementManager.ModifyStructureModel(temporaryPosition, threeWay, rotation);
    }

    private void CreateFourWay(PlacementManager placementManager, Vector3Int temporaryPosition)
    {
        placementManager.ModifyStructureModel(temporaryPosition, fourWay, Quaternion.identity);
    }
}