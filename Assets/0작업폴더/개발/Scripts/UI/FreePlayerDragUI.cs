using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class FreePlayerDragUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private bool _isDragging;
    [SerializeField] private CameraLogic _cameraLogic;
    [SerializeField] private FreeCamMove _freeCamMove;
    [SerializeField] private float multiplier = 2f;

    private bool _freeCamWasEnabled;

    private void Start()
    {
        _isDragging = false;
    }

    public void OnBeginDrag(PointerEventData eventData) // IBeginDragHandler override
    {
        _isDragging = true;
        _freeCamMove.DragMouseEnabled = false;
        PlayerLogic.LockPlayer();
        if (!(_freeCamWasEnabled = _freeCamMove.gameObject.activeSelf)) _cameraLogic.SetFreeCam(true);
    }

    public void OnDrag(PointerEventData eventData) // IDragHandler override
    {
        _isDragging = true;
        transform.position = Mouse.current.position.ReadValue();
        PlayerLogic.SetPlayerXYPos(ReadMouse.GetWorldMousePos(_freeCamMove.CamPosZ - PlayerLogic.Player.transform.position.z, multiplier));
        MoveUIToPlayerPosition();
    }

    public void OnEndDrag(PointerEventData eventData) // IEndDragHandler override
    {
        _isDragging = false;
        _freeCamMove.DragMouseEnabled = true;
        PlayerLogic.FreePlayer();
        PlayerLogic.NearestPlayerRespawn.SetToPrevRespawnPos();
        if (!_freeCamWasEnabled) _cameraLogic.SetFreeCam(false);
    }

    private void LateUpdate()
    {
        if (!_isDragging)
        {
            MoveUIToPlayerPosition();
        }
    }

    public void MoveUIToPlayerPosition()
    {
        transform.position = GetWorldToFreeAspectPoint(PlayerLogic.Player.transform.position);
    }

    private Vector2 GetWorldToFreeAspectPoint(Vector3 position)
    {
        Vector3 worldToScreenPos = Camera.main.WorldToScreenPoint(position);
        Vector2 freeAspectPos;
        freeAspectPos.x = Screen.width / 1920f * worldToScreenPos.x / 2; // because of new rendertexture width
        freeAspectPos.y = Screen.height / 1080f * worldToScreenPos.y;
        return freeAspectPos;
    }
}
