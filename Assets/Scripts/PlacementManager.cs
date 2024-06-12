using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public int width;
    public int height;
    private Grid placementGrid;

    private Dictionary<Vector3Int, StructureModel> temporaryRoadObject;
    private Dictionary<Vector3Int, StructureModel> structures;

    private void Awake()
    {
        placementGrid = new(width, height);
        temporaryRoadObject = new();
        structures = new();
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

    public void PlaceTemporaryStructure(Vector3Int pos, GameObject prefab, CellType type)
    {
        placementGrid[pos.x, pos.z] = type;
        var structure = CreateNewStructureModel(pos, prefab, type);
        temporaryRoadObject.Add(pos, structure);
    }

    private StructureModel CreateNewStructureModel(Vector3Int pos, GameObject prefab, CellType type)
    {
        var structure = new GameObject(type.ToString());
        structure.transform.SetParent(transform);
        structure.transform.localPosition = pos;
        var structureModel = structure.AddComponent<StructureModel>();
        structureModel.CreateModel(prefab);
        return structureModel;
    }

    public void ModifyStructureModel(Vector3Int pos, GameObject newModel, Quaternion rotation)
    {
        if (temporaryRoadObject.ContainsKey(pos))
        {
            temporaryRoadObject[pos].SwapModel(newModel, rotation);
        }
        else if (structures.ContainsKey(pos))
        {
            structures[pos].SwapModel(newModel, rotation);
        }
    }

    public CellType[] GetNeighborTypeFor(Vector3Int pos)
    {
        return placementGrid.GetAllAdjacentCellTypes(pos.x, pos.z);
    }

    public List<Vector3Int> GetNeighborTypeFor(Vector3Int pos, CellType type)
    {
        var neighborVertices = placementGrid.GetAdjacentCellsOfType(pos.x, pos.z, type);
        var neighbors = new List<Vector3Int>();
        foreach (var point in neighborVertices)
        {
            neighbors.Add(new Vector3Int(point.X, 0, point.Y));
        }
        return neighbors;
    }

    public List<Vector3Int> GetPathBetween(Vector3Int startPos, Vector3Int endPos)
    {
        var result = GridSearch.AStarSearch(placementGrid,
                                            new Point(startPos.x, startPos.z),
                                            new Point(endPos.x, endPos.z));
        var path = new List<Vector3Int>();
        foreach (var point in result)
        {
            path.Add(new Vector3Int(point.X, 0, point.Y));
        }
        return path;
    }

    public void RemoveAllTemporaryStructures()
    {
        foreach (var structure in temporaryRoadObject.Values)
        {
            var position = Vector3Int.RoundToInt(structure.transform.position);
            placementGrid[position.x, position.z] = CellType.Empty;
            Destroy(structure.gameObject);
        }
        temporaryRoadObject.Clear();
    }

    public void AddTemporaryStructuresToDictionary()
    {
        foreach (var structure in temporaryRoadObject)
        {
            structures.Add(structure.Key, structure.Value);
            DestroyNature(structure.Key);
        }
        temporaryRoadObject.Clear();
    }

    public void PlaceObjectOnMap(Vector3Int pos, GameObject prefab, CellType type, int width = 1, int height = 1)
    {
        var structure = CreateNewStructureModel(pos, prefab, type);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var newPosition = pos + new Vector3Int(x, 0, y);
                placementGrid[newPosition.x, newPosition.z] = type;
                structures.Add(newPosition, structure);
                DestroyNature(newPosition);
            }
        }
    }

    private void DestroyNature(Vector3Int pos)
    {
        var hits = Physics.BoxCastAll(pos + new Vector3(0, 0.5f, 0),
                                      new Vector3(0.5f, 0.5f, 0.5f),
                                      transform.up,
                                      Quaternion.identity,
                                      1f,
                                      1 << LayerMask.NameToLayer("Nature"));
        foreach (var hit in hits)
        {
            Destroy(hit.collider.gameObject);
        }
    }
}