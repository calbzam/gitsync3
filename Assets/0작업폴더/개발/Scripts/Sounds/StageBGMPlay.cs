using System;
using UnityEngine;

public class StageBGMPlay : MonoBehaviour
{
    [SerializeField] private AudioSource _stageBgm;
    [SerializeField] private BoxCollider2D _selfCol;

    public event Action PlayerEntered;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!_stageBgm.isPlaying && col.CompareTag("Player"))
        {
            PlayerEntered?.Invoke();
        }
    }
}
