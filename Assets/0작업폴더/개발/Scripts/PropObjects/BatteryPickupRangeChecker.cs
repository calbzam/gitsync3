using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPickupRangeChecker : MonoBehaviour
{
    [SerializeField] private BatteryPickup _battery;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _battery.PlayerIsInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _battery.PlayerIsInRange = false;
        }
    }
}
