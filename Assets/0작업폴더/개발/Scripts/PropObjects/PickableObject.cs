using UnityEngine;
using UnityEngine.InputSystem;

public class PickableObject : MonoBehaviour // WITHOUT collision nor mass of object
{
    [SerializeField] protected Rigidbody2D _objRb;
    [SerializeField] protected Collider2D _objCol;
    [SerializeField] protected float _pickedUpZAngle = 0;
    private Vector3 _pickedUpEulerAngle;
    private int _pickedUpFaceDir;
    private int _prevFaceDir;

    //protected static float _playerCanHoldMaxMass = 5;
    protected bool _playerIsInRange;

    public bool IsHeldByPlayer { get; protected set; }

    private void Awake()
    {
        IsHeldByPlayer = false;
        _playerIsInRange = false;
        _prevFaceDir = 1;
    }

    private void OnEnable()
    {
        CentralInputReader.Input.Player.PickupActivate.started += PickupActivateStarted;
    }

    private void OnDisable()
    {
        CentralInputReader.Input.Player.PickupActivate.started -= PickupActivateStarted;
    }

    private void Update()
    {
        if (IsHeldByPlayer && _prevFaceDir != PlayerLogic.AnimController.FaceDirX)
        {
            _prevFaceDir = PlayerLogic.AnimController.FaceDirX;
            UpdateObjectFacingDir();
        }
    }

    private void PickupActivateStarted(InputAction.CallbackContext ctx)
    {
        if (IsHeldByPlayer)
        {
            DetachObjectFromPlayer();
        }
        else if /* !IsHeldByPlayer */(!PlayerLogic.PlayerHoldingSomething)
        {
            if (_playerIsInRange /*&& _objRb.mass < _playerCanHoldMaxMass + 0.1f*/)
                AttachObjectToPlayer();
        }
    }

    protected virtual void AttachObjectToPlayer()
    {
        _objRb.simulated = false;
        _objRb.transform.SetParent(PlayerLogic.Player.transform, true);
        SetObjectFacingDir();

        IsHeldByPlayer = true;
        PlayerLogic.PlayerHoldingSomething = true;
    }

    protected virtual void DetachObjectFromPlayer()
    {
        IsHeldByPlayer = false;
        PlayerLogic.PlayerHoldingSomething = false;

        _objRb.transform.SetParent(null, true);
        _objRb.simulated = true;
    }

    protected void SetObjectFacingDir()
    {
        _pickedUpFaceDir = PlayerLogic.AnimController.FaceDirX;
        _pickedUpEulerAngle = new Vector3(0, 0, _pickedUpZAngle);

        setObjAngle();
        Physics2D.SyncTransforms();
        setObjPosition(PlayerLogic.AnimController.FaceDirX);
    }

    private void setObjAngle()
    {
        _objRb.transform.eulerAngles = _pickedUpEulerAngle;
        _objCol.transform.eulerAngles = _pickedUpEulerAngle;
    }

    private void setObjPosition(int faceDirX)
    {
        Vector3 halfSize = _objCol.bounds.extents;
        if (faceDirX < 0) halfSize.x = -halfSize.x;
        halfSize.z = 0;

        _objRb.transform.position = PlayerLogic.Player.transform.position + halfSize;
        _objCol.transform.position = _objRb.transform.position;
    }

    private void UpdateObjectFacingDir()
    {
        if (_pickedUpFaceDir != PlayerLogic.AnimController.FaceDirX) FlipAngleAndFaceDir();
        setObjPosition(PlayerLogic.AnimController.FaceDirX);
    }

    private void FlipAngleAndFaceDir()
    {
        _pickedUpFaceDir = -_pickedUpFaceDir;
        _pickedUpEulerAngle.z = -_pickedUpEulerAngle.z;
        _objRb.transform.eulerAngles = _pickedUpEulerAngle;
        _objCol.transform.eulerAngles = _pickedUpEulerAngle;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag(Tags.PlayerObjectPickupperTag))
        {
            if (!PlayerLogic.PlayerHoldingSomething) _playerIsInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag(Tags.PlayerObjectPickupperTag))
        {
            _playerIsInRange = false;
        }
    }
}
