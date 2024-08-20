using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ControlVolumeTrigger: MonoBehaviour
{
    [SerializeField] private Volume _volumeToControl;
    [SerializeField] private float _blendSpeed = 5f;

    private bool _isInTransition;
    private float _toWeight;

    private void Start()
    {
        _isInTransition = false;
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
