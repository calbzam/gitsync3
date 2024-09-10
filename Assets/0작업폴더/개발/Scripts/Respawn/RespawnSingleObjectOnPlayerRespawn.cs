using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnSingleObjectOnPlayerRespawn : MonoBehaviour
{
    private Transform _initialParent;
    private Vector3 _initialPos;
    private Quaternion _initialRot;

    private Rigidbody2D _rb2D;
    private Rigidbody _rb3D;

    private Vector3 _initialVelocity;

    private void Start()
    {
        _initialParent = transform.parent;
        _initialPos = transform.position;
        _initialRot = transform.rotation;

        _rb2D = GetComponent<Rigidbody2D>();
        _rb3D = GetComponent<Rigidbody>();

        if (_rb2D != null) _initialVelocity = _rb2D.velocity;
        else if (_rb3D != null) _initialVelocity = _rb3D.velocity;
        else _initialVelocity = Vector3.zero;
    }

    private void OnEnable()
    {
        PlayerLogic.PlayerRespawned += PlayerRespawnedAction;
    }

    private void OnDisable()
    {
        PlayerLogic.PlayerRespawned -= PlayerRespawnedAction;
    }

    private void PlayerRespawnedAction()
    {
        transform.SetParent(_initialParent);
        ResetTransform();
        ResetRb();
    }

    private void ResetTransform()
    {
        transform.position = _initialPos;
        transform.rotation = _initialRot;
    }

    private void ResetRb()
    {
        if (_rb2D != null)
        {
            _rb2D.velocity = _initialVelocity;
        }
        else if (_rb3D != null)
        {
            _rb3D.velocity = _initialVelocity;
        }
    }
}
