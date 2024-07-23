using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPickupper : MonoBehaviour
{
    private InputControls _input;

    private BatteryPickup _battery;

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

    private void PickUpItemStarted(InputAction.CallbackContext ctx)
    {
        BatteryAttachToPlayer();
    }

    public void BatteryAttachToPlayer()
    {
        if (_battery.IsHeldByPlayer)
        {
            Debug.Log("detached");
            _battery.transform.SetParent(null);
            _battery.Rb.constraints = _battery.OrigRbConstraints;
        }
        else
        {
            Debug.Log("attached");
            _battery.transform.SetParent(PlayerLogic.PlayerRb.transform);
            _battery.Rb.constraints = _battery.LockedRbConstraints;

            _battery.transform.localPosition = Vector2.zero + _battery.AttachOffset;
            //_battery.transform.rotation = Quaternion.Euler(new Vector3(0, 0, _battery.AttachRotation));

            if (_battery.IsInBox) _battery.IsInBox = false;
            _battery.IsHeldByPlayer = true;
        }

        _battery.IsHeldByPlayer = !_battery.IsHeldByPlayer;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Battery"))
        {
            _battery = col.GetComponent<BatteryPickup>();
            _battery.PlayerInPickupRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Battery"))
        {
            _battery = col.GetComponent<BatteryPickup>();
            _battery.PlayerInPickupRange = false;
        }
    }
}
