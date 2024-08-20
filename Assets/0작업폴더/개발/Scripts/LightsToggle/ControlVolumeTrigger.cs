using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ControlVolumeTrigger: MonoBehaviour
{
    [SerializeField] private BoxCollider _triggerColSelf;
    [SerializeField] private Volume _volumeToControl;
    [SerializeField] private float _blendSpeed = 5f;

    private ColBounds3D _triggerColBounds;
    private Collider _mainCameraCol;

    private bool _isInTransition;
    private float _toWeight;

    private void Start()
    {
        _isInTransition = false;

        _triggerColBounds = new ColBounds3D(_triggerColSelf);
        _mainCameraCol = Camera.main.GetComponentInChildren<Collider>();
        EvalVolumeInitialState();
    }

    private void EvalVolumeInitialState()
    {
        if (_triggerColBounds.OtherIsInSelf(_mainCameraCol))
            _volumeToControl.weight = 1;
        else
            _volumeToControl.weight = 0;
    }

    private void Update()
    {
        if (_isInTransition)
        {
            _volumeToControl.weight = Mathf.MoveTowards(_volumeToControl.weight, _toWeight, _blendSpeed * Time.deltaTime);

            if (Mathf.Abs(_volumeToControl.weight - _toWeight) < 0.04f)
            {
                _volumeToControl.weight = _toWeight;
                _isInTransition = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera Collider"))
        {
            _toWeight = 1;
            _isInTransition = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera Collider"))
        {
            _toWeight = 0;
            _isInTransition = true;
        }
    }
}
