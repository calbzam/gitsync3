using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FreeCameraDrag : MonoBehaviour
{
    private Vector3 _mouseOrigin;
    private bool _isDragging;

    public float CamPosZ { get; private set; }

    private void Start()
    {
        _isDragging = false;
        CamPosZ = transform.position.z;
    }

    private void OnEnable()
    {
        CentralInputReader.Input.Camera.Drag.started += DragEvaluate;
        CentralInputReader.Input.Camera.Drag.performed += DragEvaluate;
        CentralInputReader.Input.Camera.Drag.canceled += DragEvaluate;
    }

    private void OnDisable()
    {
        CentralInputReader.Input.Camera.Drag.started -= DragEvaluate;
        CentralInputReader.Input.Camera.Drag.performed -= DragEvaluate;
        CentralInputReader.Input.Camera.Drag.canceled -= DragEvaluate;
    }

    public void DragEvaluate(InputAction.CallbackContext ctx)
    {
        if (ctx.started) _mouseOrigin = getWorldMousePos();
        _isDragging = ctx.started || ctx.performed;
    }

    private Vector3 getWorldMousePos()
    {
        Vector3 mousePos3D = Mouse.current.position.ReadValue();
        mousePos3D.z = -CamPosZ;
        return Camera.main.ScreenToWorldPoint(mousePos3D);
    }

    private void LateUpdate()
    {
        if (_isDragging)
        {
            transform.position += _mouseOrigin - getWorldMousePos();
        }
    }
}
