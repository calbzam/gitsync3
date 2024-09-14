using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform _respawnPoint; // transform of yellow circle sprite

    public Transform RespawnPoint => _respawnPoint; // for public access
    public Vector3 Position => RespawnPoint.position;
    public int IndexNum { get; set; }
}
