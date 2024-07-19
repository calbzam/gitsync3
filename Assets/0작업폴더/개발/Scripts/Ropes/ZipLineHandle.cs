using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ZipLineHandle : RidableObject
{
    public override event Action<int, bool> PlayerOnThisObject;
    [SerializeField] private ZipLineRopeCalculator _ropeCalculator;

    private Rigidbody _pulleyRb;
    [SerializeField] private float _moveToPlayer_minDistance = 2f;
    [SerializeField] private float _moveToPlayer_maxDistance = 28f;
    [SerializeField] private float _moveToPlayer_speed = 2f;

    private static RigidbodyConstraints2D _origPlayerConstraints;

    private RigidbodyConstraints _lockXPos_PulleyConstraints;
    private RigidbodyConstraints _freeXPos_PulleyConstraints;

    protected override void Awake()
    {
        base.Awake();
        _freeXPos_PulleyConstraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        _lockXPos_PulleyConstraints = _freeXPos_PulleyConstraints | RigidbodyConstraints.FreezePositionX;
    }

    private void Start()
    {
        _pulleyRb = GetComponentInParent<Rigidbody>();
        if (PlayerLogic.PlayerRb != null) _origPlayerConstraints = PlayerLogic.PlayerRb.constraints;
    }

    private void Update()
    {
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        if (PlayerLogic.PlayerRb == null) return;
        if (_playerIsAttached) return;

        float distance = PlayerLogic.Player.transform.position.x - transform.position.x;
        Vector2 pulleyDir;
        if (_moveToPlayer_minDistance < Mathf.Abs(distance) && Mathf.Abs(distance) < _moveToPlayer_maxDistance && (pulleyDir = _ropeCalculator.GetNextPulleyDir(distance)) != Vector2.zero)
        {
            _pulleyRb.constraints = _freeXPos_PulleyConstraints;
            _pulleyRb.velocity = _moveToPlayer_speed * pulleyDir;
        }
        else
        {
            _pulleyRb.constraints = _lockXPos_PulleyConstraints;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_playerOnOtherObject) return;
        if (_origPlayerConstraints == RigidbodyConstraints2D.None) _origPlayerConstraints = PlayerLogic.PlayerRb.constraints;

        if (collision.gameObject.CompareTag("Player"))
        {
            ConnectPlayer(collision);
        }
    }

    private void ConnectPlayer(Collider2D playerCollision)
    {
        Vector2 playerVelocityNow = PlayerLogic.PlayerRb.velocity;
        PlayerLogic.PlayerRb.transform.SetParent(transform);

        FreezePlayerDirInput();
        _pulleyRb.constraints = _freeXPos_PulleyConstraints;
        AddPulleyVelocity(playerVelocityNow);

        PlayerOnThisObject?.Invoke(gameObject.GetInstanceID(), true);
        _playerIsAttached = true;
    }

    private void FreezePlayerDirInput()
    {
        PlayerLogic.PlayerRb.transform.localPosition = Vector3.zero;
        PlayerLogic.PlayerRb.constraints |= RigidbodyConstraints2D.FreezePosition;
        PlayerLogic.Player.DirInputSetActive(false);
    }

    // Todo: change later so that the velocity/AddForce direction follows the rope (next particle pos)
    private void AddPulleyVelocity(Vector2 playerVelocityNow)
    {
        if (playerVelocityNow.x < 0)
            _pulleyRb.velocity = PlayerLogic.PlayerStats.PlayerAttachedObjectAddVelocity * Vector2.left;
        else if (playerVelocityNow.x > 0)
            _pulleyRb.velocity = PlayerLogic.PlayerStats.PlayerAttachedObjectAddVelocity * Vector2.right;
        else
            _pulleyRb.velocity = PlayerLogic.PlayerStats.PlayerAttachedObjectAddVelocity * _ropeCalculator.GetFurtherRopeDir();
    }

    protected override void DisconnectPlayer(InputAction.CallbackContext ctx)
    {
        if (_playerIsAttached)
        {
            PlayerLogic.PlayerRb.transform.SetParent(null);
            PlayerLogic.PlayerRb.constraints = _origPlayerConstraints;
            PlayerLogic.Player.DirInputSetActive(true);

            _pulleyRb.constraints = _lockXPos_PulleyConstraints;

            PlayerOnThisObject?.Invoke(gameObject.GetInstanceID(), false);
            _playerIsAttached = false;

            PlayerLogic.DisconnectedPlayerAddJump();
        }
    }
}
