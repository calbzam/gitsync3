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
        Vector2 playerVelocity = PlayerLogic.PlayerRb.velocity;
        PlayerLogic.PlayerRb.transform.SetParent(transform);

        PlayerLogic.PlayerRb.transform.localPosition = Vector3.zero;
        PlayerLogic.PlayerRb.constraints |= RigidbodyConstraints2D.FreezePosition;
        PlayerLogic.Player.InputDirSetActive(false);

        _pulleyRb.constraints = _freeXPos_PulleyConstraints;
        _pulleyRb.velocity = playerVelocity;
        ConnectedAddForce();

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

    // Todo: change later so that AddForce direction follows the rope (next particle pos)
    private void ConnectedAddForce()
    {
        if (PlayerLogic.PlayerRb.velocity.x < 0)
            _pulleyRb.AddForce(Vector2.left * PlayerLogic.PlayerStats.PlayerAttachedObjectAddForce, ForceMode.Impulse);
        else if (PlayerLogic.PlayerRb.velocity.x > 0)
            _pulleyRb.AddForce(Vector2.right * PlayerLogic.PlayerStats.PlayerAttachedObjectAddForce, ForceMode.Impulse);
    }
}
