//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class LeverHandle_useHingeJointMotor3D : MonoBehaviour
//{
//    [SerializeField] private LeverActivate _leverActivate;
//    [SerializeField] private HingeJoint _hingeJoint;
//    private JointMotor _motor;

//    [Header("")]
//    [SerializeField] private LeverHandleReader _leverHandleReader;
//    [SerializeField] private LeverBatteryReader_useHingeJointMotor _batteryReader;

//    private bool _isToggling;
//    private bool _onState;

//    private void Start()
//    {
//        _isToggling = false;
//        _onState = false;

//        _motor = _hingeJoint.motor;
//        ToggleRotateLeverHandle();
//    }

//    private void OnEnable()
//    {
//        CentralInputReader.Input.Player.PickupActivate.started += PickupActivateStarted;
//    }

//    private void OnDisable()
//    {
//        CentralInputReader.Input.Player.PickupActivate.started -= PickupActivateStarted;
//    }

//    private void PickupActivateStarted(InputAction.CallbackContext ctx)
//    {
//        if (!_leverActivate.IsAutomatic && (!_leverActivate.NeedBattery || _batteryReader.BatteryInserted) && _leverHandleReader.PlayerIsInRange)
//        {
//            ToggleActivateLeverHandle_RotateOnly();
//        }
//    }

//    private void OnStateEvalActivate() // run after lever rotation end
//    {
//        bool _prevOnState = _onState;
//        if ((_onState = (_hingeJoint.motor.motorSpeed > 0)) != _prevOnState)
//        {
//            _leverActivate.ActivatedAction();
//        }
//    }

//    private void Update()
//    {
//        if (_isToggling)
//        {
//            if (_hingeJoint.motor.motorSpeed > 0 && _hingeJoint.limits.max - _hingeJoint.jointAngle < 0.1)
//            {
//                _isToggling = false;
//                OnStateEvalActivate();
//            }
//            else if (_hingeJoint.motor.motorSpeed < 0 && _hingeJoint.jointAngle - _hingeJoint.limits.min < 0.1)
//            {
//                _isToggling = false;
//                OnStateEvalActivate();
//            }
//        }
//    }

//    public void ToggleActivateLeverHandle_RotateOnly()
//    {
//        _leverActivate.ToggleActivateBool();
//        ToggleRotateLeverHandle();
//        _isToggling = true;
//    }

//    public void ToggleActivateLeverHandle()
//    {
//        _leverActivate.ToggleActivateBool();
//        ToggleRotateLeverHandle();
//        _isToggling = true;
//        OnStateEvalActivate();
//    }

//    private void ToggleRotateLeverHandle()
//    {
//        //_hingeJoint.attachedRigidbody.velocity = Vector2.zero;
//        //_hingeJoint.attachedRigidbody.totalForce = Vector2.zero;
//        _motor.motorSpeed = -_motor.motorSpeed;
//        _hingeJoint.motor = _motor;
//    }
//}
