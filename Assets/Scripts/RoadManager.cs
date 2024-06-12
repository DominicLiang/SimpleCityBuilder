using System.Collections.Generic;
using SVS;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public List<Vector3Int> temporaryPlacementPosition;
    public List<Vector3Int> roadPositionToRecheck;

    private bool placementMode = false;
    private Vector3Int startPosition;

    private PlacementManager placementManager;
    private RoadFixer roadFixer;

    private void Awake()
    {
        temporaryPlacementPosition = new();
        roadPositionToRecheck = new();

        placementManager = GetComponent<PlacementManager>();
        roadFixer = GetComponent<RoadFixer>();
    }

    public void PlaceRoad(Vector3Int pos)
    {
        if (!placementManager.CheckPositionInBound(pos)) return;
        if (!placementManager.CheckPositionIsFree(pos)) return;

        if (!placementMode)
        {
            temporaryPlacementPosition.Clear();
            roadPositionToRecheck.Clear();
            temporaryPlacementPosition.Add(pos);

            placementMode = true;
            startPosition = pos;

            placementManager.PlaceTemporaryStructure(pos, roadFixer.deadEnd, CellType.Road);
        }
        else
        {
            placementManager.RemoveAllTemporaryStructures();
            temporaryPlacementPosition.Clear();


            foreach (var posToFix in roadPositionToRecheck)
            {
                roadFixer.FixRoadAtPosition(placementManager, posToFix);
            }

            roadPositionToRecheck.Clear();

            temporaryPlacementPosition = placementManager.GetPathBetween(startPosition, pos);

            foreach (var temporaryPosition in temporaryPlacementPosition)
            {
                if (!placementManager.CheckPositionIsFree(temporaryPosition)) continue;
                placementManager.PlaceTemporaryStructure(temporaryPosition, roadFixer.deadEnd, CellType.Road);
            }
        }

        FixRoadPrefab();
    }

    private void FixRoadPrefab()
    {
        foreach (var pos in temporaryPlacementPosition)
        {
            roadFixer.FixRoadAtPosition(placementManager, pos);
            var neighbors = placementManager.GetNeighborTypeFor(pos, CellType.Road);
            foreach (var roadPosition in neighbors)
            {
                if (roadPositionToRecheck.Contains(roadPosition)) continue;
                roadPositionToRecheck.Add(roadPosition);
            }
        }
        foreach (var positionToFix in roadPositionToRecheck)
        {
            roadFixer.FixRoadAtPosition(placementManager, positionToFix);
        }
    }

    public void FinishPlacingRoad()
    {
        placementManager.AddTemporaryStructuresToDictionary();
        if (temporaryPlacementPosition.Count > 0)
        {
            AudioPlayer.instance.PlayPlacementSound();
        }
        temporaryPlacementPosition.Clear();
        startPosition = Vector3Int.zero;
        placementMode = false;
    }
}