using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class BatteryPickup : MonoBehaviour
{
    private InputControls _input;

    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Vector2 _offset;
    [SerializeField] private float _rotation;

    public RigidbodyConstraints2D OrigRbConstraints { get; private set; } // unused at the moment

    public bool PlayerInPickupRange { get; set; }

    public bool IsHeldByPlayer { get; private set; }
    public bool BatteryIsInBox { get; private set; }

    private void Awake()
    {
        _input = new InputControls();
    }

    private void Start()
    {
        _rb.simulated = false;

        PlayerInPickupRange = false;
        IsHeldByPlayer = false;
        BatteryIsInBox = true;
        OrigRbConstraints = _rb.constraints;
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

    private void PickUpItemStarted(InputAction.CallbackContext ctx)
    {
        ToggleAttachBatteryToPlayer();
    }

    public void SetBatteryParent(Transform toParent)
    {
        _rb.simulated = (toParent == null) ? true : false;
        transform.SetParent(toParent);
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
        transform.localPosition = Vector2.zero + _offset;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, _rotation));

        if (BatteryIsInBox) BatteryIsInBox = false;
        IsHeldByPlayer = true;
    }
}
