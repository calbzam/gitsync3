using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeverHandle_useHingeJointMotor : MonoBehaviour
{
    [SerializeField] private LeverActivate _leverActivate;
    [SerializeField] private HingeJoint2D _hingeJoint;
    private JointMotor2D _motor;

    [Header("")]
    [SerializeField] private LeverHandleReader _leverHandleReader;
    [SerializeField] private LeverBatteryReader _batteryReader;

    private void Start()
    {
        _motor = _hingeJoint.motor;
        ToggleRotateLeverHandle();
    }

    private void OnEnable()
    {
        CentralInputReader.Input.Player.PickupActivate.started += PickupActivateStarted;
    }

    private void OnDisable()
    {
        CentralInputReader.Input.Player.PickupActivate.started -= PickupActivateStarted;
    }

    private void PickupActivateStarted(InputAction.CallbackContext ctx)
    {
        if (!_leverActivate.IsAutomatic && (!_leverActivate.NeedBattery || _batteryReader.BatteryInserted) && _leverHandleReader.PlayerIsInRange)
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
        _hingeJoint.attachedRigidbody.velocity = Vector2.zero;
        //_hingeJoint.attachedRigidbody.totalForce = Vector2.zero;
        _motor.motorSpeed = -_motor.motorSpeed;
        _hingeJoint.motor = _motor;
    }
}
