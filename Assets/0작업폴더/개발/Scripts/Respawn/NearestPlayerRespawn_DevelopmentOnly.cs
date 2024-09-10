using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearestPlayerRespawn_DevelopmentOnly : MonoBehaviour
{
    private Checkpoint[] _checkpoints;

    [SerializeField] private Checkpoint _initialSpawnPoint;

    private PlayerController _player; // do not replace with PlayerLogic.Player, for loading consistency in testing purposes

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        RefreshCheckpoints();
        _player.SetRespawnPoint(_initialSpawnPoint);
    }

    private void RefreshCheckpoints()
    {
        _checkpoints = gameObject.GetComponentsInChildren<Checkpoint>();
        for (int i = 0; i < _checkpoints.Length; ++i)
            _checkpoints[i].IndexNum = i;
    }

    public void SetToPrevRespawnPos() // nearest respawn point from left side of player
    {
        RefreshCheckpoints();
        if (_player.RespawnPoint.IndexNum > 0)
            _player.SetRespawnPoint(_checkpoints[_player.RespawnPoint.IndexNum - 1]);
    }

    public void SetToNextRespawnPos() // nearest respawn point from right side of player
    {
        RefreshCheckpoints();

        if (_player.RespawnPoint.IndexNum < _checkpoints.Length - 1)
            _player.SetRespawnPoint(_checkpoints[_player.RespawnPoint.IndexNum + 1]);
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
