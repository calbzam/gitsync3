using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BatteryCase : MonoBehaviour
{
    private InputControls _input;

    [SerializeField] private BatteryPickup _battery;
    [SerializeField] private Transform _batteryTransform;

    private Rigidbody2D _batteryRb;
    private RigidbodyConstraints2D _origRbConstraints;

    private bool _playerInPickupRange = false;

    private void Awake()
    {
        _input = new InputControls();
        _batteryRb = _battery.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _playerInPickupRange = false;
        _origRbConstraints = _batteryRb.constraints;
    }

    private void OnEnable()
    {
        _input.Enable();
        //_input.Player.PickUpItem.started += PickUpItemStarted;
    }

    private void OnDisable()
    {
        _input.Disable();
        //_input.Player.PickUpItem.started -= PickUpItemStarted;
    }

    //private void PickUpItemStarted(InputAction.CallbackContext ctx)
    //{
    //    //if (_battery.BatteryIsInBox)
    //    //{
    //        //if (_playerInPickupRange) _battery.AttachBatteryToPlayer();
    //        if (_playerInPickupRange)
    //        {
    //            _batteryTransform.SetParent(PlayerLogic.Player.transform, false);
    //            _batteryRb.constraints = RigidbodyConstraints2D.FreezeAll;
    //            _battery.BatteryIsInBox = false;
    //        }
    //    //}
    //    else
    //    {
    //        _batteryTransform.SetParent(null);
    //    }
    //}

    //public void SetBatteryParent(Transform parent)
    //{
    //    _battery.transform.SetParent(parent);

    //    if (parent == null) _batteryRb.constraints = _origRbConstraints;
    //    else _batteryRb.constraints = _origRbConstraints | RigidbodyConstraints2D.FreezePosition;
    //}

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _playerInPickupRange = true;

            //_batteryRb.constraints = RigidbodyConstraints2D.FreezeAll;
            _batteryRb.simulated = false;
            _batteryTransform.SetParent(col.transform);
            _batteryTransform.localPosition = Vector3.zero;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _playerInPickupRange = false;
        }
    }
}
