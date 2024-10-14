using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class LeverHandle_useQuatRot : MonoBehaviour
{
    [SerializeField] private LeverActivate _leverActivate;

    [Header("")]
    [SerializeField] private LeverHandleReader _leverHandleReader;
    [SerializeField] private LeverBatteryReader_useQuatRot _batteryReader;

    private bool _isToggling;
    private bool _onState;

    [Header("")] // useQuatRot variables
    [SerializeField] private float _activatedRot = 0f;
    [SerializeField] private float _deactivatedRot = 45f;
    [SerializeField] private float _rotationSpeed = 100f;
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
        _isToggling = false;
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
        _isToggling = true;
    }

    public void ToggleActivateLeverHandle()
    {
        _leverActivate.ToggleActivateBool();
        ToggleRotateLeverHandle();
        _isToggling = true;
        OnStateEvalActivate();
    }

    public void ToggleRotateLeverHandle()
    {
        _targetAngle = _leverActivate.IsActivated ? _activatedRot : _deactivatedRot;
    }

    private void RotateLeverHandle()
    {
        if (_isToggling)
        {
            if (MyMath.RotateAndEvalDone(transform, _targetAngle, _rotationSpeed)) _isToggling = false;

            if (!_isToggling) OnStateEvalActivate();
        }
    }
}
