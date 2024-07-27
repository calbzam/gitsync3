using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeverHandleReader : MonoBehaviour
{
    [SerializeField] private Transform _leverHandleTransform;
    [SerializeField] private LeverBatteryReader _batteryReader;

    [SerializeField] private float _activatedRot = 0f;
    [SerializeField] private float _deactivatedRot = 45f;
    [SerializeField] private float _rotationSpeed = 100f;
    private float _currentAngle;
    private float _targetAngle;

    public bool PlayerIsInRange { get; private set; }
    public bool IsActivated { get; private set; }

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
        PlayerIsInRange = false;
        IsActivated = false;
        _rotating = false;
    }

    private void Update()
    {
        RotateLeverHandle();
    }

    private void PickupActivateStarted(InputAction.CallbackContext ctx)
    {
        if (_batteryReader.BatteryInserted && PlayerIsInRange)
        {
            ToggleActivateLeverHandle();
        }
    }

    private void ToggleActivateLeverHandle()
    {
        IsActivated = !IsActivated;
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
