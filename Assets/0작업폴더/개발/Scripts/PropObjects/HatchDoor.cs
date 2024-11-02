using System.Security.Cryptography;
using UnityEngine;

public class HatchDoor : LeverConnectedObject
{
    [SerializeField] private HingeJoint2D _hatchLid;
    private JointMotor2D _motor;

    private float _thresholdSpeed = 5;

    private TargetAngleState _targetAngleState;
    private enum TargetAngleState { MaxAngle, MinAngle, }

    private float _targetAngle;

    private void Start()
    {
        _motor = _hatchLid.motor;
        InitTargetAngle();

        FlipMotor();
        _hatchLid.attachedRigidbody.bodyType = RigidbodyType2D.Static;
        _hatchLid.enabled = false;
    }

    public override void ActivatedAction(bool enabledState)
    {
        _hatchLid.enabled = true;
        _hatchLid.attachedRigidbody.bodyType = RigidbodyType2D.Dynamic;
        FlipMotor();
    }

    private void FlipMotor()
    {
        _motor.motorSpeed = -_motor.motorSpeed;
        _hatchLid.motor = _motor;
    }

    private void InitTargetAngle()
    {
        if (_hatchLid.limits.max - _hatchLid.jointAngle < _hatchLid.jointAngle - _hatchLid.limits.min)
            SetTargetAngle(TargetAngleState.MaxAngle);
        else
            SetTargetAngle(TargetAngleState.MinAngle);
    }

    private void FlipTargetAngle()
    {
        if (_targetAngleState == TargetAngleState.MaxAngle) SetTargetAngle(TargetAngleState.MinAngle);
        else SetTargetAngle(TargetAngleState.MaxAngle);
    }

    private void SetTargetAngle(TargetAngleState angleState)
    {
        _targetAngleState = angleState;
        _targetAngle = (angleState == TargetAngleState.MaxAngle) ? _hatchLid.limits.max : _hatchLid.limits.min;
    }

    private void Update()
    {
        //if (_hatchLid.enabled) Debug.Log(name + ": " + Mathf.Abs(_hatchLid.jointAngle - _targetAngle));
        if (_hatchLid.enabled && Mathf.Abs(_hatchLid.jointAngle - _targetAngle) < 0.01f)
        {
            FlipTargetAngle();
            _hatchLid.attachedRigidbody.bodyType = RigidbodyType2D.Static;
            _hatchLid.enabled = false;
        }
    }
}
