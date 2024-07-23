using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPickupRangeChecker : MonoBehaviour
{
    [SerializeField] private BatteryPickup _battery;
    //public bool PlayerInPickupRange { get; private set; }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _battery.PlayerInPickupRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _battery.PlayerInPickupRange = false;
        }
    }
}
