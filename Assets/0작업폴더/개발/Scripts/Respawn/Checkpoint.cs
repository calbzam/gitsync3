using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform _respawnPoint; // yellow circle sprite
    public Transform RespawnPoint => _respawnPoint; // for public access
}
