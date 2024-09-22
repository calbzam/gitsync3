using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeverHandle_useQuatRot : MonoBehaviour
{
    [SerializeField] private LeverActivate _leverActivate;

    [Header("")]
    [SerializeField] private LeverHandleReader _leverHandleReader;
    [SerializeField] private LeverBatteryReader_useQuatRot _batteryReader;

    private bool _toggleStarted;
    private bool _onState;

    [Header("")] // useQuatRot variables
    [SerializeField] private float _activatedRot = 0f;
    [SerializeField] private float _deactivatedRot = 45f;
    [SerializeField] private float _rotationSpeed = 100f;
    private float _currentAngle;
    private float _targetAngle;

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
        _toggleStarted = false;
        _onState = false;
    }

    private void Update()
    {
        RotateLeverHandle();
    }

    private void PickupActivateStarted(InputAction.CallbackContext ctx)
    {
        if (!_leverActivate.IsAutomatic && (!_leverActivate.NeedBattery || _batteryReader.BatteryInserted) && _leverHandleReader.PlayerIsInRange)
        {
            ToggleActivateLeverHandle_RotateOnly();
        }
    }

    private void OnStateEvalActivate() // run after lever rotation end
    {
        bool _prevOnState = _onState;
        if ((_onState = (_targetAngle == _activatedRot)) != _prevOnState)
        {
            _leverActivate.ActivatedAction();
        }
    }

    public void ToggleActivateLeverHandle_RotateOnly()
    {
        _leverActivate.ToggleActivateBool();
        ToggleRotateLeverHandle();
        _toggleStarted = true;
    }

    public void ToggleActivateLeverHandle()
    {
        _leverActivate.ToggleActivateBool();
        ToggleRotateLeverHandle();
        _toggleStarted = true;
        OnStateEvalActivate();
    }

    public void ToggleRotateLeverHandle()
    {
        _currentAngle = transform.rotation.eulerAngles.z;
        _targetAngle = _leverActivate.IsActivated ? _activatedRot : _deactivatedRot;
    }

    private void RotateLeverHandle()
    {
        if (_toggleStarted)
        {
            _currentAngle = Mathf.MoveTowardsAngle(_currentAngle, _targetAngle, Time.deltaTime * _rotationSpeed);
            transform.rotation = Quaternion.Euler(0, 0, _currentAngle);

            if (_leverActivate.IsActivated) { if (_currentAngle <= _targetAngle) _toggleStarted = false; }
            else { if (_currentAngle >= _targetAngle) _toggleStarted = false; }

            if (!_toggleStarted) // 직전에 if (_toggleStarted) 안에서 수정된 _toggleStarted
            {
                OnStateEvalActivate();
            }
        }
    }
}
