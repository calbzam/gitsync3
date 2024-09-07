using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleObjectRespawnOnPlayerRespawn : MonoBehaviour
{
    private Transform _initialParent;
    private Vector3 _initialPos;
    private Quaternion _initialRot;

    private void Start()
    {
        _initialParent = transform.parent;
        _initialPos = transform.position;
        _initialRot = transform.rotation;
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
    }

    private void ResetTransform()
    {
        transform.position = _initialPos;
        transform.rotation = _initialRot;
    }
}
