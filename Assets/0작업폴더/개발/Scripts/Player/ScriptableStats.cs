using UnityEngine;

[CreateAssetMenu]
public class ScriptableStats : ScriptableObject
{
    [Header("LAYERS")]

    [Tooltip("Set this to the layer your player is on")]
    public LayerMask PlayerLayer;

    public LayerMask SwingingGroundLayer;
    public LayerMask GroundLayer;

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

    public float AccelerationX = 30;
    public float GroundDecelerationX = 30;
    public float AirDecelerationX = 30;

    [Tooltip("A constant downward force applied while grounded. Helps on slopes"), Range(0f, -10f)]
    public float GroundingForce = -1.5f;

    [Tooltip("The detection distance for grounding and roof detection"), Range(0f, 0.5f)]
    public float GrounderDistance = 0.12f;

    public Vector2 GroundCheckCapsuleSize = new Vector2(0.2f, 1.2f);


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
    public float JumpBuffer = 0.2f;


    [Header("ROPE")]

    [Tooltip("The minimum distance threshold between the player and the position where the player jumped from the rope, until the player can interact with rope again.")]
    public float RopeJumpedDistance = 2f;


    [Header("RESPAWN")]

    public Vector3 respawnPoint = new Vector3(-4.5f, 3f, 0);

    [Tooltip("Respawn if player Y position is below this value")]
    public float deadPositionY = -6;
}
