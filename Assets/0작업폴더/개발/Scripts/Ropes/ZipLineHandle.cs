using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ZipLineHandle : MonoBehaviour
{
    private InputControls _input;
    private Rigidbody _pulleyRb;

    private Rigidbody2D _playerRb;
    private RigidbodyConstraints2D _origPlayerConstraints;

    private RigidbodyConstraints _lockXPos_PulleyConstraints;
    private RigidbodyConstraints _freeXPos_PulleyConstraints;

    private bool _playerIsAttached = false;

    private void Awake()
    {
        _input = new InputControls();
        _freeXPos_PulleyConstraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        _lockXPos_PulleyConstraints = _freeXPos_PulleyConstraints | RigidbodyConstraints.FreezePositionX;
    }

    private void Start()
    {
        _pulleyRb = GetComponentInParent<Rigidbody>();
        _origPlayerConstraints = PlayerLogic.PlayerRb.constraints;
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.Jump.started += DisconnectPlayer;
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.Player.Jump.started -= DisconnectPlayer;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
        _playerIsAttached = true;
    }

    private void DisconnectPlayer(InputAction.CallbackContext ctx)
    {
        if (_playerIsAttached)
        {
            PlayerLogic.PlayerRb.transform.SetParent(null);
            PlayerLogic.PlayerRb.constraints = _origPlayerConstraints;
            PlayerLogic.Player.InputDirSetActive(true);

            _pulleyRb.constraints = _lockXPos_PulleyConstraints;
            _playerIsAttached = false;

            PlayerLogic.DisconnectedPlayerAddJump();
        }
    }
}
