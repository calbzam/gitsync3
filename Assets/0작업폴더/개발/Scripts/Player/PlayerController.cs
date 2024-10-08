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

    private Rigidbody2D _swingingGround;

    public bool GroundCheckAllowed { get; set; }
    private Vector3 _groundCheckerPos;
    private float _groundCheckerRadius;
    private Vector3 _ceilCheckerPos;
    private float _ceilCheckerRadius;

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
    private bool _isInLadderRange => (CurrentLadder != null ? CurrentLadder.PlayerIsInRange : false);
    public bool IsOnLadder => (CurrentLadder != null ? CurrentLadder.PlayerIsOnLadder : false);
    public bool JumpingFromLadder { get; set; }
    public LadderTrigger CurrentLadder { get; set; }
    private float _ladderStepProgress;

    private bool _drawGizmosEnabled = false;

    //private bool _disableYVelocity = false;
    //private bool _swingingGroundHit = false;

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
        JumpingFromLadder = false;
        _drawGizmosEnabled = true;
    }

    private void OnEnable()
    {
        _drawGizmosEnabled = true;
    }

    private void OnDisable()
    {
        _drawGizmosEnabled = false;
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

        _groundCheckerPos = _col.bounds.center + Vector3.down * (_col.size.y / 2 + _stats.GrounderDistance);
        _groundCheckerRadius = _col.size.x / 2 + _stats.GroundCheckerAddRadius;
        _ceilCheckerPos = _col.bounds.center + Vector3.up * (_col.size.y / 2 + _stats.GrounderDistance);
        _ceilCheckerRadius = _groundCheckerRadius;

        //Collider2D col = Physics2D.OverlapCircle(groundCheckerPos, groundCheckerRadius, Layers.SwingingGroundLayer);
        //if (col) { swingingGroundHit = true; /*swingingGround = col.attachedRigidbody;*/ }
        //bool groundHit = swingingGroundHit || normalGroundHit;

        if (GroundCheckAllowed)
        {
            _groundCol = Physics2D.OverlapCircle(_groundCheckerPos, _groundCheckerRadius, Layers.GroundLayer.MaskValue);
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
    }

    private void ExecuteJump()
    {
        _jumpToConsume = false;
        _endedJumpEarly = false;
        _timeJumpWasPressed = 0;
        _bufferedJumpUsable = false;
        _coyoteUsable = false;

        LimitXVelocity = true;

        //_frameVelocity.y = _stats.JumpPower;
        //_frameVelocity = _rb.velocity;
        _rb.AddForce(Vector2.up * _stats.JumpPower, ForceMode2D.Impulse);

        //swingingGroundHit = false;
        //Jumped?.Invoke();
    }

    #endregion

    #region Ladder Climb

    private Vector2 GetLadderPosClosestToPlayer()
    {
        // no 2x2 matrix in unity
        // use 2 linear eqns (x, y) instead

        // ladder line: p1(x1,y1), p2(x2,y2)
        // player line: p3(x3,y3), p4(x4,y4)
        // y = (y2-y1)/(x2-x1)*(x-x1)+y1 , x = (y-y1)/((y2-y1)/(x2-x1))+x1
        // y = (y4-y3)/(x4-x3)*(x-x3)+y3 , x = (y-y3)/((y4-y3)/(x4-x3))+x3

        // solve for x:  (y2-y1)/(x2-x1)*(x-x1)+y1 = (y4-y3)/(x4-x3)*(x-x3)+y3
        // x = ( y3-y1 + (y2-y1)/(x2-x1)*x1 -(y4-y3)/(x4-x3)*x3 ) / ( (y2-y1)/(x2-x1) - (y4-y3)/(x4-x3) )
        // y3==y4:  x = ( y3-y1 + (y2-y1)/(x2-x1)*x1 -(y3-y3)/(x4-x3)*x3 ) / ( (y2-y1)/(x2-x1) - (y3-y3)/(x4-x3) )
        //            = ( y3-y1 + (y2-y1)/(x2-x1)*x1 -(0)/(x4-x3)*x3 ) / ( (y2-y1)/(x2-x1) - (0)/(x4-x3) )
        //            = ( y3-y1 + (y2-y1)/(x2-x1)*x1 ) / ( (y2-y1)/(x2-x1) )
        //            = ( y3-y1 + (y2-y1)/(x2-x1)*x1 ) / (y2-y1) * (x2-x1)

        // solve for y:  (y-y1)/((y2-y1)/(x2-x1))+x1 = (y-y3)/((y4-y3)/(x4-x3))+x3
        // y = ( x3-x1 + y1/((y2-y1)/(x2-x1)) - y3/((y4-y3)/(x4-x3)) ) / ( 1/((y2-y1)/(x2-x1)) - 1/((y4-y3)/(x4-x3)) )
        //   = ( x3-x1 + y1/(y2-y1)*(x2-x1) - y3/(y4-y3)*(x4-x3) ) / ( 1/(y2-y1)*(x2-x1) - 1/(y4-y3)*(x4-x3) )
        // x3==x4:  y = ( x3-x1 + y1/(y2-y1)*(x2-x1) - y3/(y4-y3)*(x3-x3) ) / ( 1/(y2-y1)*(x2-x1) - 1/(y4-y3)*(x3-x3) )
        //            = ( x3-x1 + y1/(y2-y1)*(x2-x1) - y3/(y4-y3)*(0) ) / ( 1/(y2-y1)*(x2-x1) - 1/(y4-y3)*(0) )
        //            = ( x3-x1 + y1/(y2-y1)*(x2-x1) ) / ( 1/(y2-y1)*(x2-x1) )

        float x1 = CurrentLadder.TopPoint.position.x, y1 = CurrentLadder.TopPoint.position.y;
        float x2 = CurrentLadder.BottomPoint.position.x, y2 = CurrentLadder.BottomPoint.position.y;
        float x3 = transform.position.x - 2, y3 = transform.position.y;
        float x4 = transform.position.x + 2, y4 = 0;
        float ladderIntersectX = (y3 - y1 + (y2 - y1) / (x2 - x1) * x1) / (y2 - y1) * (x2 - x1);

        x3 = x4 = transform.position.x;
        y3 = transform.position.y - 2; y4 = transform.position.y + 2;
        float ladderIntersectY = (x3 - x1 + y1 / (y2 - y1) * (x2 - x1)) / (1 / (y2 - y1) * (x2 - x1));

        Vector2 usePlayerX = new Vector2(transform.position.x, ladderIntersectY);
        Vector2 usePlayerY = new Vector2(ladderIntersectX, transform.position.y);
        Vector2 ladderIntersect = Vector2.Distance(usePlayerX, transform.position) < Vector2.Distance(usePlayerY, transform.position) ? usePlayerX : usePlayerY;

        return ladderIntersect;
    }

    public void SetPlayerInLadderRange(LadderTrigger ladder)
    {
        CurrentLadder = ladder;
        CurrentLadder.PlayerIsInRange = true;
    }

    public void SetPlayerOnLadder(bool onThisLadder, LadderTrigger ladder)
    {
        if (onThisLadder)
        {
            ladder.PlayerIsOnLadder = true; // 사다리에서 방향키를 처음 눌렀을 때

            Vector2 ladderIntersect;
            if (Math.Abs(ladder.Direction.x) < 0.001/*slope = inf*/) ladderIntersect = new Vector2(ladder.transform.position.x, transform.position.y);
            else if (Math.Abs(ladder.Direction.y) < 0.001/*slope = 0*/) ladderIntersect = new Vector2(transform.position.x, ladder.transform.position.y);
            else ladderIntersect = GetLadderPosClosestToPlayer();

            transform.position = new Vector3(ladderIntersect.x, ladderIntersect.y, ladder.transform.position.z - 0.1f);
            _rb.velocity = Vector2.zero;

            if (ladder.BypassGroundCollision) PlayerLogic.IgnorePlayerGroundCollision(true);
            DirInputActive = false;

            CurrentLadder = ladder;
            JumpingFromLadder = false;
        }
        else
        {
            ladder.PlayerIsOnLadder = false;
            ladder.PlayerIsInRange = false;

            // disable "ladder movement" only if the trigger-exited ladder is the ladder the Player is currently on
            if (ladder == CurrentLadder)
            {
                PlayerLogic.IgnorePlayerGroundCollision(false);
                DirInputActive = true;
                CurrentLadder = null;
            }
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
        if (_isInLadderRange)
        {
            if (FrameInputReader.FrameInput.Move.y != 0)
            {
                if (!IsOnLadder)
                {
                    if (CurrentLadder.StopClimbingUpwards && FrameInputReader.FrameInput.Move.y > 0) return;
                    else if (CurrentLadder.StopClimbingDownwards && FrameInputReader.FrameInput.Move.y < 0) return;

                    SetPlayerOnLadder(true, CurrentLadder);
                }

                _ladderStepProgress += CurrentLadder.ClimbSpeed * Time.fixedDeltaTime;
                if (_ladderStepProgress > CurrentLadder.StepSize)
                {
                    _rb.MovePosition(_rb.position + CurrentLadder.StepSize * Mathf.Sign(FrameInputReader.FrameInput.Move.y) * CurrentLadder.Direction);
                    if (PlayerAtLadderEnd()) // 2 ladders connection evaluation
                    {
                        Collider2D[] cols = Physics2D.OverlapCapsuleAll(_rb.position, _col.size, _col.direction, 0, Layers.LadderLayer.MaskValue);
                        LadderTrigger upperLadder, lowerLadder;

                        if (cols.Length < 2) CurrentLadder.JumpFromLadder();
                        else
                        {
                            if (FrameInputReader.FrameInput.Move.y > 0)
                            {
                                upperLadder = (cols[0].transform.position.y > cols[1].transform.position.y) ? cols[0].GetComponent<LadderTrigger>() : cols[1].GetComponent<LadderTrigger>();
                                if (ReferenceEquals(CurrentLadder.gameObject, upperLadder.gameObject)) CurrentLadder.JumpFromLadder();
                                else SetPlayerOnLadder(true, upperLadder);
                            }
                            else // FrameInputReader.FrameInput.Move.y < 0
                            {
                                lowerLadder = (cols[0].transform.position.y < cols[1].transform.position.y) ? cols[0].GetComponent<LadderTrigger>() : cols[1].GetComponent<LadderTrigger>();
                                if (ReferenceEquals(CurrentLadder.gameObject, lowerLadder.gameObject)) CurrentLadder.JumpFromLadder();
                                else SetPlayerOnLadder(true, lowerLadder);
                            }
                        }
                    }
                    _ladderStepProgress = 0;
                }
            }

            else // no climbing input
            {
                _ladderStepProgress = CurrentLadder.StepSize;
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


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_drawGizmosEnabled)
        {
            Handles.DrawWireDisc(_groundCheckerPos, Vector3.back, _groundCheckerRadius);
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
