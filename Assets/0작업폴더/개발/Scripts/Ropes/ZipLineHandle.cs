using System;
using UnityEngine;

public class ZipLineHandle : RidableObject
{
    public override event Action<int, bool> PlayerOnThisObject;
    [SerializeField] private ZipLineRopeCalculator _ropeCalculator;

    [SerializeField] private Rigidbody _pulleyRb;
    [SerializeField] private Transform _bottomPulley;
    [SerializeField] private float _moveToPlayer_minDistance = 3f;
    [SerializeField] private float _moveToPlayer_maxDistance = 28f;
    [SerializeField] private float _moveToPlayer_speed = 0.5f;

    [SerializeField] private Transform _startBlock;
    [SerializeField] private Transform _endBlock;

    private RigidbodyConstraints _lockXPos_PulleyConstraints;
    private RigidbodyConstraints _freeXPos_PulleyConstraints;

    public bool StopMovingPulley { get; set; }

    [SerializeField] private float _pulleyHitStopMargin = 1f;

    private void Awake()
    {
        _freeXPos_PulleyConstraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        _lockXPos_PulleyConstraints = _freeXPos_PulleyConstraints | RigidbodyConstraints.FreezePositionX;
    }

    private void Start()
    {
        StopMovingPulley = false;
    }

    private void FixedUpdate()
    {
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        if (PlayerLogic.Player.Rb == null) return;
        if (_playerIsAttached) return;

        float distance = PlayerLogic.Player.transform.position.x - transform.position.x;
        if (_moveToPlayer_minDistance < Mathf.Abs(distance) && Mathf.Abs(distance) < _moveToPlayer_maxDistance)
        {
            _pulleyRb.constraints = _freeXPos_PulleyConstraints;
            _pulleyRb.AddForce(_moveToPlayer_speed * (distance < 0 ? Vector2.left : Vector2.right), ForceMode.Force);
        }
        else
        {
            _pulleyRb.constraints = _lockXPos_PulleyConstraints;
        }
    }

    // _useVelocity: unused
    private void MoveTowardsPlayer_useVelocity_with_RopeCalculator()
    {
        if (PlayerLogic.Player.Rb == null) return;
        if (_playerIsAttached) return;

        float distance = PlayerLogic.Player.transform.position.x - transform.position.x;
        Vector2 pulleyDir;
        if (_moveToPlayer_minDistance < Mathf.Abs(distance) && Mathf.Abs(distance) < _moveToPlayer_maxDistance
            && (pulleyDir = _ropeCalculator.GetNextPulleyDir_useVelocity(distance)) != Vector2.zero)
        {
            _pulleyRb.constraints = _freeXPos_PulleyConstraints;
            _pulleyRb.velocity = _moveToPlayer_speed * pulleyDir; // [SerializeField] private float _moveToPlayer_speed = 2f;
        }
        else
        {
            _pulleyRb.constraints = _lockXPos_PulleyConstraints;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_playerOnOtherObject) return;

        if (col.CompareTag("Player"))
        {
            StopMovingPulley = false;
            ConnectPlayer();
        }
    }

    private void ConnectPlayer()
    {
        Vector2 playerVelocityNow = PlayerLogic.Player.Rb.velocity;
        PlayerLogic.Player.Rb.transform.SetParent(transform);

        PlayerLogic.LockPlayer();
        PlayerLogic.Player.Rb.transform.localPosition = Vector3.zero;
        _pulleyRb.constraints = _freeXPos_PulleyConstraints;
        StartMovingPulley(playerVelocityNow.x);

        PlayerOnThisObject?.Invoke(gameObject.GetInstanceID(), true);
        _playerIsAttached = true;
    }

    private void StartMovingPulley(float moveDir)
    {
        if (moveDir < 0 && transform.position.x - _startBlock.position.x < _pulleyHitStopMargin) return;
        if (moveDir > 0 && _endBlock.position.x - transform.position.x < _pulleyHitStopMargin) return;

        if (!StopMovingPulley)
        {
            if (moveDir < 0)
            { _pulleyRb.velocity = PlayerLogic.Player.Stats.PlayerAttachedObjectAddVelocity * Vector2.left; }
            else if (moveDir > 0)
            { _pulleyRb.velocity = PlayerLogic.Player.Stats.PlayerAttachedObjectAddVelocity * Vector2.right; }
            else
            {
                Vector2 pulleyDir = (_endBlock.position.x - transform.position.x > transform.position.x - _startBlock.position.x) ? Vector2.right : Vector2.left;
                _pulleyRb.velocity = PlayerLogic.Player.Stats.PlayerAttachedObjectAddVelocity * pulleyDir;
            }
        }
    }

    protected override void DisconnectPlayer()
    {
        if (_playerIsAttached)
        {
            PlayerLogic.FreePlayer();
            PlayerLogic.Player.Rb.transform.SetParent(null);

            _pulleyRb.constraints = _lockXPos_PulleyConstraints;

            PlayerOnThisObject?.Invoke(gameObject.GetInstanceID(), false);
            _playerIsAttached = false;

            PlayerLogic.DisconnectedPlayerAddJump();
        }
    }

//#if UNITY_EDITOR
//    private void OnDrawGizmos()
//    {
//        UnityEditor.Handles.DrawWireDisc((_pulleyRb.position + _bottomPulley.position) / 2, Vector3.back, 0.4f);
//    }
//#endif
}
