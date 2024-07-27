using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeverActivate : MonoBehaviour
{
    public bool IsActivated { get; private set; }

    [SerializeField] private Transform _leverHandleTransform;
    [SerializeField] private LeverHandleReader _leverHandleReader;
    [SerializeField] private LeverBatteryReader _batteryReader;

    [SerializeField] private float _activatedRot = 0f;
    [SerializeField] private float _deactivatedRot = 45f;
    [SerializeField] private float _rotationSpeed = 100f;
    private float _currentAngle;
    private float _targetAngle;
    private bool _rotating;

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
        IsActivated = false;
        _rotating = false;
    }

    private void Update()
    {
        RotateLeverHandle();
    }

    private void PickupActivateStarted(InputAction.CallbackContext ctx)
    {
        if (_batteryReader.BatteryInserted && _leverHandleReader.PlayerIsInRange)
        {
            ToggleActivateLeverHandle();
        }
    }

    private void ToggleActivateLeverHandle()
    {
        IsActivated = !IsActivated;
        ToggleRotateLeverHandle();
    }

    private void ToggleRotateLeverHandle()
    {
        _currentAngle = transform.rotation.eulerAngles.z;
        _targetAngle = IsActivated ? _activatedRot : _deactivatedRot;
        _rotating = true;
    }

    private void RotateLeverHandle()
    {
        if (_rotating)
        {
            _currentAngle = Mathf.MoveTowardsAngle(_currentAngle, _targetAngle, Time.deltaTime * _rotationSpeed);
            _leverHandleTransform.rotation = Quaternion.Euler(0, 0, _currentAngle);

            if (IsActivated) { if (_currentAngle <= _targetAngle) _rotating = false; }
            else { if (_currentAngle >= _targetAngle) _rotating = false; }
        }
    }
}
