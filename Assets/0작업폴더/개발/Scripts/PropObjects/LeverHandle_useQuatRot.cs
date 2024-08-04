using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeverHandle_useQuatRot : MonoBehaviour
{
    [SerializeField] private LeverActivate _leverActivate;

    [Header("")]
    [SerializeField] private Transform _leverHandleTransform;
    [SerializeField] private LeverHandleReader _leverHandleReader;
    [SerializeField] private LeverBatteryReader _batteryReader;

    [Header("")] // useQuatRot variables
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
        _rotating = false;
    }

    private void Update()
    {
        RotateLeverHandle();
    }

    private void PickupActivateStarted(InputAction.CallbackContext ctx)
    {
        if (!_leverActivate.IsAutomatic && _batteryReader.BatteryInserted && _leverHandleReader.PlayerIsInRange)
        {
            ToggleActivateLeverHandle();
        }
    }

    public void ToggleActivateLeverHandle()
    {
        _leverActivate.ToggleActivate();
        ToggleRotateLeverHandle();
        _leverActivate.ActivatedAction();
    }

    private void ToggleRotateLeverHandle()
    {
        _currentAngle = transform.rotation.eulerAngles.z;
        _targetAngle = _leverActivate.IsActivated ? _activatedRot : _deactivatedRot;
        _rotating = true;
    }

    private void RotateLeverHandle()
    {
        if (_rotating)
        {
            _currentAngle = Mathf.MoveTowardsAngle(_currentAngle, _targetAngle, Time.deltaTime * _rotationSpeed);
            _leverHandleTransform.rotation = Quaternion.Euler(0, 0, _currentAngle);

            if (_leverActivate.IsActivated) { if (_currentAngle <= _targetAngle) _rotating = false; }
            else { if (_currentAngle >= _targetAngle) _rotating = false; }
        }
    }
}
