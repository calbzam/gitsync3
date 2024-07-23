using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class BatteryPickup : MonoBehaviour
{
    private InputControls _input;

    [SerializeField] private Rigidbody2D _rb;

    [SerializeField] private BatteryCase _batteryCase;
    [SerializeField] private BatteryPickupRangeChecker _batteryPickupRangeChecker;

    private bool _isHeldByPlayer = false;
    public bool IsHeldByPlayer => _isHeldByPlayer; // for public access
    public bool BatteryIsInBox = true;

    [SerializeField] private Vector2 _offset;
    [SerializeField] private float _rotation;
    //[SerializeField] private float _pickupRangeRadius = 1.1f;

    //private void Awake()
    //{
    //    _input = new InputControls();
    //}

    private void Start()
    {
        _isHeldByPlayer = false;
        BatteryIsInBox = true;
    }

    //private void OnEnable()
    //{
    //    _input.Enable();
    //    _input.Player.PickUpItem.started += PickUpItemStarted;
    //}

    //private void OnDisable()
    //{
    //    _input.Disable();
    //    _input.Player.PickUpItem.started -= PickUpItemStarted;
    //}

    //private void PickUpItemStarted(InputAction.CallbackContext ctx)
    //{
    //    if (!BatteryIsInBox)
    //    {
    //        ToggleAttachBatteryToPlayer();
    //    }
    //}

    //public void ToggleAttachBatteryToPlayer()
    //{
    //    if (_isHeldByPlayer)
    //    {
    //        DetachBatteryFromPlayer();
    //    }
    //    else
    //    {
    //        if (_batteryPickupRangeChecker.PlayerInPickupRange)
    //            AttachBatteryToPlayer();
    //    }

    //    _isHeldByPlayer = !_isHeldByPlayer;
    //}

    //public void AttachBatteryToPlayer()
    //{
    //    Debug.Log("attached");
    //    _batteryCase.SetBatteryParent(PlayerLogic.Player.transform);

    //    transform.localPosition = Vector2.zero + _offset;
    //    transform.rotation = Quaternion.Euler(new Vector3(0, 0, _rotation));

    //    if (BatteryIsInBox) BatteryIsInBox = false;
    //    _isHeldByPlayer = true;
    //}

    //public void DetachBatteryFromPlayer()
    //{
    //    Debug.Log("detached");
    //    _batteryCase.SetBatteryParent(null);
    //}




    //private bool PlayerInPickupRange()
    //{
    //    return Physics2D.OverlapCircle(transform.position, _pickupRangeRadius, Layers.PlayerLayer);
    //}

    //#if UNITY_EDITOR
    //    private void OnDrawGizmos()
    //    {
    //        Handles.DrawWireDisc(transform.position, Vector3.back, _pickupRangeRadius);
    //    }
    //#endif
}
