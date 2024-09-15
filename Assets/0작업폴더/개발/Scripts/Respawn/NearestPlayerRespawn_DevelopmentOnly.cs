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
        SetToNearestRespawnPoint();
    }

    private void RefreshCheckpoints()
    {
        _checkpoints = gameObject.GetComponentsInChildren<Checkpoint>();
        for (int i = 0; i < _checkpoints.Length; ++i)
            _checkpoints[i].IndexNum = i;
    }

    private void SetToNearestRespawnPoint() // nearest respawnpoint regardless of actual respawnpoint order
    {
        Vector3 playerPos = _player.transform.position;
        Checkpoint currentPoint = _player.RespawnPoint;

        foreach (Checkpoint checkPoint in _checkpoints)
        {
            if (Vector2.Distance(checkPoint.Position, playerPos) < Vector2.Distance(currentPoint.Position, playerPos))
                currentPoint = checkPoint;
        }

        _player.SetRespawnPoint(currentPoint);
    }

    public void SetToPrevRespawnPos() // nearest respawnpoint from left side of player
    {
        RefreshCheckpoints();
        if (_player.RespawnPoint.IndexNum > 0)
            _player.SetRespawnPoint(_checkpoints[_player.RespawnPoint.IndexNum - 1]);
    }

    public void SetToNextRespawnPos() // nearest respawnpoint from right side of player
    {
        RefreshCheckpoints();

        if (_player.RespawnPoint.IndexNum < _checkpoints.Length - 1)
            _player.SetRespawnPoint(_checkpoints[_player.RespawnPoint.IndexNum + 1]);
    }

    public void SetToPrevRespawnPosAndRespawn_Button()
    {
        SetToPrevRespawnPos();
        _player.RespawnPlayer();
    }

    public void SetToNextRespawnPosAndRespawn_Button()
    {
        SetToNextRespawnPos();
        _player.RespawnPlayer();
    }
}
