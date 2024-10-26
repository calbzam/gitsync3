using UnityEngine;
using UnityEngine.InputSystem;

public class ReadMouse : MonoBehaviour
{
    public Vector3 MouseClickOrigin { get; private set; }
    public bool IsDragging { get; private set; }
    public float RefPosZ { get; set; }

    //public ReadMouse(float mousePosZ = 0) { MousePosZ = mousePosZ; } // no constructor for AddComponent<MonoBehaviour type>

    private void Awake()
    {
        IsDragging = false; // note: was originally in Start
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
        if (ctx.started) MouseClickOrigin = ReadMouse.GetWorldMousePos(RefPosZ);
        IsDragging = ctx.started || ctx.performed;
    }

    public static Vector3 GetWorldMousePos(float refPosZ = 0, float multiplier=2f) // for static
    {
        Vector3 mousePos3D = Mouse.current.position.ReadValue();
        mousePos3D.z = -refPosZ;

        Vector3 freeAspectPos;
        freeAspectPos.x = mousePos3D.x *1f/ Screen.width * 1920f;  freeAspectPos.x *= multiplier; // because of new rendertexture width
        freeAspectPos.y = mousePos3D.y *1f/ Screen.height * 1080f;
        freeAspectPos.z = mousePos3D.z;

        Vector3 screenToWorldPos = Camera.main.ScreenToWorldPoint(freeAspectPos);
        return screenToWorldPos;
    }

    public static Vector3 GetWorldMousePosOrig(float refPosZ = 0) // for static
    {
        Vector3 mousePos3D = Mouse.current.position.ReadValue();
        mousePos3D.z = -refPosZ;

        Vector3 screenToWorldPos = Camera.main.ScreenToWorldPoint(mousePos3D);
        return screenToWorldPos;
    }

    public Vector3 GetWorldMousePos() // for object
    {
        return ReadMouse.GetWorldMousePos(RefPosZ);
    }

    public Vector3 GetWorldMousePos(float multiplier) // for object
    {
        return ReadMouse.GetWorldMousePos(RefPosZ, multiplier);
    }

    public static float GetScrollAmount()
    {
        return CentralInputReader.Input.Camera.Zoom.ReadValue<float>();
    }
}
