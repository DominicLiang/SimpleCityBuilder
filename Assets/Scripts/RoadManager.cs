using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public GameObject roadStraightPrefab;
    public List<Vector3Int> temporaryPlacementPosition;

    private PlacementManager placementManager;

    private void Awake()
    {
        temporaryPlacementPosition = new();

        placementManager = GetComponent<PlacementManager>();
    }

    public void PlaceRoad(Vector3Int pos)
    {
        if (!placementManager.CheckPositionInBound(pos)) return;
        if (!placementManager.CheckPositionIsFree(pos)) return;
        placementManager.PlaceTemporaryStructure(pos, roadStraightPrefab, CellType.Road);
    }
}