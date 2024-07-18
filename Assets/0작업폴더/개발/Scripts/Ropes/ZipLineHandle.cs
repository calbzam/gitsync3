using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ZipLineHandle : RidableObject
{
    public override event Action<int, bool> PlayerOnThisObject;

    private Rigidbody _pulleyRb;

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
            _pulleyRb.velocity = Vector2.left * PlayerLogic.PlayerStats.PlayerAttachedObjectAddVelocity;
        else if (playerVelocityNow.x > 0)
            _pulleyRb.velocity = Vector2.right * PlayerLogic.PlayerStats.PlayerAttachedObjectAddVelocity;
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
