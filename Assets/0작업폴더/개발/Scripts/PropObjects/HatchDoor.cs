using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatchDoor : LeverConnectedObject
{
    [SerializeField] private HingeJoint2D _hatchLid;
    private JointMotor2D _motor;

    private void Start()
    {
        _motor = _hatchLid.motor;
        ActivatedAction(false);
    }

    public override void ActivatedAction(bool enabledState)
    {
        _motor.motorSpeed = -_motor.motorSpeed;
        _hatchLid.motor = _motor;
    }
}
