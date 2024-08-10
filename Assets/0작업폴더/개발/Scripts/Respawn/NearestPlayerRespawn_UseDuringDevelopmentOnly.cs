using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearestPlayerRespawn_UseDuringDevelopmentOnly : MonoBehaviour
{
    private Checkpoint[] _checkpoints;

    [SerializeField] private Transform _initialSpawnPoint;

    private PlayerController _player; // do not replace with PlayerLogic.Player, for loading consistency in testing purposes

    private void Start()
    {
        _checkpoints = gameObject.GetComponentsInChildren<Checkpoint>();

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        _player.SetRespawnPos(_initialSpawnPoint.position);
        _player.SetNearestRespawnPos(_checkpoints);
    }

    public void SetNearestRespawnPosAndRespawn()
    {
        _player.SetRespawnPos(_initialSpawnPoint.position);
        _player.SetNearestRespawnPos(_checkpoints);
        _player.RespawnPlayer();
    }
}
