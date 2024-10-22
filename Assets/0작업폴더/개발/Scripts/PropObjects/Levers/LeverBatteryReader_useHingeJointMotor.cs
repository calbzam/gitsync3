using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverBatteryReader_useHingeJointMotor : MonoBehaviour
{
    [SerializeField] private LeverActivate _leverActivate;
    [SerializeField] private /*LeverHandle_useQuatRot*/LeverHandle_useHingeJointMotor _leverHandle;

    [Header("")]
    [SerializeField] private Transform _batteryInsertPoint;
    [SerializeField] private float _insertedZRotation = -45;
    private Vector3 _offsetVec3;
    private Quaternion _insertedQuatRot;

    public bool BatteryInserted { get; private set; }

    private void Start()
    {
        CheckBatteryInsertPointNull();
        _offsetVec3 = new Vector3(0, 0, 0.52f);
        _insertedQuatRot = Quaternion.Euler(new Vector3(0, 0, _insertedZRotation));
        BatteryInserted = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!BatteryInserted)
        {
            if (col.CompareTag("Battery"))
            {
                BatteryPickup battery = col.GetComponent<BatteryPickup>();
                if (!battery.IsHeldByPlayer) InsertBatteryToLever(battery);
                if (_leverActivate.IsAutomatic) _leverHandle.ToggleActivateLeverHandle_RotateOnly();
                _leverActivate.UpdateCheckpoint();
            }
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
        BatteryInserted = battery.BatteryInserted = true;
    }
}
