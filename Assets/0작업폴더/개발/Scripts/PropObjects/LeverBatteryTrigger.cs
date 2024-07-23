using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverBatteryTrigger : MonoBehaviour
{
    [SerializeField] private Transform _batteryInsertPoint;
    [SerializeField] private float _insertedZRotation = -45;
    private Vector3 _offsetVec3;
    private Quaternion _insertedQuatRot;

    private void Start()
    {
        CheckBatteryInsertPointNull();
        _offsetVec3 = new Vector3(0, 0, transform.position.z + 0.01f);
        _insertedQuatRot = Quaternion.Euler(new Vector3(0, 0, _insertedZRotation));
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Battery"))
        {
            BatteryPickup battery = col.GetComponent<BatteryPickup>();
            if (!battery.IsHeldByPlayer) InsertBatteryToLever(battery);
        }
    }

    private void CheckBatteryInsertPointNull()
    {
        if (_batteryInsertPoint == null)
            Debug.LogError("Transform Battery Insert Point not set for: " + transform.parent.name + " > " + name);
    }

    private void InsertBatteryToLever(BatteryPickup battery)
    {
        battery.IsPickable = false;
        battery.SetBatteryParent(_batteryInsertPoint);
        battery.SetLocalTransform(_offsetVec3, _insertedQuatRot);
    }
}
