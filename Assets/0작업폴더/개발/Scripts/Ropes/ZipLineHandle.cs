using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ZipLineHandle : RidableObject
{
    public override event Action<int, bool> PlayerOnThisObject;

    private Rigidbody _pulleyRb;

    private Rigidbody2D _playerRb;
    private RigidbodyConstraints2D _origPlayerConstraints;

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
        if (PlayerLogic.IsLoaded) _origPlayerConstraints = PlayerLogic.PlayerRb.constraints;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_origPlayerConstraints == RigidbodyConstraints2D.None) _origPlayerConstraints = PlayerLogic.PlayerRb.constraints;
        if (_playerOnOtherObject) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            ConnectPlayer(collision);
        }
    }

    private void ConnectPlayer(Collider2D playerCollision)
    {
        Vector2 playerVelocity = PlayerLogic.PlayerRb.velocity;
        PlayerLogic.PlayerRb.transform.SetParent(transform);

        PlayerLogic.PlayerRb.transform.localPosition = Vector3.zero;
        PlayerLogic.PlayerRb.constraints |= RigidbodyConstraints2D.FreezePosition;
        PlayerLogic.Player.InputDirSetActive(false);

        _pulleyRb.constraints = _freeXPos_PulleyConstraints;
        _pulleyRb.velocity = playerVelocity;

        PlayerOnThisObject?.Invoke(gameObject.GetInstanceID(), true);
        _playerIsAttached = true;
    }

    protected override void DisconnectPlayer(InputAction.CallbackContext ctx)
    {
        if (_playerIsAttached)
        {
            PlayerLogic.PlayerRb.transform.SetParent(null);
            PlayerLogic.PlayerRb.constraints = _origPlayerConstraints;
            PlayerLogic.Player.InputDirSetActive(true);

            _pulleyRb.constraints = _lockXPos_PulleyConstraints;

            PlayerOnThisObject?.Invoke(gameObject.GetInstanceID(), false);
            _playerIsAttached = false;

            PlayerLogic.DisconnectedPlayerAddJump();
        }
    }
}
