using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeverHandleReader : MonoBehaviour
{
    public bool PlayerIsInRange { get; private set; }

    private void Start()
    {
        PlayerIsInRange = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerIsInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerIsInRange = false;
        }
    }
}
