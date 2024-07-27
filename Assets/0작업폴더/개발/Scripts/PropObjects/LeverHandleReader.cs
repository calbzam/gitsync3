using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeverHandleReader : MonoBehaviour
{
    [SerializeField] private LeverBatteryReader _batteryReader;
    [SerializeField] private Transform _leverHandleTransform;
    [SerializeField] private Transform _rotationPivot;
    [SerializeField] private float _rotateAmount = 45;

    public bool PlayerIsInRange { get; private set; }
    public bool IsActivated { get; private set; }

    private void OnEnable()
    {
        CentralInputReader.Input.Player.PickupActivate.started += PickupActivateStarted;
    }

    private void OnDisable()
    {
        CentralInputReader.Input.Player.PickupActivate.started -= PickupActivateStarted;
    }

    private void Start()
    {
        PlayerIsInRange = false;
        IsActivated = false;
    }

    private void PickupActivateStarted(InputAction.CallbackContext ctx)
    {
        if (_batteryReader.BatteryInserted && PlayerIsInRange)
        {
            IsActivated = !IsActivated;
            ToggleActivateLeverHandle();
        }
    }

    private void ToggleActivateLeverHandle()
    {
        float angle = IsActivated ? _rotateAmount : -_rotateAmount;

        _leverHandleTransform.RotateAround(_rotationPivot.position, Vector3.back, angle);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerIsInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerIsInRange = false;
        }
    }
}
