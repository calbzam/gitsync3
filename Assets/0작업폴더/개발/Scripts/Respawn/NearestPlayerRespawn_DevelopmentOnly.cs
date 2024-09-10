using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearestPlayerRespawn_DevelopmentOnly : MonoBehaviour
{
    private Checkpoint[] _checkpoints;

    [SerializeField] private Transform _initialSpawnPoint;

    private PlayerController _player; // do not replace with PlayerLogic.Player, for loading consistency in testing purposes

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        SetToPrevRespawnPos();
    }

    public void SetToPrevRespawnPos() // nearest respawn point from left side of player
    {
        _player.SetRespawnPos(_initialSpawnPoint.position);
        _player.SetToPrevRespawnPos(_checkpoints = gameObject.GetComponentsInChildren<Checkpoint>());
    }

    public void SetToNextRespawnPos() // nearest respawn point from right side of player
    {
        //_player.SetRespawnPos([last spawn point].position);
        _player.SetToNextRespawnPos(_checkpoints = gameObject.GetComponentsInChildren<Checkpoint>());
    }

    public void SetToPrevRespawnPosAndRespawn()
    {
        SetToPrevRespawnPos();
        _player.RespawnPlayer();
    }

    public void SetToNextRespawnPosAndRespawn()
    {
        SetToNextRespawnPos();
        _player.RespawnPlayer();
    }
}
