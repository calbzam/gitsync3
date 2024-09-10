using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnClothOnPlayerRespawn : MonoBehaviour
{
    private Cloth _cloth;

    private void Start()
    {
        _cloth = GetComponent<Cloth>();
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
        ResetCloth();
    }

    private void ResetCloth()
    {
        if (_cloth != null)
        {
            if (_cloth.enabled)
            {
                _cloth.enabled = false;
                _cloth.enabled = true;
            }
        }
    }
}
