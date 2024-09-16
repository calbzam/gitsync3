using System;
using UnityEngine;
using UnityEditor;


// PlayerController.cs and Player.Stats.cs EDITED from TarodevController on GitHub
// github: https://github.com/Matthew-J-Spencer/Ultimate-2D-Controller/tree/main
// license: https://github.com/Matthew-J-Spencer/Ultimate-2D-Controller/blob/main/LICENSE

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    public Rigidbody2D Rb => _rb;
    [SerializeField] private CapsuleCollider2D _col;
    private bool _cachedQueryStartInColliders;

    [SerializeField] private PlayerStats _stats;
    public PlayerStats Stats => _stats;

    public Checkpoint RespawnPoint { get; private set; } // use this instead of RespawnPos
    public Vector3 RespawnPos { get; private set; }
    public bool RespawnButtonAllowed { get; set; }

    public bool DirInputActive { get; set; }
    public bool LimitXVelocity { get; set; } // assigned false when speed boost from other object, assigned true when player hits ground
    public bool ZPosSetToGround { get; set; }

    private Rigidbody2D swingingGround;

    public bool GroundCheckAllowed { get; set; }
    private Vector3 groundCheckerPos;
    private float groundCheckerRadius;
    private Vector3 ceilCheckerPos;
    private float ceilCheckerRadius;

    /* Time */
    private float _time = 1f; // 1f > 0 + 0.1:  prevent character from jumping without input at scene start

    /* Interface */
    public event Action<bool, float> GroundedChanged;
    public static event Action Jumped;

    /* Collisions */
    private float _frameLeftGrounded = float.MinValue;
    public bool OnGround { get; private set; }
    public bool IsInWater { get; set; }

    public bool LadderClimbAllowed { get; set; }
    public bool IsInLadderRange { get; set; }
    public bool IsOnLadder { get; set; }
    public LadderTrigger CurrentLadder { get; set; }

    //private bool disableYVelocity = false;
    //private bool swingingGroundHit = false;

    private void Awake()
    {
        _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
    }

    private void Start()
    {
        RespawnButtonAllowed = true;

        DirInputActive = true;
        LimitXVelocity = true;
        ZPosSetToGround = false;

        GroundCheckAllowed = true;
        LadderClimbAllowed = true;
        drawGizmosEnabled = true;
    }

    private void OnEnable()
    {
        drawGizmosEnabled = true;
    }

    private void OnDisable()
    {
        drawGizmosEnabled = false;
    }

    private void Update()
    {
        _time += Time.deltaTime;
        RefineInput();

        //CheckRespawn();
    }

    private void FixedUpdate()
    {
        CheckCollisions();

        HandleJump();
        if (LadderClimbAllowed) HandleLadderClimb();

        HandleDirection();
        HandleGravity();

        //ApplyMovement();
    }

    private void RefineInput()
    {
        //// unneeded as arrow keys are automatically snapped
        //if (_stats.SnapInput)
        //{
        //    InputReader.FrameInput.Move.x = Mathf.Abs(InputReader.FrameInput.Move.x) < _stats.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(InputReader.FrameInput.Move.x);
        //    InputReader.FrameInput.Move.y = Mathf.Abs(InputReader.FrameInput.Move.y) < _stats.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(InputReader.FrameInput.Move.y);
        //}

        if (FrameInputReader.FrameInput.JumpStarted)
        {
            _jumpToConsume = true;
            _timeJumpWasPressed = _time;
        }
    }

    #region Collisions

    //_col.bounds.center: (x=0.00, y=2.30, z=0.00)
    //_col.size: (x=0.50, y=1.26)
    //_col.direction: Vertical

    private Collider2D _groundCol;
    private bool _groundHit;

    private void CheckCollisions()
    {
        Physics2D.queriesStartInColliders = false;


        // Ground and Ceiling

        // add later: Enum groundHitType - static ground, moving ground

        groundCheckerPos = _col.bounds.center + Vector3.down * (_col.size.y / 2 + _stats.GrounderDistance);
        groundCheckerRadius = _col.size.x / 2 + _stats.GroundCheckerAddRadius;
        ceilCheckerPos = _col.bounds.center + Vector3.up * (_col.size.y / 2 + _stats.GrounderDistance);
        ceilCheckerRadius = groundCheckerRadius;

        //Collider2D col = Physics2D.OverlapCircle(groundCheckerPos, groundCheckerRadius, Layers.SwingingGroundLayer);
        //if (col) { swingingGroundHit = true; /*swingingGround = col.attachedRigidbody;*/ }
        //bool groundHit = swingingGroundHit || normalGroundHit;

        if (GroundCheckAllowed)
        {
            _groundCol = Physics2D.OverlapCircle(groundCheckerPos, groundCheckerRadius, Layers.GroundLayer.MaskValue);
            _groundHit = _groundCol;
            if (!IsOnLadder && _groundCol != null)
            {
                if (!_groundCol.CompareTag("SpeedBoost Ground")) LimitXVelocity = true;
                
                // Set Z-pos to the Z-pos of the ground that Player hit
                PlayerLogic.SetPlayerZPosition(_groundCol.transform.position.z);
                ZPosSetToGround = true;

                //transform.position = new Vector3(transform.position.x, transform.position.y, col.transform.position.z);
            }
        }
        else
        {
            _groundCol = null;
            _groundHit = false;
        }

        //bool ceilingHit = Physics2D.OverlapCircle(ceilCheckerPos, ceilCheckerRadius, Layers.GroundLayer | Layers.SwingingGroundLayer);
        // Hit a Ceiling: cancel jumping from there
        //if (ceilingHit) /*_frameVelocity.y = Mathf.Min(0, _frameVelocity.y);*/_rb.velocity = new Vector2(_rb.velocity.x, Mathf.Min(0, _rb.velocity.y));


        // Landed on the Ground
        if (!OnGround && _groundHit)
        {
            OnGround = true;
            _coyoteUsable = true;
            _bufferedJumpUsable = true;
            _endedJumpEarly = false;
            GroundedChanged?.Invoke(true, Mathf.Abs(/*_frameVelocity.y*/_rb.velocity.y));
        }
        // Left the Ground
        else if (OnGround && !_groundHit)
        {
            OnGround = false;
            _frameLeftGrounded = _time;
            GroundedChanged?.Invoke(false, 0);
        }

        Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
    }

    #endregion


    #region Jumping

    private bool _jumpToConsume = false;
    private bool _bufferedJumpUsable = false;
    private bool _endedJumpEarly = false;
    private bool _coyoteUsable = false;
    private float _timeJumpWasPressed;

    private bool HasBufferedJump => _bufferedJumpUsable && (_time < _timeJumpWasPressed + _stats.JumpBuffer);
    private bool CanUseCoyote => _coyoteUsable && !OnGround && (_time < _frameLeftGrounded + _stats.CoyoteTime);

    private void HandleJump()
    {
        //Debug.Log(_bufferedJumpUsable + " && ( " + _time + " < " + _timeJumpWasPressed + " + " + _stats.JumpBuffer + " )");

        if (!_endedJumpEarly && !OnGround && !FrameInputReader.FrameInput.JumpHeld && _rb.velocity.y > 0) _endedJumpEarly = true;

        if (!_jumpToConsume && !HasBufferedJump) return;

        if (!IsOnLadder && (OnGround || CanUseCoyote)) ExecuteJump();

        _jumpToConsume = false;
    }

    private void ExecuteJump()
    {
        _endedJumpEarly = false;
        _timeJumpWasPressed = 0;
        _bufferedJumpUsable = false;
        _coyoteUsable = false;

        LimitXVelocity = true;

        //_frameVelocity.y = _stats.JumpPower;
        //_frameVelocity = _rb.velocity;
        _rb.AddForce(Vector2.up * _stats.JumpPower, ForceMode2D.Impulse);

        //swingingGroundHit = false;
        Jumped?.Invoke();
    }

    #endregion

    #region Ladder Climb

    public void SetPlayerOnLadder(bool onLadder)
    {
        if (onLadder)
        {
            IsOnLadder = true; // 사다리에서 방향키를 처음 눌렀을 때
            CurrentLadder.StepProgress = CurrentLadder.StepSize;

            base.transform.position = new Vector3(CurrentLadder.transform.position.x, base.transform.position.y, CurrentLadder.transform.position.z - 0.1f);
            _rb.velocity = Vector2.zero;

            if (CurrentLadder.BypassGroundCollision) PlayerLogic.IgnorePlayerGroundCollision(true);
            DirInputActive = false;
        }
        else
        {
            IsOnLadder = false;

            PlayerLogic.IgnorePlayerGroundCollision(false);
            DirInputActive = true;
        }

        Physics2D.SyncTransforms();
    }

    private bool PlayerAtLadderEnd()
    {
        return (FrameInputReader.FrameInput.Move.y > 0 && _rb.position.y > CurrentLadder.TopPoint.position.y)
            || (FrameInputReader.FrameInput.Move.y < 0 && _rb.position.y < CurrentLadder.BottomPoint.position.y);
    }

    private void HandleLadderClimb()
    {
        if (IsInLadderRange)
        {
            if (FrameInputReader.FrameInput.Move.y != 0)
            {
                if (!IsOnLadder)
                {
                    if (PlayerAtLadderEnd()) return;
                    if (CurrentLadder.JumpingFromLadder) CurrentLadder.JumpingFromLadder = false;
                    SetPlayerOnLadder(true);
                }

                CurrentLadder.StepProgress += CurrentLadder.ClimbSpeed;
                if (CurrentLadder.StepProgress > CurrentLadder.StepSize)
                {
                    _rb.MovePosition(_rb.position + CurrentLadder.StepSize * Mathf.Sign(FrameInputReader.FrameInput.Move.y) * Vector2.up);
                    if (PlayerAtLadderEnd()) CurrentLadder.JumpFromLadder();
                    CurrentLadder.StepProgress = 0;
                }
            }

            else // no climbing input
            {
                CurrentLadder.StepProgress = CurrentLadder.StepSize;
            }
        }
    }

    #endregion

    #region Horizontal Movement

    private void HandleDirection()
    {
        if (!DirInputActive) return;

        if (FrameInputReader.FrameInput.Move.x == 0)
        {
            if (_rb.velocity.x != 0)
            {
                //float decelerationX = OnGround ? _stats.GroundDecelerationX : _stats.AirDecelerationX;
                float decelerationX;
                if (OnGround) decelerationX = _stats.GroundDecelerationX;
                else if (IsInWater) decelerationX = _stats.WaterDecelerationX;
                else decelerationX = _stats.AirDecelerationX;

                //_frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, decelerationX * Time.fixedDeltaTime);
                float prevDir = Mathf.Sign(_rb.velocity.x);
                _rb.AddForce(decelerationX * prevDir * Vector2.left, ForceMode2D.Force);
                if (Mathf.Sign(_rb.velocity.x) * prevDir < 0 || MathF.Abs(_rb.velocity.x) < _stats.MinSpeedX) _rb.AddForce(_rb.totalForce.x * Vector2.left, ForceMode2D.Force);
            }
        }
        else
        {
            //_frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _stats.MaxSpeedX * InputReader.FrameInput.Move.x, _stats.AccelerationX * Time.fixedDeltaTime);
            if (IsInWater) _rb.AddForce(_stats.WaterAccelerationX * FrameInputReader.FrameInput.Move.x * Vector2.right, ForceMode2D.Force);
            else _rb.AddForce(_stats.GroundAccelerationX * FrameInputReader.FrameInput.Move.x * Vector2.right, ForceMode2D.Force);

            if (LimitXVelocity) LimitXVelocityTo(_stats.MaxSpeedX);
        }
    }

    public void LimitXVelocityTo(float maxSpeed)
    {
        if (Mathf.Abs(_rb.velocity.x) > maxSpeed)
            _rb.velocity = new Vector2(Math.Sign(_rb.velocity.x) * maxSpeed, _rb.velocity.y);
    }

    #endregion

    #region Gravity

    private void HandleGravity()
    {
        //if (OnGround && _frameVelocity.y <= 0f) // on ground and falling
        //{
        //    _frameVelocity.y = _stats.GroundingForce;
        //}
        //else
        //{
        //    var inAirGravity = _stats.FallAcceleration;
        //    if (_frameVelocity.y > 0)
        //    {
        //        if (_endedJumpEarly) inAirGravity *= _stats.FallDownGravityScale;
        //        else inAirGravity *= _stats.JumpUpGravityScale;
        //    }
        //    _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
        //}

        if (IsOnLadder)
        {
            _rb.gravityScale = 0;
        }
        else if (_rb.velocity.y > 0)
        {
            _rb.gravityScale = _stats.JumpUpGravityScale;
        }
        else
        {
            _rb.gravityScale = _stats.FallDownGravityScale;
        }
    }

    #endregion

    #region Respawn

    //private bool XYDiffBothMoreThan(Vector2 v1, Vector2 v2, float moreThan)
    //{
    //    return Mathf.Abs(v1.x - v2.x) > moreThan && Math.Abs(v1.y - v2.y) > moreThan;
    //}

    public void SetRespawnPoint(Checkpoint checkpoint)
    {
        RespawnPoint = checkpoint;
        RespawnPos = new Vector3(checkpoint.Position.x, checkpoint.Position.y, transform.position.z);
    }

    public void RespawnPlayer()
    {
        FrameInputReader.TriggerJump();
        PlayerLogic.FreePlayer();
        transform.position = RespawnPos;
        _rb.velocity = Vector3.zero;

        PlayerLogic.InvokePlayerRespawnedEvent();
    }

    //private void CheckRespawn()
    //{
    //    if (playerTransform.position.y < _stats.deadPositionY)
    //    {
    //        RespawnPlayer();
    //    }
    //}

    #endregion

    //public void DisableYVelocity()
    //{
    //    disableYVelocity = true;
    //}

    //private void ApplyMovement()
    //{
    //    if (disableYVelocity) _frameVelocity.y = _rb.velocity.y;
    //    //if (swingingGroundHit) _frameVelocity.y = swingingGround.velocity.y;
    //
    //    _rb.velocity = _frameVelocity;
    //    //if (!disableYVelocity) _rb.velocity = new Vector2(_frameVelocity.x, _rb.velocity.y);
    //}


    private bool drawGizmosEnabled = false;
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (drawGizmosEnabled)
        {
            Handles.DrawWireDisc(groundCheckerPos, Vector3.back, groundCheckerRadius);
            //Handles.DrawWireDisc(ceilCheckerPos, Vector3.back, ceilCheckerRadius);

            //Gizmos.DrawWireCube(_col.bounds.center, _col.size);
        }
    }


    private void OnValidate()
    {
        if (_stats == null) Debug.LogWarning("Please assign a ScriptableStats asset to the Player Controller's Stats slot", this);
    }
#endif
}
