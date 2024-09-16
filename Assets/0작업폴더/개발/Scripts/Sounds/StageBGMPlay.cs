using System;
using UnityEngine;

public class StageBGMPlay : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _selfCol;

    public bool StageBGMStarted { get; set; }
    public event Action PlayerEntered;

    private void Start()
    {
        StageBGMStarted = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!StageBGMStarted && col.CompareTag("Player"))
        {
            StageBGMStarted = true;
            PlayerEntered?.Invoke();
        }
    }
}
