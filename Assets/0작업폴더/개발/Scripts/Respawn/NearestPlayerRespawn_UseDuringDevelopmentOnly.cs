using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearestPlayerRespawn_UseDuringDevelopmentOnly : MonoBehaviour
{
    private CheckpointTrigger[] _checkpointTriggers;

    [SerializeField] private Transform _initialSpawnPoint;

    private PlayerController _player; // do not replace with PlayerLogic.Player, for loading consistency in testing purposes

    private void Start()
    {
        _checkpointTriggers = gameObject.GetComponentsInChildren<CheckpointTrigger>();

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        _player.SetRespawnPos(_initialSpawnPoint.position);
        GetNearestRespawnPos();
    }

    private void GetNearestRespawnPos()
    {
        Vector3 playerPos = _player.transform.position;
        Vector3 currentRespawnPos = _player.RespawnPos;

        foreach (CheckpointTrigger trigger in _checkpointTriggers)
        {
            if (trigger.RespawnPoint.position.x < playerPos.x)
                if (Vector2.Distance(trigger.RespawnPoint.position, playerPos) < Vector2.Distance(currentRespawnPos, playerPos))
                    _player.SetRespawnPos(trigger.RespawnPoint.position);
        }
    }
}
