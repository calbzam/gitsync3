using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPickupRangeChecker : MonoBehaviour
{
    private bool _playerInPickupRange = false;
    public bool PlayerInPickupRange => _playerInPickupRange; // for public access

    private void Start()
    {
        _playerInPickupRange = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _playerInPickupRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _playerInPickupRange = false;
        }
    }
}
