using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderSettings : MonoBehaviour
{
    [SerializeField] private float _climbSpeed = 0.05f;
    [SerializeField] private float _stepSize = 0.5f;

    [Header("공중에서 사다리에 닿으면 자동으로 사다리 탑승")]
    [SerializeField] private bool _autoClimbWhenJumpedOn = true;

    [Header("사다리에 타고 있을 때는 땅과의 충돌 무시")]
    [SerializeField] private bool _bypassGroundCollision = true;

    public float ClimbSpeed => _climbSpeed; // for public access
    public float StepSize => _stepSize;
    public bool AutoClimbWhenJumpedOn => _autoClimbWhenJumpedOn;
    public bool BypassGroundCollision => _bypassGroundCollision;
}
