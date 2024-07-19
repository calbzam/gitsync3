using UnityEngine;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
    [Header("INPUT")]

    [Tooltip("Makes all Input snap to an integer. Prevents gamepads from walking slowly. Recommended value is true to ensure gamepad/keybaord parity.")]
    public bool SnapInput = true;

    [Tooltip("Minimum input required before you mount a ladder or climb a ledge. Avoids unwanted climbing using controllers"), Range(0.01f, 0.99f)]
    public float VerticalDeadZoneThreshold = 0.3f;

    [Tooltip("Minimum input required before a left or right is recognized. Avoids drifting with sticky controllers"), Range(0.01f, 0.99f)]
    public float HorizontalDeadZoneThreshold = 0.1f;


    [Header("MOVEMENT X")]

    public float MaxSpeedX = 6;
    public float MinSpeedX = 0.5f;

    [Header("Acceleration")]

    public float GroundAccelerationX = 30;
    public float WaterAccelerationX = 30;

    [Header("Deceleration")]

    public float GroundDecelerationX = 30;
    public float WaterDecelerationX = 20;
    public float AirDecelerationX = 30;

    [Header("A constant downward force applied while grounded. Helps on slopes"), Range(0f, -10f)]
    //[Tooltip("A constant downward force applied while grounded. Helps on slopes"), Range(0f, -10f)]
    public float GroundingForce = -1.5f;

    [Header("Ground Checker")]

    [Tooltip("The detection distance for grounding and roof detection"), Range(-0.3f, 0.3f)]
    public float GrounderDistance = -0.144f;
    //public Vector2 GroundCheckCapsuleSize = new Vector2(0.2f, 1.2f);
    public float GroundCheckerAddRadius = 0.02f;


    [Header("MOVEMENT Y")]

    [Tooltip("The immediate velocity applied when jumping")]
    public float JumpPower = 9;

    //[Tooltip("The maximum vertical movement speed")]
    //public float MaxFallSpeed = 40;
    //[Tooltip("The player's capacity to gain fall speed. a.k.a. In Air Gravity")]
    //public float FallAcceleration = 30;

    public float JumpUpGravityScale = 1.8f;
    public float FallDownGravityScale = 3f;

    [Tooltip("The time before coyote jump becomes unusable. Coyote jump allows jump to execute even after leaving a ledge")]
    public float CoyoteTime = 0.15f;

    [Tooltip("The amount of time we buffer a jump. This allows jump input before actually hitting the ground")]
    public float JumpBuffer = 0.1f;


    [Header("ROPE")]

    [Tooltip("The minimum distance threshold between the player and the position where the player jumped from the rope, until the player can interact with rope again.")]
    public float RopeJumpedDistance = 2f;

    [Tooltip("로프에서 뛰었을 때, 사용자가 좌우 방향키를 누르고 있는 방향으로 Player에 이만큼 힘을 추가로 가해 줌")]
    public float RopeJumpedPlayerAddForce = 2.5f;

    [Tooltip("오브젝트에 연결되었을 때, 사용자가 좌우 방향키를 누르고 있는 방향으로 오프젝트에 이만큼 힘을 추가로 가해 줌")]
    public float PlayerAttachedObjectAddVelocity = 10f;


    [Header("RESPAWN")]

    [Tooltip("Respawn if player Y position is below this value")]
    public float deadPositionY = -6;
}
