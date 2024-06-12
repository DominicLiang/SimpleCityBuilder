using System;
using SVS;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CameraMovement cameraMovement;

    private InputManager inputManager;
    private RoadManager roadManager;
    private StructureManager structureManager;
    private UIController uiController;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        roadManager = GetComponent<RoadManager>();
        structureManager = GetComponent<StructureManager>();
        uiController = GetComponent<UIController>();
    }

    private void Start()
    {
        uiController.OnRoadPlacement += RoadPlacementHandler;
        uiController.OnHousePlacement += HousePlacementHandler;
        uiController.OnSpecialPlacement += SpecialPlacementHandler;
        uiController.OnBigStructurePlacement += BigStructurePlacementHandler;
    }

    private void BigStructurePlacementHandler()
    {
        ClearInputActions();

        inputManager.OnMouseDown += structureManager.PlaceBigStructure;
    }

    private void SpecialPlacementHandler()
    {
        ClearInputActions();

        inputManager.OnMouseDown += structureManager.PlaceSpecial;
    }

    private void HousePlacementHandler()
    {
        ClearInputActions();

        inputManager.OnMouseDown += structureManager.PlaceHouse;
    }

    private void RoadPlacementHandler()
    {
        ClearInputActions();

        inputManager.OnMouseDown += roadManager.PlaceRoad;
        inputManager.OnMouseHold += roadManager.PlaceRoad;
        inputManager.OnMouseUp += roadManager.FinishPlacingRoad;
    }

    private void ClearInputActions()
    {
        inputManager.OnMouseDown -= roadManager.PlaceRoad;
        inputManager.OnMouseHold -= roadManager.PlaceRoad;
        inputManager.OnMouseUp -= roadManager.FinishPlacingRoad;

        inputManager.OnMouseDown -= structureManager.PlaceHouse;

        inputManager.OnMouseDown -= structureManager.PlaceSpecial;

        inputManager.OnMouseDown -= structureManager.PlaceBigStructure;
    }

    private void Update()
    {
        cameraMovement.MoveCamera(new Vector3(inputManager.CameraMovementVector.x,
                                              0,
                                              inputManager.CameraMovementVector.y));
    }
}