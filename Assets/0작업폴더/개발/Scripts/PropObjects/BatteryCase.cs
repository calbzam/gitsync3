using UnityEngine;

public class BatteryCase : MonoBehaviour
{
    [SerializeField] private BatteryPickup _battery;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_battery.BatteryIsInBox)
        {
            if (col.CompareTag(Tags.PlayerTag))
                _battery.PlayerIsInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (_battery.BatteryIsInBox)
        {
            if (col.CompareTag(Tags.PlayerTag))
                _battery.PlayerIsInRange = false;
        }
    }
}
