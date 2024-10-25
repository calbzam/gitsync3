using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FreeCamMove : MonoBehaviour
{
    [SerializeField] private float _zoomReduction = 3000;
    [SerializeField] private float _minThreshold = 4;

    public bool DragMouseEnabled;

    private ReadMouse _readMouse;

    public float CamPosZ { get; private set; }
    public event Action<float> CamPosZUpdated;

    private void Start()
    {
        DragMouseEnabled = true;
        CamPosZ = transform.position.z;
        _readMouse = gameObject.AddComponent<ReadMouse>();
        _readMouse.RefPosZ = CamPosZ;
    }

    private void OnEnable()
    {
        CamPosZ = transform.position.z;
    }

    private void LateUpdate()
    {
        if (DragMouseEnabled && _readMouse.IsDragging)
        {
            transform.position += _readMouse.MouseClickOrigin - _readMouse.GetWorldMousePos();
        }

        float scrollAmount;
        if ((scrollAmount = ReadMouse.GetScrollAmount()) != 0)
        {
            float distanceSizing = Mathf.Max(Mathf.Abs(CamPosZ), _minThreshold);
            transform.position += (scrollAmount / _zoomReduction * distanceSizing) * Vector3.forward; // 그냥 Unity Editor에서 시행착오 하다가 나온 식
            
            CamPosZ = transform.position.z;
            _readMouse.RefPosZ = CamPosZ;
            CamPosZUpdated?.Invoke(CamPosZ);
        }
    }
}
