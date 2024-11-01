using UnityEngine;

public class LeverBatteryReader_useQuatRot : LeverBatteryReader
{
    [Header("")]
    [SerializeField] private LeverActivate _leverActivate;
    [SerializeField] private LeverHandle_useQuatRot _leverHandle;

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
            if (col.CompareTag(Tags.BatteryTag))
            {
                BatteryPickup battery = col.GetComponent<BatteryPickup>();
                if (!battery.IsHeldByPlayer) InsertBatteryToLever(battery);
                if (_leverActivate.IsAutomatic) _leverHandle.ToggleLeverMovement();
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
