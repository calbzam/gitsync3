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

    private bool _playerIsAttached = false;

    private void Awake()
    {
        _input = new InputControls();
    }

    private void Start()
    {
        _pulleyRb = GetComponentInParent<Rigidbody>();
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
            _playerRb = collision.attachedRigidbody;
            Vector2 playerVelocity = _playerRb.velocity;
            _playerRb.transform.SetParent(transform);

            _playerRb.transform.localPosition = Vector3.zero;
            _origPlayerConstraints = _playerRb.constraints;
            _playerRb.constraints |= RigidbodyConstraints2D.FreezePosition;
            _playerRb.GetComponent<PlayerController>().InputDirSetActive(false);

            SetPulleyConstraints(false);
            _pulleyRb.velocity = playerVelocity;
            _playerIsAttached = true;
        }
    }

    private void SetPulleyConstraints(bool lockXPos)
    {
        _pulleyRb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotation;
        if (lockXPos) _pulleyRb.constraints |= RigidbodyConstraints.FreezePositionX;
    }

    private void DisconnectPlayer(InputAction.CallbackContext ctx)
    {
        if (_playerIsAttached)
        {
            _playerRb.transform.SetParent(null);
            _playerRb.constraints = _origPlayerConstraints;
            _playerRb.GetComponent<PlayerController>().InputDirSetActive(true);
            _playerIsAttached = false;
        }
    }
}
