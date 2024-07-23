using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class BatteryPickup : MonoBehaviour
{
    //private InputControls _input;

    [SerializeField] private Rigidbody2D _rb;
    private RigidbodyConstraints2D _origRbConstraints;
    private RigidbodyConstraints2D _lockedRbConstraints;

    public Rigidbody2D Rb => _rb; // for public access: not editable by external
    public RigidbodyConstraints2D OrigRbConstraints => _origRbConstraints;
    public RigidbodyConstraints2D LockedRbConstraints => _lockedRbConstraints;

    public bool PlayerInPickupRange { get; set; }
    public bool IsHeldByPlayer { get; set; }
    public bool IsInBox { get; set; }

    public Vector2 AttachOffset = new Vector2(0, 0.3f);
    public float AttachRotation;

    private void Start()
    {
        PlayerInPickupRange = false;
        IsHeldByPlayer = false;
        IsInBox = true;

        _origRbConstraints = _rb.constraints;
        _lockedRbConstraints = _origRbConstraints | RigidbodyConstraints2D.FreezePosition;
    }

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
