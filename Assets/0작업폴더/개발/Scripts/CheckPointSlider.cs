using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CheckpointSlider : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _checkpointTrigger;
    [SerializeField] private Transform _respawnPoint; // yellow circle sprite

    [Header("리스폰 위치"), Range(0f, 1f)]
    public float respawnPlayerAt = 0.5f;

    private float topY;
    private float bottomY;

    private void getTriggerBoxExtents()
    {
        topY = _checkpointTrigger.bounds.center.y + _checkpointTrigger.bounds.size.y / 2;
        bottomY = _checkpointTrigger.bounds.center.y - _checkpointTrigger.bounds.size.y / 2;
    }

    private void Update()
    {
        getTriggerBoxExtents();
        _respawnPoint.position = new Vector3(_checkpointTrigger.transform.position.x, MyMath.Remap(respawnPlayerAt, 0f, 1f, bottomY, topY), _respawnPoint.position.z);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<PlayerController>().SetRespawnPos(transform.position);
        }
    }
}
