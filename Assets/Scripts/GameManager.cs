using System;
using SVS;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CameraMovement cameraMovement;

    private InputManager inputManager;
    private RoadManager roadManager;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        roadManager = GetComponent<RoadManager>();
    }

    private void Start()
    {
        inputManager.OnMouseDown += HandleMouseDown;
    }

    private void Update()
    {
        cameraMovement.MoveCamera(new Vector3(inputManager.CameraMovementVector.x,
                                              0,
                                              inputManager.CameraMovementVector.y));
    }

    private void HandleMouseDown(Vector3Int pos)
    {
        roadManager.PlaceRoad(pos);
    }
}