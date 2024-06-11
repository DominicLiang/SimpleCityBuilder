using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public LayerMask groundMask;

    [SerializeField] Camera mainCamera;

    public Vector2 CameraMovementVector { get; private set; }

    public event Action<Vector3Int> OnMouseDown;
    public event Action<Vector3Int> OnMouseHold;
    public event Action OnMouseUp;

    private void Update()
    {
        CheckMouseDownEvent();
        CheckMouseHoldEvent();
        CheckMouseUpEvent();
        CheckArrowInput();
    }

    private Vector3Int? RaycastGround()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, groundMask)) return null;
        return Vector3Int.RoundToInt(hit.point);
    }

    private void CheckMouseDownEvent()
    {
        if (!Input.GetMouseButtonDown(0) || EventSystem.current.IsPointerOverGameObject()) return;
        var pos = RaycastGround();
        if (pos == null) return;
        OnMouseDown?.Invoke(pos.Value);
    }

    private void CheckMouseHoldEvent()
    {
        if (!Input.GetMouseButton(0) || EventSystem.current.IsPointerOverGameObject()) return;
        var pos = RaycastGround();
        if (pos == null) return;
        OnMouseHold?.Invoke(pos.Value);
    }

    private void CheckMouseUpEvent()
    {
        if (!Input.GetMouseButtonUp(0) || EventSystem.current.IsPointerOverGameObject()) return;
        OnMouseUp?.Invoke();
    }

    private void CheckArrowInput()
    {
        CameraMovementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
}