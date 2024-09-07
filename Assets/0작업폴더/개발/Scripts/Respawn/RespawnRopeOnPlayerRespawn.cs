using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

public class RespawnRopeOnPlayerRespawn : MonoBehaviour
{
    private ObiRope _rope;

    private void Start()
    {
        _rope = GetComponent<ObiRope>();
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
        ResetRope();
    }

    private void ResetRope()
    {
        if (_rope != null)
        {
            _rope.ResetParticles();
        }
    }
}
