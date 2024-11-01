using UnityEngine;

public class LeverBatteryReader : MonoBehaviour
{
    [SerializeField] protected Transform _batteryInsertPoint;
    [SerializeField] protected float _insertedZRotation = -45;
    protected Vector3 _offsetVec3;
    protected Quaternion _insertedQuatRot;

    public bool BatteryInserted { get; protected set; }
}
