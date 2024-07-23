using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class BatteryPickup : MonoBehaviour
{
    private InputControls _input;

    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Vector2 _rightOffset = new Vector2(0, 0.6f);
    [SerializeField] private float _zRotation = 8;
    private Vector2 _leftOffset;
    private Quaternion _leftRotation, _rightRotation;

    public RigidbodyConstraints2D OrigRbConstraints { get; private set; } // unused at the moment

    public bool PlayerInPickupRange { get; set; }
    public bool IsHeldByPlayer { get; private set; }
    public bool BatteryIsInBox { get; private set; }

    private void Awake()
    {
        _input = new InputControls();
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.PickUpItem.started += PickUpItemStarted;
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.Player.PickUpItem.started -= PickUpItemStarted;
    }

    private void Start()
    {
        _rb.simulated = false;
        _leftOffset = new Vector2(-_rightOffset.x, _rightOffset.y);
        _leftRotation = Quaternion.Euler(new Vector3(0, 0, -_zRotation));
        _rightRotation = Quaternion.Euler(new Vector3(0, 0, _zRotation));

        PlayerInPickupRange = false;
        IsHeldByPlayer = false;
        BatteryIsInBox = true;
        OrigRbConstraints = _rb.constraints;
    }

    private void Update()
    {
        if (IsHeldByPlayer) SetBatteryFacingDir();
    }

    private void PickUpItemStarted(InputAction.CallbackContext ctx)
    {
        ToggleAttachBatteryToPlayer();
    }

    public void SetBatteryParent(Transform toParent)
    {
        _rb.simulated = (toParent == null) ? true : false;
        transform.SetParent(toParent);
    }

    private void SetBatteryFacingDir()
    {
        if (InputReader.FrameInput.Move.x < 0)
        {
            transform.localPosition = _leftOffset;
            transform.rotation = _leftRotation;
        }
        else if (InputReader.FrameInput.Move.x > 0)
        {
            transform.localPosition = _rightOffset;
            transform.rotation = _rightRotation;
        }
    }

    public void ToggleAttachBatteryToPlayer()
    {
        if (IsHeldByPlayer)
        {
            DetachFromPlayer();
        }
        else
        {
            if (PlayerInPickupRange) AttachToPlayer();
        }
    }

    public void DetachFromPlayer()
    {
        SetBatteryParent(null);
        IsHeldByPlayer = false;
    }

    public void AttachToPlayer()
    {
        SetBatteryParent(PlayerLogic.Player.transform);
        SetBatteryFacingDir();

        if (BatteryIsInBox) BatteryIsInBox = false;
        IsHeldByPlayer = true;
    }
}
