using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCubeSwimmingVolumeCameraBounds : MonoBehaviour
{
    [Header("CameraBound triggers to use")]
    [SerializeField] private PolygonCollider2D _aboveWaterCameraBounds;
    [SerializeField] private PolygonCollider2D _inWaterCameraBounds;

    [Header("SwapTrigger scripts to update")]
    [SerializeField] private SwapToExternalCameraBoundsOnTrigger _aboveWaterCamboundSwapper;
    [SerializeField] private SwapToExternalCameraBoundsOnTrigger _inWaterCamboundSwapper;

    private void Start()
    {
        if (_aboveWaterCameraBounds == null) Debug.Log("_aboveWaterCameraBounds is null, camerabounds also null:  " + transform.parent.name + " > " + name);
        if (_inWaterCameraBounds == null) Debug.Log("_inWaterCameraBounds is null, camerabounds also null:  " + transform.parent.name + " > " + name);
        
        _aboveWaterCamboundSwapper.SetCameraBounds(_aboveWaterCameraBounds);
        _inWaterCamboundSwapper.SetCameraBounds(_inWaterCameraBounds);
    }

    private void Update()
    {
        _aboveWaterCamboundSwapper.gameObject.SetActive(!PlayerLogic.Player.IsInWater);
        _inWaterCamboundSwapper.gameObject.SetActive(PlayerLogic.Player.IsInWater);
    }
}
