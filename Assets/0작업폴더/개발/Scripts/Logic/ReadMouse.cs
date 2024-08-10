using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReadMouse : MonoBehaviour
{
    public Vector3 MouseClickOrigin { get; private set; }
    public bool IsDragging { get; private set; }
    public float RefPosZ { get; set; }

    //public ReadMouse(float mousePosZ = 0) { MousePosZ = mousePosZ; } // no constructor for AddComponent<MonoBehaviour type<

    private void Start()
    {
        IsDragging = false;
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
        if (ctx.started) MouseClickOrigin = GetWorldMousePos(RefPosZ);
        IsDragging = ctx.started || ctx.performed;
    }

    public static Vector3 GetWorldMousePos(float refPosZ = 0) // for static
    {
        Vector3 mousePos3D = Mouse.current.position.ReadValue();
        mousePos3D.z = -refPosZ;
        return Camera.main.ScreenToWorldPoint(mousePos3D);
    }

    public Vector3 GetWorldMousePos() // for object
    {
        return GetWorldMousePos(RefPosZ);
    }

    public static float GetScrollAmount()
    {
        return CentralInputReader.Input.Camera.Zoom.ReadValue<float>();
    }
}
