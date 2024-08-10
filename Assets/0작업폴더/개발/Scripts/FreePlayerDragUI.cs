using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class FreePlayerDragUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private bool _isDragging;
    [SerializeField] private CameraLogic _cameraLogic;
    [SerializeField] private FreeCamMove _freeCamMove;

    private bool _freeCamWasEnabled;

    private void Start()
    {
        _isDragging = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDragging = true;
        _freeCamMove.DragMouseEnabled = false;
        PlayerLogic.LockPlayerPosition();
        if (!(_freeCamWasEnabled = _freeCamMove.gameObject.activeSelf)) _cameraLogic.ToggleFreeCam();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _isDragging = true;
        transform.position = Mouse.current.position.ReadValue();
        PlayerLogic.SetPlayerXYPos(ReadMouse.GetWorldMousePos(_freeCamMove.CamPosZ));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;
        _freeCamMove.DragMouseEnabled = true;
        PlayerLogic.FreePlayerPosition();
        if (!_freeCamWasEnabled) _cameraLogic.ToggleFreeCam();
    }

    private void LateUpdate()
    {
        if (!_isDragging)
        {
            MoveToPlayerPosition();
        }
    }

    public void MoveToPlayerPosition()
    {
        transform.position = Camera.main.WorldToScreenPoint(PlayerLogic.Player.transform.position);
    }
}
